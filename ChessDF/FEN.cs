using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDF
{
    public class FEN
    {
        public static readonly FEN StartingPosition = new FEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        public string PiecePlacement { get; }
        public Color ActiveColor { get; }
        public string CastlingAvailability { get; }
        public string EnpassantTargetSquare { get; }
        public int HalfmoveClock { get; }
        public int FullmoveNumber { get; }

        public FEN(string fenString)
        {
            string[] fenFields = fenString.Split(' ');

            if (fenFields.Length != 6)
            {
                throw new ArgumentException("Invalid FEN: Must contain all 6 space-separated fields.");
            }

            PiecePlacement = fenFields[0];
            ActiveColor = fenFields[1] switch
            {
                "w" => Color.White,
                "b" => Color.Black,
                _ => throw new ArgumentException("Invalid FEN: active color can only be 'w' or 'b'.")
            };
            CastlingAvailability = fenFields[2];
            EnpassantTargetSquare = fenFields[3];
            
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

        public override string ToString()
        {
            string colorString = ActiveColor switch
            {
                Color.White => "w",
                Color.Black => "b",
                _ => "?"
            };

            return string.Join(' ', new string[] { PiecePlacement, colorString, CastlingAvailability, EnpassantTargetSquare, HalfmoveClock.ToString(), FullmoveNumber.ToString() });
        }
    }
}
