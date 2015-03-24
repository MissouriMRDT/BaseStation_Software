namespace RED.ViewModels
{
    using Caliburn.Micro;
    using ControlCenter;
    using Models;
    using Octokit;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Net.NetworkInformation;

    public class ControlCenterViewModel : Screen
    {
        private readonly ControlCenterModel _model;

        public RemoveModuleStateViewModel RemoveModuleState
        {
            get
            {
                return _model._removeModuleState;
            }
            set
            {
                _model._removeModuleState = value;
                NotifyOfPropertyChange(() => RemoveModuleState);
            }
        }
        public SaveModuleStateViewModel SaveModuleState
        {
            get
            {
                return _model._saveModuleState;
            }
            set
            {
                _model._saveModuleState = value;
                NotifyOfPropertyChange(() => SaveModuleState);
            }
        }

        public StateViewModel StateManager
        {
            get
            {
                return _model._stateManager;
            }
            set
            {
                _model._stateManager = value;
                NotifyOfPropertyChange();
            }
        }
        public ConsoleViewModel Console
        {
            get
            {
                return _model._console;
            }
            set
            {
                _model._console = value;
                NotifyOfPropertyChange();
            }
        }
        public DataRouter DataRouter
        {
            get
            {
                return _model._dataRouter;
            }
            set
            {
                _model._dataRouter = value;
                NotifyOfPropertyChange(() => DataRouter);
            }
        }
        public MetadataManager MetadataManager
        {
            get
            {
                return _model._metadataManager;
            }
            set
            {
                _model._metadataManager = value;
                NotifyOfPropertyChange(() => MetadataManager);
            }
        }
        public AsyncTcpServerViewModel TcpAsyncServer
        {
            get
            {
                return _model._tcpAsyncServer;
            }
            set
            {
                _model._tcpAsyncServer = value;
                NotifyOfPropertyChange(() => TcpAsyncServer);
            }
        }
        public ModuleManagerViewModel ModuleManager
        {
            get
            {
                return _model._gridManager;
            }
            set
            {
                _model._gridManager = value;
                NotifyOfPropertyChange(() => ModuleManager);
            }
        }

        public ControlCenterViewModel()
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

                        System.Environment.Exit(0);
                    }
                });
            }

            base.DisplayName = "Rover Engagement Display";
            _model = new ControlCenterModel();
            StateManager = new StateViewModel(this);
            Console = new ConsoleViewModel();
            DataRouter = new DataRouter();
            MetadataManager = new MetadataManager();
            TcpAsyncServer = new AsyncTcpServerViewModel(11000, this);
            ModuleManager = new ModuleManagerViewModel(this);

            RemoveModuleState = new RemoveModuleStateViewModel(this);
            SaveModuleState = new SaveModuleStateViewModel(ModuleManager.ModuleGrid, this);

            ModuleManager.ReloadModuleButtonContexts();
        }
    }
}
