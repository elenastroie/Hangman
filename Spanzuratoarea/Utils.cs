using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Spanzuratoarea.Models;
using Spanzuratoarea.ViewModels;

namespace Spanzuratoarea
{
    public class Utils
    {
        public static List<UserModel> GetUsers()
        {
            string user;
            List<UserModel> users = new List<UserModel>();
            var path = GetProjectPath() + "/GameData/UsersInfo/usersBasicInfo.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((user = file.ReadLine()) != null)
            {
                string[] userData = user.Split(' ');
                var username = userData[0];
                var profilePicture = userData[1];
                users.Add(new UserModel(username, profilePicture));
            }

            file.Close();

            return users;
        }

        public static string GetProjectPath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        }

        public static string GetProfilePicturePath()
        {
            return GetProjectPath() + "\\Resources\\ProfilePictures\\";
        }

        public static BitmapFrame GetWindowIcon()
        {
            Uri iconUri = new Uri("pack://application:,,,/Resources/Icons/2.png", UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(iconUri);
        }

        public static string GetUsersInfoPath()
        {
            return Utils.GetProjectPath() + "\\GameData\\UsersInfo\\usersBasicInfo.txt";
        }

        public static string GetCurrentGameInfoPath()
        {
            return Utils.GetProjectPath() + "\\GameData\\GameInfo\\current_game.txt";
        }

        public static string GetSavedGamesPath()
        {
            return Utils.GetProjectPath() + "\\GameData\\GameInfo\\saved_games.txt";
        }

        public static string GetStatisticGamePath()
        {
            return Utils.GetProjectPath() + "\\GameData\\GameInfo\\statistics.txt";
        }

        public static string ChangeToNextPicture(string selectedPicture)
        {
            var currentPicture = Path.GetFileNameWithoutExtension(selectedPicture);
            if (currentPicture != null &&
                short.Parse(currentPicture) < Directory.GetFiles(Utils.GetProfilePicturePath()).Length)
            {
                selectedPicture = Utils.GetProfilePicturePath() + (short.Parse(currentPicture) + 1) + ".png";
                if (!File.Exists(selectedPicture))
                {
                    selectedPicture = Utils.GetProfilePicturePath() + (short.Parse(currentPicture) + 1) + ".jpg";
                }
            }
            else
            {
                selectedPicture = Utils.GetProfilePicturePath() + "1.png";
            }

            return selectedPicture;
        }

        public static string ChangeToPrevPicture(string selectedPicture)
        {
            var currentPicture = Path.GetFileNameWithoutExtension(selectedPicture);
            if (currentPicture != null && short.Parse(currentPicture) > 1)
            {
                selectedPicture = Utils.GetProfilePicturePath() + (short.Parse(currentPicture) - 1) + ".png";
                if (!File.Exists(selectedPicture))
                {
                    selectedPicture = Utils.GetProfilePicturePath() + (short.Parse(currentPicture) - 1) + ".jpg";
                }
            }
            else
            {
                selectedPicture = Utils.GetProfilePicturePath() +
                                  Directory.GetFiles(Utils.GetProfilePicturePath()).Length + ".png";
                if (!File.Exists(selectedPicture))
                {
                    selectedPicture =
                        Utils.GetProfilePicturePath() + Directory.GetFiles(Utils.GetProfilePicturePath()).Length +
                        ".jpg";
                }
            }

            return selectedPicture;
        }

        public static void OpenAddNewUserWindow()
        {
            var windowManager = new WindowManager();
            var result = windowManager.ShowDialog(new AddUserViewModel());
        }

        public static string SelectProfilePictureDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Picture";
            dlg.DefaultExt = ".png";
            dlg.Filter = "Png Image (.png)|*.png|Jpg Image (.jpg)|*.jpg";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string profilePicture = "";
                var selectedFileNamePath = dlg.FileName;
                var extentionFile = selectedFileNamePath.Split('.').Last();
                var destinationPath = Utils.GetProfilePicturePath();
                var totalPictures = Directory.GetFiles(destinationPath).Length;
                if (!string.Equals(Path.GetDirectoryName(selectedFileNamePath) + "\\", destinationPath,
                    StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(selectedFileNamePath, destinationPath + (totalPictures + 1) + "." + extentionFile);
                    profilePicture = (totalPictures + 1) + "." + extentionFile;
                }

                profilePicture = Path.GetFileName(selectedFileNamePath);
                return profilePicture;
            }

            return "1.png";
        }

        public static void SaveUser(string username, string profilePicture)
        {
            var destinationPath = GetUsersInfoPath();
            using (StreamWriter sw = File.AppendText(destinationPath))
            {
                var userData = username + " " + profilePicture;
                sw.WriteLine(userData);
            }
        }

        public static void OpenGameWindow()
        {
            var windowManager = new WindowManager();
            var result = windowManager.ShowDialog(new GameViewModel());
        }

        public static void DeleteUser(string username)
        {
            var pathStatistiscs = GetStatisticGamePath();
            if (File.Exists(pathStatistiscs))
            {
                File.WriteAllLines(pathStatistiscs,
                    File.ReadLines(pathStatistiscs).Where(l => !l.Contains(username)).ToList());
            }
            var path = Utils.GetUsersInfoPath();
            List<string> lines = new List<string>(System.IO.File.ReadAllLines(path));
            lines.RemoveAll(u => u.Contains(username));
            System.IO.File.WriteAllLines(path, lines);
        }

        public static void StartGame(string username, string profilePicture)
        {
            var gameInfoPath = Utils.GetCurrentGameInfoPath();
            if (File.Exists(gameInfoPath))
            {
                using (StreamWriter sw = File.CreateText(gameInfoPath))
                {
                    sw.WriteLine(username + " " + profilePicture.Split('/').Last());
                }
            }

            Utils.OpenGameWindow();
        }

        public static string GetUserCurrentGame()
        {
            var path = GetProjectPath() + "/GameData/GameInfo/current_game.txt";
            var username = "";
            if (File.Exists(path))
            {
                var firstLine = File.ReadLines(path).FirstOrDefault();
                if (firstLine != null) username = firstLine.Split(' ')[0];
            }

            return username;
        }

        public static string GetUserPhotoCurrentGame()
        {
            var path = GetProjectPath() + "/GameData/GameInfo/current_game.txt";
            var profilePicture = "";
            if (File.Exists(path))
            {
                var firstLine = File.ReadLines(path).FirstOrDefault();
                if (firstLine != null) profilePicture = firstLine.Split(' ')[1];
            }

            return profilePicture;
        }

        public static string ChooseWord(string category)
        {
            string line;
            List<String> words = new List<string>();
            System.IO.StreamReader file =
                new System.IO.StreamReader(Utils.GetProjectPath() + "/GameData/GameCategories/all.txt");
            while ((line = file.ReadLine()) != null)
            {
                words.Add(line);
            }

            file.Close();
            var random = new Random();
            int index = random.Next(words.Count);
            return words[index];
        }

        public static void UpdateStatistics(string username, bool isWin)
        {
            var path = GetStatisticGamePath();

            string line;
            var userExists = false;
            string[] userData = null;
            var fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.Close();
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(username))
                    {
                        userExists = true;
                        userData = line.Split(' ');
                        userData[1] = (short.Parse(userData[1]) + 1).ToString();
                        if (isWin)
                        {
                            userData[2] = (short.Parse(userData[2]) + 1).ToString();
                        }
                        else
                        {
                            userData[3] = (short.Parse(userData[3]) + 1).ToString();
                        }
                        break;
                    }
                }

                if (userExists == false)
                {
                    if (isWin)
                    {
                        userData = new string[] { username, "1", "1", "0" };
                    }
                    else
                    {
                        userData = new string[] { username, "1", "0", "1" };
                    }
                }
            }
            if (userExists)
            {
                File.WriteAllLines(path,
                    File.ReadLines(path).Where(l => !l.Contains(username)).ToList());
            }
            using (StreamWriter sw = new StreamWriter(path,append:true))
            {
                foreach (var data in userData)
                {
                    sw.Write(data + " ");
                }
                sw.Write('\n');
            }

        }

        public static List<UserModel> GetStats()
        {
            string user;
            List<UserModel> users = new List<UserModel>();
            var path = GetStatisticGamePath();
            if (File.Exists(path))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                while ((user = file.ReadLine()) != null)
                {
                    string[] userData = user.Split(' ');
                    var username = userData[0];
                    var allGames = short.Parse(userData[1]);
                    var winGames = short.Parse(userData[2]);
                    var lostGames = short.Parse(userData[3]);
                    users.Add(new UserModel(username, allGames,winGames,lostGames));
                }

                file.Close();
            }

            return users;
        }

        public static void ShowStats()
        {
            var windowManager = new WindowManager();
            var result = windowManager.ShowDialog(new StatsViewModel());
        }
    }
}
