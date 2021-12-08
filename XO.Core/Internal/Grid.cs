﻿using System.Collections;

namespace XO.Core.Internal
{
    internal class Grid : IReadOnlyGrid
    {
        public Grid()
            => rows = new[]
            {
                new Symbol?[3],
                new Symbol?[3],
                new Symbol?[3],
            };

        public Symbol? this[Position position]
        {
            get => rows[position.Row][position.Column];
            set => rows[position.Row][position.Column] = value;
        }

        public IEnumerable<Position> FreePositions
        {
            get
            {
                for (var rowIndex = 0; rowIndex < 3; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < 3; columnIndex++)
                    {
                        var position = new Position(rowIndex, columnIndex);

                        if (this[position] is null)
                            yield return position;
                    }
                }
            }
        }

        public bool IsFilled()
            => this.All(cell => cell is not null);

        public Symbol?[] GetRow(int index)
            => rows[index];

        public Symbol?[] GetColumn(int index)
            => new[]
            {
                rows[0][index],
                rows[1][index],
                rows[2][index],
            };

        /// <summary> Get diagonal from top left end to bottom right end. </summary>
        public IEnumerable<Symbol?> GetDiagonal(Position position)
        {
            // Set position to top left diagonal end.
            while (position.Row > 0 && position.Column > 0)
                position = new Position(position.Row - 1, position.Column - 1);

            // Yield positions from top left end to bottom right end.
            while (position.Row <= 2 && position.Column <= 2)
            {
                yield return this[position];

                position = new Position(position.Row + 1, position.Column + 1);
            }
        }

        /// <summary> Get diagonal from top right end to bottom left end. </summary>
        public IEnumerable<Symbol?> GetAntidiagonal(Position position)
        {
            // Set position to top right diagonal end.
            while (position.Row > 0 && position.Column < 2)
                position = new Position(position.Row - 1, position.Column + 1);

            // Yield positions from top right end to bottom left end.
            while (position.Row <= 2 && position.Column >= 0)
            {
                yield return this[position];

                position = new Position(position.Row + 1, position.Column - 1);
            }
        }

        public IEnumerator<Symbol?> GetEnumerator()
            => rows[0]
                .Concat(rows[1])
                .Concat(rows[2])
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private readonly Symbol?[][] rows;
    }
}
