using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    class King : Piece
    {
        public King(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.King;
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();

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
            if (x <= 7)
            {
                addToPosDes(new Pos(y, x));
            }

            x = Pos.x - 1;
            if (x >= 0)
            {
                addToPosDes(new Pos(y, x));
            }

            //possibleDestinations = master.filterCheckedFields(PosDes, this.Color);
            checkForCastling();
        }

        private void checkForCastling()
        {
            if (!master.check(this.Color, this.Pos))
            {
                if (this.Color == Color.White)
                {
                    if (!master.whiteKingMoved)
                    {
                        //Kurze Castling
                        if (!master.whiteShortRookMoved)
                        {
                            //Beide Squareer leer
                            if (master.getPiece(new Pos(this.Pos.y, this.Pos.x + 1)) == null &&
                                master.getPiece(new Pos(this.Pos.y, this.Pos.x + 2)) == null)
                            {
                                if (!master.check(this.Color, new Pos(this.Pos.y, this.Pos.x + 1)) &&
                                    !master.check(this.Color, new Pos(this.Pos.y, this.Pos.x + 2)))
                                {
                                    possibleDestinations.Add(new Pos(this.Pos.y, this.Pos.x + 2));
                                }
                            }
                        }
                        //Lange Castling
                        if (!master.whiteLongRookMoved)
                        {
                            //Beide Squareer leer
                            if (master.getPiece(new Pos(this.Pos.y, this.Pos.x - 1)) == null &&
                                master.getPiece(new Pos(this.Pos.y, this.Pos.x - 2)) == null)
                            {
                                if (!master.check(this.Color, new Pos(this.Pos.y, this.Pos.x - 1)) &&
                                    !master.check(this.Color, new Pos(this.Pos.y, this.Pos.x - 2)))
                                {
                                    possibleDestinations.Add(new Pos(this.Pos.y, this.Pos.x - 2));
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
                            //Beide Squareer leer
                            if (master.getPiece(new Pos(this.Pos.y, this.Pos.x + 1)) == null &&
                                master.getPiece(new Pos(this.Pos.y, this.Pos.x + 2)) == null)
                            {
                                if (!master.check(this.Color, new Pos(this.Pos.y, this.Pos.x + 1)) &&
                                    !master.check(this.Color, new Pos(this.Pos.y, this.Pos.x + 2)))
                                {
                                    possibleDestinations.Add(new Pos(this.Pos.y, this.Pos.x + 2));
                                }
                            }
                        }
                        //Langes Castling
                        if (!master.blackLongRookMoved)
                        {
                            //Beide Felder leer
                            if (master.getPiece(new Pos(this.Pos.y, this.Pos.x - 1)) == null &&
                                master.getPiece(new Pos(this.Pos.y, this.Pos.x - 2)) == null)
                            {
                                if (!master.check(this.Color, new Pos(this.Pos.y, this.Pos.x - 1)) &&
                                    !master.check(this.Color, new Pos(this.Pos.y, this.Pos.x - 2)))
                                {
                                    possibleDestinations.Add(new Pos(this.Pos.y, this.Pos.x - 2));
                                }
                            }
                        }
                    }
                }
            }            
        }
    }
}
