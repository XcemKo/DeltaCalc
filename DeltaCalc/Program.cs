﻿using System;
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

            /*Tower t1 = new Tower(1, new Vector2(0, 0));
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
            Vector2 coor31_6 = new Vector2(-53296.00f, -13536.71f);          //src31_6
            Vector2 coor31_32 = new Vector2(10115.37f, 5723.23f);      //src31_32
            Vector2 coor31_40 = new Vector2(-4341.66f, -64991.58f);       //src31_40
            Vector2 coor31_130 = new Vector2(-5077.12f, -6621.27f);    //src31_130
            Vector2 coor31_132 = new Vector2(13408.64f, -72532.50f);      //src31_132
            Vector2 coor31_134 = new Vector2(30351.28f, -6309.96f);        //src31_134
            //! @fixme исправить координаты
            Tower3 src31_6_0 = new Tower3(new Vector3(coor31_6, -120.01f), nameof(src31_6_0));

            Tower3 src31_32_0 = new Tower3(new Vector3(coor31_32, 43.66f), nameof(src31_32_0));
            Tower3 src31_32_1 = new Tower3(new Vector3(coor31_32, 43.66f), nameof(src31_32_1));
            Tower3 src31_32_2 = new Tower3(new Vector3(coor31_32, 43.66f), nameof(src31_32_2));
            Tower3 src31_32_3 = new Tower3(new Vector3(coor31_32, 43.66f), nameof(src31_32_3));

            Tower3 src31_40_0 = new Tower3(new Vector3(coor31_40, -198.50f), nameof(src31_40_0));

            Tower3 src31_130_0 = new Tower3(new Vector3(coor31_130, 69.71f), nameof(src31_130_0));

            Tower3 src31_132_0 = new Tower3(new Vector3(coor31_132, -333.65f), nameof(src31_132_0));
            Tower3 src31_132_1 = new Tower3(new Vector3(coor31_132, -333.65f), nameof(src31_132_1));
            Tower3 src31_132_2 = new Tower3(new Vector3(coor31_132, -333.65f), nameof(src31_132_2));
            Tower3 src31_132_3 = new Tower3(new Vector3(coor31_132, -333.65f), nameof(src31_132_3));

            Tower3 src31_134_0 = new Tower3(new Vector3(coor31_134, -41.90f), nameof(src31_134_0));

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
            int towersSize = towers.Count();
            List<CenterParser> Cntrs = new List<CenterParser>{
                new CenterParser(towers),
                new CenterParser(towers),
                new CenterParser(towers),
                new CenterParser(towers),
                new CenterParser(towers)
             };

            CenterParser calcCenter = new CenterParser(towers);
            CenterParser checkCenter1 = new CenterParser(towers);
            CenterParser checkCenter2 = new CenterParser(towers);

            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            //Console.WriteLine(float.Parse("3.1488", culture));
            foreach (var tow in towers)
                tow.Print();

            double[] deltas = new double[towersSize];
            StreamReader fs = new StreamReader("C:\\Users\\Xcem\\source\\repos\\CsvParse\\CsvParse\\log8clean.csv");
            FileStream file = new FileStream("C:\\Users\\Xcem\\source\\repos\\CsvParse\\CsvParse\\test.txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(file);
            fs.ReadLine();
            string tmp;
            int i = 0; int iter = 0; // 10
            repeat:
            i = 0;
            while (fs.Peek() != -1)
            {
                tmp = fs.ReadLine();
                string[] array = tmp.Split(',');
                if (CheckLenght(array))
                {
                    calcCenter.GetMetkiFromString(array); i++;
                    if (i > 1000) break;
                }
            }
            calcCenter.CalcKoef();
            deltas = calcCenter.Delta();
            for (int j = 0; j < towersSize; j++)
            {
                writer.Write("{0:E2}\t", deltas[j]);
            }
            writer.WriteLine();
            calcCenter.reset();
            if (iter++ < 20)  
                goto repeat;
            return;



            while (fs.Peek() != -1)
            {
                tmp = fs.ReadLine();
                string[] array = tmp.Split(',');
                if (CheckLenght(array))
                {
                    calcCenter.GetMetkiFromString(array); i++;
                    if (i > 10000) break;
                }
            }
            calcCenter.CalcKoef();
            deltas = calcCenter.Delta();
            checkCenter1.setKoef(deltas);
            writer.WriteLine("Первый расчет");
            for (int j = 0; j < towersSize; j++)
            {
                writer.Write("{0:E2}\t", deltas[j]);
            }
            writer.WriteLine();
            i = 0;
            while (fs.Peek() != -1)
            {
                tmp = fs.ReadLine();
                string[] array = tmp.Split(',');
                if (CheckLenght(array))
                {
                    checkCenter1.GetMetkiFromString(array);
                    checkCenter2.GetMetkiFromString(array);
                    i++;
                    if (i > 100) break;
                }
            }
            checkCenter2.CalcKoef();
            deltas = checkCenter2.Delta();
            writer.WriteLine("Без поправки расчет");
            for (int j = 0; j < towersSize; j++)
            {
                writer.Write("{0:E2}\t", deltas[j]);
            }
            writer.WriteLine();
            checkCenter1.CalcKoef();
            deltas = checkCenter1.Delta();
            writer.WriteLine("С поправкой расчет");
            for (int j = 0; j < towersSize; j++)
            {
                writer.Write("{0:E2}\t", deltas[j]);
            }
            writer.WriteLine();

            //int iter = 0; int j = 1;
            //const int lenghtPatch = 10;
            //while (fs.Peek() != -1)
            //{
            //    tmp = fs.ReadLine();
            //    string[] array = tmp.Split(',');
            //    if (CheckLenght(array))
            //    {
            //        Cntrs[j-1].GetMetkiFromString(array); iter++;
            //        if (iter > lenghtPatch * j) j++;
            //        if (iter > Cntrs.Count() * lenghtPatch)
            //            break;
            //    }
            //}
            ////Cntr.PrintAll();
            //double[,] deltas = new double[Cntrs.Count(), towersSize]; j = 0;
            //foreach (var cnt in Cntrs)
            //{
            //    cnt.CalcKoef();
            //    double[] tmpDelta = new double[towersSize];
            //    tmpDelta = cnt.Delta();
            //    for (int i = 0; i < towersSize; i++) {
            //        deltas[j, i] = tmpDelta[i];
            //    }
            //    j++;
            //}

            //FileStream file = new FileStream("C:\\Users\\Xcem\\source\\repos\\CsvParse\\CsvParse\\test.txt", FileMode.Create, FileAccess.ReadWrite);
            //StreamWriter writer = new StreamWriter(file);

            //for (int cntIter = 0; cntIter < Cntrs.Count; cntIter++)
            //{
            //    for (int i = 0; i < towersSize; i++)
            //    {
            //        Console.Write("{0:E2}\t", deltas[cntIter, i]);
            //        writer.Write("{0:E2}\t", deltas[cntIter, i]);
            //    }
            //    Console.WriteLine();
            //    writer.WriteLine();
            //}
            writer.Close();
            file.Close();
            fs.Close();
        }


    }
}
