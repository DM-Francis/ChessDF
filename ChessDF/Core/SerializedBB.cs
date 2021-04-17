using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Core
{
    public struct SerializedBB
    {
        public int Length { get; internal set; }
        public int this[int i]
        {
            get
            {
                return i switch
                {
                    0 => _0,
                    1 => _1,
                    2 => _2,
                    3 => _3,
                    4 => _4,
                    5 => _5,
                    6 => _6,
                    7 => _7,
                    8 => _8,
                    9 => _9,
                    10 => _10,
                    11 => _11,
                    12 => _12,
                    13 => _13,
                    14 => _14,
                    15 => _15,
                    16 => _16,
                    17 => _17,
                    18 => _18,
                    19 => _19,
                    20 => _20,
                    21 => _21,
                    22 => _22,
                    23 => _23,
                    24 => _24,
                    25 => _25,
                    26 => _26,
                    27 => _27,
                    28 => _28,
                    29 => _29,
                    30 => _30,
                    31 => _31,
                    32 => _32,
                    33 => _33,
                    34 => _34,
                    35 => _35,
                    36 => _36,
                    37 => _37,
                    38 => _38,
                    39 => _39,
                    40 => _40,
                    41 => _41,
                    42 => _42,
                    43 => _43,
                    44 => _44,
                    45 => _45,
                    46 => _46,
                    47 => _47,
                    48 => _48,
                    49 => _49,
                    50 => _50,
                    51 => _51,
                    52 => _52,
                    53 => _53,
                    54 => _54,
                    55 => _55,
                    56 => _56,
                    57 => _57,
                    58 => _58,
                    59 => _59,
                    60 => _60,
                    61 => _61,
                    62 => _62,
                    63 => _63,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
            set
            {
                switch (i)
                {
                    case 0:
                        _0 = value;
                        break;
                    case 1:
                        _1 = value;
                        break;
                    case 2:
                        _2 = value;
                        break;
                    case 3:
                        _3 = value;
                        break;
                    case 4:
                        _4 = value;
                        break;
                    case 5:
                        _5 = value;
                        break;
                    case 6:
                        _6 = value;
                        break;
                    case 7:
                        _7 = value;
                        break;
                    case 8:
                        _8 = value;
                        break;
                    case 9:
                        _9 = value;
                        break;
                    case 10:
                        _10 = value;
                        break;
                    case 11:
                        _11 = value;
                        break;
                    case 12:
                        _12 = value;
                        break;
                    case 13:
                        _13 = value;
                        break;
                    case 14:
                        _14 = value;
                        break;
                    case 15:
                        _15 = value;
                        break;
                    case 16:
                        _16 = value;
                        break;
                    case 17:
                        _17 = value;
                        break;
                    case 18:
                        _18 = value;
                        break;
                    case 19:
                        _19 = value;
                        break;
                    case 20:
                        _20 = value;
                        break;
                    case 21:
                        _21 = value;
                        break;
                    case 22:
                        _22 = value;
                        break;
                    case 23:
                        _23 = value;
                        break;
                    case 24:
                        _24 = value;
                        break;
                    case 25:
                        _25 = value;
                        break;
                    case 26:
                        _26 = value;
                        break;
                    case 27:
                        _27 = value;
                        break;
                    case 28:
                        _28 = value;
                        break;
                    case 29:
                        _29 = value;
                        break;
                    case 30:
                        _30 = value;
                        break;
                    case 31:
                        _31 = value;
                        break;
                    case 32:
                        _32 = value;
                        break;
                    case 33:
                        _33 = value;
                        break;
                    case 34:
                        _34 = value;
                        break;
                    case 35:
                        _35 = value;
                        break;
                    case 36:
                        _36 = value;
                        break;
                    case 37:
                        _37 = value;
                        break;
                    case 38:
                        _38 = value;
                        break;
                    case 39:
                        _39 = value;
                        break;
                    case 40:
                        _40 = value;
                        break;
                    case 41:
                        _41 = value;
                        break;
                    case 42:
                        _42 = value;
                        break;
                    case 43:
                        _43 = value;
                        break;
                    case 44:
                        _44 = value;
                        break;
                    case 45:
                        _45 = value;
                        break;
                    case 46:
                        _46 = value;
                        break;
                    case 47:
                        _47 = value;
                        break;
                    case 48:
                        _48 = value;
                        break;
                    case 49:
                        _49 = value;
                        break;
                    case 50:
                        _50 = value;
                        break;
                    case 51:
                        _51 = value;
                        break;
                    case 52:
                        _52 = value;
                        break;
                    case 53:
                        _53 = value;
                        break;
                    case 54:
                        _54 = value;
                        break;
                    case 55:
                        _55 = value;
                        break;
                    case 56:
                        _56 = value;
                        break;
                    case 57:
                        _57 = value;
                        break;
                    case 58:
                        _58 = value;
                        break;
                    case 59:
                        _59 = value;
                        break;
                    case 60:
                        _60 = value;
                        break;
                    case 61:
                        _61 = value;
                        break;
                    case 62:
                        _62 = value;
                        break;
                    case 63:
                        _63 = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        int _0;
        int _1;
        int _2;
        int _3;
        int _4;
        int _5;
        int _6;
        int _7;
        int _8;
        int _9;
        int _10;
        int _11;
        int _12;
        int _13;
        int _14;
        int _15;
        int _16;
        int _17;
        int _18;
        int _19;
        int _20;
        int _21;
        int _22;
        int _23;
        int _24;
        int _25;
        int _26;
        int _27;
        int _28;
        int _29;
        int _30;
        int _31;
        int _32;
        int _33;
        int _34;
        int _35;
        int _36;
        int _37;
        int _38;
        int _39;
        int _40;
        int _41;
        int _42;
        int _43;
        int _44;
        int _45;
        int _46;
        int _47;
        int _48;
        int _49;
        int _50;
        int _51;
        int _52;
        int _53;
        int _54;
        int _55;
        int _56;
        int _57;
        int _58;
        int _59;
        int _60;
        int _61;
        int _62;
        int _63;
    }
}
