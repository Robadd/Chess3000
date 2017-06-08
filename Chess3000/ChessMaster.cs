using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{
    public class ChessMaster
    {
        public ChessMaster()
        {

        }

        public void createInitialBoardState()
        {
            m_schachbrett = new List<List<Feld>>();
            
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    m_schachbrett[x][y] = new Feld(new Pos(x, y));
                }
            }


            foreach ( Feld feld in m_schachbrett[1])
            {
                feld.figur = new Bauer();
            }

            foreach (Feld feld in m_schachbrett[6])
            {
                feld.figur = new Bauer();
            }

        }

        List<List<Feld>> m_schachbrett;
    }
}
