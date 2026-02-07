using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Server.Klase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Forms;

namespace Client
{
    internal class NewMain
    {
        //====================//
        // Ispisi informacija //
        //====================//
        private void IspisiKarte(List<Karta> karte)
        {
            Console.WriteLine("\n===== STANJE KARATA =====\n");

            for(int i = 0; i < karte.Count; i++) 
            {
                Console.WriteLine($"Karta{i}: {karte[i].Naziv} ({karte[i].Boja}) - {karte[i].Efekat}");
            }
        }
        private void IspisiTrake(List<Traka> trake)
        {
            Console.WriteLine("\n===== STANJE TRAKA =====\n");

            for (int i = 0; i < trake.Count; i++)
            {
                Console.WriteLine("-----------------------------------------------------------------------------------------------");
                Console.WriteLine($"Traka{i} - {trake[i].BojaTrake} - Hp {trake[i].BrojZidinaZamka}");
                Console.WriteLine("[Suma]                [Strelac]             [Vitez]               [Macevalac]           [Zamak]");
                foreach (var p in trake[i].SumaZona)
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
        private void IspisiUDPInfo(List<IPEndPoint> igraciUDP)
        {
            Console.WriteLine("\n===== UDP INFO =====\n");

            for (int i = 0; i < igraciUDP.Count; i++)
            {

                Console.WriteLine($"Igrac{i}: {igraciUDP[i].ToString()}");
            }
        }

        //========================//
        // Parsiranje informacija //
        //========================//
        private void ParsirajMapu(List<Traka> trake, ref Traka trenutnaTraka, string linija)
        {
            string[] part = linija.Split('|');

            if (part[0] == "TRACK")
            {
                Boja boja = (Boja)Enum.Parse(typeof(Boja), part[2]);
                int hp = int.Parse(part[3]);
                int index = int.Parse(part[1]);

                trenutnaTraka = new Traka(0, boja);
                trenutnaTraka.BrojZidinaZamka = hp;

                trake.Insert(index, trenutnaTraka);
                return;
            }

            if (part[0] == "ENDTRACK")
            {
                trenutnaTraka = null;
                return;
            }

            if (part[1] == "ENEMY" && trenutnaTraka != null)
            {
                string zona = part[0];
                string ime = part[2];
                int hp = int.Parse(part[3]);

                Protivnik p = new Protivnik(ime, hp);

                if (zona == "SUMA")
                    trenutnaTraka.SumaZona.Add(p);
                else if (zona == "STRELAC")
                    trenutnaTraka.StrelacZona.Add(p);
                else if (zona == "VITEZ")
                    trenutnaTraka.VitezZona.Add(p);
                else if (zona == "MACEVALAC")
                    trenutnaTraka.MacevalacZona.Add(p);

                return;
            }
        }
        private void ParsirajKarte(List<Karta> karte, string linija)
        {
            string[] part = linija.Split('|');
            Karta k = new Karta(part[0], part[1], (BojaKarte)Enum.Parse(typeof(BojaKarte), part[2]));
            karte.Add(k);
        }
        private void ParsirajUDPInfo(List<IPEndPoint> igraciUDP, string linija)
        {
            string[] part = linija.Split(' ');
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(part[0]), int.Parse(part[1]));
            igraciUDP.Add(ip);
        }

        //============================//
        // Proveri ispravnost komande //
        //============================//
        private bool ProveriProtivnikPostojanje(Traka t, string zona, int indeksProtivnika)
        {
            if (zona == "Suma")
            {
                if ((indeksProtivnika >= 0) && (indeksProtivnika < t.SumaZona.Count))
                {
                    return true;
                }
            }
            if (zona == "Strelac")
            {
                if ((indeksProtivnika >= 0) && (indeksProtivnika < t.StrelacZona.Count))
                {
                    return true;
                }
            }
            if (zona == "Vitez")
            {
                if ((indeksProtivnika >= 0) && (indeksProtivnika < t.VitezZona.Count))
                {
                    return true;
                }
            }
            if (zona == "Macevalac")
            {
                if ((indeksProtivnika >= 0) && (indeksProtivnika < t.MacevalacZona.Count))
                {
                    return true;
                }
            }

            return false;
        }
        private bool ProveriKartaSyntax(Karta k, Traka t, string[] komandaDelovi)
        {
            if (k.Naziv == "Pojacanje zida")
            {
                if (komandaDelovi.Count() == 3)
                {
                    return true;
                }
            }
            else
            {
                string imeZone = komandaDelovi[3];
                int indeksProtivnika = int.Parse(komandaDelovi[4]);
                if ((k.Naziv == "Strelac" && imeZone == "Strelac") ||
                   (k.Naziv == "Vitez" && imeZone == "Vitez") ||
                   (k.Naziv == "Macevalac" && imeZone == "Macevalac") ||
                   (k.Naziv == "Heroj") ||
                   (k.Naziv == "Varvarin") ||
                   (k.Naziv == "Vracanje nazad") ||
                   (k.Naziv == "Katran"))
                {
                    if (k.Boja == BojaKarte.Ljubicasta)
                    {
                        return ProveriProtivnikPostojanje(t, imeZone, indeksProtivnika);
                    }
                    // NOTE: Imaju isti redosled boja
                    //       tako da ce ovakva provera raditi
                    if ((int)k.Boja == (int)t.BojaTrake)
                    {
                        return ProveriProtivnikPostojanje(t, imeZone, indeksProtivnika);
                    }
                }
            }

            return false;
        }

        void SendLine(Socket igrac, string line)
        {
            byte[] data = Encoding.UTF8.GetBytes(line + "\n");
            igrac.Send(data);
        }

        void UdpSwapListener(Socket udpSocket, List<Karta> karte)
        {
            byte[] buf = new byte[1024];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                int bytes = udpSocket.ReceiveFrom(buf, ref remote);
                string msg = Encoding.UTF8.GetString(buf, 0, bytes);

                string[] p = msg.Split('|');

                if (p[0] == "SWAPREQ")
                {
                    Console.WriteLine("\nSwap request received!");

                    Karta primljena = new Karta(
                        p[1],
                        p[2],
                        (BojaKarte)Enum.Parse(typeof(BojaKarte), p[3])
                    );

                    Console.WriteLine($"They offer: {primljena.Naziv}");

                    if (karte.Count == 0)
                    {
                        SendUdp(udpSocket, "SWAPNO", remote);
                        continue;
                    }

                    Console.WriteLine("Choose card index to return (-1 reject):");
                    int izbor = int.Parse(Console.ReadLine());

                    if (izbor < 0 || izbor >= karte.Count)
                    {
                        SendUdp(udpSocket, "SWAPNO", remote);
                        continue;
                    }

                    Karta moja = karte[izbor];

                    string resp =
                        $"SWAPOK|{moja.Naziv}|{moja.Efekat}|{moja.Boja}";

                    SendUdp(udpSocket, resp, remote);

                    karte.RemoveAt(izbor);
                    karte.Add(primljena);
                }
            }
        }

        void SendUdp(Socket udp, string msg, EndPoint ep)
        {
            byte[] d = Encoding.UTF8.GetBytes(msg);
            udp.SendTo(d, ep);
        }


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

            //===========//
            // Main loop //
            //===========//
            List<Traka> trake = new List<Traka>(6);
            List<Karta> karte = new List<Karta>();
            List<IPEndPoint> igraciUDP = new List<IPEndPoint>();

            string bufferText = "";
            byte[] recvBuffer = new byte[1024];

            bool citamMapu = false;
            bool citamKarte = false;
            bool citamUDPInfo = false;
            Traka trenutnaTraka = null;

            //Task.Run(() => UdpSwapListener(udpSocket, karte));
            bool running = true;
            while (running)
            {
                int dataReceived = tcpSocket.Receive(recvBuffer);
                if (dataReceived == 0)
                    continue;

                bufferText += Encoding.UTF8.GetString(recvBuffer, 0, dataReceived);

                while (bufferText.Contains("\n"))
                {
                    int idx = bufferText.IndexOf("\n");
                    string linija = bufferText.Substring(0, idx).Trim();
                    bufferText = bufferText.Substring(idx + 1);

                    if (linija.Length == 0)
                        continue;

                    if (linija == "MAPSTART")
                    {
                        trake.Clear();
                        citamMapu = true;
                        continue;
                    }
                    if (linija == "MAPEND")
                    {
                        IspisiTrake(trake);
                        citamMapu = false;
                        continue;
                    }

                    if(linija == "KARTESTART")
                    {
                        karte.Clear();
                        citamKarte = true;
                        continue;
                    }
                    if(linija == "KARTEEND")
                    {
                        IspisiKarte(karte);
                        citamKarte = false;
                        continue;
                    }

                    if(linija == "UDPINFOSTART")
                    {
                        igraciUDP.Clear();
                        citamUDPInfo = true;
                        continue;
                    }
                    if(linija == "UDPINFOEND")
                    {
                        IspisiUDPInfo(igraciUDP);
                        citamUDPInfo = false;
                        continue;
                    }

                    if (linija == "IZBACI")
                    {
                        Console.WriteLine("\n===== IZBACI KARTU =====\n");
                        Console.WriteLine("Format:");
                        Console.WriteLine("IZBACUJEM indeks_karte");
                        Console.WriteLine("Ili samo: IZBACUJEM (ako ne izbacujes)");

                        bool valid = false;
                        string komanda = "";

                        while (!valid)
                        {
                            Console.Write("Komanda: ");
                            komanda = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(komanda))
                                continue;

                            string[] parts = komanda.Split(' ');

                            if (parts[0] != "IZBACUJEM")
                                continue;

                            if (parts.Length == 1)
                            {
                                valid = true;
                            }
                            else if (parts.Length == 2)
                            {
                                if (int.TryParse(parts[1], out int indeks))
                                {
                                    if (indeks >= 0 && indeks < karte.Count)
                                    {
                                        valid = true;
                                        karte.RemoveAt(indeks);
                                    }
                                }
                            }
                        }

                        IspisiKarte(karte);
                        SendLine(tcpSocket, komanda);
                        continue;
                    }

                    if (linija == "SWAP")
                    {
                        Console.WriteLine("=== SWAP FAZA ===");

                        Console.WriteLine("Swap? y/n");
                        string ans = Console.ReadLine();

                        if (ans.ToLower() == "y")
                        {
                            IspisiKarte(karte);
                            IspisiUDPInfo(igraciUDP);

                            Console.Write("Card index: ");
                            int ci = int.Parse(Console.ReadLine());

                            Console.Write("Player index: ");
                            int pi = int.Parse(Console.ReadLine());

                            if (ci >= 0 && ci < karte.Count &&
                                pi >= 0 && pi < igraciUDP.Count)
                            {
                                Karta k = karte[ci];
                                IPEndPoint target = igraciUDP[pi];

                                string req =
                                    $"SWAPREQ|{k.Naziv}|{k.Efekat}|{k.Boja}";

                                SendUdp(udpSocket, req, target);

                                byte[] buf = new byte[1024];
                                EndPoint r = new IPEndPoint(IPAddress.Any, 0);

                                int b = udpSocket.ReceiveFrom(buf, ref r);
                                string resp = Encoding.UTF8.GetString(buf, 0, b);

                                if (resp.StartsWith("SWAPOK"))
                                {
                                    string[] pr = resp.Split('|');

                                    Karta nova = new Karta(
                                        pr[1],
                                        pr[2],
                                        (BojaKarte)Enum.Parse(typeof(BojaKarte), pr[3])
                                    );

                                    karte.RemoveAt(ci);
                                    karte.Add(nova);

                                    Console.WriteLine("Swap success!");
                                }
                                else
                                {
                                    Console.WriteLine("Swap rejected");
                                }
                            }
                        }

                        SendLine(tcpSocket, "SWAPDONE");
                        continue;
                    }


                    if (linija == "ODIGRAJ")
                    {
                        Console.WriteLine("\n===== ODIGRAJ POTEZ =====\n");
                        Console.WriteLine("Format:");
                        Console.WriteLine("AKTIVIRAM indeks_karte indeks_trake ime_zone indeks_protivnika");
                        Console.WriteLine("Ili samo: AKTIVIRAM (ako ne aktiviras ni jednu kartu)");

                        bool valid = false;
                        string komanda = "";

                        while (!valid)
                        {
                            Console.Write("Komanda: ");
                            komanda = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(komanda))
                                continue;

                            string[] parts = komanda.Split(' ');

                            if (parts[0] != "AKTIVIRAM")
                                continue;

                            if (parts.Length == 1)
                            {
                                valid = true;
                                SendLine(tcpSocket, "AKTIVIRAM");
                            }
                            else
                            {
                                if (parts.Length < 3)
                                    continue;

                                if (!int.TryParse(parts[1], out int kartaIndeks))
                                    continue;

                                if (!int.TryParse(parts[2], out int trakaIndeks))
                                    continue;

                                if (kartaIndeks < 0 || kartaIndeks >= karte.Count)
                                    continue;

                                if (trakaIndeks < 0 || trakaIndeks >= trake.Count)
                                    continue;

                                Karta k = karte[kartaIndeks];
                                Traka t = trake[trakaIndeks];

                                valid = ProveriKartaSyntax(k, t, parts);

                                if (valid)
                                {
                                    string msg = $"AKTIVIRAM|{k.Naziv}|{k.Efekat}|{k.Boja}|{trakaIndeks}";

                                    if (parts.Length == 5)
                                    {
                                        msg += $"|{parts[3]}|{parts[4]}";
                                    }

                                    SendLine(tcpSocket, msg);
                                    karte.RemoveAt(kartaIndeks);
                                }
                            }
                        }

                        continue;
                    }

                    if (linija == "GAMEWIN")
                    {
                        Console.WriteLine("\n===== POBEDA =====");
                        running = false;
                        break;
                    }

                    if (linija == "GAMELOSE")
                    {
                        Console.WriteLine("\n===== PORAZ =====");
                        running = false;
                        break;
                    }


                    if (citamMapu)
                    {
                        ParsirajMapu(trake, ref trenutnaTraka, linija);
                    }
                    if (citamKarte)
                    {
                        ParsirajKarte(karte, linija);
                    }
                    if(citamUDPInfo)
                    {
                        ParsirajUDPInfo(igraciUDP, linija);
                    }
                }
            }
            
            tcpSocket.Close();
            udpSocket.Close();
        }
    }
}
