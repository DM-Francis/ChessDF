using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public record Position
    {
        public Position(Board board, Side sideToMove, Square? enPassantSquare, CastlingRights castlingRights, int halfmoveClock)
        {
            Board = board;
            SideToMove = sideToMove;
            EnPassantSquare = enPassantSquare;
            CastlingRights = castlingRights;
            HalfmoveClock = halfmoveClock;

            _kingIsInCheck = new Lazy<bool>(() => Mover.KingIsInCheck(sideToMove, board));
            _legalMoves = new Lazy<List<Move>>(() => MoveGenerator.GetAllMoves(this));
        }

        private readonly Lazy<bool> _kingIsInCheck;
        private readonly Lazy<List<Move>> _legalMoves;
        private bool NoLegalMoves => _legalMoves.Value.Count == 0;
        
        
        public List<Move> GetAllLegalMoves() => _legalMoves.Value;
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
        public Board Board { get; init; }
        public Side SideToMove { get; init; }
        public Square? EnPassantSquare { get; init; }
        public CastlingRights CastlingRights { get; init; }
        public int HalfmoveClock { get; init; }

        public string ToFENString()
            => new FEN(Board.ToPiecePlacementString(), SideToMove, CastlingRights, EnPassantSquare, HalfmoveClock, FullMoveNumber).ToString();

        public bool IsInCheckmate()
        {
            return _kingIsInCheck.Value && NoLegalMoves;
        }

        public bool IsInStalemate()
        {
            if (Board[SideToMove, Piece.King] == 0)
                return false;

            return !_kingIsInCheck.Value && NoLegalMoves;
        }
    }
}
