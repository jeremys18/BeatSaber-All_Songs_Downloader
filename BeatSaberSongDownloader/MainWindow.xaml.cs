using BeatSaberSongDownloader.Comparers;
using BeatSaberSongDownloader.Helpers;
using BeatSaberSongDownloader.Data.Models.BareModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace BeatSaberSongDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _folderBasePath = string.Empty;
        private List<Song> _songs;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var songCount = NumberOfSongsAtOnceBox.Text;
            var downloadSongData = DownloadSongDataCB.IsChecked.Value;
            var newSongs = NewSongsLB.Items.Cast<Song>().ToList();
            SongErrorsLB.Items.Clear();
            new Thread(async () =>
            {
                await StartDownloadAsync(songCount, downloadSongData, newSongs);

            }).Start();
        }

        private void CheckForNewSongsBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadNewData = DownloadSongDataCB.IsChecked.Value;
            SongErrorsLB.Items.Clear();
            NewSongsLB.Items.Clear();
            new Thread(async () =>
            {
                await OnlyCheckForNewSongsAsync(downloadNewData);
            }).Start();
        }

        private async Task OnlyCheckForNewSongsAsync(bool downloadNewData)
        {
            if(string.IsNullOrWhiteSpace(_folderBasePath))
            {
                UpdateTextBox("You MUST select a folder first! It's required...");
                return;
            }

            DisableButtons();
            UpdateTextBox("\nGetting songs from file....");
            _songs = FileHelper.GetSongsFromFile();

            if (!downloadNewData)
            {
                if (_songs.Count == 0)
                {
                    UpdateTextBox("\nIt looks like you haven't downloaded the song data yet. You need to before this can continue. Download all songs or you can check download new song data and try again.");
                }
                else
                {
                    UpdateTextBox($"\nGot songs from local file. Because you opted not to download song data we will only check for missing song files. Checking downloaded songs against local list of known songs.....");
                    var newSongs = FileHelper.FindMissingSongFiles(_folderBasePath, _songs, this);
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
                UpdateTextBox($"\nGot songs from local file. Getting latest list of songs from server. This could take awhile....");
                _songs = await new Downloader().GetAllSongInfo(this, _folderBasePath);
                var newSongs = _songs.Except(knownSongs, new SongComparer());
                foreach (var song in newSongs)
                {
                    AddSongToNewList(song);
                }
               
                FileHelper.SaveSongsToFile(_songs);
                UpdateTextBox("\nSaved all songs list locally......");
                UpdateTextBox($"\n\nFound {newSongs.Count()} new songs on the server. To download them click the download all button.");
            }

            EnableButtons();
        }

        private async Task StartDownloadAsync(string numberOfSongs, bool downloadSongInfo, List<Song> newSongs)
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
            DisableButtons();

            if (maxThreads > 50)
            {
                UpdateTextBox("\nYou entered more than 50 songs at once. To prevent errors and crashes this has automaicatlly been set to 50. Please do not enter more than 50 next time. ");
            }

            UpdateTextBox("\nStarting......");

            if (newSongs != null && newSongs.Any())
            {
                UpdateTextBox("\nIt looks like you already got the list of new songs. Downloading ONLY those songs now (to save a bunch of time)......");
                _songs = newSongs;
            }
            else if (downloadSongInfo)
            {
                UpdateTextBox($"\nGetting known songs from local file......");
                var knownSongs = FileHelper.GetSongsFromFile();
                UpdateTextBox("\nDownloading song info for every song from server............"); // our server not theirs
                _songs = await downloader.GetAllSongInfo(this, _folderBasePath);
                var songsToAdd = _songs.Except(knownSongs, new SongComparer()).ToList();
                foreach (var s in songsToAdd)
                {
                    AddSongToNewList(s);
                }
                UpdateTextBox($"\n\nGot all song info\nThere are {songsToAdd.Count()} new songs since the last time you checked.\nSaving all songs to local file...");
                FileHelper.SaveSongsToFile(_songs);
                _songs = songsToAdd;
            }
            else
            {
                _songs = FileHelper.GetSongsFromFile();
                if (_songs == null || _songs.Count == 0)
                {
                    UpdateTextBox("\n It seems you don't have the song data downloaded yet and you choose not to download it. So this can't continue. Check to download the song data before continuing....");
                    return;
                }
                UpdateTextBox($"\nGot current songs from local file.....");

                _songs = FileHelper.FindMissingSongFiles(_folderBasePath, _songs, this);

                UpdateTextBox($"\nFound {_songs.Count} songs not downloaded. Will now download all missing songs....");
            }

            max = _songs.Count;

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
                    await downloader.DownloadAllForRangeAsync(_folderBasePath, this, _songs.GetRange(start, count), true);
                    if (++completedThreads == threadCount)
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
                SongErrorsLB.Items.Add(song.Name);
            });
        }

        internal void AddSongToNewList(Song song)
        {
            Dispatcher.Invoke(() =>
            {
                NewSongsLB.Items.Add(song);
            });
        }
    }
}
