using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Spanzuratoarea.Models;

namespace Spanzuratoarea.ViewModels
{
    public class StatsViewModel : Screen
    {
        private List<UserModel> _allUsers;

        public List<UserModel> AllUsers
        {
            get { return _allUsers; }
            set
            {
                _allUsers = value;
                NotifyOfPropertyChange(()=>AllUsers);
            }
        }

        public StatsViewModel()
        {
            AllUsers = Utils.GetStats();
        }

        public void OnClose()
        {
            TryClose();
        }
    }
}
