using Beat_Saber_All_Songs_Downloader.Models;
using BeatSaber___All_Songs_Downloader;
using BeatSaber___All_Songs_Downloader.Helpers;
using BeatSaber_All_Songs_Downloader.DB;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace BeatSaber_All_Songs_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _folderBasePath = @"R:\BeatSaber Songs";
        private PageResult _songs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var songCount = NumberOfSongsAtOnceBox.Text;
            var downloadSongData = DownloadSongDataCB.IsChecked.Value;
            var useDb = SaveToDbCB.IsChecked.Value;
            var newSongs = NewSongsLB.Items.Cast<Song>().ToList();
            SongErrorsLB.Items.Clear();
            new Thread(async () =>
            {
                await StartDownloadAsync(songCount, downloadSongData, useDb, newSongs);

            }).Start();
        }

        private void CheckForNewSongsBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadNewData = DownloadSongDataCB.IsChecked.Value;
            var useDb = SaveToDbCB.IsChecked.Value;
            SongErrorsLB.Items.Clear();
            NewSongsLB.Items.Clear();
            new Thread(async () =>
            {
                await OnlycCheckForNewSongsAsync(downloadNewData, useDb);
            }).Start();
        }

        private async Task OnlycCheckForNewSongsAsync(bool downloadNewData, bool useDb)
        {
            var message = useDb ? "\nGetting songs from the database": "\nGetting songs from file....";
            UpdateTextBox(message);
            _songs = useDb ? GetSongsFromDb() : FileHelper.GetPageResultFromFile();

            if (!downloadNewData)
            {  
                if (_songs.totalDocs == 0)
                {
                    UpdateTextBox("\nIt looks like you haven't downloaded the song data yet. You need to before this can continue. Download all songs or you can check download new song data and try again.");
                }
                else
                {
                    var source = useDb ? "database" : "file";
                    UpdateTextBox($"\nGot songs from {source}. Because you opted not to download song data we will only check for missing song files. Checking downloaded songs against known songs.....");
                    var newSongs = FileHelper.FindMissingSongFiles(_folderBasePath, _songs.docs, this);
                    foreach (var song in newSongs)
                    {
                        AddSongToNewList(song);
                    }
                    UpdateTextBox("\nThe new songs are listed above. To get them just hit the download all button."); 
                }
            }
            else
            {
                var knownSongs = _songs;
                var source = useDb ? "database" : "computer";
                UpdateTextBox($"\nGot songs from {source}. Getting latest list of songs from server. This could take awhile....");
                _songs = await new Downloader().GetAllSongInfoForAllFiltersAsync(this);
                var newSongs = _songs.docs.Except(knownSongs.docs, new SongComparer());
                foreach (var song in newSongs)
                {
                    AddSongToNewList(song);
                }
                UpdateTextBox($"\n\nFound {newSongs.Count()} new songs on the server. To download them click the download all button.");

                if (useDb)
                {
                    BeatSaverDbHelper.SaveSongsToDb(newSongs.ToList());
                }
                else
                {
                    FileHelper.SavePageResultToFile(_songs);
                }
            }

            EnableButtons();
        }

        private async Task StartDownloadAsync(string numberOfSongs, bool downloadSongInfo, bool useDb, List<Song> newSongs)
        {
            if (string.IsNullOrWhiteSpace(numberOfSongs) || string.IsNullOrWhiteSpace(_folderBasePath))
            {
                UpdateTextBox("\nYou did not set a number of songs to download at once. You must do that first.");
                return;
            }
            var downloader = new Downloader();
            int i = 0;
            int max = 0;
            var keepGoing = true;
            var threadCount = 0;
            var completedThreads = 0;
            var maxThreads = int.Parse(numberOfSongs);
            var dataSource = useDb ? "database" : "file";
            DisableButtons();

            if (maxThreads > 50)
            {
                UpdateTextBox("\nYou entered more than 50 songs at once. To prevent errors and crashes this has automaicatlly been set to 50. Please do not enter more than 50 next time. ");
            }

            UpdateTextBox("\nStarting......");

            if(newSongs != null && newSongs.Any())
            {
                UpdateTextBox("\nIt looks like you already got the list of new songs. Downloading ONLY those songs now (to save a bunch of time)......");
                _songs.docs = newSongs;
            }
            else if (downloadSongInfo)
            {
                UpdateTextBox($"\nGetting songs from {dataSource}......");
                var knownSongs = useDb ? GetSongsFromDb() : FileHelper.GetPageResultFromFile();
                UpdateTextBox("\nDownloading song info for every song from server. This will take awhile. Go get a snack...........");
                _songs = await downloader.GetAllSongInfoForAllFiltersAsync(this);
                var songsToAdd = _songs.docs.Except(knownSongs.docs, new SongComparer()).ToList();
                foreach (var s in songsToAdd)
                {
                    AddSongToNewList(s);
                }
                UpdateTextBox($"\n\nGot all song info\nThere are {songsToAdd.Count()} new songs since the last time you checked.\nSaving new songs to {dataSource}");
                if (useDb)
                {
                    BeatSaverDbHelper.SaveSongsToDb(songsToAdd);
                }
                else
                {
                    FileHelper.SavePageResultToFile(_songs);
                }
                _songs.docs = songsToAdd;
            }
            else
            {
                _songs = useDb ? GetSongsFromDb() : FileHelper.GetPageResultFromFile();
                if (_songs.docs == null || _songs.docs.Count == 0)
                {
                    UpdateTextBox("\n It seems you don't have the song data downloaded yet and you choose not to download it. So this can't continue. Check to download the song data before continuing....");
                    return;
                }
                UpdateTextBox($"\nGot current songs from {dataSource}.....");

                _songs = GetMissingSongs();

                UpdateTextBox($"\nFound {_songs.docs.Count} songs not downloaded. Will now download  all missing songs....");
            }

            max = _songs.docs.Count;

            UpdateTextBox("\n\nStarting download of every song......");

            int songsPerthread = max % maxThreads == 0 ? max / maxThreads : (max / maxThreads) + 1;

            while (keepGoing)
            {
                var start = i;
                var count = 0;
                if (start + songsPerthread > (max - 1) && (max - 1 - i) > 0)
                {
                    count = (max - 1 - i);
                }
                else if (start + songsPerthread > (max - 1)) 
                {
                    count = 1;
                } 
                else 
                {
                    count = songsPerthread;
                }

                i += count;
                new Thread(async () =>
                {
                    await downloader.DownloadAllForRangeAsync(_folderBasePath, this, _songs.docs.GetRange(start, count));
                    if(++completedThreads == threadCount)
                    {
                        EnableButtons();
                    }
                }).Start();
                if (i >= max)
                {
                    keepGoing = false;
                }
                threadCount++;
            }
            UpdateTextBox($"\n\nStarted {threadCount} threads....");
        }

        private PageResult GetMissingSongs()
        {
            var result = new PageResult
            {
                docs = new List<Song>()
            };
            foreach(var song in _songs.docs)
            {
                var fileName = TextHandler.GetValidFileName(_folderBasePath, song);
                if (!File.Exists(fileName))
                {
                    result.docs.Add(song);
                }
            }

            return result;
        }

        private void EnableButtons()
        {
            Dispatcher.Invoke(() =>
            {
                DownloadBtn.IsEnabled = true;
                BrowseBtn.IsEnabled = true;
                NumberOfSongsAtOnceBox.IsEnabled = true;
                NewSongsBtn.IsEnabled = true;
            });
        }

        private void DisableButtons()
        {
            Dispatcher.Invoke(() =>
            {
                DownloadBtn.IsEnabled = false;
                BrowseBtn.IsEnabled = false;
                NumberOfSongsAtOnceBox.IsEnabled = false;
                NewSongsBtn.IsEnabled = false;
                TextBlock.Text = "";
            });  
        }

        private void OpenFolderDialog(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                SongFolderLabel.Content = dialog.SelectedPath;
                this._folderBasePath = dialog.SelectedPath;
            }
            else
            {
                UpdateTextBox("\nYou did  not select a folder. You must select one to continue");
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        internal void UpdateTextBox(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TextBlock.Text += message;
                TextBlock.ScrollToEnd();
            });
        }

        internal void AddSongToErrorList(Song song)
        {
            Dispatcher.Invoke(() =>
            {
                var item = $"{song.metadata.songName} - {song.metadata.songAuthorName}";
                SongErrorsLB.Items.Add(item);
            });
        }

        internal void AddSongToNewList(Song song)
        {
            Dispatcher.Invoke(() =>
            {
                NewSongsLB.Items.Add(song);
            });
        }

        private PageResult GetSongsFromDb()
        {
            var result = new PageResult();
            using (var repo = new BeatSaverRepo())
            {
                result.docs = repo.GetAllSongs();
            }
                
            result.totalDocs = result.docs.Count;

            return result;
        }
    }
}
