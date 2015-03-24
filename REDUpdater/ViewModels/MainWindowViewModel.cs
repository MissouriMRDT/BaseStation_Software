using REDUpdater.Models;
using Caliburn.Micro;
using Octokit;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace REDUpdater.ViewModels
{
    class MainWindowViewModel : PropertyChangedBase
    {
        private MainWindowModel Model;

        public ConsoleViewModel Console
        {
            get
            {
                return Model.console;
            }
            set
            {
                Model.console = value;
                NotifyOfPropertyChange(() => Model.console);
            }
        }

        public short percent
        {
            get
            {
                return Model.percent;
            }
            set
            {
                Model.percent = value;
                NotifyOfPropertyChange(() => Model.percent);
            }
        }

        public MainWindowViewModel()
        {
            Model = new MainWindowModel();
            Model.console = new ConsoleViewModel(this);
            Model.percent = 0;

            const string originalFolder = "Debug";//folder RED.exe is in WARNING!!! ALL FILES IN THIS DIRECTORY WILL BE DELETED
            string dir = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8);
            string running = System.AppDomain.CurrentDomain.FriendlyName;
            string zipPath, delDir;
            string[] zipList, delList;

            dir = dir.Substring(0, dir.Length - 1);
            while (dir[dir.Length - 1] != '/')
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            Task.Run(async () =>
            {
                var github = new GitHubClient(new ProductHeaderValue("name"));//Creates link to github

                var releases = await github.Release.GetAll("JakePickle", "GitHubUpdateTest");//gets all releases of RED   

                delDir = dir.Substring(0, dir.Length - 1);

                while (delDir[delDir.Length - 1] != '/')
                {
                    delDir = delDir.Substring(0, delDir.Length - 1);
                }

                delDir = delDir + originalFolder + "/";

                delList = Directory.GetFiles(delDir);

                foreach (string f in delList)
                {
                    File.Delete(f);
                }

                percent = 20;

                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://github.com/MST-MRDT/Rover-Engagement-Display/releases/download/" + releases[0].TagName + "/1.1.0.zip",
                    dir + releases[0].TagName + ".zip");

                percent = 50
                    ;
                zipPath = dir + releases[0].TagName + ".zip";

                ZipFile.ExtractToDirectory(zipPath, delDir);

                percent = 75;

                zipList = Directory.GetFiles(dir, "*.zip");

                foreach (string f in zipList)
                {
                    File.Delete(f);
                }

                percent = 95;

                Process updateTest = new Process();
                updateTest.StartInfo.FileName = delDir + "RED.exe";
                updateTest.Start();

                percent = 100;

                System.Environment.Exit(1);
            });
        }
    }
}
