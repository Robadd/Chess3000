using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Rook : Piece
    {
        public Rook(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.Rook;
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();

            for (int y = Pos.y + 1; y <= 7; y++)
            {
                if (!addToPosDes(new Pos(y, Pos.x))) { break; }
            }

            for (int y = Pos.y - 1; y >= 0; y--)
            {
                if (!addToPosDes(new Pos(y, Pos.x))) { break; }
            }

            for (int x = Pos.x + 1; x <= 7; x++)
            {
                if (!addToPosDes(new Pos(Pos.y, x))) { break; }
            }

            for (int x = Pos.x - 1; x >= 0; x--)
            {
                if (!addToPosDes(new Pos(Pos.y, x))) { break; }
            }
        }
    }
}
