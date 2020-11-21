using ChessDF.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test
{
    public class BoardTests
    {
        [Fact]
        public void CanCreateBoard()
        {
            // Assemble
            var board = new Board();

            var wp = board.WhitePawns;
        }
    }
}
