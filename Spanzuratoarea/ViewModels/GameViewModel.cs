using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;

namespace Spanzuratoarea.ViewModels
{
    public class GameViewModel : Screen
    {
        private int _duration;
        private string _timeLeft;
        private int _lives;
        private string _hangmanImage;
        private string _category;
        private string _word;
        private BindableCollection<char> _wordDisplayed;
        private string _playerPicture;
        private string _username;
        private string _currentRound;
        
        public DispatcherTimer Timer { get; set; }

        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                this._duration = value;
                NotifyOfPropertyChange(() => Duration);
            }
        }

        public string TimeLeft
        {
            get
            {
                return this._timeLeft;
            }
            set
            {
                this._timeLeft = value;
                NotifyOfPropertyChange(() => TimeLeft);
            }
        }

        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                NotifyOfPropertyChange(() => Lives);
            }
        }

        public string HangmanImage
        {
            get { return _hangmanImage; }
            set
            {
                _hangmanImage = value;
                NotifyOfPropertyChange(() => HangmanImage);
            }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string Word
        {
            get { return _word; }
            set { _word = value; }
        }

        public BindableCollection<char> WordDisplayed
        {
            get { return _wordDisplayed; }
            set
            {
                _wordDisplayed = value;
                NotifyOfPropertyChange(() => WordDisplayed);
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public string PlayerPicture
        {
            get { return _playerPicture; }
            set
            {
                _playerPicture = value;
                NotifyOfPropertyChange(() => PlayerPicture);
            }
        }

        public string CurrentRound
        {
            get { return _currentRound; }
            set
            {
                _currentRound = value;
                NotifyOfPropertyChange(() => CurrentRound);
            }
        }

        public GameViewModel()
        {
            Username = Utils.GetUserCurrentGame();
            PlayerPicture = Utils.GetUserPhotoCurrentGame();
            Category = "All";
            NewGame();
        }

        public void Click(char letter)
        {
            if (WordDisplayed.Contains(letter))
            {
                MessageBox.Show("Letter can't be clicked twice!");
            }
            else
            {
                if (Word.ToUpper().Contains(letter))
                {
                    var position = 0;
                    foreach (var character in Word)
                    {
                        if (letter.Equals(Char.ToUpper(character)))
                        {
                            WordDisplayed[position] = letter;
                            position++;

                        }
                        else
                        {
                            position++;
                        }
                    }

                    if (!WordDisplayed.Contains('_'))
                    {
                        Timer.Stop();
                        MessageBox.Show("You won the round!");
                        NextRound();
                    }
                }
                else
                {
                    var currentPicture = Path.GetFileNameWithoutExtension(HangmanImage);
                    var hangmanImagesPath = Utils.GetProjectPath() + "/Resources/GameHangman/";
                    if (currentPicture != null &&
                        short.Parse(currentPicture) < Directory.GetFiles(hangmanImagesPath).Length - 2)
                    {
                        HangmanImage = hangmanImagesPath + (short.Parse(currentPicture) + 1) + ".png";
                        Lives -= 1;
                    }
                    else
                    {
                        HangmanImage = hangmanImagesPath + (short.Parse(currentPicture) + 1) + ".png";
                        Lives -= 1;
                        Timer.Stop();
                        MessageBox.Show("You lost the game");
                        Utils.UpdateStatistics(Username, false);
                        NewGame();
                    }
                }
            }
        }

        public void ChooseCategory(bool isChecked,string category)
        {
            if (!isChecked) return;
            Category = category;
            MessageBox.Show(Category);
        }

        public void NewGame()
        {
            CurrentRound = "0/5";
            HangmanImage = Utils.GetProjectPath() + "/Resources/GameHangman/0.png";
            Word = Utils.ChooseWord(Category);
            Lives = 6;
            WordDisplayed = new BindableCollection<char>();
            foreach (var letter in Word)
            {
                WordDisplayed.Add('_');
            }
            Timer?.Stop();
            StartClock();
        }

        public void NextRound()
        {
            var nextRound = short.Parse(CurrentRound.Split('/').First()) + 1;
            if (nextRound >= 5)
            {
                MessageBox.Show("You won");
                Utils.UpdateStatistics(Username, true);
                Timer.Stop();
                NewGame();
            }
            else
            {
                NewGame();
                CurrentRound = nextRound + "/5";
            }
        }

        public void SaveGame()
        {

        }

        public void OpenGame()
        {

        }

        private void TimerTick(object send, EventArgs e)
        {
            if (Duration > 0)
            {
                Duration--;
                if (Duration > 9)
                    TimeLeft = string.Format("0{0}:{1}", Duration / 60, Duration % 60);
                else
                    TimeLeft = string.Format("0{0}:0{1}", Duration / 60, Duration % 60);
            } 
            else
            {
                Timer.Stop();
                Utils.UpdateStatistics(Username, false);
                MessageBox.Show("Time is up!You lose this round!");
                NewGame();
            }
        }

        public void StartClock()
        {
            Duration = 60;
            Timer = new DispatcherTimer(DispatcherPriority.Send) {Interval = new TimeSpan(0, 0, 1)};
            Timer.Tick += TimerTick;
            Timer.Start();
        }

        public void OnClose()
        {
            MessageBox.Show("You are trying to close this!");
            SaveGame();
        }
    }
}
