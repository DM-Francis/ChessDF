using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDF
{
    public class FEN
    {
        public static readonly FEN StartingPosition = new FEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        public string PiecePlacement { get; }
        public Side ActiveSide { get; }
        public CastlingRights CastlingAvailability { get; }
        public Square? EnpassantTargetSquare { get; }
        public int HalfmoveClock { get; }
        public int FullmoveNumber { get; }

        public FEN(string fenString)
        {
            fenString = fenString.Trim();
            string[] fenFields = fenString.Split(' ');

            if (fenFields.Length != 4 && fenFields.Length != 6)
            {
                throw new ArgumentException("Invalid FEN: Must contain either 4 or 6 space-separated fields.");
            }

            PiecePlacement = fenFields[0];
            ActiveSide = fenFields[1] switch
            {
                "w" => Side.White,
                "b" => Side.Black,
                _ => throw new ArgumentException($"Invalid FEN: {nameof(ActiveSide)} can only be 'w' or 'b'.")
            };
            CastlingAvailability = CreateCastlingRightsFromString(fenFields[2]);

            if (Enum.TryParse(fenFields[3], out Square enPassantSquare))
                EnpassantTargetSquare = enPassantSquare;
            else
                EnpassantTargetSquare = null;

            if (fenFields.Length == 4)
                return;
            
            if (!int.TryParse(fenFields[4], out int halfmove) || halfmove < 0)
            {
                throw new ArgumentException("Invalid FEN: Halfmove Clock must be a nonnegative integer.");
            }

            HalfmoveClock = halfmove;

            if (!int.TryParse(fenFields[5], out int fullmove) || fullmove <= 0)
            {
                throw new ArgumentException("Invalid FEN: Fullmove number must be a positive integer.");
            }

            FullmoveNumber = fullmove;
        }

        public FEN(string piecePlacement, Side activeSide, CastlingRights castlingAvailability, Square? enpassantTargetSquare, int halfmoveClock, int fullmoveNumber)
        {
            PiecePlacement = piecePlacement;
            ActiveSide = activeSide;
            CastlingAvailability = castlingAvailability;
            EnpassantTargetSquare = enpassantTargetSquare;
            HalfmoveClock = halfmoveClock;
            FullmoveNumber = fullmoveNumber;
        }

        public override string ToString()
        {
            string colorString = ActiveSide switch
            {
                Side.White => "w",
                Side.Black => "b",
                _ => "?"
            };

            string castling = CreateStringFromCastlingRights(CastlingAvailability);
            string enPassantString = EnpassantTargetSquare?.ToString() ?? "-";

            return string.Join(' ', new string[] { PiecePlacement, colorString, castling, enPassantString, HalfmoveClock.ToString(), FullmoveNumber.ToString() });
        }

        private static CastlingRights CreateCastlingRightsFromString(string castlingRightsString)
        {
            CastlingRights whiteKing = castlingRightsString.Contains('K') ? CastlingRights.WhiteKingSide : 0;
            CastlingRights whiteQueen = castlingRightsString.Contains('Q') ? CastlingRights.WhiteQueenSide : 0;
            CastlingRights blackKing = castlingRightsString.Contains('k') ? CastlingRights.BlackKingSide : 0;
            CastlingRights blackQueen = castlingRightsString.Contains('q') ? CastlingRights.BlackQueenSide : 0;

            return whiteKing | whiteQueen | blackKing | blackQueen;
        }

        private static string CreateStringFromCastlingRights(CastlingRights castlingRights)
        {
            string output = "";
            if (castlingRights.HasFlag(CastlingRights.WhiteKingSide)) output += 'K';
            if (castlingRights.HasFlag(CastlingRights.WhiteQueenSide)) output += 'Q';
            if (castlingRights.HasFlag(CastlingRights.BlackKingSide)) output += 'k';
            if (castlingRights.HasFlag(CastlingRights.BlackQueenSide)) output += 'q';

            if (string.IsNullOrEmpty(output))
                return "-";
            else
                return output;            
        }
    }
}
