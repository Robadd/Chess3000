using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Koenig : Figur
    {
        public Koenig(Farbe farbe, Feld feld, ChessMaster ChessMaster) : base(farbe, feld, ChessMaster)
        {
        }

        public override void updatePosDes()
        {
            int y = Pos.y + 1;
            int x = Pos.x;

            if (y <= 7)
            {
                addToPosDes(new Pos(y, x));

                x = Pos.x - 1;
                if (x >= 0)
                {
                    addToPosDes(new Pos(y, x));
                }

                x = Pos.x + 1;
                if (x <= 7)
                {
                    addToPosDes(new Pos(y, x));
                }
            }

            y = Pos.y - 1;
            x = Pos.x;
            if (y >= 0)
            {
                addToPosDes(new Pos(y, x));

                x = Pos.x - 1;
                if (x >= 0)
                {
                    addToPosDes(new Pos(y, x));
                }

                x = Pos.x + 1;
                if (x <= 7)
                {
                    addToPosDes(new Pos(y, x));
                }
            }

            y = Pos.y;
            x = Pos.x + 1;
            if ( x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            x = Pos.x - 1;
            if (x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }
        }
    }
}
