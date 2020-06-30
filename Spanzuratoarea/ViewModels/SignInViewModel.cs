using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Spanzuratoarea.Models;

namespace Spanzuratoarea.ViewModels
{
    public class SignInViewModel : Screen
    {   
        private BindableCollection<UserModel> _users;
        private string _selectedProfilePicture;
        private UserModel _selectedUser;

        public BindableCollection<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public string SelectedProfilePicture
        {
            get { return _selectedProfilePicture; }
            set
            {
                _selectedProfilePicture = value;
                NotifyOfPropertyChange(() => SelectedProfilePicture);
            }
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (value != null)
                {
                    SelectedProfilePicture = Utils.GetProfilePicturePath() + value.ProfilePicture;
                    NotifyOfPropertyChange(() => SelectedUser);
                }
                NotifyOfPropertyChange(() => CanDeleteUser);
                NotifyOfPropertyChange(() => CanStartGame);
            }
        }

        public BitmapFrame WindowIcon { get; set; }

        public bool CanDeleteUser
        {
            get { return SelectedUser != null; }
        }

        public bool CanStartGame
        {
            get { return SelectedUser != null; }
        }

        public SignInViewModel()
        {
            WindowIcon = Utils.GetWindowIcon();
            SelectedProfilePicture = Utils.GetProfilePicturePath() + "1.png";
            Users = new BindableCollection<UserModel>(Utils.GetUsers());
        }

        public void ChangeToNextPicture()
        {
            SelectedProfilePicture = Utils.ChangeToNextPicture(SelectedProfilePicture);
        }

        public void ChangeToPrevPicture()
        {
            SelectedProfilePicture = Utils.ChangeToPrevPicture(SelectedProfilePicture);
        }

        public void AddUser()
        {
            Utils.OpenAddNewUserWindow();
            Users = new BindableCollection<UserModel>(Utils.GetUsers());
        }

        public void DeleteUser()
        {
            Utils.DeleteUser(SelectedUser.Username);
            Users.Remove(SelectedUser);
        }

        public void StartGame()
        {
            Utils.StartGame(SelectedUser.Username, SelectedProfilePicture);
        }

        public void Highscore()
        {
            Utils.ShowStats();
        }

        public void ExitGame()
        {
            TryClose();
        }
    }
}
