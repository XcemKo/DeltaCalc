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
        public double time;
        public Vector3 pos;

        public MetkaNew(int _packet, int _from, int _to, double _time, Vector3 _pos)
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
            //Console.Write("  {0}:{1}:{2}", x, y, z);
            return ret;
        }

        public void GetMetkiFromString(string[] str)
        {
            List<MetkaNew> ret = new List<MetkaNew>();
            int firstTower = 0;
            for (int i = 2; i < 14; i++)
                if (str[i].Length != 0)
                {
                    double num = double.Parse(str[i]);
                    num = num / Math.Pow(2, 38);
                    //Console.Write("{0}:{1} ",i, num);
                    ret.Add(new MetkaNew(  numberOfPacket, 
                                        i-2,
                                        i-2,
                                        num, 
                                        parseCoord(str)
                                        )
                           );
                    firstTower = i;
                    break;
                }
            for (int i = firstTower + 1; i < 14; i++)
                if (str[i].Length != 0)
                {
                    double num = double.Parse(str[i]);
                    num = num / Math.Pow(2, 38);
                    //Console.Write("{0} ", num);
                    ret.Add(new MetkaNew(numberOfPacket, firstTower-2, i-2, num, parseCoord(str)));
                }
            metki.AddRange(ret); numberOfPacket++;
            //Console.Write("  {0}:{1}:{2}", parseCoord(str).X, parseCoord(str).Y, parseCoord(str).Z);
           // Console.WriteLine();
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
                        Console.WriteLine("Find {0} {1} -> [{2}{3}]++ \t [{4}{5}]--", metka.from, metka.to
                                                                                     , i + 1, i + 1
                                          , i, (metka.from == i) ? metka.to : metka.from);
                    }
                }
                //Console.WriteLine();
            }
            ;
        }

        private void deleteRowColumn( int index) {
            int size = solKoef.Count();
            for (int i = 0; i < size; i++)
            {
                ;
                if (i < index)
                    solKoef[i] = solKoef[i];
                else if (i == index)
                    continue;
                else
                    solKoef[i - 1] = solKoef[i];
                for (int j = 0; j < size; j++)
                {
                    if (j < index)
                        if (i < index)
                            koef[i, j] = koef[i, j];
                        else
                            koef[i - 1, j] = koef[i, j];
                    else if (j == index)
                        ;
                    else
                    {
                        if (i < index)
                            koef[i, j - 1] = koef[i, j];
                        else
                            koef[i - 1, j - 1] = koef[i, j];
                    }

                }
                /*/(-2);*/
            }
            for (int i = 0; i < size; i++)
            {
                koef[size - 1, i] = 0;
                koef[i, size -1] = 0;
            }
            solKoef[size - 1] = 0;
        }

        public double[] Delta()
        {
            int size = towers.Count;
            double[] ret = new double[size];
            double[,] tmpKoef = new double[size - 1, size - 1];
            double[] solLow = new double[size - 1];
            int smallSize = size - 1;

            deleteRowColumn(index:6);
            
            //Console.WriteLine("===============");
            //printKoef(1);
            double smallDiag = koef[0,0];
            int index = 0;
            for (int i = 1; i < size - 1; i++)
                if (smallDiag > koef[i, i])
                {
                    index = i;
                    smallDiag = koef[i, i];
                }
            index = 0;
            deleteRowColumn(index);
            Console.WriteLine("index = {0}===============", index);
            printKoef(2);

            int info = 0; alglib.densesolverreport reporter;
            alglib.rmatrixsolve(koef, size-2, solKoef, out info, out reporter, out ret);

            if (index >= 6)
                index++;

            for (int i = 0; i < size-2; i++)
                solveDelta[i] = ret[i];

            for (int i = size-2; i > Math.Min(6, index); i--)
                solveDelta[i] = solveDelta[i-1];
            solveDelta[Math.Min(6, index)] = 0;
            for (int i = size - 1; i > Math.Max(6, index); i--)
                solveDelta[i] = solveDelta[i - 1];
            solveDelta[Math.Max(6, index)] = 0;
            Console.Write("ret = [ ");
            for (int i = 0; i < size; i++)
                if(solveDelta[i] != 0)
                    Console.Write("{0:.000_000_000}; ", solveDelta[i]);
                else
                    Console.Write("{0}; ",solveDelta[i]);
            Console.WriteLine("] info - {0}",info);
            
            return solveDelta;
        }

        public void printKoef(int minus = 0)
        {
            //CalcKoef();
            int size = towers.Count;
            for (int i = 0; i < size - minus; i++)
            {
                for (int j = 0; j < size - minus; j++)
                    Console.Write("{0}\t", koef[i, j]);
                Console.WriteLine("| {0}", solKoef[i]);
            }
        }
    }
}
