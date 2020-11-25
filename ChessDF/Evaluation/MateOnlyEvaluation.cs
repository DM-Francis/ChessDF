using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Evaluation
{
    class MateOnlyEvaluation : IEvaluator
    {
        public double Evaluate(Position position)
        {
            if (position.IsInCheckmate())
                return -1;
            else
                return 0;
        }
    }
}
