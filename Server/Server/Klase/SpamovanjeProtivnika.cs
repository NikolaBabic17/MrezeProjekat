using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    public class SpamovanjeProtivnika : ISpamovanjeProtivnika
    {
        void ISpamovanjeProtivnika.SpamujProtivnike(List<Traka> trake, List<Protivnik> protivnici, int brojPoteza)
        {
            Random rand = new Random();

            //Ako je broj protivnika manji od 1.5 puta (3 + brojPoteza) Izbaci sve protivnike
            //Crescendo Event
            if (protivnici.Count < 1.5 * (3 + brojPoteza))
            {
                foreach (var Protivnik in protivnici)
                {
                    int trakaIndex;
                    do
                    {
                        trakaIndex = rand.Next(trake.Count);
                    } while (trake[trakaIndex].BrojZidinaZamka <= 0);

                    if (Protivnik.Ime == "Prelazi u prethodnu traku")
                    {
                        int newIndex;
                        do
                        {
                            newIndex = rand.Next(trake.Count);
                        } while (newIndex == trakaIndex);

                        foreach (var protivnik in trake[trakaIndex].SumaZona)
                        {
                            trake[newIndex].SumaZona.Add(protivnik);
                            trake[trakaIndex].SumaZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].StrelacZona)
                        {
                            trake[newIndex].StrelacZona.Add(protivnik);
                            trake[trakaIndex].StrelacZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].VitezZona)
                        {
                            trake[newIndex].VitezZona.Add(protivnik);
                            trake[trakaIndex].VitezZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].MacevalacZona)
                        {
                            trake[newIndex].MacevalacZona.Add(protivnik);
                            trake[trakaIndex].MacevalacZona.Remove(protivnik);
                        }
                    }
                    else if (Protivnik.Ime == "Postavi jos 3 protivnika")
                    {
                        continue;
                    }
                    else if (Protivnik.Ime == "Plavi, Zeleni ili Crveni se pomeraju 1 polje unazad")
                    {
                        int bojaTrake = rand.Next(trake.Count / 2);
                        if (bojaTrake == 0)
                        {
                            //PLAVA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Plava)
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
                        }
                        else if (bojaTrake == 1)
                        {
                            //ZELENA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Zelena)
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
                        }
                        else
                        {
                            //CRVENA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Crvena)
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
                        }
                    }
                    else if (Protivnik.Ime == "Kuga")
                    {
                        continue;
                    }
                    else
                    {
                        trake[trakaIndex].SumaZona.Add(Protivnik);
                    }
                }
                protivnici.Clear();
            }
            else
            {
                //Zarad postepenog povecavanje tezine igre
                //Broj prot ivnika se konstantno povecava
                for (int i = 0; i < 3 + brojPoteza; i++)
                {
                    int trakaIndex = rand.Next(trake.Count);
                    do
                    {
                        trakaIndex = rand.Next(trake.Count);
                    } while (trake[trakaIndex].BrojZidinaZamka <= 0);

                    int protivnikIndex = rand.Next(protivnici.Count);

                    Protivnik izabraniProtivnik = protivnici[protivnikIndex];

                    if (izabraniProtivnik.Ime == "Prelazi u prethodnu traku")
                    {
                        int newIndex;
                        do
                        {
                            newIndex = rand.Next(trake.Count);
                        } while (newIndex == trakaIndex);

                        foreach (var protivnik in trake[trakaIndex].SumaZona)
                        {
                            trake[newIndex].SumaZona.Add(protivnik);
                            trake[trakaIndex].SumaZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].StrelacZona)
                        {
                            trake[newIndex].StrelacZona.Add(protivnik);
                            trake[trakaIndex].StrelacZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].VitezZona)
                        {
                            trake[newIndex].VitezZona.Add(protivnik);
                            trake[trakaIndex].VitezZona.Remove(protivnik);
                        }
                        foreach (var protivnik in trake[trakaIndex].MacevalacZona)
                        {
                            trake[newIndex].MacevalacZona.Add(protivnik);
                            trake[trakaIndex].MacevalacZona.Remove(protivnik);
                        }
                    }
                    else if (izabraniProtivnik.Ime == "Postavi jos 3 protivnika")
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int randomIndex;
                            do
                            {
                                randomIndex = rand.Next(protivnici.Count);
                            }
                            while (protivnici[randomIndex].Ime == "Prelazi u prethodnu traku" ||
                                   protivnici[randomIndex].Ime == "Plavi, Zeleni ili Crveni se pomeraju 1 polje unazad" ||
                                   protivnici[randomIndex].Ime == "Kuga");

                            trake[trakaIndex].SumaZona.Add(protivnici[randomIndex]);
                            protivnici.RemoveAt(randomIndex);
                        }
                    }
                    else if (izabraniProtivnik.Ime == "Plavi, Zeleni ili Crveni se pomeraju 1 polje unazad")
                    {
                        int bojaTrake = rand.Next(trake.Count / 2);
                        if (bojaTrake == 0)
                        {
                            //PLAVA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Plava)
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
                        }
                        else if (bojaTrake == 1)
                        {
                            //ZELENA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Zelena)
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
                        }
                        else
                        {
                            //CRVENA
                            foreach (Traka t in trake)
                            {
                                if (t.BojaTrake == Boja.Crvena)
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
                        }
                    }
                    else if (izabraniProtivnik.Ime == "Kuga")
                    {
                        continue;
                    }
                    else
                    {
                        trake[trakaIndex].SumaZona.Add(izabraniProtivnik);
                    }

                    protivnici.RemoveAt(protivnikIndex);
                }
            }
        }

        void ISpamovanjeProtivnika.SpecialniProtivnici(List<Traka> trake)
        {
            foreach (var traka in trake)
            {
                foreach (var protivnik in traka.SumaZona)
                {
                    if (protivnik.Ime == "Veliki kamen")
                    {
                        traka.SumaZona.Clear();
                        traka.SumaZona.Add(new Protivnik("Veliki kamen", 0));
                    }
                }
                foreach (var protivnik in traka.StrelacZona)
                {
                    if (protivnik.Ime == "Veliki kamen")
                    {
                        traka.StrelacZona.Clear();
                        traka.StrelacZona.Add(new Protivnik("Veliki kamen", 0));
                    }
                }
                foreach (var protivnik in traka.VitezZona)
                {
                    if (protivnik.Ime == "Veliki kamen")
                    {
                        traka.VitezZona.Clear();
                        traka.VitezZona.Add(new Protivnik("Veliki kamen", 0));
                    }
                }
                foreach (var protivnik in traka.MacevalacZona)
                {
                    if (protivnik.Ime == "Veliki kamen")
                    {
                        traka.MacevalacZona.Clear();
                        traka.MacevalacZona.Add(new Protivnik("Veliki kamen", 0));
                    }
                }
            }
        }
    }
}