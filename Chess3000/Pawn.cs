using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class Pawn : Piece
    {
        List<Pos> capDes;

        public Pawn(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.Pawn;
            capDes = new List<Pos>();
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();
            capDes.Clear();

            if (Color == Chess3000.Color.White)
            {
                int y = Pos.y + 1;
                int x = Pos.x;
                if (y <= 7)
                {
                    if (master.getPiece(new Pos(y, x)) == null)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                    }
                }

                if (Pos.y == 1)
                {
                    y = Pos.y + 2;
                    if (master.getPiece(new Pos(y,x)) == null)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                    }
                }

                y = Pos.y + 1;
                x = Pos.x + 1;
                if (y <= 7 && x <= 7)
                {
                    if (master.getPiece(new Pos(y, x)) != null && master.getPiece(new Pos(y, x)).Color != this.Color)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                        capDes.Add(new Pos(y, x));
                    }
                }

                x = Pos.x - 1;
                if (y <= 7 && x >= 0)
                {
                    if (master.getPiece(new Pos(y, x)) != null && master.getPiece(new Pos(y, x)).Color != this.Color)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                        capDes.Add(new Pos(y, x));
                    }
                }
            }
            else if (Color == Chess3000.Color.Black)
            {
                int y = Pos.y - 1;
                int x = Pos.x;
                if (y >= 0)
                {
                    if (master.getPiece(new Pos(y, x)) == null)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                    }                   
                }

                if (Pos.y == 6)
                {
                    y = Pos.y - 2;
                    if (master.getPiece(new Pos(y, x)) == null)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                    }
                }

                y = Pos.y - 1;
                x = Pos.x + 1;
                if (y >= 0 && x <= 7)
                {
                    if (master.getPiece(new Pos(y, x)) != null && master.getPiece(new Pos(y, x)).Color != this.Color)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                        capDes.Add(new Pos(y, x));
                    }
                }

                x = Pos.x - 1;
                if (y >= 0 && x >= 0)
                {
                    if (master.getPiece(new Pos(y, x)) != null && master.getPiece(new Pos(y, x)).Color != this.Color)
                    {
                        possibleDestinations.Add(new Pos(y, x));
                        capDes.Add(new Pos(y, x));
                    }
                }
            }

            if (master.EligiblePawn1 != null && master.EligiblePawn1.Equals(this) || master.EligiblePawn2 != null && master.EligiblePawn2.Equals(this))
            {
                possibleDestinations.Add(master.EnPassantPos);
                capDes.Add(master.EnPassantPos);
            }
        }

        public override List<Pos> CapDes
        {
            get { return capDes; }
        }
    }
}
