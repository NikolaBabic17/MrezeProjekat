using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Klase;

namespace Client
{
    internal class NewMain
    {
        public void Run()
        {
            //=============//
            // UDP Prijava //
            //=============//
            Console.Write("Unesite IP adresu servera: ");
            string ipString = Console.ReadLine();

            Console.Write("Unesite UDP port servera: ");
            int udpPort = int.Parse(Console.ReadLine());

            IPAddress serverIP = IPAddress.Parse(ipString);
            IPEndPoint serverUdpEP = new IPEndPoint(serverIP, udpPort);

            Socket udpSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );

            byte[] prijava = Encoding.UTF8.GetBytes("PRIJAVA");
            udpSocket.SendTo(prijava, serverUdpEP);

            Console.WriteLine("Poslata PRIJAVA serveru.");

            //==========================//
            // Primi informacije o TCP  //
            //==========================//
            byte[] buffer = new byte[1024];
            EndPoint serverEP = new IPEndPoint(IPAddress.Any, 0);

            // TODO: Ako server nije zapocet, ovde ce crashovati client-a
            int bytes = udpSocket.ReceiveFrom(buffer, ref serverEP);
            
            string tcpInfo = Encoding.UTF8.GetString(buffer, 0, bytes);

            // NOTE: Ocekivani format: "TCP 127.0.0.1 16000"
            string[] delovi = tcpInfo.Split(' ');
            string tcpIP = delovi[1];
            int serverPort = int.Parse(delovi[2]);

            Console.WriteLine($"Primljeni TCP podaci: {serverIP}:{serverPort}");

            //================//
            // TCP konekcija  //
            //================//
            Socket tcpSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            tcpSocket.Connect(
                new IPEndPoint(IPAddress.Parse(tcpIP), serverPort)
            );

            Console.WriteLine("TCP konekcija uspostavljena.");

            //==============//
            // Primi karte  //
            //==============//
            List<Karta> mojeKarte = new List<Karta>();
            string bufferText = "";

            int ocekivaniBrojKarata = 6;

            while (mojeKarte.Count < ocekivaniBrojKarata)
            {
                byte[] porukaBuffer = new byte[1024];
                int porukaBytes;

                porukaBytes = tcpSocket.Receive(porukaBuffer);

                if (porukaBytes == 0)
                    break;

                bufferText += Encoding.UTF8.GetString(porukaBuffer, 0, porukaBytes);

                while (bufferText.Contains("\n"))
                {
                    int index = bufferText.IndexOf("\n");
                    string linija = bufferText.Substring(0, index);
                    bufferText = bufferText.Substring(index + 1);

                    // NOTE: Format: Naziv|Boja|Efekat
                    string[] p = linija.Split('|');

                    if (p.Length != 3)
                        continue;

                    Karta karta = new Karta(
                        p[0],
                        p[2],
                        (BojaKarte)Enum.Parse(typeof(BojaKarte), p[1])
                    );

                    mojeKarte.Add(karta);
                }
            }

            // NOTE: Ispis karata
            Console.WriteLine("\nDodeljene karte:");

            int broj = 1;
            foreach (Karta k in mojeKarte)
            {
                Console.WriteLine(
                    $"{broj++}. {k.Naziv} ({k.Boja}) - {k.Efekat}"
                );
            }

            Console.WriteLine("\nKlijent spreman za igru.");
            Console.ReadKey();

            tcpSocket.Close();
            udpSocket.Close();
        }
    }
}
