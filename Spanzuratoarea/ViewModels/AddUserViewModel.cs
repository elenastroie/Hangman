using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Spanzuratoarea.Models;

namespace Spanzuratoarea.ViewModels
{
    public class AddUserViewModel : Screen
    {
        private string _username;
        private string _errorMessage;

        public string ProfilePicture { get; set; }

        public BitmapFrame WindowIcon { get; set; }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                if (value.Contains(" "))
                {
                    ErrorMessage = "Username must be only one word!";
                }
                else
                {
                    ErrorMessage = "";
                }
            }
        }
        
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public AddUserViewModel()
        {
            WindowIcon = Utils.GetWindowIcon();
            ProfilePicture = "1.png";
        }

        public void AddPicture()
        {
            ProfilePicture = Utils.SelectProfilePictureDialog();
        }

        public bool CanSaveUser(string username, string errorMessage)
        {
            if (!String.IsNullOrEmpty(username) && String.IsNullOrEmpty(ErrorMessage))
                return true;
            return false;
        }

        public void SaveUser(string username, string errorMessage)
        {
            Utils.SaveUser(username,ProfilePicture);
            TryClose();
        }
    }
}
