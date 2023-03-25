namespace Chess.Engine
{
    /// <summary>
    /// A Position in the grid
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// X axis (Coloumn)
        /// </summary>
        
        private int X { get; set; }

        /// <summary>
        /// Y axis (Row)
        /// </summary>
        private int Y { get; set; }

        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}