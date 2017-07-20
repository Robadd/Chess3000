using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess3000
{

    public enum Result {
        SUCCESS,
        ERROR_CHECK,
        ERROR_CHECK_UNHANDLED,
        ERROR_NULL_PIECE,
        ERROR_INVALID_DES,
        ERROR_WRONG_COLOR
    };

    public class ChessMaster
    {
        Feld[][] m_schachbrett;
        Chess3000.Farbe drawing = Farbe.WEISS;

        public ChessMaster()
        {
            //Test
            Console.BufferHeight = Int16.MaxValue - 1;
            createInitialBoardState();
            updatePossibleDestinations();
            Result res;
            Console.WriteLine("#####################");
            res = move(new Pos(1, 3), new Pos(2, 3));
            Console.WriteLine(res.ToString());
            res = move(new Pos(6, 4), new Pos(5, 4));
            Console.WriteLine(res.ToString());
            res = move(new Pos(1, 0), new Pos(2, 0));
            Console.WriteLine(res.ToString());
            res = move(new Pos(7, 5), new Pos(3, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(0, 4), new Pos(1, 3));
            Console.WriteLine(res.ToString());
            Console.WriteLine("#####################");
        }

        public Chess3000.Farbe Drawing
        {
            get { return drawing; }
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

        bool check(Chess3000.Farbe kingColor)
        {
            Pos kingPos = getKingPosition(kingColor);
            Chess3000.Farbe enemyColor = (kingColor == Farbe.WEISS ? Farbe.SCHWARZ : Farbe.WEISS);

            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld feld in m_schachbrett[y])
                {
                    if (feld.figur != null && feld.figur.Farbe == enemyColor)
                    {
                        foreach (Pos pos in feld.figur.PosDes)
                        {
                            //Equals() != ==-Operator
                            if (pos.Equals(kingPos)) { return true; }
                        }
                    }
                }
            }

            return false;
        }

        public Chess3000.Result move( Pos from, Pos to)
        {
            Figur piece = getFigur(from);

            if (piece == null) { return Result.ERROR_NULL_PIECE; }
            else if (piece.Farbe != drawing) { return Result.ERROR_WRONG_COLOR; }
            else if (!piece.validDes(to)) { return Result.ERROR_INVALID_DES; }
            else
            {
                Figur fromPiece = m_schachbrett[from.y][from.x].figur;
                Figur toPiece = m_schachbrett[to.y][to.x].figur;

                m_schachbrett[to.y][to.x].figur = m_schachbrett[from.y][from.x].figur;
                m_schachbrett[to.y][to.x].figur.Feld = m_schachbrett[to.y][to.x];
                m_schachbrett[from.y][from.x].figur = null;
                updatePossibleDestinations();

                if (check(drawing))
                {
                    //Änderungen rückgängig machen
                    m_schachbrett[from.y][from.x].figur = fromPiece;
                    m_schachbrett[from.y][from.x].figur.Feld = m_schachbrett[from.y][from.x];
                    m_schachbrett[to.y][to.x].figur = toPiece;
                    updatePossibleDestinations();

                    return Result.ERROR_CHECK;
                }

                endDraw();
                return Result.SUCCESS;
            }
        }

        public Figur getFigur( Pos pos )
        {
            return m_schachbrett[pos.y][pos.x].figur;
        }

        private void endDraw()
        {
            drawing = (drawing == Farbe.WEISS ? Farbe.SCHWARZ : Farbe.WEISS);
        }
    }
}
