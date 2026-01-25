using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Klase;

namespace Server
{
    internal class NewMain
    {
        public void Run()
        {
            Console.Write("Unesite broj igraca (1-3):");
            int brojIgraca = int.Parse(Console.ReadLine());

            //============//
            // UDP Socket //
            //============//
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint udpEP = new IPEndPoint(IPAddress.Any, 15000);
            udpSocket.Bind(udpEP);

            List<EndPoint> igraci = new List<EndPoint>();
            byte[] buffer = new byte[1024];

            Console.WriteLine("Server ceka prijave...");

            while (igraci.Count < brojIgraca)
            {
                EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
                int bytes = udpSocket.ReceiveFrom(buffer, ref senderEP);
                string poruka = Encoding.UTF8.GetString(buffer, 0, bytes);

                if (poruka == "PRIJAVA")
                {
                    igraci.Add(senderEP);
                    Console.WriteLine($"Igrac prijavljen: {senderEP}");
                    Console.WriteLine($"Jos je potrebno {igraci.Count-1} igraca");
                }
            }

            //=================//
            // Kreiranje Traka //
            //=================//
            List<Traka> trake = new List<Traka>();
            int brojTraka = brojIgraca * 2;

            Boja[] boje = { Boja.Plava, Boja.Zelena, Boja.Crvena };

            for (int i = 0; i < brojTraka; i++)
            {
                Boja bojaTrake = boje[i / 2];
                Traka traka = new Traka(brojIgraca, bojaTrake);
                trake.Add(traka);

                Console.WriteLine($"Traka {i + 1} - {bojaTrake}");
            }

            //============//
            // TCP Socket //
            //============//
            Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint tcpEP = new IPEndPoint(IPAddress.Any, 16000);
            tcpListener.Bind(tcpEP);
            tcpListener.Listen(brojIgraca);

            // NOTE: Saljemo TCP podatke igracima
            string tcpInfo = $"TCP 127.0.0.1 16000";
            byte[] tcpInfoBytes = Encoding.UTF8.GetBytes(tcpInfo);

            foreach (EndPoint ep in igraci)
                udpSocket.SendTo(tcpInfoBytes, ep);

            // NOTE: Povezi TCP konekcije
            List<Socket> tcpIgraci = new List<Socket>();

            for (int i = 0; i < brojIgraca; i++)
            {
                Socket client = tcpListener.Accept();
                tcpIgraci.Add(client);
                Console.WriteLine($"TCP povezan: {client.RemoteEndPoint}");
            }

            // NOTE: Kreiranje i dodela karata
            Karta kartaHelper = new Karta("", "", BojaKarte.Plava);
            List<Karta> spil = kartaHelper.KreirajSpil(brojIgraca);

            int brojKarataPoIgracu = (brojIgraca == 1) ? 6 : 5;
            int indexSpila = 0;

            foreach (Socket igrac in tcpIgraci)
            {
                for (int i = 0; i < brojKarataPoIgracu; i++)
                {
                    Karta k = spil[indexSpila++];
                    string poruka = k.Naziv + "|" + k.Boja + "|" + k.Efekat + "\n";
                    byte[] data = Encoding.UTF8.GetBytes(poruka);
                    igrac.Send(data);   
                }
            }

            Console.WriteLine("Igra inicijalizovana.");
            Console.ReadKey();
        }
    }
}
