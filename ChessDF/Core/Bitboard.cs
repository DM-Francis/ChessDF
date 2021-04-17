using ChessDF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public readonly struct Bitboard : IEquatable<Bitboard>
    {
        private readonly ulong _bits;
        private const ulong _notAFile = 0xfefefefefefefefe;
        private const ulong _notHFile = 0x7f7f7f7f7f7f7f7f;

        public Bitboard(ulong bits)
        {
            _bits = bits;
        }

        public static Bitboard FromSquare(Square square) => (ulong)1 << (int)square;
        public static Bitboard FromSquare(Square? square)
        {
            if (square is not null)
                return (ulong)1 << (int)square;
            else
                return 0;
        }

        public bool IsEmpty => _bits == 0;

        public int PopCount()
        {
            if (IsEmpty)
                return 0;

            int count = 0;
            ulong x = _bits;
            while (x > 0)
            {
                count++;
                x &= x - 1;
            }
            return count;
        }

        public Bitboard FlipVertical()
        {
            ulong k1 = 0x00FF00FF00FF00FF;
            ulong k2 = 0x0000FFFF0000FFFF;
            ulong x = _bits;
            x = ((x >> 8) & k1) | ((x & k1) << 8);
            x = ((x >> 16) & k2) | ((x & k2) << 16);
            x = (x >> 32) | (x << 32);
            return x;
        }

        public Bitboard MirrorHorizontal()
        {
            ulong k1 = 0x5555555555555555;
            ulong k2 = 0x3333333333333333;
            ulong k4 = 0x0f0f0f0f0f0f0f0f;
            ulong x = _bits;
            x = ((x >> 1) & k1) | ((x & k1) << 1);
            x = ((x >> 2) & k2) | ((x & k2) << 2);
            x = ((x >> 4) & k4) | ((x & k4) << 4);
            return x;
        }

        public SerializedBB Serialize()
        {
            var output = new SerializedBB
            {
                Length = PopCount()
            };

            if (IsEmpty)
                return output;

            int i = 0;
            for (ulong x = _bits; x > 0; x &= x - 1)
            {
                ulong firstBit = x & (0 - x);
                int bitIndex = BitUtils.BitScanForward(firstBit);
                output[i++] = bitIndex;
            }

            return output;
        }

        public Bitboard[] IndividualBits()
        {
            int i = 0;
            var output = new Bitboard[PopCount()];
            for (ulong x = _bits; x > 0; x &= x - 1)
            {
                output[i++] = x & (0 - x);
            }

            return output;
        }

        public Bitboard SoutOne() => _bits >> 8;
        public Bitboard NortOne() => _bits << 8;
        public Bitboard EastOne() => (_bits << 1) & _notAFile;
        public Bitboard NoEaOne() => (_bits << 9) & _notAFile;
        public Bitboard SoEaOne() => (_bits >> 7) & _notAFile;
        public Bitboard WestOne() => (_bits >> 1) & _notHFile;
        public Bitboard SoWeOne() => (_bits >> 9) & _notHFile;
        public Bitboard NoWeOne() => (_bits << 7) & _notHFile;

        public override bool Equals(object? obj) => obj is Bitboard bitboard && Equals(bitboard);
        public bool Equals(Bitboard other) => _bits == other._bits;
        public override int GetHashCode() => HashCode.Combine(_bits);
        public static bool operator ==(Bitboard left, Bitboard right) => left.Equals(right);
        public static bool operator !=(Bitboard left, Bitboard right) => !(left == right);
        public static Bitboard operator ~(Bitboard bitboard) => new Bitboard(~bitboard._bits);
        public static Bitboard operator <<(Bitboard left, int right) => new Bitboard(left._bits << right);
        public static Bitboard operator >>(Bitboard left, int right) => new Bitboard(left._bits >> right);
        public static Bitboard operator &(Bitboard left, Bitboard right) => new Bitboard(left._bits & right._bits);
        public static Bitboard operator ^(Bitboard left, Bitboard right) => new Bitboard(left._bits ^ right._bits);
        public static Bitboard operator |(Bitboard left, Bitboard right) => new Bitboard(left._bits | right._bits);

        public override string ToString()
        {
            string hex = _bits.ToString("x16");

            var builder = new StringBuilder("0x");

            for (int i = 0; i < 8; i++)
            {
                builder.Append(hex.Substring(i * 2, 2));

                if (i < 7)
                    builder.Append('_');
            }

            return builder.ToString();
        }

        public static implicit operator Bitboard(ulong bits) => new Bitboard(bits);
        public static implicit operator ulong(Bitboard bb) => bb._bits;
    }
}
