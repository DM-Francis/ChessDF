using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class BestMoveCommand : Command
    {
        public override string CommandName => "bestmove";

        public string MoveString { get; }

        public BestMoveCommand(Move move)
        {
            if (move.IsPromotion)
            {
                char promoChar = move.PromotionPiece switch
                {
                    Piece.Bishop => 'b',
                    Piece.Knight => 'n',
                    Piece.Rook => 'r',
                    Piece.Queen => 'q',
                    _ => throw new InvalidOperationException($"Invalid piece for promotion '{move.PromotionPiece}'")
                };

                MoveString = $"{move.From}{move.To}{promoChar}";
            }
            else
            {
                MoveString = $"{move.From}{move.To}";
            }
        }

        public override string ToString()
        {
            return $"bestmove {MoveString}";
        }
    }
}
