using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Numerics;
using System.IO;
namespace DeltaCalc
{
    class Program
    {
        public static int N = 6;

        static bool CheckLenght(string[] array)
        {
            int countLenght = 0;
            for (int i = 8; i < 12; i++)
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
            Tower t1 = new Tower(1, new Vector2(0, 0));
            Tower t2 = new Tower(2, new Vector2(100, 100));
            Tower t3 = new Tower(3, new Vector2(-80, 70));
            Tower t4 = new Tower(4, new Vector2(50, -130));

            Center cnt = new Center(new List<Tower>(new Tower[] { t1,t2,t3,t4}));

            StreamReader fs = new StreamReader("C:\\Users\\Xcem\\source\\repos\\DeltaCalc\\DeltaCalc\\log_associate.csv");
            fs.ReadLine();
            Console.WriteLine(fs);
            string tmp;
            int i = 0;
            while (fs.Peek() != -1)
            {
                tmp = fs.ReadLine();
                string[] array = tmp.Split(',');
                if (CheckLenght(array))
                {
                    cnt.GetMetkiFromString(array); i++;
                    if (i > 10) break;
                }
            }
            cnt.PrintAll();
            cnt.CalcKoef();

            cnt.printKoef();




            fs.Close();
        }


    }
}
