﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class Bauer : Figur
    {
        public Bauer(Farbe farbe, Feld feld, ChessMaster ChessMaster) : base(farbe, feld, ChessMaster)
        {
            pieceType = Chess3000.PieceType.Bauer;
        }

        public override void updatePosDes()
        {
            moeglicheZiele.Clear();

            if (Farbe == Chess3000.Farbe.WEISS)
            {
                int y = Pos.y + 1;
                int x = Pos.x;
                if (y <= 7)
                {
                    if (master.getFigur(new Pos(y, x)) == null)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }

                if (Pos.y == 1)
                {
                    y = Pos.y + 2;
                    if (master.getFigur(new Pos(y,x)) == null)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }

                y = Pos.y + 1;
                x = Pos.x + 1;
                if (y <= 7 && x <= 7)
                {
                    if (master.getFigur(new Pos(y, x)) != null && master.getFigur(new Pos(y, x)).Farbe != this.Farbe)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }

                x = Pos.x - 1;
                if (y <= 7 && x >= 0)
                {
                    if (master.getFigur(new Pos(y, x)) != null && master.getFigur(new Pos(y, x)).Farbe != this.Farbe)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }
            }
            else if (Farbe == Chess3000.Farbe.SCHWARZ)
            {
                int y = Pos.y - 1;
                int x = Pos.x;
                if (y >= 0)
                {
                    if (master.getFigur(new Pos(y, x)) == null)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }                   
                }

                if (Pos.y == 6)
                {
                    y = Pos.y - 2;
                    if (master.getFigur(new Pos(y, x)) == null)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }

                y = Pos.y - 1;
                x = Pos.x + 1;
                if (y >= 0 && x <= 7)
                {
                    if (master.getFigur(new Pos(y, x)) != null && master.getFigur(new Pos(y, x)).Farbe != this.Farbe)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }

                x = Pos.x - 1;
                if (y >= 0 && x >= 0)
                {
                    if (master.getFigur(new Pos(y, x)) != null && master.getFigur(new Pos(y, x)).Farbe != this.Farbe)
                    {
                        moeglicheZiele.Add(new Pos(y, x));
                    }
                }
            }
        }
    }
}
