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

        public bool IsEmpty() => _bits == 0;
        public int PopCount()
        {
            if (IsEmpty())
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

        public Bitboard FlipVertical() => throw new NotImplementedException();

        public List<int> Serialize()
        {
            var output = new List<int>(64);

            if (IsEmpty())
                return output;

            ulong x = _bits;
            while (x > 0)
            {
                ulong firstBit = x & (0 - x);
                int bitIndex = BitUtils.BitScanForward(firstBit);
                output.Add(bitIndex);

                x &= x - 1;
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

        private static string ToBinaryString(ulong number)
        {
            const ulong mask = 1;
            var binary = string.Empty;
            while (number > 0)
            {
                // Logical AND the number and prepend it to the result string
                binary = (number & mask) + binary;
                number >>= 1;
            }

            return binary;
        }

        public static implicit operator Bitboard(ulong bits) => new Bitboard(bits);
        public static implicit operator ulong(Bitboard bb) => bb._bits;
    }
}
