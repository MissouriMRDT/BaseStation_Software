namespace RED.Addons
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Speech.Recognition;
    using System.Threading;
    using System.Windows;
    using Properties;
    using ViewModels;
    using ViewModels.ControlCenter;
    using ViewModels.Modules;

    public class LexiconEngine
    {
        private ControlCenterVM ControlCenter { get; set; }
        public bool HazelIsReady
        {
            get { return  ControlCenterVM.StateVM.HazelIsReady; }
            set { ControlCenterVM.StateVM.HazelIsReady = value; }
        }
        private readonly SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        #region Dictionary
        private static readonly IDictionary<LexiconToken, List<string>> language = new Dictionary<LexiconToken, List<string>>()
        {
            {LexiconToken.Hazel, new List<string>()
            {
                "hazel"
            }},
            {LexiconToken.Cancel, new List<string>()
            {
                "nevermind", 
                "never mind", 
                "nothing", 
                "sorry", 
                "stop", 
                "cancel"
            }},
            {LexiconToken.ThankYou, new List<string>()
            {
                "thank you", 
                "thank you hazel"
            }},
            {LexiconToken.Chores, new List<string>()
            {
                "chores"
            }},
            {LexiconToken.Shutdown, new List<string>()
            {
                "shut down",
                "shutdown"
            }},
            {LexiconToken.Status, new List<string>()
            {
                "what is status", 
                "what is the status", 
                "what is the status of the rover"
            }},
            {LexiconToken.OpenNetwork, new List<string>()
            {
                "open network"
            }},
            {LexiconToken.CloseNetwork, new List<string>()
            {
                "close network"
            }},
            {LexiconToken.NextControlMode, new List<string>()
            {
                "next control mode",
            }},
            {LexiconToken.MoveAuxiliary, new List<string>()
            {
                "Auxiliary to the Left",
                "Auxiliary to the Right",
                "Auxiliary to the Top",
                "Auxiliary to the Middle",
                "Auxiliary to the Bottom",
            }},
            {LexiconToken.MoveBattery, new List<string>()
            {
                "Battery Management to the Left",
                "Battery Management to the Right",
                "Battery Management to the Top",
                "Battery Management to the Middle",
                "Battery Management to the Bottom",
            }},
            {LexiconToken.MoveGPS, new List<string>()
            {
                "GPS to the Left",
                "GPS to the Right",
                "GPS to the Top",
                "GPS to the Middle",
                "GPS to the Bottom",
            }},
            {LexiconToken.MoveController, new List<string>()
            {
                "Controller Input to the Left",
                "Controller Input to the Right",
                "Controller Input to the Top",
                "Controller Input to the Middle",
                "Controller Input to the Bottom",
            }},
            {LexiconToken.MoveMotor, new List<string>()
            {
                "Main Motor Drive to the Left",
                "Main Motor Drive to the Right",
                "Main Motor Drive to the Top",
                "Main Motor Drive to the Middle",
                "Main Motor Drive to the Bottom",
            }},
            {LexiconToken.MoveMotherboard, new List<string>()
            {
                "Motherboard to the Left",
                "Motherboard to the Right",
                "Motherboard to the Top",
                "Motherboard to the Middle",
                "Motherboard to the Bottom",
            }},
            {LexiconToken.MovePowerboard, new List<string>()
            {
                "Powerboard to the Left",
                "Powerboard to the Right",
                "Powerboard to the Top",
                "Powerboard to the Middle",
                "Powerboard to the Bottom",
            }},
            {LexiconToken.MoveArm, new List<string>()
            {
                "Robotic Arm to the Left",
                "Robotic Arm to the Right",
                "Robotic Arm to the Top",
                "Robotic Arm to the Middle",
                "Robotic Arm to the Bottom",
            }},
            {LexiconToken.MoveNetworking, new List<string>()
            {
                "Networking Control to the Left",
                "Networking Control to the Right",
                "Networking Control to the Top",
                "Networking Control to the Middle",
                "Networking Control to the Bottom",
            }},
            {LexiconToken.MoveCamera1, new List<string>()
            {
                "Camera 1 to the Left",
                "Camera 1 to the Right",
                "Camera 1 to the Top",
                "Camera 1 to the Middle",
                "Camera 1 to the Bottom",
            }}
        };
        #endregion

        public LexiconEngine(ControlCenterVM controlCenter)
        {
            ControlCenter = controlCenter;

            // Request for recognizer update and load grammar. NOTE: Always call RequestRecognizerUpdate before making changes to the engine
            recognizer.RequestRecognizerUpdate();
            recognizer.LoadGrammar(GetGrammar());
            // Establish event handler for recognition of speech
            recognizer.SpeechRecognized += SpeechRecognized;
            // set the input of the speech recognizer to the default audio device
            recognizer.SetInputToDefaultAudioDevice();
            // Recognize speech asynchronous
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static Grammar GetGrammar()
        {
            var valueList = language.Values.ToList();
            var listOfWords = new List<string>();
            foreach (var list in valueList)
            {
                listOfWords.AddRange(list);
            }

            var choices = new Choices(listOfWords.ToArray());
            //var modules = new List<string>(ControlCenterVM.ManagedModules.Select(m => m.Title).ToList());
            //var finalList = new List<string>();
            //foreach (var module in modules)
            //{
            //    finalList.Add(module + " to the Left");
            //    finalList.Add(module + " to the Right");
            //    finalList.Add(module + " to the Top");
            //    finalList.Add(module + " to the Middle");
            //    finalList.Add(module + " to the Bottom");
            //}
            //choices.Add(finalList.ToArray());
            //foreach (var word in finalList)
            //{
            //    Console.WriteLine("\"" + word + "\",");
            //}
            var grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);
            
            return new Grammar(grammarBuilder);
        }
        private void ProcessLexicon(string word)
        {
            var token = language.FirstOrDefault(l => l.Value.Contains(word)).Key;

            switch (token)
            {
                case LexiconToken.Hazel:
                    {
                        HazelIsReady = true;
                        BaseVM.Hazel.EnqueueMessage("yes");
                    }
                    break;
                case LexiconToken.Cancel:
                    {
                        HazelIsReady = false;
                    }
                    break;
                case LexiconToken.OpenNetwork:
                    {
                        if (!HazelIsReady) return;
                        var networking = ControlCenter.GetModuleViewModel<NetworkingVM>();
                        if (networking.NetworkListenCommand.CanExecute(null))
                        {
                            networking.NetworkListenCommand.Execute(null);
                        }
                        HazelIsReady = false;
                    }
                    break;
                case LexiconToken.CloseNetwork:
                    {
                        if (!HazelIsReady) return;
                        var networking = ControlCenter.GetModuleViewModel<NetworkingVM>();
                        if (networking.NetworkDisconnectCommand.CanExecute(null))
                        {
                            networking.NetworkDisconnectCommand.Execute(null);
                        }
                        HazelIsReady = false;
                    }
                    break;
                case LexiconToken.NextControlMode:
                    {
                        if (!HazelIsReady) return;
                        ControlCenterVM.StateVM.NextControlMode();
                        HazelIsReady = false;
                    }
                    break;
                case LexiconToken.Status:
                    if (!HazelIsReady) return;
                    VocalizeStatus();
                    HazelIsReady = false;
                    break;
                case LexiconToken.Chores:
                    if (!HazelIsReady) return;
                    BaseVM.Hazel.EnqueueMessage("No way");
                    HazelIsReady = false;
                    break;
                case LexiconToken.ThankYou:
                    BaseVM.Hazel.EnqueueMessage("you are welcome");
                    break;
                case LexiconToken.Shutdown:
                    if (!HazelIsReady) return;
                    BaseVM.Hazel.EnqueueMessage("Goodbye");
                    Thread.Sleep(2500);
                    Application.Current.Shutdown();
                    break;
                case LexiconToken.MoveArm:
                    break;
                case LexiconToken.MoveAuxiliary:
                    break;
                case LexiconToken.MoveBattery:
                    break;
                case LexiconToken.MoveCamera1:
                    break;
                case LexiconToken.MoveController:
                    break;
                case LexiconToken.MoveGPS:
                    break;
                case LexiconToken.MoveMotherboard:
                    break;
                case LexiconToken.MoveMotor:
                    break;
                case LexiconToken.MoveNetworking:
                    break;
                case LexiconToken.MovePowerboard:
                    break;
                default:
                    {
                        if (!HazelIsReady) return;
                        if (word.Contains("to the Left"))
                        {
                            ControlCenter.LeftSelection = ControlCenterVM.ManagedModules.Single(m => m.Title == word.Substring(0, word.Length - " to the Left".Length)).Title;
                        }
                        else if (word.Contains("to the Right"))
                        {
                            ControlCenter.RightSelection = ControlCenterVM.ManagedModules.Single(m => m.Title == word.Substring(0, word.Length - " to the Right".Length)).Title;
                        }
                        else if (word.Contains("to the Top"))
                        {
                            ControlCenter.TopSelection = ControlCenterVM.ManagedModules.Single(m => m.Title == word.Substring(0, word.Length - " to the Top".Length)).Title;
                        }
                        else if (word.Contains("to the Middle"))
                        {
                            ControlCenter.MiddleSelection = ControlCenterVM.ManagedModules.Single(m => m.Title == word.Substring(0, word.Length - " to the Middle".Length)).Title;
                        }
                        else if (word.Contains("to the Bottom"))
                        {
                            ControlCenter.BottomSelection = ControlCenterVM.ManagedModules.Single(m => m.Title == word.Substring(0, word.Length - " to the Bottom".Length)).Title;
                        }
                        HazelIsReady = false;
                    }
                    break;
            }
        }
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (Settings.Default.VoiceCommandsMute)
            {
                HazelIsReady = false;
                return;
            }
            if (e.Result == null) return;
            ProcessLexicon(e.Result.Text);
        }
        private static void VocalizeStatus()
        {
            BaseVM.Hazel.EnqueueMessage("Everything is functioning normally");
        }
    }

    public enum LexiconToken
    {
        Hazel,
        Cancel,
        ThankYou,
        Chores,
        Shutdown,
        Status,
        OpenNetwork,
        CloseNetwork,
        NextControlMode,
        MoveAuxiliary,
        MoveBattery,
        MoveGPS,
        MoveController,
        MoveMotor,
        MoveMotherboard,
        MovePowerboard, 
        MoveArm,
        MoveNetworking,
        MoveCamera1
    }
}
