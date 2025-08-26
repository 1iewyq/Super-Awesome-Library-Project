using System;
using System.ServiceModel;

class Program
{
    static void Main(string[] args)
    {
        var tcp = new NetTcpBinding();
        var host = new ServiceHost(typeof(BusinessServer));
        host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8200/BusinessService");
        host.Open();
        Console.WriteLine("Business Tier Online");
        Console.ReadLine();
        host.Close();
    }
}
