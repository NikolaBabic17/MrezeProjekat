using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Klase;
using Server.Interfejs;

namespace Server
{


    internal class NewMain
    {
        IKreirajProtivnike kreirajProtivnike;
        INapraviMapu napraviMapu;
        IZapocniIgru ZapocniIgru;

        private void IscrtajTrake(List<Traka> trake)
        {
            Console.WriteLine("\n===== STANJE TRAKA =====\n");

            for (int i = 0; i < trake.Count; i++)
            {
                Console.WriteLine($"Traka{i} - {trake[i].BojaTrake} - Hp {trake[i].BrojZidinaZamka}");
                Console.WriteLine("[Suma]                [Strelac]             [Vitez]               [Macevalac]           [Zamak]");
                foreach(var p in trake[i].SumaZona)
                {
                    Console.WriteLine($"{p.Ime} - Hp: {p.Poeni}");
                }
                foreach (var p in trake[i].StrelacZona)
                {
                    Console.WriteLine($"{"",22}{p.Ime} - Hp: {p.Poeni}");
                }
                foreach (var p in trake[i].VitezZona)
                {
                    Console.WriteLine($"{"",44}{p.Ime} - Hp: {p.Poeni}");
                }
                foreach (var p in trake[i].MacevalacZona)
                {
                    Console.WriteLine($"{"",66}{p.Ime} - Hp: {p.Poeni}");
                }

                Console.WriteLine();
            }
        }

        void SendLine(Socket igrac, string line)
        {
            byte[] data = Encoding.UTF8.GetBytes(line + "\n");
            igrac.Send(data);
        }

        // KARTESTART
        // NAZIV Vitez
        // EFAKAT Udara jednog protivnika u vitez zoni
        // BOJA Ljubicasta
        // KARTEEND
        private void PosaljiKarte(Socket igrac, List<Karta> karte)
        {
            SendLine(igrac, "KARTESTART");
            for (int i = 0; i < karte.Count; i++)
            {
                Karta k = karte[i];
                SendLine(igrac, $"NAZIV {k.Naziv}");
                SendLine(igrac, $"EFEKAT {k.Efekat}");
                SendLine(igrac, $"BOJA {k.Boja}");
            }
            SendLine(igrac, "KARTEEND");
        }

        // MAPSTART
        // TRACK 0 Plava 2
        // STRELAC ENEMY Goblin 1
        // ENDTRACK
        // TRACK 1 Plava 2
        // VITEZ ENEMY Trol 3
        // ENDTRACK
        // MAPEND
        private void PosaljiMapu(Socket igrac, List<Traka> trake)
        {
            SendLine(igrac, "MAPSTART");

            for (int i = 0; i < trake.Count; i++)
            {
                Traka t = trake[i];

                SendLine(igrac, $"TRACK {i} {t.BojaTrake} {t.BrojZidinaZamka}");

                foreach (Protivnik p in t.SumaZona) {
                    SendLine(igrac, $"SUMA ENEMY {p.Ime} {p.Poeni}");
                }
                foreach (Protivnik p in t.StrelacZona) {
                    SendLine(igrac, $"STRELAC ENEMY {p.Ime} {p.Poeni}");
                }
                foreach (Protivnik p in t.VitezZona) {
                    SendLine(igrac, $"VITEZ ENEMY {p.Ime} {p.Poeni}");
                }
                foreach (Protivnik p in t.MacevalacZona) {
                    SendLine(igrac, $"MACEVALAC ENEMY {p.Ime} {p.Poeni}");
                }

                SendLine(igrac, "ENDTRACK");
            }

            SendLine(igrac, "MAPEND");
        }

        public void Run()
        {
            kreirajProtivnike = new Protivnik("",0);
            napraviMapu = new KreirajMapu();
            ZapocniIgru = new ZapocniIgru(kreirajProtivnike);

            Console.Write("Unesite broj igraca (1-3): ");
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

            //===================//
            // Inicijalizuj igru //
            //===================//
            List<Karta> karte = new KreirajSpit().KreirajSpil(brojIgraca);
            List<Traka> trake = napraviMapu.kreirajMapu(brojIgraca);
            List<Protivnik> protivnici = kreirajProtivnike.KreirajProtivnike();

            // TODO: Lose rasporedjuje protivnike po trakama
            ZapocniIgru.ZapocniIgru(brojIgraca, trake, protivnici);
            IscrtajTrake(trake);

            foreach (Socket igrac in tcpIgraci)
            {
                PosaljiMapu(igrac, trake);
                PosaljiKarte(igrac, karte); // NOTE: Trenutno salje sve karte
            }

            Console.WriteLine("Igra inicijalizovana.");
            Console.ReadKey();
        }
    }
}
