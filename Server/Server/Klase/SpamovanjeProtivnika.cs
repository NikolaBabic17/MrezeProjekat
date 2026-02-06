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
        void ISpamovanjeProtivnika.SpamovanjeProtivnika(List<Traka> trake, List<Protivnik> protivnici, int brojPoteza)
        {
            Random rand = new Random();

            //Ako je broj protivnika manji od 1.5 puta (3 + brojPoteza) Izbaci sve protivnike
            //Crescendo Event
            if (protivnici.Count < 1.5 * (3 + brojPoteza))
            {
                foreach (var Protivnik in protivnici)
                {
                    int trakaIndex = rand.Next(trake.Count);
                    trake[trakaIndex].SumaZona.Add(Protivnik);
                }
                protivnici.Clear();
            }
            else
            {
                //Zarad postepenog povecavanje tezine igre
                //Broj protivnika se konstantno povecava
                for (int i = 0; i < 3 + brojPoteza; i++)
                {
                    int trakaIndex = rand.Next(trake.Count);
                    do
                    {
                        trakaIndex = rand.Next(trake.Count);
                    } while(trake[trakaIndex].BrojZidinaZamka <= 0);

                    int protivnikIndex = rand.Next(protivnici.Count);

                    Protivnik izabraniProtivnik = protivnici[protivnikIndex];
                    trake[trakaIndex].SumaZona.Add(izabraniProtivnik);
                    protivnici.RemoveAt(protivnikIndex);
                }
            }
        }
    }
}