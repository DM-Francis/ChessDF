using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public readonly struct Move : IEquatable<Move>
    {
        private readonly uint _moveData;

        public Move(Square from, Square to, MoveFlags flags)
        {
            _moveData = ((uint)flags & 0xf) << 12 | ((uint)from & 0x3f) << 6 | ((uint)to & 0x3f);
        }

        public Square From => (Square)((_moveData >> 6) & 0x3f);
        public Square To => (Square)(_moveData & 0x3f);
        public MoveFlags Flags => (MoveFlags)((_moveData >> 12) & 0xf);

        public bool IsCapture => (Flags & MoveFlags.Capture) != 0;
        public bool IsPromotion => (Flags & MoveFlags.KnightPromotion) != 0;
        public bool IsAnyCastle => Flags == MoveFlags.KingCastle || Flags == MoveFlags.QueenCastle;

        public override bool Equals(object? obj) => obj is Move move && Equals(move);
        public bool Equals(Move other) => _moveData == other._moveData;
        public override int GetHashCode() => HashCode.Combine(_moveData);
        public static bool operator ==(Move left, Move right) => left.Equals(right);
        public static bool operator !=(Move left, Move right) => !(left == right);
        public override string ToString() => IsCapture ? $"{From}x{To}" : $"{From}->{To}";
    }
}
