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
            pieceType = Chess3000.PieceType.Koenig;
        }

        public override void updatePosDes()
        {
            moeglicheZiele.Clear();

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

            //moeglicheZiele = master.filterCheckedFields(PosDes, this.Farbe);
            checkForCastling();
        }

        private void checkForCastling()
        {
            if (!master.check(this.Farbe, this.Pos))
            {
                if (this.Farbe == Farbe.WEISS)
                {
                    if (!master.whiteKingMoved)
                    {
                        //Kurze Castling
                        if (!master.whiteShortRookMoved)
                        {
                            //Beide Felder leer
                            if (master.getFigur(new Pos(this.Pos.y, this.Pos.x + 1)) == null &&
                                master.getFigur(new Pos(this.Pos.y, this.Pos.x + 2)) == null)
                            {
                                if (!master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x + 1)) &&
                                    !master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x + 2)))
                                {
                                    moeglicheZiele.Add(new Pos(this.Pos.y, this.Pos.x + 2));
                                }
                            }
                        }
                        //Lange Castling
                        if (!master.whiteLongRookMoved)
                        {
                            //Beide Felder leer
                            if (master.getFigur(new Pos(this.Pos.y, this.Pos.x - 1)) == null &&
                                master.getFigur(new Pos(this.Pos.y, this.Pos.x - 2)) == null)
                            {
                                if (!master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x - 1)) &&
                                    !master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x - 2)))
                                {
                                    moeglicheZiele.Add(new Pos(this.Pos.y, this.Pos.x - 2));
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (!master.blackKingMoved)
                    {
                        //Kurzes Castling
                        if (!master.blackShortRookMoved)
                        {
                            //Beide Felder leer
                            if (master.getFigur(new Pos(this.Pos.y, this.Pos.x + 1)) == null &&
                                master.getFigur(new Pos(this.Pos.y, this.Pos.x + 2)) == null)
                            {
                                if (!master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x + 1)) &&
                                    !master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x + 2)))
                                {
                                    moeglicheZiele.Add(new Pos(this.Pos.y, this.Pos.x + 2));
                                }
                            }
                        }
                        //Langes Castling
                        if (!master.blackLongRookMoved)
                        {
                            //Beide Felder leer
                            if (master.getFigur(new Pos(this.Pos.y, this.Pos.x - 1)) == null &&
                                master.getFigur(new Pos(this.Pos.y, this.Pos.x - 2)) == null)
                            {
                                if (!master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x - 1)) &&
                                    !master.check(this.Farbe, new Pos(this.Pos.y, this.Pos.x - 2)))
                                {
                                    moeglicheZiele.Add(new Pos(this.Pos.y, this.Pos.x - 2));
                                }
                            }
                        }
                    }
                }
            }            
        }
    }
}
