using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class Pos
    {
        public Pos( int y_val, int x_val)
        {
            y = y_val;
            x = x_val;
        }

        public int x
        {
            get { return _x; }
            set
            {
                if (value <= 7 && value >= 0)
                {
                    _x = value;
                }
            }
        }
        public int y
        {
            get { return _y; }
            set
            {
                if (value <= 7 && value >= 0)
                {
                    _y = value;
                }
            }
        }

        private int _y = -1;
        private int _x = -1;
    }
}
