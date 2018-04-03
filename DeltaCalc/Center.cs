using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DeltaCalc
{
	static class Other
	{
		public const double LightSpeed = 299792458d;
		public static Random rnd = new Random();
	}

	struct Metka
	{
		public int packet;
		public int from, to;
		public long time;

		public Metka(int _packet, int _from, int _to, long _time)
		{
			packet = _packet;
			from = _from;
			to = _to;
			time = _time;
		}

		public void Print()
		{
            if (from == to)
                Console.WriteLine("{0} {1}     -  {2}", packet, from, time);
            else
                Console.WriteLine("{0} {1}->{2}  -  {3}", packet, from, to, time);
		}
	}
	/*************************/
	class Center
	{
		List<Tower> towers;
		double[,] distance;
		double[,] koef;
		double[]  solKoef;
        double[] solveDelta;
		List<Metka> metki;
		int currentTower;
		int numberOfPacket;
        int size;

		public Center(List<Tower> _towers){
			towers = _towers;
            size = towers.Count;
            distance = new double[size, size];
			koef = new double[size, size];
			solKoef = new double[size];
            solveDelta = new double[size];

            for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					distance[i, j] = Vector2.Distance(towers[i].position, towers[j].position);
				}
			}
			numberOfPacket = 1;
			metki = new List<Metka>();
		}

		public void Opros(int seances, int timeOut = 50){
            long now = (DateTime.Now - Tower.programmBegin).Ticks;
            Console.WriteLine("Nachal opros - {0}", (DateTime.Now - Tower.programmBegin).Ticks);
			for (int i = 0; i < seances;)
			{
				metki.AddRange(towers[currentTower].Transfer(numberOfPacket, now));
				currentTower++; numberOfPacket++;
				if (currentTower == size)
				{
					currentTower = 0;
					i++;
				}
                //System.Threading.Thread.Sleep(timeOut + Other.rnd.Next(-25, 25));
                now += (timeOut + Other.rnd.Next(-25, 25))*10000;
            }
            Console.WriteLine("Проиведено сеансов {0}\t Пакетов-{1}", seances, numberOfPacket);
		}

		public void PrintAll(){
			int prev = 1;
			foreach (var a in metki)
			{	
				if (prev != a.from)
				{
					prev = a.from; Console.WriteLine("===========");
				}
				a.Print();

			}
		}

		public void CalcKoef()
		{
			for (int i = 0; i < size; i++){
				Metka parent = new Metka();
				foreach (var metka in metki)
				{
					if (metka.from == metka.to){
						parent = metka;
						continue;
					}
					if (metka.from-1 == i || metka.to-1 == i)
					{
						koef[ i, i]++;
						if(metka.from-1 == i){
							koef[i,metka.to-1]--;
							solKoef[i] -= metka.time - parent.time - distance[i,metka.to-1];
						}else{
							koef[i,metka.from-1]--;
							solKoef[i] -= 0 - metka.time + parent.time + distance[i,metka.from-1];
						}

                        //Console.WriteLine("Find {0} {1} -> [{2}{3}]++ \t [{4}{5}]--", metka.from, metka.to
                        //                                                             , i + 1, i + 1
                        //                  , i, (metka.from == i) ? metka.to : metka.from);
                    }
				}
                //Console.WriteLine();
            }

		}

		public void printKoef()
		{
			//CalcKoef();

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
					Console.Write("{0}\t",koef[i, j]);
				Console.WriteLine(solKoef[i]);
			}
		}

        public double[] Delta()
        {
            double[] ret = new double[size-1];
            double[,] tmpKoef = new double[size-1, size-1];
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
            int info=0; alglib.densesolverreport reporter;
            alglib.rmatrixsolve(tmpKoef, smallSize, solLow, out info, out reporter, out ret);
            Console.Write("ret = { ");
            for (int i = 0; i < size-1; i++)    Console.Write("{0:00.0000}\t", ret[i]);
            Console.WriteLine("}");
            for(int i=1;i<size;i++)
                solveDelta[i] = ret[i-1];
            return ret;
        }
        public void Compare() {

            for (int i = 0; i < size; i++)
                Console.WriteLine("Отклонение {0:00.0000} {1:00.0000} {2:00.0000}", 
                    towers[i].delta, 
                    solveDelta[i],
                    Math.Abs(towers[i].delta - solveDelta[i]));
        }
    }
}