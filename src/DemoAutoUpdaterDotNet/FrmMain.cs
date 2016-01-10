using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using AutoUpdaterDotNET;

namespace DemoAutoUpdaterDotNet
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.Text = string.Format("Demo AutoUpdater.NET Versi {0}", GetCurrentVersion());

            var url = "http://localhost/update/DemoAutoUpdaterDotNet.xml";
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;            
            AutoUpdater.Start(url);
        }          

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    var msg = "Update terbaru versi {0} sudah tersedia. Saat ini Anda sedang menggunakan Versi {1}\n\nApakah Anda ingin memperbarui aplikasi ini sekarang ?";

                    var installedVersion = string.Format("{0}.{1}.{2}", args.InstalledVersion.Major, args.InstalledVersion.Minor, args.InstalledVersion.Build);
                    var currentVersion = string.Format("{0}.{1}.{2}", args.CurrentVersion.Major, args.CurrentVersion.Minor, args.CurrentVersion.Build);

                    var dialogResult = MessageBox.Show(string.Format(msg, currentVersion, installedVersion), "Update Tersedia",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Information);

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            //You can use Download Update dialog used by AutoUpdater.NET to download the update.
                            AutoUpdater.DownloadUpdate();
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }                
            }            
        }

        private string GetCurrentVersion()
        {
            var fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            var version = fvi.ProductMajorPart + "." + fvi.ProductMinorPart + "." + fvi.ProductBuildPart;

            return version;
        }
    }
}
