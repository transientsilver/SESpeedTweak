using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace SESpeedTweak
{
    /// <summary>
    /// Interaction logic for CubeBlocksSpeed.xaml
    /// </summary>
    public partial class CubeBlocksSpeed : UserControl
    {
        private BlockManger manager;

        public CubeBlocksSpeed()
        {
            InitializeComponent();
            initBlockManager();
        }

        private void initBlockManager()
        {
            var path = @"C:\Program Files (x86)\Steam\SteamApps\common\SpaceEngineers";
            var filepath = @"\Content\Data\CubeBlocks.sbc";
            var fullpath = path + filepath;

            if (!File.Exists(fullpath))
            {
                path = null;
                fullpath = null;
                RegistryKey rk = Registry.LocalMachine;
                path = rk.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 244850").GetValue("InstallLocation").ToString();
                fullpath = path + filepath;

                if (!File.Exists(fullpath))
                {
                    fullpath = null;

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.FileName = "CubeBlocks";
                    dlg.DefaultExt = ".sbc";
                    dlg.Filter = "SE File (.sbc)|*.sbc";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        fullpath = dlg.FileName;
                    }

                }
            }

            if (fullpath != null)
            {
                manager = new BlockManger(fullpath);
                blockDataGrid.ItemsSource = manager.blocks;
            }
            else
            {
                MessageBox.Show("Unable to located CubeBlocks file. Closing");                
            }
        }

        private void sliderMultiplier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            decimal currentval = Convert.ToDecimal(sliderMultiplier.Value);
            manager.resetBlocks();

            if (currentval > 1)
            {
                lblInfo.Content = String.Format("Block speed will be {0} times faster", Math.Abs(currentval));
                manager.increaseSpeedByMultiplier(currentval);
            }
            else if (currentval < -1)
            {
                lblInfo.Content = String.Format("Block speed will be {0} times slower", Math.Abs(currentval));
                manager.decreaseSpeedByMultiplier(currentval);
            }

            blockDataGrid.ItemsSource = manager.blocks;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource blockViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("blockViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // blockViewSource.Source = [generic data source]
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            manager.Save();
            MessageBox.Show("Saved.");
        }

    }
}
