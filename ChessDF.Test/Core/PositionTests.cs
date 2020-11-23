using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Core
{
    public class PositionTests
    {
        [Fact]
        public void FromFENStringIsConsistentWithToFENString()
        {
            // Assemble
            string initialFenString = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 4 10";

            // Act
            var position = Position.FromFENString(initialFenString);
            var fenString = position.ToFENString();

            // Assert
            Assert.Equal(initialFenString, fenString);
        }
    }
}
