using System;
using System.ServiceModel;

namespace DBServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome");
            var tcp = new NetTcpBinding();
            var host = new ServiceHost(typeof(DataServer));
            host.AddServiceEndpoint(typeof(DBInterface.DataServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
