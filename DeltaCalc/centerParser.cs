using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace CsvParse
{
    struct MetkaNew
    {
        public int packet;
        public int from, to;
        public long time;
        public Vector3 pos;

        public MetkaNew(int _packet, int _from, int _to, long _time, Vector3 _pos)
        {
            packet = _packet;
            from = _from;
            to = _to;
            time = _time;
            pos = _pos;
        }

        public void Print()
        {
            if (from == to)
                Console.WriteLine("{0} {1}     -  {2}\t{3}", packet, from, time, pos);
            else
                Console.WriteLine("{0} {1}->{2}  -  {3}\t{4}", packet, from, to, time, pos);
        }
    }

    

    class CenterParser
    {
        int numberOfPacket;
        private List<MetkaNew> metki;
        private List<Tower3> towers;
        private CultureInfo culture;
        double[,] koef;
        double[] solKoef;

        public CenterParser(List<Tower3> _tower) {
            numberOfPacket = 1;
            towers = _tower;
            culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            metki = new List<MetkaNew>();
            koef = new double[towers.Count, towers.Count];
            solKoef = new double[towers.Count];
        }

        private Vector3 parseCoord(string[] str)
        {
            Vector3 ret = Vector3.Zero;
            float x = float.Parse(str[str.Length-3], culture);
            float y = float.Parse(str[str.Length-2], culture);
            float z = float.Parse(str[str.Length-1], culture);
            ret.X = x; ret.Y = y; ret.Z = z;
            return ret;
        }

        public void GetMetkiFromString(string[] str)
        {
            List<MetkaNew> ret = new List<MetkaNew>();
            int firstTower = 0;
            for (int i = 2; i < 14; i++)
                if (str[i].Length != 0)
                {
                    ret.Add(new MetkaNew(  numberOfPacket, 
                                        i,
                                        i,
                                        long.Parse(str[i]), 
                                        parseCoord(str)
                                        )
                           );
                    firstTower = i;
                    break;
                }
            for (int i = firstTower + 1; i < 14; i++)
                if (str[i].Length != 0)
                {
                    ret.Add(new MetkaNew(numberOfPacket, firstTower, i, long.Parse(str[i]), parseCoord(str)));
                }
            metki.AddRange(ret); numberOfPacket++;
        }

        public void PrintAll() {
            Console.WriteLine("Всего меток - {0}", metki.Count);
            foreach (var met in metki)
                met.Print();
        }

        public void CalcKoef()
        {
            MetkaNew parent = new MetkaNew();
            foreach (var metka in metki) {
                
                if (metka.from == metka.to)
                {
                    parent = metka;
                    continue;
                }
                else
                {
                    koef[metka.from - 2, metka.from - 2]++;
                    koef[metka.from -2, metka.to - 2]--;
                    //koef[metka.to - 2, metka.from - 2]--;
                    double c =      (   Vector3.Distance(parent.pos,towers[metka.from -2].position) - 
                                        Vector3.Distance(parent.pos, towers[metka.to - 2].position)
                                    ) / Other.LightSpeed;
                    solKoef[metka.from - 2] += metka.time - parent.time - c;
                }

            }



            //    int size = towers.Count + 2;
            //for (int i = 2; i < size; i++)
            //{
            //    //MetkaNew parent = new MetkaNew();
            //    foreach (var metka in metki)
            //    {
            //        if (metka.from == metka.to)
            //        {
            //            parent = metka;
            //            continue;
            //        }
            //        if (metka.from - 1 == i || metka.to - 1 == i)
            //        {
            //            koef[i, i]++;
            //            if (metka.from - 1 == i)
            //            {
            //                koef[i, metka.to - 1]--;
            //                //solKoef[i] -= metka.time - parent.time - distance[i, metka.to - 1];
            //            }
            //            else
            //            {
            //                koef[i, metka.from - 1]--;
            //                //solKoef[i] -= 0 - metka.time + parent.time + distance[i, metka.from - 1];
            //            }
            //            //Console.WriteLine("Find {0} {1} -> [{2}{3}]++ \t [{4}{5}]--", metka.from, metka.to
            //            //                                                             , i + 1, i + 1
            //            //                  , i, (metka.from == i) ? metka.to : metka.from);
            //        }
            //    }
            //    //Console.WriteLine();
            //}
        }

        public void printKoef()
        {
            //CalcKoef();
            int size = towers.Count;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    Console.Write("{0}\t", koef[i, j]);
                Console.WriteLine(solKoef[i]);
            }
        }
    }
}
