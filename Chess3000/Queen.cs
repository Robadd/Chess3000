namespace Chess3000
{
    class Queen : Piece
    {
        public Queen(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.Queen;
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();

            for (int y = Pos.y + 1; y <= 7; y++)
            {
                if (!addToPosDes(new Pos(y, Pos.x))) { break; }
            }

            for (int y = Pos.y - 1; y >= 0; y--)
            {
                if (!addToPosDes(new Pos(y, Pos.x))) { break; }
            }

            for (int x = Pos.x + 1; x <= 7; x++)
            {
                if (!addToPosDes(new Pos(Pos.y, x))) { break; }
            }

            for (int x = Pos.x - 1; x >= 0; x--)
            {
                if (!addToPosDes(new Pos(Pos.y, x))) { break; }
            }

            for (int y = Pos.y + 1, x = Pos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y - 1, x = Pos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y + 1, x = Pos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }

            for (int y = Pos.y - 1, x = Pos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                if (!addToPosDes(new Pos(y, x))) { break; }
            }
        }
    }
}
