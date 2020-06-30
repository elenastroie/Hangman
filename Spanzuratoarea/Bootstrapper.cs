﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Spanzuratoarea.ViewModels;

namespace Spanzuratoarea
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
                Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<SignInViewModel>();
        }
    }
}