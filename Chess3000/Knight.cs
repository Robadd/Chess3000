using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Knight : Piece
    {
        public Knight(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.Knight;
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();

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
