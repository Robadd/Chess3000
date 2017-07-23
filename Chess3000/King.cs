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

            checkForCastling();
        }

        private void checkForCastling()
        {
            if (!master.check(this.Color, this.Pos))
            {
                bool whiteShortCastlingCase = this.Color == Color.White &&
                                              master.WhiteShortCastlingPiecesNotMoved;
                bool blackShortCastlingCase = this.Color == Color.Black && 
                                              master.BlackShortCastlingPiecesNotMoved;

                if (whiteShortCastlingCase || blackShortCastlingCase)
                {
                    Pos kingShortCrossPos = new Pos(this.Pos.y, this.Pos.x + 1);
                    Pos kingShortCastlingPos = new Pos(this.Pos.y, this.Pos.x + 2);

                    //Beide Felder leer
                    if (master.getPiece(kingShortCrossPos) == null &&
                        master.getPiece(kingShortCastlingPos) == null)
                    {
                        if (!master.check(this.Color, kingShortCrossPos) &&
                            !master.check(this.Color, kingShortCastlingPos))
                        {
                            possibleDestinations.Add(kingShortCastlingPos);
                        }
                    }
                }

                bool whiteLongCastlingCase = this.Color == Color.White &&
                                             master.WhiteLongCastlingPiecesNotMoved;
                bool blackLongCastlingCase = this.Color == Color.Black &&
                                             master.BlackLongCastlingPiecesNotMoved;

                if (whiteLongCastlingCase || blackLongCastlingCase)
                {
                    Pos kingLongCrossPos = new Pos(this.Pos.y, this.Pos.x - 1);
                    Pos kingLongCastlingPos = new Pos(this.Pos.y, this.Pos.x - 2);

                    //Beide Felder leer
                    if (master.getPiece(kingLongCrossPos) == null &&
                        master.getPiece(kingLongCastlingPos) == null)
                    {
                        if (!master.check(this.Color, kingLongCrossPos) &&
                            !master.check(this.Color, kingLongCastlingPos))
                        {
                            possibleDestinations.Add(kingLongCastlingPos);
                        }
                    }
                }              
            }
        }
    }
}
