using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Globalization;

namespace CsvParse
{
    class Program
    {
        public static int N = 6;

        static bool CheckLenght(string[] array)
        {
            int countLenght = 0;
            for (int i = 2; i < 14; i++)
                if (array[i].Length > 0)
                    countLenght++;
            return countLenght > 1 ? true : false;
        }

        static void Main(string[] args)
        {

            //double[,] a = new double[3, 3] { { 3, -2, 5 },
            //                                 { 7, 4, -8 }, 
            //                                 { 5, -3, -4 } };
            //double[] b = new double[3] { 7, 3, -12 };

            //double[] x = new double[3];
            //int info = 0; alglib.densesolverreport reporter;
            //alglib.rmatrixsolve(a, 3, b, out info, out reporter, out x);
            //Console.WriteLine("x1 = {0} x2 = {1} x3 = {2}", x[0], x[1], x[2]);
            /*      
                t1 - t2,t3,t5
                t2 - t1,t3,t6
                t3 - t1,t4,t5,t2
                t4 - t3,t5
                t5 - t1,t3,t4
                t6 - t2
             */
            Console.WriteLine("Start");
            /*
            Tower t1 = new Tower(1, new Vector2(0, 0));
            Tower t2 = new Tower(2, new Vector2(100, 100));
            Tower t3 = new Tower(3, new Vector2(-80, 70));
            Tower t4 = new Tower(4, new Vector2(50, -130));
            Tower t5 = new Tower(5, new Vector2(-205, -40));
            Tower t6 = new Tower(6, new Vector2(200, -100));

            List<Metka> some = new List<Metka>();
            t1.SetTransition(ref t2); t1.SetTransition(ref t3); t1.SetTransition(ref t5);
            t2.SetTransition(ref t1); t2.SetTransition(ref t3); t2.SetTransition(ref t6);
            t3.SetTransition(ref t1); t3.SetTransition(ref t4); t3.SetTransition(ref t5); t3.SetTransition(ref t2);
            t4.SetTransition(ref t3); t4.SetTransition(ref t5);
            t5.SetTransition(ref t1); t5.SetTransition(ref t3); t5.SetTransition(ref t4);
            t6.SetTransition(ref t2);

            Center cnt = new Center(new List<Tower>(new Tower[] { t1, t2, t3, t4, t5, t6 }));
            int count = Int32.Parse(Console.ReadLine());
            if (count > 0)
                cnt.Opros(count);
            else
                return;
            //cnt.PrintAll();
            Console.WriteLine("******************");
            cnt.CalcKoef();

            cnt.printKoef();
            cnt.Delta();

            cnt.Compare();

            cnt.Opros(count*2);
            //cnt.PrintAll();
            Console.WriteLine("******************");
            cnt.CalcKoef();

            cnt.printKoef();
            cnt.Delta();

            cnt.Compare();*/
            Vector2 coor1 = new Vector2(0, 0);          //src31_6
            Vector2 coor2 = new Vector2(100, 100);      //src31_32
            Vector2 coor3 = new Vector2(50, 130);       //src31_40
            Vector2 coor4 = new Vector2(-150, -230);    //src31_130
            Vector2 coor5 = new Vector2(50, -130);      //src31_132
            Vector2 coor6 = new Vector2(0, -30);        //src31_134
            //! @fixme исправить координаты
            Tower3 src31_6_0 = new Tower3(new Vector3(coor1, 54.2328f));

            Tower3 src31_32_0 = new Tower3( new Vector3(coor2, 33.374f));
            Tower3 src31_32_1 = new Tower3( new Vector3(coor2, 33.374f));
            Tower3 src31_32_2 = new Tower3( new Vector3(coor2, 33.374f));
            Tower3 src31_32_3 = new Tower3( new Vector3(coor2, 33.374f));

            Tower3 src31_40_0 = new Tower3( new Vector3(coor3, 75.16f));

            Tower3 src31_130_0 = new Tower3( new Vector3(coor4, 133.28f));

            Tower3 src31_132_0 = new Tower3( new Vector3(coor5, 116.82f));
            Tower3 src31_132_1 = new Tower3( new Vector3(coor5, 116.82f));
            Tower3 src31_132_2 = new Tower3( new Vector3(coor5, 116.82f));
            Tower3 src31_132_3 = new Tower3( new Vector3(coor5, 116.82f));

            Tower3 src31_134_0 = new Tower3( new Vector3(coor6, 91.83f));

            List<Tower3> towers = new List<Tower3>
            {
                src31_6_0,
                src31_32_0,
                src31_32_1,
                src31_32_2,
                src31_32_3,
                src31_40_0,
                src31_130_0,
                src31_132_0,
                src31_132_1,
                src31_132_2,
                src31_132_3,
                src31_134_0
            };

            CenterParser Cntr = new CenterParser(towers);
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Console.WriteLine(float.Parse("3.1488", culture));
            //foreach (var tow in towers)
            //    tow.Print();
            StreamReader fs = new StreamReader("C:\\Users\\Xcem\\source\\repos\\CsvParse\\CsvParse\\log8clean.csv");
            fs.ReadLine();
            string tmp;
            int i = 0;
            while (fs.Peek() != -1)
            {
                tmp = fs.ReadLine();
                string[] array = tmp.Split(',');
                if (CheckLenght(array))
                {
                    Cntr.GetMetkiFromString(array); i++;
                    if (i > 10) break;
                }
            }
            Cntr.PrintAll();
            Cntr.CalcKoef();

            Cntr.printKoef();




            fs.Close();
        }


    }
}
