using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Laeufer : Figur
    {
        public Laeufer(Farbe farbe, Feld feld, ChessMaster ChessMaster) : base(farbe, feld, ChessMaster)
        {
            pieceType = Chess3000.PieceType.Laeufer;
        }

        public override void updatePosDes()
        {

            for (int y = Pos.y + 1, x = Pos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y - 1, x = Pos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y + 1, x = Pos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y - 1, x = Pos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }
        }


/*
        private void posDesEmptyBoard()
        {
            for (int y = pos.y + 1, x = pos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                moeglicheZiele.Add(new Pos(y, x));
            }

            for (int y = pos.y - 1, x = pos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                moeglicheZiele.Add(new Pos(y, x));
            }

            for (int y = pos.y + 1, x = pos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                moeglicheZiele.Add(new Pos(y, x));
            }

            for (int y = pos.y - 1, x = pos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                moeglicheZiele.Add(new Pos(y, x));
            }
        }
        */
    }
}
