using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanzuratoarea.Models
{
    public class UserModel
    {
		private string _username;
        private string _profilePicture;

		public string Username
		{
			get { return _username; }
			set { _username = value; }
		}

		public string ProfilePicture
		{
			get { return _profilePicture; }
			set { _profilePicture = value; }
		}

        private int _allMatches;

        public int AllMatches
        {
            get { return _allMatches; }
            set { _allMatches = value; }
        }

        private int _winMatches;

        public int WinMatches
        {
            get { return _winMatches; }
            set { _winMatches = value; }
        }

        private int _loseMatches;

        public int LoseMatches
        {
            get { return _loseMatches; }
            set { _loseMatches = value; }
        }

		public UserModel()
        {
            Username = "";
            ProfilePicture = "";
            AllMatches = 0;
            WinMatches = 0;
            LoseMatches = 0;
        }

        public UserModel(string username, string profilePicture)
        {
            Username = username;
            ProfilePicture = profilePicture;
        }

        public UserModel(string userName, int allMatches, int winMatches, int loseMatches)
        {
            Username = userName;
            AllMatches = allMatches;
            WinMatches = winMatches;
            LoseMatches = loseMatches;
        }


    }
}
