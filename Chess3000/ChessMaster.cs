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

            foreach (Feld feld in m_schachbrett[1])
            {
                feld.figur = new Bauer(Chess3000.Farbe.WEISS);
            }

            foreach (Feld feld in m_schachbrett[6])
            {
                feld.figur = new Bauer(Chess3000.Farbe.SCHWARZ);
            }

            m_schachbrett[0][0].figur = new Turm(Chess3000.Farbe.WEISS);
            m_schachbrett[0][7].figur = new Turm(Chess3000.Farbe.WEISS);
            m_schachbrett[7][0].figur = new Turm(Chess3000.Farbe.SCHWARZ);
            m_schachbrett[7][7].figur = new Turm(Chess3000.Farbe.SCHWARZ);

            m_schachbrett[0][1].figur = new Springer(Chess3000.Farbe.WEISS);
            m_schachbrett[0][6].figur = new Springer(Chess3000.Farbe.WEISS);
            m_schachbrett[7][1].figur = new Springer(Chess3000.Farbe.SCHWARZ);
            m_schachbrett[7][6].figur = new Springer(Chess3000.Farbe.SCHWARZ);

            m_schachbrett[0][2].figur = new Laeufer(Chess3000.Farbe.WEISS);
            m_schachbrett[0][5].figur = new Laeufer(Chess3000.Farbe.WEISS);
            m_schachbrett[7][2].figur = new Laeufer(Chess3000.Farbe.SCHWARZ);
            m_schachbrett[7][5].figur = new Laeufer(Chess3000.Farbe.SCHWARZ);

            m_schachbrett[0][3].figur = new Dame(Chess3000.Farbe.WEISS);
            m_schachbrett[7][3].figur = new Dame(Chess3000.Farbe.SCHWARZ);

            m_schachbrett[0][4].figur = new Koenig(Chess3000.Farbe.WEISS);
            m_schachbrett[7][4].figur = new Koenig(Chess3000.Farbe.SCHWARZ);
        }

        private void updatePossibleDestinations()
        {

        }
    }
}
