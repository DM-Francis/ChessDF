using ChessDF.Core;
using ChessDF.Magics;
using ChessDF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Moves
{
    public static class SlidingPieceMoves
    {
        private static readonly MagicDetails[] _bishopMagics = new MagicDetails[64];
        private static readonly MagicDetails[] _rookMagics = new MagicDetails[64];

        private static readonly Bitboard[] _bishopMasks = new Bitboard[64];
        private static readonly Bitboard[] _rookMasks = new Bitboard[64];

        private static readonly Bitboard[][] _rookAttacks = new Bitboard[64][];
        private static readonly Bitboard[][] _bishopAttacks = new Bitboard[64][];

        private static int GetAttackIndex(Bitboard occupied, Bitboard magic, int bits)
            => (int)((occupied * magic) >> (64 - bits));

        public static Bitboard RookAttacks(Square square) => RookAttacks(square, 0);
        public static Bitboard BishopAttacks(Square square) => BishopAttacks(square, 0);
        public static Bitboard QueenAttacks(Square square) => RookAttacks(square) | BishopAttacks(square);

        public static Bitboard RookAttacks(Square square, Bitboard occupied)
        {
            MagicDetails magic = _rookMagics[(int)square];
            Bitboard mask = _rookMasks[(int)square];
            occupied &= mask;

            int attackIndex = GetAttackIndex(occupied, magic.Magic, magic.IndexSize);
            return _rookAttacks[(int)square][attackIndex];
        }

        public static Bitboard BishopAttacks(Square square, Bitboard occupied)
        {
            MagicDetails magic = _bishopMagics[(int)square];
            Bitboard mask = _bishopMasks[(int)square];
            occupied &= mask;

            int attackIndex = GetAttackIndex(occupied, magic.Magic, magic.IndexSize);
            return _bishopAttacks[(int)square][attackIndex];
        }

        public static Bitboard QueenAttacks(Square square, Bitboard occupied) => RookAttacks(square, occupied) | BishopAttacks(square, occupied);

        public static Bitboard AllRookAttacks(Bitboard rooks, Bitboard occupied)
        {
            Bitboard attacks = 0;
            var rooksSerialized = rooks.Serialize();

            for (int i = 0; i < rooksSerialized.Length; i++)
            {
                attacks |= RookAttacks((Square)rooksSerialized[i], occupied);
            }

            return attacks;
        }

        public static Bitboard AllBishopAttacks(Bitboard bishops, Bitboard occupied)
        {
            Bitboard attacks = 0;
            var bishopsSerialized = bishops.Serialize();

            for (int i = 0; i < bishopsSerialized.Length; i++)
            {
                attacks |= BishopAttacks((Square)bishopsSerialized[i], occupied);
            }

            return attacks;
        }

        public static Bitboard AllQueenAttacks(Bitboard queens, Bitboard occupied)
        {
            Bitboard attacks = 0;
            var queensSerialized = queens.Serialize();

            for (int i = 0; i < queensSerialized.Length; i++)
            {
                attacks |= QueenAttacks((Square)queensSerialized[i], occupied);
            }

            return attacks;
        }

        static SlidingPieceMoves()
        {
            var magics = MagicGenerator.LoadMagics();

            foreach (var magicDetail in magics)
            {
                if (magicDetail.Piece == Piece.Bishop)
                {
                    _bishopMagics[(int)magicDetail.Square] = magicDetail;
                    _bishopAttacks[(int)magicDetail.Square] = MagicGenerator.CreateAttacksMapForMagic(magicDetail);
                }
                else if (magicDetail.Piece == Piece.Rook)
                {
                    _rookMagics[(int)magicDetail.Square] = magicDetail;
                    _rookAttacks[(int)magicDetail.Square] = MagicGenerator.CreateAttacksMapForMagic(magicDetail);
                }
            }

            for (int s = 0; s < 64; s++)
            {
                _bishopMasks[s] = MagicGenerator.BishopMask((Square)s);
                _rookMasks[s] = MagicGenerator.RookMask((Square)s);
            }
        }        
    }
}
