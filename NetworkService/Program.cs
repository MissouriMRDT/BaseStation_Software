using System;
using System.Threading;

namespace NetworkService
{
    public class NS_NetworkManager : MarshalByRefObject
    {
        public NS_NetworkManager()
        {
            RoverNetworkManager.App.Main();
        }
    }

    public class NS_RED : MarshalByRefObject
    {
        public NS_RED()
        {
            RED.App.Main();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing RED...");

            Thread nm = new Thread(new ThreadStart(StartNetworkManagerWrapper));
            nm.SetApartmentState(ApartmentState.STA);
            nm.Start();

            Thread red = new Thread(new ThreadStart(StartREDWrapper));
            red.SetApartmentState(ApartmentState.STA);
            red.Start();
        }

        static void StartNetworkManagerWrapper()
        {
            AppDomain.CreateDomain("nm").CreateInstanceAndUnwrap(typeof(NS_NetworkManager).Assembly.FullName, "NetworkService.NS_NetworkManager");
        }

        static void StartREDWrapper()
        {
            AppDomain.CreateDomain("red").CreateInstanceAndUnwrap(typeof(NS_RED).Assembly.FullName, "NetworkService.NS_RED");
        }
    }
}
