using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DiscordRpcLauncher
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon = null;

        private bool IsHide = false;

        private bool ProgramClose = false;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Discord RPC Launcher";

            notifyIcon = new NotifyIcon();
            notifyIcon.DoubleClick += new EventHandler((sender, e) =>
            {
                if(IsHide)
                {
                    this.Show();
                    IsHide = false;
                }
            });
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + "\\discordIcon.ico");
            notifyIcon.BalloonTipText = "Discord RPC Application";
            notifyIcon.Text = "Discord RPC Application";
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] {
                new System.Windows.Forms.MenuItem("Open", new EventHandler((sender, e) => {
                    if(IsHide)
                    {
                        this.Show();
                        IsHide = false;
                    }
                })),
                new System.Windows.Forms.MenuItem("Close", new EventHandler((sender, e) => {
                    ProgramClose = true;
                    this.Close();
                }))
            });

            string[] configs = new string[4];

            if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\\config.txt"))
            {
                File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\config.txt", new string[] { "CLIENT_ID=", "DETAILS=", "STATE=", "IMAGE_SOURCE=" });
            }

            configs = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\config.txt").ToList().ConvertAll(x => x.Split('=')[1]).ToArray();

            if (configs.Length == 4)
            {
                txtBoxClientID.Text = configs[0];
                txtBoxDetails.Text = configs[1];
                txtBoxState.Text = configs[2];
                if(configs[3] != "")
                {
                    ImageBrush img = new ImageBrush();
                    img.ImageSource = new BitmapImage(new Uri(configs[3]));
                    ImageBorder.Background = img;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(ProgramClose)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }
            else
            {
                this.Hide();
                IsHide = true;
                notifyIcon.ShowBalloonTip(1000, "Discord RPC", "Program is not exit. Still running background.", ToolTipIcon.None);
                e.Cancel = true;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private string imgPath = "";

        private void ImageBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "image files(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            fd.InitialDirectory = "C:\\";

            if(fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imgPath = fd.FileName;
                ImageBrush img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(fd.FileName));
                ImageBorder.Background = img;

                ImageGrid.Visibility  = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\config.txt", new string[] { "CLIENT_ID=" + txtBoxClientID.Text, "DETAILS=" + txtBoxDetails, "STATE=" + txtBoxState, "IMAGE_SOURCE=" + imgPath });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
