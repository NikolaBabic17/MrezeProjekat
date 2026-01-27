using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    internal class KreirajMapu : INapraviMapu
    {
        public List<Traka> kreirajMapu(int brojIgraca)
        {
            List<Traka> trake = new List<Traka>();
            switch (brojIgraca)
            {
                case 1:
                    trake.Add(new Traka(1, Boja.Plava));
                    trake.Add(new Traka(1, Boja.Plava));
                    break;
                case 2:
                    trake.Add(new Traka(2, Boja.Plava));
                    trake.Add(new Traka(2, Boja.Plava));
                    trake.Add(new Traka(2, Boja.Zelena));
                    trake.Add(new Traka(2, Boja.Zelena));
                    break;
                case 3:
                    trake.Add(new Traka(3, Boja.Plava));
                    trake.Add(new Traka(3, Boja.Plava));
                    trake.Add(new Traka(3, Boja.Zelena));
                    trake.Add(new Traka(3, Boja.Zelena));
                    trake.Add(new Traka(3, Boja.Crvena));
                    trake.Add(new Traka(3, Boja.Crvena));
                    break;
                default:
                    throw new Exception();
            }
            return trake;
        }
    }
}
