using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Evaluation
{
    public interface IEvaluator
    {
        double Evaluate(Position position);
    }
}
