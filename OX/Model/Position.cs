using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace OX.Model
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position()
        {
            
        }

        public Position(Position pos)
        {
            set(pos);
        }

        public void set(Position pos)
        {
            x = pos.x;
            y = pos.y;
        }
    }
}
