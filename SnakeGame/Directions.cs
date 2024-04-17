

using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public class Directions
    {
        public readonly static Directions Left = new Directions(0, -1);
        public readonly static Directions Right = new Directions(0, 1);
        public readonly static Directions Up = new Directions(-1, 0);
        public readonly static Directions Down = new Directions(1, 0);
        public int RowOffset { get; }
        public int ColumnOffset { get; }

        private Directions(int rowoffset, int columnoffset)
        {
            RowOffset = rowoffset;
            ColumnOffset = columnoffset;
        }

        public Directions Opposite()
        {
            return new Directions(-RowOffset, -ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Directions directions &&
                   RowOffset == directions.RowOffset &&
                   ColumnOffset == directions.ColumnOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColumnOffset);
        }

        public static bool operator ==(Directions left, Directions right)
        {
            return EqualityComparer<Directions>.Default.Equals(left, right);
        }

        public static bool operator !=(Directions left, Directions right)
        {
            return !(left == right);
        }

    }
}
