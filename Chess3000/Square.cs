namespace Chess3000
{
    public class Square
    {
        public Square(Pos coord)
        {
            coordinate = coord;
            piece = null;
        }

        public Pos Coordinate
        {
            get { return coordinate; }
        }

        private Pos coordinate;
        public Piece piece;
    }
}
