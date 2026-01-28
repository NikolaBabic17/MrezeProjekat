using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Server.Klase;

namespace Client
{
    internal class NewMain
    {
        private void IspisiKartu(Karta k)
        {
            Console.WriteLine($"{k.Naziv} ({k.Boja}) - {k.Efekat}");
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
            List<Karta> mojeKarte = new List<Karta>();

            string bufferText = "";
            byte[] recvBuffer = new byte[1024];

            bool citamMapu = false;
            bool citamKarte = false;
            Traka trenutnaTraka = null;
            Karta trenutnaKarta = null;

            while (true)
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

                    //================//
                    // MAP PROTOKOL   //
                    //================//
                    if (linija == "MAPSTART")
                    {
                        citamMapu = true;
                        continue;
                    }

                    if (linija == "MAPEND")
                    {
                        citamMapu = false;
                        continue;
                    }

                    if(linija == "KARTESTART")
                    {
                        citamKarte = true;
                        continue;
                    }
                    if(linija == "KARTEEND")
                    {
                        citamKarte = false;
                        continue;
                    }

                    if (citamMapu)
                    {
                        string[] part = linija.Split(' ');

                        // TRACK Index Boja HP
                        if (part[0] == "TRACK")
                        {
                            // TRACK index boja hp
                            Boja boja = (Boja)Enum.Parse(typeof(Boja), part[2]);
                            int hp = int.Parse(part[3]);
                            int index = int.Parse(part[1]);

                            trenutnaTraka = new Traka(0, boja);
                            trenutnaTraka.BrojZidinaZamka = hp;

                            trake.Insert(index, trenutnaTraka);
                            continue;
                        }

                        if (part[0] == "ENDTRACK")
                        {
                            trenutnaTraka = null;
                            continue;
                        }

                        // ZONE ENEMY
                        // SUMA ENEMY Goblin 1
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

                            continue;
                        }
                    }

                    //================//
                    // KARTE          //
                    //================//
                    if (citamKarte)
                    {
                        // Naziv|Boja|Efekat
                        string[] part = linija.Split(' ');
                        if (part[0] == "NAZIV")
                        {
                            trenutnaKarta = new Karta(part[1], "", 0);
                        }
                        if (part[0] == "EFEKAT")
                        {
                            int spaceIndex = linija.IndexOf(' ');
                            string key = linija.Substring(spaceIndex + 1);
                            trenutnaKarta.Efekat = key;
                        }
                        if (part[0] == "BOJA")
                        {
                            trenutnaKarta.Boja = (BojaKarte)Enum.Parse(typeof(BojaKarte), part[1]);
                            mojeKarte.Add(trenutnaKarta);
                            IspisiKartu(trenutnaKarta);
                        }
                    }
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
