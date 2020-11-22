using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    [Flags]
    public enum MoveFlags : byte
    {
        QuietMove = 0b0000,
        DoublePawnPush = 0b0001,
        KingCastle = 0b0010,
        QueenCastle = 0b0011,
        Capture = 0b0100,
        EnPassantCapture = 0b0101,
        KnightPromotion = 0b1000, // Promotions can be combined with captures
        BishopPromotion = 0b1001,
        RookPromotion = 0b1010,
        QueenPromotion = 0b1011
    }

    public readonly struct Move : IEquatable<Move>
    {
        private readonly ushort _moveData;

        public Move(Square from, Square to, MoveFlags flags)
        {
            _moveData = (ushort)(((byte)flags & 0xf) << 12 | ((byte)from & 0x3f) << 6 | ((byte)to & 0x3f));
        }

        public Square From => (Square)((_moveData >> 6) & 0x3f);
        public Square To => (Square)(_moveData & 0x3f);
        public MoveFlags Flags => (MoveFlags)(_moveData >> 12);

        public bool IsCapture => (Flags & MoveFlags.Capture) != 0;

        public override bool Equals(object? obj)
        {
            return obj is Move move && Equals(move);
        }

        public bool Equals(Move other)
        {
            return _moveData == other._moveData;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_moveData);
        }

        public static bool operator ==(Move left, Move right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !(left == right);
        }

        public override string ToString() => $"{From}->{To}";
    }
}
