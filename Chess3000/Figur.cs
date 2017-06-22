using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public enum Farbe { WEISS, SCHWARZ };

    public abstract class Figur
    {
        public Figur( Chess3000.Farbe farbe )
        {
            this.farbe = new Chess3000.Farbe();
            this.farbe = farbe;

            moeglicheZiele = new List<Pos>();
        }

        abstract public void updatePosDes();

        private Chess3000.Farbe farbe;
        private List<Pos> moeglicheZiele;
    }
}
