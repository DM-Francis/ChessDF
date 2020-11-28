using ChessDF.Uci.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessDF.Test.Uci
{
    public class GoCommandTests
    {
        [Fact]
        public void CanParseGoCommandWithDepth()
        {
            // Assemble
            string[] commandArgs = "depth 5".Split(' ');

            // Act
            var command = new GoCommand(commandArgs);

            // Assert
            Assert.Equal(5, command.Depth);
        }
    }
}
