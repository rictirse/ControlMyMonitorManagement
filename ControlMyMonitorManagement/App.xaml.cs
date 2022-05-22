using CMM.Library.Config;
using CMM.Library.Method;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CMM.Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static XConfig cfg { get; private set; }
        internal static CMMMgr CMMMgr { get; private set; }

        public App()
        {
            //cfg = new();
            //cfg.Load();
            CMMMgr = new CMMMgr();
            Startup += async (s, e) => await App_Startup(s, e);
        }

        private async Task App_Startup(object sender, StartupEventArgs e)
        {
            await CMMMgr.Init();
        }
    }
}
