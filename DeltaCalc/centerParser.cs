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
        double[] solveDelta;

        public CenterParser(List<Tower3> _tower) {
            numberOfPacket = 1;
            towers = _tower;
            culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            metki = new List<MetkaNew>();
            koef = new double[towers.Count, towers.Count];
            solKoef = new double[towers.Count];
            solveDelta = new double[towers.Count];
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
                                        i-2,
                                        i-2,
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
                    ret.Add(new MetkaNew(numberOfPacket, firstTower-2, i-2, long.Parse(str[i]), parseCoord(str)));
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
            //foreach (var metka in metki) {

            //    if (metka.from == metka.to)
            //    {
            //        parent = metka;
            //        continue;
            //    }
            //    else
            //    {
            //        koef[metka.from , metka.from ]++;
            //        koef[metka.from , metka.to]--;
            //        koef[metka.to , metka.from ]--;
            //        double c =      (   Vector3.Distance(parent.pos,towers[metka.from].position) - 
            //                            Vector3.Distance(parent.pos, towers[metka.to ].position)
            //                        ) / Other.LightSpeed;
            //        solKoef[metka.from ] += metka.time - parent.time - c;
            //        //solKoef[[metka.to] -= 0 - metka.time + parent.time + c; //????????
            //    }
            //}


            for (int i = 0; i < towers.Count; i++)
            {
                //MetkaNew parent = new MetkaNew();
                foreach (var metka in metki)
                {
                    if (metka.from == metka.to)
                    {
                        parent = metka;
                        continue;
                    }
                    if (metka.from == i || metka.to == i)
                    {
                        koef[i, i]++;
                        if (metka.from == i)
                        {
                            koef[i, metka.to]--;
                            double c = (Vector3.Distance(parent.pos, towers[metka.from].position) -
                                                        Vector3.Distance(parent.pos, towers[metka.to ].position)
                                                    ) / Other.LightSpeed;
                            solKoef[i] -= metka.time - parent.time - c;
                        }
                        else
                        {
                            koef[i, metka.from]--;
                            double c = (Vector3.Distance(parent.pos, towers[metka.from].position) -
                                                        Vector3.Distance(parent.pos, towers[metka.to].position)
                                                    ) / Other.LightSpeed;
                            solKoef[i] -= 0 - metka.time + parent.time + c;
                        }
                        //Console.WriteLine("Find {0} {1} -> [{2}{3}]++ \t [{4}{5}]--", metka.from, metka.to
                        //                                                             , i + 1, i + 1
                        //                  , i, (metka.from == i) ? metka.to : metka.from);
                    }
                }
                //Console.WriteLine();
            }
        }

        public double[] Delta()
        {
            int size = towers.Count;
            double[] ret = new double[size - 1];
            double[,] tmpKoef = new double[size - 1, size - 1];
            double[] solLow = new double[size - 1];
            int smallSize = size - 1;
            for (int i = 1; i < size; i++)
            {
                solLow[i - 1] = solKoef[i];
                for (int j = 1; j < size; j++)
                    tmpKoef[i - 1, j - 1] = koef[i, j];/*/(-2);*/
            }

            Console.WriteLine("===============");
            for (int i = 0; i < smallSize; i++)
            {
                for (int j = 0; j < smallSize; j++)
                    Console.Write("{0}\t", tmpKoef[i, j]);
                Console.WriteLine(solLow[i]);
            }
            int info = 0; alglib.densesolverreport reporter;
            alglib.rmatrixsolve(tmpKoef, smallSize, solLow, out info, out reporter, out ret);
            Console.Write("ret = [ ");
            for (int i = 0; i < size - 1; i++) Console.Write("{0:00.0000}\t", ret[i]);
            Console.WriteLine("] info - {0}",info);
            for (int i = 1; i < size; i++)
                solveDelta[i] = ret[i - 1];
            return ret;
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
