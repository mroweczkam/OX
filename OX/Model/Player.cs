using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Model
{
    class Player
    {
        public string Name { get; set; }
        public static int counter=1;
        public int sign;
        public TimeSpan time;
        public Position position;

        public  Player(){
            this.Name = "Player " + counter.ToString(); 
            sign = counter;
            counter++;
            time = new TimeSpan(0, 20, 0);
            position = new Position();

        }


        
    }
}
