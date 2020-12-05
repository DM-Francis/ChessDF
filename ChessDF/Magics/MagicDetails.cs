using ChessDF.Core;
using System;
using System.Globalization;

namespace ChessDF.Magics
{
    public record MagicDetails(Bitboard Magic, int IndexSize, Piece Piece, Square Square)
    {
        public override string ToString()
        {
            return $"{Piece} {Square} {Magic} {IndexSize}";
        }

        public static MagicDetails FromString(string @string)
        {
            string[] items = @string.Split(' ');
            Piece piece = Enum.Parse<Piece>(items[0]);
            Square square = Enum.Parse<Square>(items[1]);
            string magicHex = items[2][2..].Replace("_", "");
            Bitboard magic = ulong.Parse(magicHex, NumberStyles.HexNumber);
            int indexSize = int.Parse(items[3]);

            return new MagicDetails(magic, indexSize, piece, square);
        }
    }
}
