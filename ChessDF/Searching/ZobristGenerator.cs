using ChessDF.Core;
using ChessDF.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Searching
{
    public class ZobristGenerator
    {
        private readonly Random _rng;

        private ulong _blackToMove;
        private readonly ulong[,,] _pieceLocations = new ulong[2, 6, 64];
        private readonly ulong[] _castlingRights = new ulong[16];
        private readonly ulong[] _enPassantFiles = new ulong[8];

        public ZobristGenerator()
        {
            _rng = new Random();
            InitialiseRoots();
        }
        public ZobristGenerator(int seed)
        {
            _rng = new Random(seed);
            InitialiseRoots();
        }

        private void InitialiseRoots()
        {
            _blackToMove = RandomUlong();

            for (int s = 0; s < 2; s++)
            {
                for (int p = 0; p < 6; p++)
                {
                    for (int sq = 0; sq < 64; sq++)
                    {
                        _pieceLocations[s, p, sq] = RandomUlong();
                    }
                }
            }

            for (int c = 0; c < 16; c++)
            {
                _castlingRights[c] = RandomUlong();
            }

            for (int f = 0; f < 8; f++)
            {
                _enPassantFiles[f] = RandomUlong();
            }
        }

        private ulong RandomUlong()
        {
            byte[] bytes = new byte[8];
            _rng.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes);
        }

        public ulong GetHash(Position position)
        {
            ulong hash = 0;

            if (position.SideToMove == Side.Black)
                hash ^= _blackToMove;

            hash ^= _castlingRights[(int)position.CastlingRights];

            if (position.EnPassantSquare.HasValue)
                hash ^= _enPassantFiles[(int)position.EnPassantSquare % 8];

            for (int s = 0; s < 2; s++)
            {
                for (int p = 0; p < 6; p++)
                {
                    Bitboard pieces = position.Board[(Side)s, (Piece)p];
                    int[] pieceSqs = pieces.Serialize();

                    for (int i = 0; i < pieceSqs.Length; i++)
                    {
                        hash ^= _pieceLocations[s, p, pieceSqs[i]];
                    }
                }
            }

            return hash;
        }
    }
}
