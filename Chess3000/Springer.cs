using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Springer : Figur
    {
        public Springer(Farbe farbe, Feld feld, ChessMaster ChessMaster) : base(farbe, feld, ChessMaster)
        {
            pieceType = Chess3000.PieceType.Springer;
        }

        public override void updatePosDes()
        {
            moeglicheZiele.Clear();

            int y = Pos.y + 2;
            int x = Pos.x + 1;
            if (y <= 7 && x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y + 2;
            x = Pos.x - 1;
            if (y <= 7 && x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y - 2;
            x = Pos.x - 1;
            if (y >= 0 && x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y - 2;
            x = Pos.x + 1;
            if (y >= 0 && x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y + 1;
            x = Pos.x + 2;
            if (y <= 7 && x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y - 1;
            x = Pos.x + 2;
            if (y >= 0 && x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y - 1;
            x = Pos.x - 2;
            if (y >= 0 && x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }

            y = Pos.y + 1;
            x = Pos.x - 2;
            if (y <= 7 && x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }
        }
    }
}
