using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Numerics;

namespace DeltaCalc
{

    class Vector2 {
        public double x, y;
        public Vector2() { x = 0d; y = 0d; }
        public Vector2(double _x, double _y) { x = _x; y = _y; }
        static public double Distance(Vector2 r, Vector2 l) {
            double ret = Math.Sqrt(Math.Pow((r.x - l.x),2)+ Math.Pow((r.y - l.y), 2));
            return ret;
        }
    }

    class Tower
    {

        public static int maxRangeRnd = 5;
        public static DateTime programmBegin = new DateTime(2018, 03, 26,09,37,13);
		private bool programmStart = false;
        
        public Vector2 position;
        int id;
        List<Tower> visibleTowers;
        List<double> distance;
		public long delta { get;}

		public Tower(int _id, Vector2 _position) {
			if (programmStart == false) {
                programmBegin = DateTime.Now;
				programmStart = true;
			}

            id = _id;
            position = _position;
            visibleTowers = new List<Tower>();
            distance = new List<double>();

            if(id != 1) delta = Other.rnd.Next(5,25);
            Console.WriteLine("Tower #{0}\tpos[{1},{2}]\t delta - {3}", id, position.x, position.y, delta);
        }


        public void SetTransition(ref Tower tower) {
            visibleTowers.Add(tower);
            distance.Add(Vector2.Distance(this.position,tower.position));
            //Console.WriteLine("{0}\\Dis to {1} = {2}", id, tower.id, distance.Last());
        }

        long getTime() {
            long now = (DateTime.Now - programmBegin).Ticks + delta;
            //Console.WriteLine("{0} - {1}", id, now);
            return now;
        }

        public List<Metka> Transfer(int packet, long timeStart) {
            List<Metka> ret = new List<Metka>();
            long now = timeStart + this.delta + Other.rnd.Next(maxRangeRnd); /* t + d + w*/
            ret.Add(new Metka(packet, id, id, now));
            for (int i = 0; i < visibleTowers.Count; i++)
            {
                long towerTime = timeStart + visibleTowers[i].delta;
                double disTime = (distance[i] / Other.LightSpeed)*10000000;
                //Console.WriteLine("dis {0}-{1} -> {2}", id, visibleTowers[i].id, disTime);
				if(Other.rnd.Next(0,100) <95)/* Типо ошибка связи */
                	ret.Add(new Metka(packet, id, visibleTowers[i].id, towerTime + (long)disTime + Other.rnd.Next(maxRangeRnd)));
            }
            return ret;
        }

    }
}
