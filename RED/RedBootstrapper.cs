namespace RED
{
    using Caliburn.Micro;
    using System.Windows;
    using ViewModels;
    using Octokit;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Net.NetworkInformation;

    public class RedBootstrapper : BootstrapperBase
    {
        public RedBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            string dir = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8);
            string running = System.AppDomain.CurrentDomain.FriendlyName;
            string binDir = dir.Substring(0, dir.Length - running.Length);
            int major = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            int minor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            int build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            Ping pingClass = new Ping();
            PingReply reply;

            dir = dir.Substring(0, dir.Length - 1);
            while (dir[dir.Length - 1] != '/')
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            reply = pingClass.Send("github.com");
            if (reply.Status == IPStatus.Success)
            {
                Task.Run(async () =>
                {
                    var github = new GitHubClient(new ProductHeaderValue("name"));//Creates link to github

                    var releases = await github.Release.GetAll("MST-MRDT", "Rover-Engagement-Display");//gets all releases

                    Console.WriteLine(releases[0].TagName);//Prints the tag of the latest release

                    string[] nums = releases[0].TagName.Split('.'); 

                    //Code to check if update is needed, needs to be commented for testing.
                    if (true)//(!(int.Parse(nums[0]) < major || int.Parse(nums[1]) < minor || int.Parse(nums[2]) < build))
                    {               
                        string fileName;

                        string tarDir = dir.Substring(0, dir.Length - 1);
                        while (tarDir[tarDir.Length - 1] != '/')
                        {
                            tarDir = tarDir.Substring(0, tarDir.Length - 1);
                        }

                        tarDir += "temp/";
                        string destFile;

                        if (!System.IO.Directory.Exists(tarDir))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(tarDir);
                        }

                        if (System.IO.Directory.Exists(dir))
                        {
                            string[] files = System.IO.Directory.GetFiles(dir);

                            foreach (string s in files)
                            {
                                fileName = System.IO.Path.GetFileName(s);
                                destFile = System.IO.Path.Combine(tarDir, fileName);
                                System.IO.File.Copy(s, destFile, true);
                            }
                        }

                        Process updater = new Process();
                        updater.StartInfo.FileName = tarDir + "REDUpdater.exe";
                        updater.Start();
                    }
                });
            }

            DisplayRootViewFor<ControlCenterViewModel>();
        }
    }
}
