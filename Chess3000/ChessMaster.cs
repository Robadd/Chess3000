using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class ChessMaster
    {
        Feld[][] m_schachbrett;

        public ChessMaster()
        {
            createInitialBoardState();

            //Test
            //            move(new Pos(1, 1), new Pos(2, 1));
            //           move(new Pos(6, 6), new Pos(4, 6));
            //           move(new Pos(1, 4), new Pos(2, 4));
            //            move(new Pos(1, 1), new Pos(5, 1));
//            move(new Pos(0, 4), new Pos(2, 7));
//            m_schachbrett[2][7].figur.updatePosDes();
        }

        private void createInitialBoardState()
        {
            m_schachbrett = new Feld[8][];

            for (int y = 0; y < 8; y++)
            {
                m_schachbrett[y] = new Feld[8];
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    m_schachbrett[y][x] = new Feld(new Pos(y, x));
                }
            }

            for (int x = 0; x <= 7; x++)
            {
                m_schachbrett[1][x].figur = new Bauer(Chess3000.Farbe.WEISS, m_schachbrett[1][x], this);
                m_schachbrett[6][x].figur = new Bauer(Chess3000.Farbe.SCHWARZ, m_schachbrett[6][x], this);
            }

            m_schachbrett[0][0].figur = new Turm(Chess3000.Farbe.WEISS, m_schachbrett[0][0], this);
            m_schachbrett[0][7].figur = new Turm(Chess3000.Farbe.WEISS, m_schachbrett[0][7], this);
            m_schachbrett[7][0].figur = new Turm(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][0], this);
            m_schachbrett[7][7].figur = new Turm(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][7], this);

            m_schachbrett[0][1].figur = new Springer(Chess3000.Farbe.WEISS, m_schachbrett[0][1], this);
            m_schachbrett[0][6].figur = new Springer(Chess3000.Farbe.WEISS, m_schachbrett[0][6], this);
            m_schachbrett[7][1].figur = new Springer(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][1], this);
            m_schachbrett[7][6].figur = new Springer(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][6], this);

            m_schachbrett[0][2].figur = new Laeufer(Chess3000.Farbe.WEISS, m_schachbrett[0][2], this);
            m_schachbrett[0][5].figur = new Laeufer(Chess3000.Farbe.WEISS, m_schachbrett[0][5], this);
            m_schachbrett[7][2].figur = new Laeufer(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][2], this);
            m_schachbrett[7][5].figur = new Laeufer(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][5], this);

            m_schachbrett[0][3].figur = new Dame(Chess3000.Farbe.WEISS, m_schachbrett[0][3], this);
            m_schachbrett[7][3].figur = new Dame(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][3], this);

            m_schachbrett[0][4].figur = new Koenig(Chess3000.Farbe.WEISS, m_schachbrett[0][4], this);
            m_schachbrett[7][4].figur = new Koenig(Chess3000.Farbe.SCHWARZ, m_schachbrett[7][4], this);
        }

        private void updatePossibleDestinations()
        {

        }
        
        //Momentan noch ohne Prüfung, zum Testen!
        bool move( Pos from, Pos to)
        {
            m_schachbrett[to.y][to.x].figur = m_schachbrett[from.y][from.x].figur;
            m_schachbrett[to.y][to.x].figur.Feld = m_schachbrett[to.y][to.x];
            m_schachbrett[from.y][from.x].figur = null;
            return true;
        }

        public Figur getFigur( Pos pos )
        {
            return m_schachbrett[pos.y][pos.x].figur;
        }
    }
}
