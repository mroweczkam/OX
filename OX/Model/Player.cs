using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Model
{
    public class Player
    {
        public string Name { get; set; }
        public string sign;
        public TimeSpan time;
        public Position position;

        public  Player(string sign, int minutes){
            this.Name = "Player " + sign;
            this.sign = sign;
            time = new TimeSpan(0, minutes, 0);
            position = new Position();
        }


        
    }
}
