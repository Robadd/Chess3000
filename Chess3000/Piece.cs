using System.Collections.Generic;

namespace Chess3000
{
    public enum Color { White, Black };
    public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King };

    public abstract class Piece
    {
        public Piece(Chess3000.Color color, Square square, ChessMaster chessMaster)
        {
            this.color = new Chess3000.Color();
            this.color = color;
            this.square = square;
            this.master = chessMaster;

            possibleDestinations = new List<Pos>();
        }

        abstract public void updatePosDes();

        //Rückgabe --> soll es weitergehen?
        protected bool addToPosDes(Pos pos)
        {
            if (master.getPiece(pos) == null)
            {
                possibleDestinations.Add(pos);
                return true;
            }
            else
            {
                if (master.getPiece(pos).Color != this.Color)
                {
                    possibleDestinations.Add(pos);
                }
                return false;
            }
        }


        public Chess3000.Color Color
        {
            get { return color; }
        }

        protected Pos Pos
        {
            get
            {
                if (square != null) { return square.Coordinate; }
                else { return new Pos(-1, -1); }                
            }
        }

        public Square Square
        {
            get { return square; }
            set
            {
                if (value != null)
                {
                    square = value;
                }
            }
        }

        public List<Pos> PosDes
        {
            get { return possibleDestinations; }
        }

        //Nötig da CapDes != PosDes beim Pawnn
        virtual public List<Pos> CapDes
        {
            get { return possibleDestinations; }
        }

        public string PosDesString
        {
            get
            {
                string posDes = "";
                foreach (Pos des in possibleDestinations)
                {
                    posDes += "(y,x): (" + des.ToString() + ")\n";
                }
                return posDes;
            }
        }

        public bool validDes(Pos des)
        {
            return possibleDestinations.Contains(des);
        }

        protected Chess3000.Color color;
        protected List<Pos> possibleDestinations;
        protected Square square;
        protected ChessMaster master;
        protected PieceType pieceType;

        public string currentPosString
        {
            get
            {
                return "(" + square.Coordinate.ToString() + ")";
            }
        }

        public PieceType PieceType
        {
            get
            {
                return pieceType;
            }
        }

        public override bool Equals(object obj)
        {
            return (
                obj != null &&
                (obj as Piece).Color == this.Color &&
                (obj as Piece).PieceType == this.PieceType &&
                (obj as Piece).Pos.Equals(this.Pos)
                );
        }

        //Black magic
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Color.GetHashCode();
            hash = hash * 23 + PieceType.GetHashCode();
            hash = hash * 23 + Pos.GetHashCode();
            return hash;
        }

    }

}
