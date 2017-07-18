using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class Feld
    {
        public Feld(Pos koord)
        {
            m_koordinate = koord;
            m_figur = null;
        }

        public Figur figur
        {
            get { return m_figur; }
            set { m_figur = value; }
        }

        public Pos Koordinate
        {
            get { return m_koordinate; }
        }

        private Pos m_koordinate;
        private Figur m_figur;
    }
}
