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
            updatePossibleDestinations();
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
            //Debug
            Console.WriteLine("********************************************************");
            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld feld in m_schachbrett[y])
                {
                    if (feld.figur != null)
                    {
                        feld.figur.updatePosDes();

                        //Debug
                        Console.WriteLine("Figur: " + feld.figur.PieceType.ToString());
                        Console.WriteLine("Farbe: " + feld.figur.Farbe.ToString());
                        Console.WriteLine("Position: " + feld.figur.currentPosString);
                        Console.WriteLine("Mögliche Ziele:");
                        Console.WriteLine(feld.figur.PosDesString);
                        Console.WriteLine("");
                    }
                }
            }
        }
        
        Pos getKingPosition( Chess3000.Farbe player)
        {
            Pos kingPos = new Pos(-1,-1);
            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld feld in m_schachbrett[y])
                {
                    if (feld.figur != null && feld.figur.PieceType == PieceType.Koenig && feld.figur.Farbe == player)
                    {
                        kingPos = feld.Koordinate;
                    }
                }
            }
            return kingPos;
        }

        bool foundPiece(Chess3000.Farbe color, Chess3000.PieceType pieceType, Pos currentPos)
        {
            Figur figur = getFigur(new Pos(currentPos.y, currentPos.x));
            return (
                figur != null &&
                figur.Farbe == color &&
                figur.PieceType == pieceType
                );
        }

        bool bishopCheck(Pos kingPos, Farbe kingColor)
        {
            for (int y = kingPos.y + 1, x = kingPos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Laeufer) { return true; }
            }

            for (int y = kingPos.y - 1, x = kingPos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Laeufer) { return true; }
            }

            for (int y = kingPos.y + 1, x = kingPos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Laeufer) { return true; }
            }

            for (int y = kingPos.y - 1, x = kingPos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Laeufer) { return true; }
            }

            return false;
        }


        bool rookCheck(Pos kingPos, Farbe kingColor)
        {
            for (int y = kingPos.y + 1; y <= 7; y++)
            {
                Figur figur = getFigur(new Pos(y, kingPos.x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Turm) { return true; }
            }

            for (int y = kingPos.y - 1; y >= 0; y--)
            {
                Figur figur = getFigur(new Pos(y, kingPos.x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Turm) { return true; }
            }

            for (int x = kingPos.x + 1; x <= 7; x++)
            {
                Figur figur = getFigur(new Pos(kingPos.y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Turm) { return true; }
            }

            for (int x = kingPos.x - 1; x >= 0; x--)
            {
                Figur figur = getFigur(new Pos(kingPos.y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Turm) { return true; }
            }

            return false;
        }

        bool queenCheck(Pos kingPos, Farbe kingColor)
        {
            for (int y = kingPos.y + 1, x = kingPos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int y = kingPos.y - 1, x = kingPos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int y = kingPos.y + 1, x = kingPos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int y = kingPos.y - 1, x = kingPos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                Figur figur = getFigur(new Pos(y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int y = kingPos.y + 1; y <= 7; y++)
            {
                Figur figur = getFigur(new Pos(y, kingPos.x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int y = kingPos.y - 1; y >= 0; y--)
            {
                Figur figur = getFigur(new Pos(y, kingPos.x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int x = kingPos.x + 1; x <= 7; x++)
            {
                Figur figur = getFigur(new Pos(kingPos.y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            for (int x = kingPos.x - 1; x >= 0; x--)
            {
                Figur figur = getFigur(new Pos(kingPos.y, x));
                if (figur == null) { continue; }
                else if (figur.Farbe == kingColor) { break; }
                else if (figur.PieceType == PieceType.Dame) { return true; }
            }

            return false;
        }

        bool knightCheck(Pos kingPos, Farbe kingColor)
        {
            Chess3000.Farbe enemyColor = (kingColor == Farbe.WEISS ? Farbe.SCHWARZ : Farbe.WEISS);

            int y = kingPos.y + 2;
            int x = kingPos.x + 1;
            if (y <= 7 && x <= 7)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y + 2;
            x = kingPos.x - 1;
            if (y <= 7 && x >= 0)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y - 2;
            x = kingPos.x - 1;
            if (y >= 0 && x >= 0)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y - 2;
            x = kingPos.x + 1;
            if (y >= 0 && x <= 7)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y + 1;
            x = kingPos.x + 2;
            if (y <= 7 && x <= 7)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y - 1;
            x = kingPos.x + 2;
            if (y >= 0 && x <= 7)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y - 1;
            x = kingPos.x - 2;
            if (y >= 0 && x >= 0)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            y = kingPos.y + 1;
            x = kingPos.x - 2;
            if (y <= 7 && x >= 0)
            {
                if (foundPiece(enemyColor, PieceType.Springer, kingPos))
                {
                    return true;
                }
            }

            return false;
        }

        bool check(Chess3000.Farbe kingColor)
        {
            Pos kingPos = getKingPosition(kingColor);
            Chess3000.Farbe enemyColor = (kingColor == Farbe.WEISS ? Farbe.SCHWARZ : Farbe.WEISS);

            /*
            bishopCheck(kingPos, kingColor);
            rookCheck(kingPos, kingColor);
            queenCheck(kingPos, kingColor);
            knightCheck(kingPos, kingColor);
            */

            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld feld in m_schachbrett[y])
                {
                    if (feld.figur != null && feld.figur.Farbe == enemyColor)
                    {
                        foreach (Pos pos in feld.figur.PosDes)
                        {
                            if (pos == kingPos) { return true; }
                        }
                    }
                }
            }

            return false;
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
