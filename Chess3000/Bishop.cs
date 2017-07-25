namespace Chess3000
{
    class Bishop : Piece
    {
        public Bishop(Color color, Square square, ChessMaster chessMaster) : base(color, square, chessMaster)
        {
            pieceType = Chess3000.PieceType.Bishop;
        }

        public override void updatePosDes()
        {
            possibleDestinations.Clear();

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


/*
        private void posDesEmptyBoard()
        {
            for (int y = pos.y + 1, x = pos.x + 1; y <= 7 && x <= 7; y++, x++)
            {
                possibleDestinations.Add(new Pos(y, x));
            }

            for (int y = pos.y - 1, x = pos.x - 1; y >= 0 && x >= 0; y--, x--)
            {
                possibleDestinations.Add(new Pos(y, x));
            }

            for (int y = pos.y + 1, x = pos.x - 1; y <= 7 && x >= 0; y++, x--)
            {
                possibleDestinations.Add(new Pos(y, x));
            }

            for (int y = pos.y - 1, x = pos.x + 1; y >= 0 && x <= 7; y--, x++)
            {
                possibleDestinations.Add(new Pos(y, x));
            }
        }
        */
    }
}
