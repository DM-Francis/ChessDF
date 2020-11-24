using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDF.Uci.Commands
{
    class IdCommand : Command
    {
        public override string CommandName => "id";

        public string? Name { get; }
        public string? Author { get; }

        public IdCommand(IdCommandType idType, string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            if (idType == IdCommandType.Name)
                Name = value;
            else if (idType == IdCommandType.Author)
                Author = value;
            else
                throw new ArgumentOutOfRangeException(nameof(idType));
        }

        public override string ToString()
        {
            if (Name is not null)
                return $"{CommandName} name {Name}";
            else if (Author is not null)
                return $"{CommandName} author {Author}";

            throw new InvalidOperationException("Both name and author values are null");
        }
    }

    enum IdCommandType
    {
        Name, Author
    }
}
