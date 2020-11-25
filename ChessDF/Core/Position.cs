using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public record Position(Board Board, Side SideToMove, Square? EnPassantSquare, CastlingRights CastlingRights, int HalfmoveClock)
    {
        public int FullMoveNumber { get; init; } = 1;

        public Side OpposingSide => SideToMove switch
        {
            Side.White => Side.Black,
            Side.Black => Side.White,
            _ => throw new IndexOutOfRangeException(nameof(SideToMove))
        };

        public static Position FromFENString(string fenString)
        {
            var fen = new FEN(fenString);
            var board = Board.FromPiecePlacementString(fen.PiecePlacement);

            return new Position(board, fen.ActiveSide, fen.EnpassantTargetSquare, fen.CastlingAvailability, fen.HalfmoveClock)
            {
                FullMoveNumber = fen.FullmoveNumber
            };
        }

        public static Position StartPosition => FromFENString(FEN.StartingPosition.ToString());

        public string ToFENString()
            => new FEN(Board.ToPiecePlacementString(), SideToMove, CastlingRights, EnPassantSquare, HalfmoveClock, FullMoveNumber).ToString();


        public bool IsInCheckmate()
        {
            bool isInCheck = Mover.KingIsInCheck(SideToMove, Board);
            bool noLegalMoves = MoveGenerator.GetAllMoves(this).Count == 0;

            return isInCheck && noLegalMoves;
        }
    }
}
