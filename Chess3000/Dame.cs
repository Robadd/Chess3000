﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class Dame : Figur
    {
        public Dame(Farbe farbe, Feld feld, ChessMaster ChessMaster) : base(farbe, feld, ChessMaster)
        {
            pieceType = Chess3000.PieceType.Dame;
        }

        public override void updatePosDes()
        {
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
    }
}
