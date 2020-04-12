using Beat_Saber_All_Songs_Downloader.Models;
using BeatSaber___All_Songs_Downloader.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            new Thread(async () =>
            {
                await StartDownloadAsync(songCount, downloadSongData, useDb);

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

            _songs = GetSongsFromFile();
            var d = _songs.docs.Where(x => x.metadata.characteristics == null || x.metadata.characteristics.Count == 0);
            var f = _songs.docs.Take(2).ToList();
            SaveSongsToDb(f);
            return;



            var message = useDb ? "\nGetting songs from the database": "\nGetting songs from file....";
            UpdateTextBox(message);
            _songs = useDb ? GetSongsFromDb() : GetSongsFromFile();
            if (!downloadNewData)
            {  
                if (_songs.totalDocs == 0)
                {
                    UpdateTextBox("\nIt looks like you haven't downloaded the song data yet. You need to before this can continue. Download all songs or you can check download new song data and try again.");
                }
                else
                {
                    var source = useDb ? "database" : "file";
                    UpdateTextBox($"\nGot songs from {source}. Because you opted not to download song data we will only check for missing songs. Checking downloaded songs against known songs.....");
                    var newSongs = FindMissingSongs();
                    foreach (var song in newSongs)
                    {
                        AddSongToNewList(song);
                    }
                }
            }
            else
            {
                var knownSongs = _songs;
                var source = useDb ? "database" : "computer";
                UpdateTextBox($"\nGot songs from {source}. Getting list of songs from server....");
                _songs = await new Downloader().GetAllSongInfoAsync(this);
                var newSongs = _songs.docs.Except(knownSongs.docs);
                foreach (var song in newSongs)
                {
                    AddSongToNewList(song);
                }
                UpdateTextBox($"\n\nFound {newSongs.Count()} new songs on the server. To download them click the download all button");

                if (useDb)
                {
                    //SaveSongsToDb();
                }
                else
                {
                    SaveSongsToFile();
                }
            } 
        }

        private async Task StartDownloadAsync(string numberOfSongs, bool downloadSongInfo, bool useDb)
        {
            if (string.IsNullOrWhiteSpace(numberOfSongs) || string.IsNullOrWhiteSpace(_folderBasePath))
            {
                return;
            }
            var downloader = new Downloader();
            int i = 0;
            int max = await downloader.GetTotalCountOfSongsAsync();
            var keepGoing = true;
            var threadCount = 0;
            var maxThreads = int.Parse(numberOfSongs);
            DisableButtons();

            if (maxThreads > 50)
            {
                UpdateTextBox("\nYou entered more than 50 songs at once. To prevent errors and crashes this has automaicatlly been set to 50. Please do not enter more than 50 next time. ");
            }

            UpdateTextBox("\nStarting......");

            if (downloadSongInfo)
            {
                UpdateTextBox("\nDownloading song info for every song..........");
                _songs = await downloader.GetAllSongInfoAsync(this);
                UpdateTextBox("\n\nGot all song info\nSaving info to file........");
                SaveSongsToFile();
            }
            else
            {
                _songs = GetSongsFromFile();
                if (_songs.docs == null || _songs.docs.Count == 0)
                {
                    UpdateTextBox("\n It seems you don't have the song data downloaded yet and you choose not to download it. So this can't continue. Check to download the song data before continuing....");
                    return;
                }
                UpdateTextBox("\nGot current songs from file.....");

                _songs = GetMissingSongs();

                UpdateTextBox($"\nFound {_songs.docs.Count} songs not downloaded. Will now download  all mmissing songs....");
                max = _songs.docs.Count;
            }
            
            UpdateTextBox("\n\nStarting download of every song......");

            int songsPerthread = max % maxThreads == 0 ? max / maxThreads : (max / maxThreads) + 1;

            while (keepGoing)
            {
                var start = i;
                var count = 0;
                if (start + songsPerthread > (max - 1) && (max - 1 - i) > 0)
                {
                    count = songsPerthread - (max - 1 - i);
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

        private void DisableButtons()
        {
            Dispatcher.Invoke(() =>
            {
                DownloadBtn.IsEnabled = false;
                BrowseBtn.IsEnabled = false;
                //TextBlock.IsEnabled = false;
                NumberOfSongsAtOnceBox.IsEnabled = false;
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
                var item = $"{song.metadata.songName} - {song.metadata.songAuthorName}";
                NewSongsLB.Items.Add(item);
            });
        }

        private void SaveSongsToFile()
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - All Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                Directory.CreateDirectory(saveFolderLocaion);
            }
            var json = JsonConvert.SerializeObject(_songs);
            File.WriteAllText($@"{saveFolderLocaion}\songs.json", json);
        }

        private void SaveSongsToDb(List<Song> songs)
        {
            using(var context = new BeatSaverContext())
            {
                foreach(var song in songs)
                {

                }
            }
        }

        private void UpdateSongStats()
        {

        }

        private PageResult GetSongsFromFile()
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - All Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                return new PageResult();
            }
            var json = File.ReadAllText($@"{saveFolderLocaion}\songs.json");
            var result = JsonConvert.DeserializeObject<PageResult>(json);
            return result;
        }

        private PageResult GetSongsFromDb()
        {
            var result = new PageResult();
            using(var context = new BeatSaverContext())
            {
                var songs = context.Songs.ToList();
                result.docs = songs;
            }
            result.totalDocs = result.docs.Count;

            return result;
        }

        private List<Song> FindMissingSongs()
        {
            var result = new List<Song>();
            foreach(var song in _songs.docs)
            {
                var fileName = TextHandler.GetValidFileName(_folderBasePath, song);
                if (!File.Exists(fileName))
                {
                    result.Add(song);
                }
            }

            UpdateTextBox($"\n\nFound {result.Count} new songs.");
            return result;
        }
    }
}
