using ChessDF.Core;
using ChessDF.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Searching
{
    public class ZobristTests
    {
        [Fact]
        public void CanGenerateHashForStartingPosition()
        {
            // Assemble
            var generator = new ZobristGenerator();
            var startpos = Position.StartPosition;
            var pos1 = Position.FromFENString("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1 ");

            // Act
            ulong hashStart = generator.GetHash(startpos);
            ulong hash1 = generator.GetHash(pos1);

            // Assert
            Assert.NotEqual(hashStart, hash1);
        }
    }
}
