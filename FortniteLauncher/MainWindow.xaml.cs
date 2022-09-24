using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

using System.Text.Json;
using RestSharp;

namespace FortniteLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Process.Start("https://discord.gg/fortnitedev");
            Process.Start("https://github.com/noteason/FortniteLauncher");
        }
        private async void AuthCode_Focus(object sender, RoutedEventArgs e)
        {
            if (AuthCode.Text == "Authorization Code") AuthCode.Text = "";
        }
        private async void AuthCode_UnFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthCode.Text)) AuthCode.Text = "Auth Code";
        }
        string exchange = "";
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (LoginButton.Content.ToString() == "Login")
            {
                LoggedInAs.Content = "Logging in...";
                var client = new RestClient("https://backend.boogiefn.dev/");
                var request = new RestRequest($"/exchange?code={AuthCode.Text}", Method.Get);
                exchange = client.Execute(request).Content;

                exchangecode.Content = exchange;
                if (exchange.ToString() == "[]")
                {
                    MessageBox.Show($@"The authorization code you supplied was invalid, please try again with a valid code!");
                    return;
                }



                LoggedInAs.Content = $"Successfully Logged in to FortniteLauncher! github.com/noteason/FortniteLauncher";

                LoginButton.Content = "Launch!";
                return;
            }
            if (LoginButton.Content.ToString() == "Launch!")
            {
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Please select the path you want to use!";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = "Desktop";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var folder = dlg.FileName;
                    var binaries = $@"{folder}\FortniteGame\Binaries\Win64";
                    if (File.Exists(binaries + "/FortniteClient-Win64-Shipping.exe") == false)
                    {
                        MessageBox.Show($@"The path selected does not have Fortnite installed correctly: {folder}\nPlease retry with a valid path");
                    }
                    if (File.Exists(binaries + "/FortniteClient-Win64-Shipping.exe") == true)
                    {
                        WebClient client = new WebClient();
                        string DllPath = folder + "\\FortniteGame\\Binaries\\Win64\\sslbypass.dll";
                        client.DownloadFile("PUTYOURDLLLINKHERE", DllPath);

                        Process.Start("https://discord.gg/fortnitedev");

                        string FN = System.IO.Path.Combine(folder, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");

                        string arguments = "-PutYourOwnArgs";

                        Process Fortnite = new Process
                        {
                            StartInfo = new ProcessStartInfo(FN, arguments)
                            {
                                UseShellExecute = false,
                                RedirectStandardOutput = false,
                                CreateNoWindow = true
                            }
                        };

                        Fortnite.Start();


                        LoginButton.Content = "Login";


                    }

                }
            }

        }

    }
}
