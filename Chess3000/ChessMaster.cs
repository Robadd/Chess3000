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
        Square[][] chessboard;
        Chess3000.Color drawing = Color.White;
        Pos enPassantPos = null;
        Pos enPassantPiecePos = null;
        Piece enPassantPiece = null;
        bool enPassantAllowed = false;
        Piece eligiblePawn1 = null;
        Piece eligiblePawn2 = null;
        bool whiteKingMoved = false;
        bool blackKingMoved = false;
        bool whiteShortRookMoved = false;
        bool blackShortRookMoved = false;
        bool whiteLongRookMoved = false;
        bool blackLongRookMoved = false;
        readonly Pos White_KING_START_POS = new Pos(0, 4);
        readonly Pos Black_KING_START_POS = new Pos(7, 4);
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
        Pos lastFrom;
        Pos lastTo;

        public Pos EnPassantPos
        {
            get { return enPassantPos; }
        }

        public Piece EligiblePawn1
        {
            get { return eligiblePawn1; }
        }

        public Piece EligiblePawn2
        {
            get { return eligiblePawn2; }
        }

        public ChessMaster()
        {
#if DEBUG
            //Damit frühere Züge noch nachgelesen werden können
            Console.BufferHeight = (Int16)(Int16.MaxValue / 4.0);
#endif
            createInitialBoardState();
        }

#if DEBUG
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

            res = move(new Pos(1, 1), new Pos(2, 1)); //Pawn
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 7), new Pos(5, 7)); //Pawn schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(1, 2), new Pos(3, 2)); //Pawn
            Console.WriteLine(res.ToString());

            res = move(new Pos(5, 7), new Pos(4, 7)); //Pawn schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 1), new Pos(2, 2)); //Knight
            Console.WriteLine(res.ToString());

            res = move(new Pos(4, 7), new Pos(3, 7)); //Pawn schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 2), new Pos(1, 1)); //Läufer
            Console.WriteLine(res.ToString());

            res = move(new Pos(3, 7), new Pos(2, 7)); //Pawn schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 3), new Pos(1, 2)); //Queen
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 6), new Pos(5, 6)); //Pawn schwarz
            Console.WriteLine(res.ToString());

            Console.WriteLine("#####################");
        }

        private void castlingCheckTest()
        {
            Result res;
            Console.WriteLine("#####################");

            res = move(new Pos(1, 3), new Pos(2, 3)); //Bauer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 6), new Pos(5, 6)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(1, 4), new Pos(3, 4)); //Bauer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 0), new Pos(5, 0)); //Bauer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 1), new Pos(2, 0)); //Springer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(7, 0), new Pos(6, 0)); //Turm schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 2), new Pos(2, 4)); //Läufer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 0), new Pos(7, 0)); //Turm schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(2, 4), new Pos(3, 3)); //Läufer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(7, 0), new Pos(6, 0)); //Turm schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 3), new Pos(1, 4)); //Dame weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(6, 0), new Pos(7, 0)); //Turm schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 6), new Pos(2, 7)); //Springer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(7, 5), new Pos(5, 7)); //Läufer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(1, 6), new Pos(2, 6)); //Springer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(5, 7), new Pos(7, 5)); //Läufer schwarz
            Console.WriteLine(res.ToString());

            res = move(new Pos(0, 5), new Pos(1, 6)); //Springer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(7, 0), new Pos(6, 0)); //Turm schwarz
            Console.WriteLine(res.ToString());
           
            res = move(new Pos(1, 1), new Pos(2, 1)); //Bauer weiß
            Console.WriteLine(res.ToString());

            res = move(new Pos(7, 5), new Pos(5, 7)); //Läufer schwarz
            Console.WriteLine(res.ToString());
            

            Console.WriteLine("#####################");
        }
#endif

        public Chess3000.Color Drawing
        {
            get { return drawing; }
        }

        //Achtung: Am Anfang null!
        public Pos LastFrom
        {
            get { return lastFrom; }
        }

        //Achtung: Am Anfang null!
        public Pos LastTo
        {
            get { return lastTo; }
        }

        private void createInitialBoardState()
        {
            drawing = Color.White;
            enPassantPos = null;
            enPassantPiecePos = null;
            enPassantPiece = null;
            enPassantAllowed = false;
            eligiblePawn1 = null;
            eligiblePawn2 = null;
            whiteKingMoved = false;
            blackKingMoved = false;
            whiteShortRookMoved = false;
            blackShortRookMoved = false;
            whiteLongRookMoved = false;
            blackLongRookMoved = false;
            lastFrom = null;
            lastTo = null;
            chessboard = new Square[8][];

            for (int y = 0; y < 8; y++)
            {
                chessboard[y] = new Square[8];
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    chessboard[y][x] = new Square(new Pos(y, x));
                }
            }

            for (int x = 0; x <= 7; x++)
            {
                chessboard[1][x].piece = new Pawn(Chess3000.Color.White, chessboard[1][x], this);
                chessboard[6][x].piece = new Pawn(Chess3000.Color.Black, chessboard[6][x], this);
            }

            chessboard[0][0].piece = new Rook(Chess3000.Color.White, chessboard[0][0], this);
            chessboard[0][7].piece = new Rook(Chess3000.Color.White, chessboard[0][7], this);
            chessboard[7][0].piece = new Rook(Chess3000.Color.Black, chessboard[7][0], this);
            chessboard[7][7].piece = new Rook(Chess3000.Color.Black, chessboard[7][7], this);

            chessboard[0][1].piece = new Knight(Chess3000.Color.White, chessboard[0][1], this);
            chessboard[0][6].piece = new Knight(Chess3000.Color.White, chessboard[0][6], this);
            chessboard[7][1].piece = new Knight(Chess3000.Color.Black, chessboard[7][1], this);
            chessboard[7][6].piece = new Knight(Chess3000.Color.Black, chessboard[7][6], this);

            chessboard[0][2].piece = new Bishop(Chess3000.Color.White, chessboard[0][2], this);
            chessboard[0][5].piece = new Bishop(Chess3000.Color.White, chessboard[0][5], this);
            chessboard[7][2].piece = new Bishop(Chess3000.Color.Black, chessboard[7][2], this);
            chessboard[7][5].piece = new Bishop(Chess3000.Color.Black, chessboard[7][5], this);

            chessboard[0][3].piece = new Queen(Chess3000.Color.White, chessboard[0][3], this);
            chessboard[7][3].piece = new Queen(Chess3000.Color.Black, chessboard[7][3], this);

            chessboard[0][4].piece = new King(Chess3000.Color.White, chessboard[0][4], this);
            chessboard[7][4].piece = new King(Chess3000.Color.Black, chessboard[7][4], this);

            updatePossibleDestinations();
        }

        private void updatePossibleDestinations()
        {
#if DEBUG
            Console.WriteLine("********************************************************");
#endif
            for (int y = 0; y <= 7; y++)
            {
                foreach (Square square in chessboard[y])
                {
                    if (square.piece != null)
                    {
                        square.piece.updatePosDes();
#if DEBUG
                        Console.WriteLine("Piece: " + square.piece.PieceType.ToString());
                        Console.WriteLine("Color: " + square.piece.Color.ToString());
                        Console.WriteLine("Position: " + square.piece.currentPosString);
                        Console.WriteLine("Mögliche Ziele:");
                        Console.WriteLine(square.piece.PosDesString);
                        Console.WriteLine("");
#endif
                    }
                }
            }
        }
        
        Pos getKingPosition(Chess3000.Color player)
        {
            Pos kingPos = new Pos(-1,-1);
            for (int y = 0; y <= 7; y++)
            {
                foreach (Square square in chessboard[y])
                {
                    if (square.piece != null && square.piece.PieceType == PieceType.King && square.piece.Color == player)
                    {
                        kingPos = square.Coordinate;
                    }
                }
            }
            return kingPos;
        }

        bool foundPiece(Chess3000.Color color, Chess3000.PieceType pieceType, Pos currentPos)
        {
            Piece piece = getPiece(new Pos(currentPos.y, currentPos.x));
            return (
                piece != null &&
                piece.Color == color &&
                piece.PieceType == pieceType
                );
        }

        public bool check(Chess3000.Color kingColor, Pos posInQuestion)
        {
            for (int y = 0; y <= 7; y++)
            {
                foreach (Square square in chessboard[y])
                {
                    if (square.piece != null && square.piece.Color != kingColor)
                    {
                        foreach (Pos pos in square.piece.CapDes)
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
        public List<Pos> filterCheckedFields(List<Pos> posDes, Color kingColor)
        {
            for (int y = 0; y <= 7; y++)
            {
                foreach (Square field in chessboard[y])
                {
                    if (field.piece != null && field.piece.Color != kingColor)
                    {
                        List<Pos> capDes = field.piece.CapDes;
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

        public void reset()
        {
            createInitialBoardState();
        }

        public Chess3000.Result move(Pos from, Pos to)
        {
            Piece fromPiece = getPiece(from);
            if (fromPiece == null) { return Result.ERROR_NULL_PIECE; }
            else if (fromPiece.Color != drawing) { return Result.ERROR_WRONG_COLOR; }
            else if (!fromPiece.validDes(to)) { return Result.ERROR_INVALID_DES; }
            else
            {
                //Sicherung falls check() true liefert
                Piece toPiece = getPiece(to);

                draw(from, to);
                //muss vor updatePossibleDestinations() ausgeführt werden

                updateCastlingAvailability(fromPiece, from);

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

                if (fromPiece.PieceType == PieceType.Pawn && (to.y == 7 || to.y == 0))
                {
                    promote(to);
                }

                handleCastling(fromPiece, from, to);

                updateEnPassantAvailability(from, to);

                endTurn(from, to);

                return Result.SUCCESS;
            }
        }

        private void setPiece(Pos pos, Piece piece)
        {
            if (piece != null)
            {
                piece.Square = chessboard[pos.y][pos.x];
            }

            chessboard[pos.y][pos.x].piece = piece;
        }

        private void draw(Pos from, Pos to)
        {
            setPiece(to, chessboard[from.y][from.x].piece);
            setPiece(from, null);
        }

        //Änderungen rückgängig machen
        private void reverse(Pos from, Pos to, Piece fromPiece, Piece toPiece)
        {
            setPiece(from, fromPiece);
            setPiece(to, toPiece);
        }

        private bool handleCastling(Piece fromPiece, Pos from, Pos to)
        {
            bool toWhiteCastlingPos = to.Equals(WHITE_KING_SHORT_CASTLING_POS) ||
                                      to.Equals(WHITE_KING_LONG_CASTLING_POS);
            bool whiteCastling = fromPiece.PieceType == PieceType.King &&
                                 from.Equals(White_KING_START_POS) &&
                                 toWhiteCastlingPos;

            bool toBlackCastlingPos = to.Equals(BLACK_KING_SHORT_CASTLING_POS) ||
                                      to.Equals(BLACK_KING_LONG_CASTLING_POS);
            bool blackCastling = fromPiece.PieceType == PieceType.King &&
                                 from.Equals(Black_KING_START_POS) &&
                                 toBlackCastlingPos;

            if (whiteCastling || blackCastling)
            {
                castlingSecondStep(to);

                return true;
            }

            return false;
        }

        //Schachprüfung hier nicht nötig, da dies bereits in King.checkForCastling() durchgeführt wurde
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

        private bool updateCastlingAvailability(Piece fromPiece, Pos from)
        {
            bool situationChanged = false;
            if (drawing == Color.White)
            {
                if (fromPiece.PieceType == PieceType.King &&
                    from.Equals(White_KING_START_POS))
                {
                    whiteKingMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Rook &&
                    from.Equals(WHITE_ROOK_SHORT_START_POS))
                {
                    whiteShortRookMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Rook &&
                    from.Equals(WHITE_ROOK_LONG_START_POS))
                {
                    whiteLongRookMoved = true;
                    situationChanged = true;
                }
            }
            else
            {
                if (fromPiece.PieceType == PieceType.King &&
                    from.Equals(Black_KING_START_POS))
                {
                    blackKingMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Rook &&
                    from.Equals(BLACK_ROOK_SHORT_START_POS))
                {
                    blackShortRookMoved = true;
                    situationChanged = true;
                }

                if (fromPiece.PieceType == PieceType.Rook &&
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

        public bool WhiteShortCastlingPiecesNotMoved
        {
            get { return !whiteKingMoved && !whiteShortRookMoved; }
        }

        public bool BlackShortCastlingPiecesNotMoved
        {
            get { return !blackKingMoved && !blackShortRookMoved; }
        }

        public bool WhiteLongCastlingPiecesNotMoved
        {
            get { return !whiteKingMoved && !whiteLongRookMoved; }
        }

        public bool BlackLongCastlingPiecesNotMoved
        {
            get { return !blackKingMoved && !blackLongRookMoved; }
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
                chessboard[pos.y][pos.x].piece = new Queen(drawing,
                                                           chessboard[pos.y][pos.x],
                                                           this);
            }
            else
            {
                chessboard[pos.y][pos.x].piece = new Knight(drawing,
                                                            chessboard[pos.y][pos.x],
                                                            this);
            }
            updatePossibleDestinations();
        }

        private bool handleEnPassant(Piece fromPiece, Pos from, Pos to)
        {
            enPassantPiece = null;
            enPassantPiecePos = null;

            bool isEligiblePawn = EligiblePawn1 != null && fromPiece.Equals(EligiblePawn1) ||
                                  EligiblePawn2 != null && fromPiece.Equals(EligiblePawn2);

            if (enPassantAllowed && to.Equals(enPassantPos) && isEligiblePawn)
            {
                if (drawing == Color.White)
                {
                    enPassantPiecePos = new Pos(enPassantPos.y - 1, enPassantPos.x);
                }
                else
                {
                    enPassantPiecePos = new Pos(enPassantPos.y + 1, enPassantPos.x);
                }

                enPassantPiece = getPiece(enPassantPiecePos);

                //beim En Passent wird eine Piece entfernt auf deren Position nicht gezogen wurde
                setPiece(enPassantPiecePos, null);

                return true;
            }

            return false;
        }

        private bool updateEnPassantAvailability(Pos formerPos, Pos currentPos)
        {
            eligiblePawn1 = null;
            eligiblePawn2 = null;
            Piece piece = getPiece(currentPos);

            if (piece.PieceType == PieceType.Pawn)
            {
                if (drawing == Color.White)
                {
                    if (currentPos.y == 3 && formerPos.y == 1)
                    {
                        if (currentPos.x + 1 <= 7)
                        {
                            //Kann null werden
                            eligiblePawn1 = getPiece(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            //Kann null werden
                            eligiblePawn2 = getPiece(new Pos(currentPos.y, currentPos.x - 1));
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
                            eligiblePawn1 = getPiece(new Pos(currentPos.y, currentPos.x + 1));
                        }
                        if (currentPos.x - 1 >= 0)
                        {
                            //Kann null werden
                            eligiblePawn2 = getPiece(new Pos(currentPos.y, currentPos.x - 1));
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

        public Piece getPiece(Pos pos)
        {
            return chessboard[pos.y][pos.x].piece;
        }

        private void endTurn(Pos from, Pos to)
        {
            lastFrom = from;
            lastTo = to;
            drawing = (drawing == Color.White ? Color.Black : Color.White);
        }
    }
}
