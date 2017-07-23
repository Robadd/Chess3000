using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class Pos
    {
        public Pos(int y_val, int x_val)
        {
            y = y_val;
            x = x_val;
        }

        public Pos(Pos otherPos)
        {
            _y = otherPos._y;
            _x = otherPos._x;
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

        public override string ToString()
        {
            return _y.ToString() + ',' + _x.ToString();
        }

        public override bool Equals(object obj)
        {
            return (
                obj != null &&
                (obj as Pos)._y == this._y &&
                (obj as Pos)._x == this._x               
                );
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + _y.GetHashCode();
            hash = hash * 23 + _x.GetHashCode();
            return hash; 
        }
    }
}
