using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public enum Farbe { WEISS, SCHWARZ };
    public enum PieceType { Bauer, Turm, Springer, Laeufer, Dame, Koenig };

    public abstract class Figur
    {
        public Figur(Chess3000.Farbe farbe, Feld feld, ChessMaster ChessMaster)
        {
            this.farbe = new Chess3000.Farbe();
            this.farbe = farbe;
            this.feld = feld;
            this.master = ChessMaster;

            moeglicheZiele = new List<Pos>();
        }

        abstract public void updatePosDes();

        //Rückgabe --> soll es weitergehen?
        protected bool addToPosDes(Pos pos)
        {
            if (master.getFigur(pos) == null)
            {
                moeglicheZiele.Add(pos);
                return true;
            }
            else
            {
                if (master.getFigur(pos).Farbe != this.Farbe)
                {
                    moeglicheZiele.Add(pos);
                }
                return false;
            }
        }


        public Chess3000.Farbe Farbe
        {
            get { return farbe; }
        }

        protected Pos Pos
        {
            get { return feld.Koordinate; }
        }

        public Feld Feld
        {
            get { return feld; }
            set
            {
                if ( value != null )
                {
                    feld = value;
                }
            }
        }

        public List<Pos> PosDes
        {
            get { return PosDes; }
        }

        public string PosDesString
        {
            get
            {
                string posDes = "";
                foreach (Pos des in moeglicheZiele)
                {
                    posDes += "(y, x): (" + des.y + ", " + des.x + ")\n";
                }
                return posDes;
            }
        }

        protected Chess3000.Farbe farbe;
        protected List<Pos> moeglicheZiele;
        protected Feld feld;
        protected ChessMaster master;
        protected PieceType pieceType;

        public string currentPosString
        {
            get
            {
                return "(" + feld.Koordinate.y + ", " + feld.Koordinate.x + ")";
            }
        }

        public PieceType PieceType
        {
            get
            {
                return pieceType;
            }
        }

    }

}
