using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Model
{
    class History
    {
        public Position position;
        public string sign;

        public History(Position lastMove, string sign)
        {
            // TODO: Complete member initialization
            this.position = lastMove;
            this.sign = sign;
        }
    }
}
