using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerSiteTcpListener
{
    public class Program
    {

        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;

        public List<TcpClient> Clients { get; set; }


        static void Main(string[] args)
        {

            var ip = IPAddress.Parse("10.2.13.36");
            var port = 27001;

            var ep = new IPEndPoint(ip, port);

            listener = new TcpListener(ep);
            listener.Start();


            Console.WriteLine($"Listening on {listener.LocalEndpoint}");


            while (true)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine($"{client.Client.RemoteEndPoint}");

                Task.Run(() =>
                {
                    var reader = Task.Run(() =>
                    {
                        var stream = client.GetStream();
                        br = new BinaryReader(stream);


                        while (true)
                        {
                            var msg = br.ReadString();
                            Console.WriteLine($"Client : {client.Client.RemoteEndPoint} : {msg}");
                        }
                    });

                    var writer = Task.Run(() =>
                    {
                        var stream = client.GetStream();
                        bw = new BinaryWriter(stream);


                        while (true)
                        {
                            var msg = Console.ReadLine();
                            bw.Write(msg);
                        }

                    });

                });

            }




        }
    }
}
