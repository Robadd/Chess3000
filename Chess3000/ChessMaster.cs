using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        Pos enPassantPos;
        Pos enPassantPiecePos = null;
        Figur enPassantPiece = null;
        bool enPassantAllowed = false;
        Figur eligiblePawn1 = null;
        Figur eligiblePawn2 = null;
        public bool whiteKingMoved = false;
        public bool blackKingMoved = false;
        public bool whiteShortRookMoved = false;
        public bool blackShortRookMoved = false;
        public bool whiteLongRookMoved = false;
        public bool blackLongRookMoved = false;
        readonly Pos WHITE_KING_START_POS = new Pos(0, 4);
        readonly Pos BLACK_KING_START_POS = new Pos(7, 4);
        readonly Pos WHITE_ROOK_SHORT_START_POS = new Pos(0, 7);
        readonly Pos BLACK_ROOK_SHORT_START_POS = new Pos(7, 7);
        readonly Pos WHITE_ROOK_LONG_START_POS = new Pos(0, 0);
        readonly Pos BLACK_ROOK_LONG_START_POS = new Pos(7, 0);
        readonly Pos WHITE_KING_SHORT_CASTLING_POS = new Pos(0, 6);
        readonly Pos BLACK_KING_SHORT_CASTLING_POS = new Pos(7, 6);
        readonly Pos WHITE_KING_LONG_CASTLING_POS = new Pos(0, 2);
        readonly Pos BLACK_KING_LONG_CASTLING_POS = new Pos(7, 2);
        readonly Pos WHITE_ROOK_SHORT_CASTLING_POS = new Pos(0, 5);
        readonly Pos BLACK_ROOK_SHORT_CASTLING_POS = new Pos(7, 5);
        readonly Pos WHITE_ROOK_LONG_CASTLING_POS = new Pos(0, 3);
        readonly Pos BLACK_ROOK_LONG_CASTLING_POS = new Pos(7, 3);

        public Pos EnPassantPos
        {
            get { return enPassantPos; }
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

            enPassantTest();
        }

        private void checkTest()
        {
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

        private void enPassantTest()
        {
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
        }

        private void promotionTest()
        {
            Result res;
            Console.WriteLine("#####################");
            res = move(new Pos(1, 1), new Pos(3, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(6, 6), new Pos(4, 6));
            Console.WriteLine(res.ToString());
            res = move(new Pos(3, 1), new Pos(4, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(6, 5), new Pos(4, 5));
            Console.WriteLine(res.ToString());
            res = move(new Pos(4, 1), new Pos(5, 1));
            Console.WriteLine(res.ToString());
            res = move(new Pos(4, 5), new Pos(3, 5));
            Console.WriteLine(res.ToString());
            res = move(new Pos(5, 1), new Pos(6, 0));
            Console.WriteLine(res.ToString());
            res = move(new Pos(3, 5), new Pos(2, 5));
            Console.WriteLine(res.ToString());
            Console.WriteLine("#####################");
        }

        private void castlingTest()
        {
            Result res;
            Console.WriteLine("#####################");

            res = move(new Pos(1, 1), new Pos(2, 1)); //Bauer
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 7), new Pos(5, 7)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(1, 2), new Pos(3, 2)); //Bauer
            Console.WriteLine(res.ToString());

            res = move(new Pos(5, 7), new Pos(4, 7)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 1), new Pos(2, 2)); //Springer
            Console.WriteLine(res.ToString());

            res = move(new Pos(4, 7), new Pos(3, 7)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 2), new Pos(1, 1)); //Läufer
            Console.WriteLine(res.ToString());

            res = move(new Pos(3, 7), new Pos(2, 7)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 3), new Pos(1, 2)); //Dame
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 6), new Pos(5, 6)); //Bauer schwarz
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

        public bool check(Chess3000.Farbe kingColor, Pos posInQuestion)
        {
            for (int y = 0; y <= 7; y++)
            {
                foreach (Feld feld in m_schachbrett[y])
                {
                    if (feld.figur != null && feld.figur.Farbe != kingColor)
                    {
                        foreach (Pos pos in feld.figur.CapDes)
                        {
                            //Equals() != ==-Operator
                            if (pos.Equals(posInQuestion)) { return true; }
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
            Figur fromPiece = getFigur(from);
            if (fromPiece == null) { return Result.ERROR_NULL_PIECE; }
            else if (fromPiece.Farbe != drawing) { return Result.ERROR_WRONG_COLOR; }
            else if (!fromPiece.validDes(to)) { return Result.ERROR_INVALID_DES; }
            else
            {
                Figur toPiece = getFigur(to);
                draw(from, to);
                bool enPassantPerformed = handleEnPassant(fromPiece, from, to);
                
                updatePossibleDestinations();

                //Eigener König würde im Schach stehen
                if (check(drawing, getKingPosition(drawing)))
                {
                    //Änderungen rückgängig machen
                    reverse(from, to, fromPiece, toPiece);
                    if (enPassantPerformed)
                    {
                        setPiece(enPassantPiecePos, enPassantPiece);
                    }

                    updatePossibleDestinations();

                    return Result.ERROR_CHECK;
                }

                if (fromPiece.PieceType == PieceType.Bauer && (to.y == 7 || to.y == 0))
                {
                    promote(to);
                }

                handleCastling(fromPiece, from, to);

                updateCastlingAvailability(fromPiece, from);
                updateEnPassantAvailability(from, to);

                endTurn();

                return Result.SUCCESS;
            }
        }

        private void setPiece(Pos pos, Figur piece)
        {
            if (piece != null)
            {
                piece.Feld = m_schachbrett[pos.y][pos.x];
            }

            m_schachbrett[pos.y][pos.x].figur = piece;
        }

        private void draw(Pos from, Pos to)
        {
            setPiece(to, m_schachbrett[from.y][from.x].figur);
            setPiece(from, null);
        }

        //Änderungen rückgängig machen
        private void reverse(Pos from, Pos to, Figur fromPiece, Figur toPiece)
        {
            setPiece(from, fromPiece);
            setPiece(to, toPiece);
        }

        private bool handleCastling(Figur fromPiece, Pos from, Pos to)
        {
            bool toWhiteCastlingPos = to.Equals(WHITE_KING_SHORT_CASTLING_POS) ||
                                      to.Equals(WHITE_KING_LONG_CASTLING_POS);
            bool whiteCastling = fromPiece.PieceType == PieceType.Koenig &&
                                 from.Equals(WHITE_KING_START_POS) &&
                                 toWhiteCastlingPos;

            bool toBlackCastlingPos = to.Equals(BLACK_KING_SHORT_CASTLING_POS) ||
                                      to.Equals(BLACK_KING_LONG_CASTLING_POS);
            bool blackCastling = fromPiece.PieceType == PieceType.Koenig &&
                                 from.Equals(BLACK_KING_START_POS) &&
                                 toBlackCastlingPos;

            if (whiteCastling || blackCastling)
            {
                castlingSecondStep(to);

                return true;
            }

            return false;
        }

        //Schachprüfung hier nicht nötig, da dies bereits in Koenig.checkForCastling() durchgeführt wurde
        private void castlingSecondStep(Pos kingCastlingPos)
        {
            if (kingCastlingPos.Equals(WHITE_KING_SHORT_CASTLING_POS))
            {
                draw(WHITE_ROOK_SHORT_START_POS, WHITE_ROOK_SHORT_CASTLING_POS);               
            }
            else if (kingCastlingPos.Equals(WHITE_KING_LONG_CASTLING_POS))
            {
                draw(WHITE_ROOK_LONG_START_POS, WHITE_ROOK_LONG_CASTLING_POS);
            }
            else if (kingCastlingPos.Equals(BLACK_KING_SHORT_CASTLING_POS))
            {
                draw(BLACK_ROOK_SHORT_START_POS, BLACK_ROOK_SHORT_CASTLING_POS);
            }
            else if (kingCastlingPos.Equals(BLACK_KING_LONG_CASTLING_POS))
            {
                draw(BLACK_ROOK_LONG_START_POS, BLACK_ROOK_LONG_CASTLING_POS);
            }
            updatePossibleDestinations();
        }

        private bool updateCastlingAvailability(Figur fromPiece, Pos from)
        {
            bool situationChanged = false;
            if (drawing == Farbe.WEISS)
            {
                if (fromPiece.PieceType == PieceType.Koenig &&
                    from.Equals(WHITE_KING_START_POS))
                {
                    whiteKingMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Turm &&
                    from.Equals(WHITE_ROOK_SHORT_START_POS))
                {
                    whiteShortRookMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Turm &&
                    from.Equals(WHITE_ROOK_LONG_START_POS))
                {
                    whiteLongRookMoved = true;
                    situationChanged = true;
                }
            }
            else
            {
                if (fromPiece.PieceType == PieceType.Koenig &&
                    from.Equals(BLACK_KING_START_POS))
                {
                    blackKingMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Turm &&
                    from.Equals(BLACK_ROOK_SHORT_START_POS))
                {
                    blackShortRookMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Turm &&
                    from.Equals(BLACK_ROOK_LONG_START_POS))
                {
                    blackLongRookMoved = true;
                    situationChanged = true;
                }
            }
            if (situationChanged)
            {
                updatePossibleDestinations();
                return true;
            }
            else { return false; }
        }

        private void promote(Pos pos)
        {
            MessageBoxResult res = MessageBox.Show(
                "Soll der Bauer durch eine Dame ersetzt werden?\n" +
                "Wenn nicht wird er durch einen Springer ersetzt",
                "Beförderung",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );

            if (res == MessageBoxResult.Yes)
            {
                m_schachbrett[pos.y][pos.x].figur = new Dame(drawing,
                                                             m_schachbrett[pos.y][pos.x],
                                                             this);
            }
            else
            {
                m_schachbrett[pos.y][pos.x].figur = new Springer(drawing,
                                                                 m_schachbrett[pos.y][pos.x],
                                                                 this);
            }
            updatePossibleDestinations();
        }

        private bool handleEnPassant(Figur fromPiece, Pos from, Pos to)
        {
            enPassantPiece = null;
            enPassantPiecePos = null;

            bool isEligiblePawn = EligiblePawn1 != null && fromPiece.Equals(EligiblePawn1) ||
                                  EligiblePawn2 != null && fromPiece.Equals(EligiblePawn2);

            if (enPassantAllowed && to.Equals(enPassantPos) && isEligiblePawn)
            {
                if (drawing == Farbe.WEISS)
                {
                    enPassantPiecePos = new Pos(enPassantPos.y - 1, enPassantPos.x);
                }
                else
                {
                    enPassantPiecePos = new Pos(enPassantPos.y + 1, enPassantPos.x);
                }

                enPassantPiece = getFigur(enPassantPiecePos);

                //beim En Passent wird eine Figur entfernt auf deren Position nicht gezogen wurde
                setPiece(enPassantPiecePos, null);

                return true;
            }

            return false;
        }

        private bool updateEnPassantAvailability(Pos formerPos, Pos currentPos)
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
                            //Kann null werden
                            eligiblePawn1 = getFigur(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            //Kann null werden
                            eligiblePawn2 = getFigur(new Pos(currentPos.y, currentPos.x - 1));
                        }

                        if (eligiblePawn1 != null || eligiblePawn2 != null)
                        {
                            enPassantPos = new Pos(currentPos.y - 1, currentPos.x);
                            enPassantAllowed = true;
                            updatePossibleDestinations();
                            return enPassantAllowed;
                        }
                    }
                }
                else
                {
                    if (currentPos.y == 4 && formerPos.y == 6)
                    {
                        if (currentPos.x + 1 <= 7)
                        {
                            //Kann null werden
                            eligiblePawn1 = getFigur(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            //Kann null werden
                            eligiblePawn2 = getFigur(new Pos(currentPos.y, currentPos.x - 1));
                        }

                        if (eligiblePawn1 != null || eligiblePawn2 != null)
                        {
                            enPassantPos = new Pos(currentPos.y + 1, currentPos.x);
                            enPassantAllowed = true;
                            updatePossibleDestinations();
                            return enPassantAllowed;
                        }
                    }
                }
            }

            enPassantAllowed = false;
            updatePossibleDestinations(); //Nötig?
            return enPassantAllowed;
        }

        public Figur getFigur( Pos pos )
        {
            return m_schachbrett[pos.y][pos.x].figur;
        }

        private void endTurn()
        {
            drawing = (drawing == Farbe.WEISS ? Farbe.SCHWARZ : Farbe.WEISS);
        }
    }
}
