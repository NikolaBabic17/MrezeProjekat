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

        class ClientState
        {
            public Socket Socket;
            public string Buffer = "";
        }

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

        //===============================//
        // Slanje informacija klijentima //
        //===============================//
        void SendLine(Socket igrac, string line)
        {
            byte[] data = Encoding.UTF8.GetBytes(line + "\n");
            igrac.Send(data);
        }
        private void PosaljiKarte(Socket igrac, List<Karta> karte)
        {
            // KARTESTART
            // Vitez|Udara jednog protivnika u vitez zoni|Ljubicasta
            // KARTEEND
            SendLine(igrac, "KARTESTART");
            for (int i = 0; i < karte.Count; i++)
            {
                Karta k = karte[i];
                SendLine(igrac, $"{k.Naziv}|{k.Efekat}|{k.Boja}");
            }
            SendLine(igrac, "KARTEEND");
        }
        private void PosaljiMapu(Socket igrac, List<Traka> trake)
        {
            // MAPSTART
            // TRACK 0 Plava 2
            // STRELAC ENEMY Goblin 1
            // ENDTRACK
            // TRACK 1 Plava 2
            // VITEZ ENEMY Trol 3
            // ENDTRACK
            // MAPEND
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
        private void PosaljiUDPInfo(Socket igrac, List<EndPoint> igraci)
        {
            // UDPINFOSTART
            // 127.0.0.1 65145
            // UDPINFOEND
            SendLine(igrac, "UDPINFOSTART");

            foreach(EndPoint p in igraci)
            {
                IPEndPoint ip = p as IPEndPoint;
                SendLine(igrac, $"{ip.Address} {ip.Port}");
            }

            SendLine(igrac, "UDPINFOEND");
        }

        private void AktivirajKartu(List<Traka> trake, string[] komandaDelovi)
        {
            string imeKarte = komandaDelovi[1];
            int indeksTrake = int.Parse(komandaDelovi[4]);
            if (imeKarte == "Pojacanje zida")
            {
                trake[indeksTrake].BrojZidinaZamka += 1;
            }
            else
            {
                string imeZone = komandaDelovi[5];
                int indeksProtivnika = int.Parse(komandaDelovi[6]);
                if (imeKarte == "Vracanje nazad")
                {
                    if (imeZone == "Suma")
                    {
                        Protivnik p = trake[indeksTrake].SumaZona[indeksProtivnika];
                        trake[indeksTrake].SumaZona.RemoveAt(indeksProtivnika);
                        trake[indeksTrake].SumaZona.Add(p);
                    }
                    if (imeZone == "Strelac")
                    {
                        Protivnik p = trake[indeksTrake].StrelacZona[indeksProtivnika];
                        trake[indeksTrake].StrelacZona.RemoveAt(indeksProtivnika);
                        trake[indeksTrake].SumaZona.Add(p);
                    }
                    if (imeZone == "Vitez")
                    {
                        Protivnik p = trake[indeksTrake].VitezZona[indeksProtivnika];
                        trake[indeksTrake].VitezZona.RemoveAt(indeksProtivnika);
                        trake[indeksTrake].SumaZona.Add(p);
                    }
                    if (imeZone == "Macevalac")
                    {
                        Protivnik p = trake[indeksTrake].MacevalacZona[indeksProtivnika];
                        trake[indeksTrake].MacevalacZona.RemoveAt(indeksProtivnika);
                        trake[indeksTrake].SumaZona.Add(p);
                    }
                }
                else
                {
                    if (imeZone == "Suma")
                    {
                        trake[indeksTrake].SumaZona[indeksProtivnika].Poeni -= 1;
                        if (trake[indeksTrake].SumaZona[indeksProtivnika].Poeni <= 0)
                        {
                            trake[indeksTrake].SumaZona.RemoveAt(indeksProtivnika);
                        }
                    }
                    if (imeZone == "Strelac")
                    {
                        trake[indeksTrake].StrelacZona[indeksProtivnika].Poeni -= 1;
                        if (trake[indeksTrake].StrelacZona[indeksProtivnika].Poeni <= 0)
                        {
                            trake[indeksTrake].StrelacZona.RemoveAt(indeksProtivnika);
                        }
                    }
                    if (imeZone == "Vitez")
                    {
                        trake[indeksTrake].VitezZona[indeksProtivnika].Poeni -= 1;
                        if (trake[indeksTrake].VitezZona[indeksProtivnika].Poeni <= 0)
                        {
                            trake[indeksTrake].VitezZona.RemoveAt(indeksProtivnika);
                        }
                    }
                    if (imeZone == "Macevalac")
                    {
                        trake[indeksTrake].MacevalacZona[indeksProtivnika].Poeni -= 1;
                        if (trake[indeksTrake].MacevalacZona[indeksProtivnika].Poeni <= 0)
                        {
                            trake[indeksTrake].MacevalacZona.RemoveAt(indeksProtivnika);
                        }
                    }
                }
            }
        }

        private Dictionary<Socket, string> CekajPorukeOdSvih(List<ClientState> klijenti)
        {
            Dictionary<Socket, string> odgovori = new Dictionary<Socket, string>();

            while (odgovori.Count < klijenti.Count)
            {
                List<Socket> read = new List<Socket>();

                foreach (var k in klijenti)
                    if (!odgovori.ContainsKey(k.Socket))
                        read.Add(k.Socket);

                Socket.Select(read, null, null, 100000);

                foreach (Socket s in read)
                {
                    byte[] buf = new byte[1024];
                    int bytes = s.Receive(buf);

                    if (bytes <= 0)
                        continue;

                    string msg = Encoding.UTF8.GetString(buf, 0, bytes).Trim();

                    odgovori[s] = msg;
                }
            }

            return odgovori;
        }

        private void PomeriProtivnike(List<Traka> trake)
        {
            foreach (Traka t in trake)
            {
                t.BrojZidinaZamka -= t.MacevalacZona.Count();

                t.MacevalacZona.AddRange(t.VitezZona);
                t.VitezZona.Clear();

                t.VitezZona.AddRange(t.StrelacZona);
                t.StrelacZona.Clear();

                t.StrelacZona.AddRange(t.SumaZona);
                t.SumaZona.Clear();
            }
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
                    Console.WriteLine($"Jos je potrebno {brojIgraca - igraci.Count} igraca");
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

            ZapocniIgru.ZapocniIgru(brojIgraca, trake, protivnici);
            IscrtajTrake(trake);

            ILogikaSpila dodelaSpila = new LogikaSpila();
            ISpamovanjeProtivnika protivnikSpawner = new SpamovanjeProtivnika();
            
            foreach (Socket igrac in tcpIgraci)
            {
                PosaljiMapu(igrac, trake);
                PosaljiKarte(igrac, dodelaSpila.DodeliKarteIzSpila(karte, brojIgraca));
                PosaljiUDPInfo(igrac, igraci);
            }
            Console.WriteLine("Igra inicijalizovana.");


            List<ClientState> klijenti = new List<ClientState>();

            foreach (Socket s in tcpIgraci)
            {
                klijenti.Add(new ClientState { Socket = s });
            }

            //int trenutniPotez = 0;
            //===========//
            // Main loop //
            //===========//
            while (true)
            {
                //====================//
                // Izbacivanje karata //
                //====================//
                Console.WriteLine("=== DISCARD FAZA ===");

                foreach (var k in klijenti)
                    SendLine(k.Socket, "IZBACI");

                var discardOdgovori = CekajPorukeOdSvih(klijenti);

                foreach (var o in discardOdgovori)
                    Console.WriteLine($"Discard {o.Key.RemoteEndPoint}: {o.Value}");

                //====================//
                // Aktiviranje karata //
                //====================//
                Console.WriteLine("=== PLAY FAZA ===");

                foreach (var k in klijenti)
                    SendLine(k.Socket, "ODIGRAJ");

                var playOdgovori = CekajPorukeOdSvih(klijenti);

                foreach (var o in playOdgovori)
                {
                    string msg = o.Value;
                    string[] delovi = msg.Split('|');
                    if (delovi.Count() > 1)
                    {
                        if (delovi[0] == "AKTIVIRAM")
                            AktivirajKartu(trake, delovi);
                    }
                }

                //===========================//
                // Update-ovanje stanja mape //
                //===========================//

                PomeriProtivnike(trake);
                //protivnikSpawner.SpamovanjeProtivnika(trake, protivnici, trenutniPotez++);

                IscrtajTrake(trake);

                foreach (var k in klijenti)
                {
                    PosaljiMapu(k.Socket, trake);
                    PosaljiKarte(k.Socket, dodelaSpila.DodeliKarteIzSpila(karte, brojIgraca));
                }
            }

            //Console.ReadKey();
        }
    }
}
