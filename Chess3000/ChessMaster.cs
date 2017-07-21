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
        Pos enPassentPos;
        bool enPassentAllowed = false;
        Figur eligiblePawn1 = null;
        Figur eligiblePawn2 = null;

        public Pos EnPassentPos
        {
            get { return enPassentPos; }
        }

        public Figur EligiblePawn1
        {
            get { return eligiblePawn1; }
        }

        public Figur EligiblePawn2
        {
            get { return eligiblePawn2; }
        }

        public ChessMaster()
        {
            //Damit frühere Züge noch nachgelesen werden können
            Console.BufferHeight = (Int16)(Int16.MaxValue / 8.0);
            createInitialBoardState();

            /*    
             *    //Schachtest
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
               */
            /*
            //En passent test
            Result res;
            Console.WriteLine("#####################");
            res = move(new Pos(1, 1), new Pos(3, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(6, 6), new Pos(4, 6));
            Console.WriteLine(res.ToString());
            res = move(new Pos(3, 1), new Pos(4, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(6, 0), new Pos(4, 0));
            Console.WriteLine(res.ToString());
            Console.WriteLine("#####################");
            */
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

            updatePossibleDestinations();
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
                        foreach (Pos pos in feld.figur.CapDes)
                        {
                            //Equals() != ==-Operator
                            if (pos.Equals(kingPos)) { return true; }
                        }
                    }
                }
            }

            return false;
        }

        //Problem: schachbedrohte Felder auf denen gegnerische Figuren stehen werden nicht beachtet!
        public List<Pos> filterCheckedFields(List<Pos> posDes, Farbe kingColor)
        {
            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld field in m_schachbrett[y])
                {
                    if (field.figur != null && field.figur.Farbe != kingColor)
                    {
                        List<Pos> capDes = field.figur.CapDes;
                        //reverse loop wegen removeAt()
                        for (int i = posDes.Count - 1; i >= 0; i--)
                        {                            
                            if (capDes.Contains(posDes.ElementAt(i)))
                            {
                                posDes.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            return posDes;
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
                Figur enPassentPiece = null;

                m_schachbrett[to.y][to.x].figur = m_schachbrett[from.y][from.x].figur;
                m_schachbrett[to.y][to.x].figur.Feld = m_schachbrett[to.y][to.x];
                m_schachbrett[from.y][from.x].figur = null;

                Pos enPassentPiecePos = null;
                if (enPassentAllowed && to.Equals(enPassentPos))
                {                   
                    if (drawing == Farbe.WEISS)
                    {
                        enPassentPiecePos = new Pos(enPassentPos.y - 1, enPassentPos.x);
                    }
                    else
                    {
                        enPassentPiecePos = new Pos(enPassentPos.y + 1, enPassentPos.x);
                    }

                    enPassentPiece = m_schachbrett[enPassentPiecePos.y][enPassentPiecePos.x].figur;
                    m_schachbrett[enPassentPiecePos.y][enPassentPiecePos.x].figur = null;
                }

                if (check(drawing))
                {
                    //Änderungen rückgängig machen
                    m_schachbrett[from.y][from.x].figur = fromPiece;
                    m_schachbrett[from.y][from.x].figur.Feld = m_schachbrett[from.y][from.x];
                    m_schachbrett[to.y][to.x].figur = toPiece;

                    if (enPassentAllowed && to.Equals(enPassentPos))
                    {
                        m_schachbrett[enPassentPiecePos.y][enPassentPiecePos.x].figur = enPassentPiece;
                    }

                    updatePossibleDestinations();

                    return Result.ERROR_CHECK;
                }

                enPassentAllowed = checkForEnPassent(from, to);
                updatePossibleDestinations();

                endDraw();
                return Result.SUCCESS;
            }
        }

        private bool checkForEnPassent(Pos formerPos, Pos currentPos)
        {
            eligiblePawn1 = null;
            eligiblePawn2 = null;
            Figur piece = getFigur(currentPos);

            if (piece.PieceType == PieceType.Bauer)
            {
                if (drawing == Farbe.WEISS)
                {
                    if (currentPos.y == 3 && formerPos.y == 1)
                    {
                        if (currentPos.x + 1 <= 7)
                        {
                            eligiblePawn1 = getFigur(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            eligiblePawn2 = getFigur(new Pos(currentPos.y, currentPos.x - 1));
                        }

                        if (eligiblePawn1 != null || eligiblePawn2 != null)
                        {
                            enPassentPos = new Pos(currentPos.y - 1, currentPos.x);
                            return true;
                        }
                    }
                }
                else
                {
                    if (currentPos.y == 4 && formerPos.y == 6)
                    {
                        if (currentPos.x + 1 <= 7)
                        {
                            eligiblePawn1 = getFigur(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            eligiblePawn2 = getFigur(new Pos(currentPos.y, currentPos.x - 1));
                        }

                        if (eligiblePawn1 != null || eligiblePawn2 != null)
                        {
                            enPassentPos = new Pos(currentPos.y + 1, currentPos.x);
                            return true;
                        }
                    }
                }
            }
            return false;
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
