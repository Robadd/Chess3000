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
            x = x_val;
            y = y_val;
        }

        public int x
        {
            get { return _x; }
            set
            {
                if (value < 8 && value >= 0)
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
                if (value < 8 && value >= 0)
                {
                    _y = value;
                }
            }
        }

        private int _x = 0;
        private int _y = 0;
    }
}
