using System.Collections.Generic;
using System.Linq;

namespace ZenQL.Parsing
{
    public class QueryTokenizerConfig
    {
        public IEnumerable<ContainerTokens> ContainerTokens { get; }
        public IEnumerable<string> Operators { get; }

        public QueryTokenizerConfig(IEnumerable<ContainerTokens> containerTokens, IEnumerable<string> operators)
        {
            ContainerTokens = containerTokens;
            Operators = operators;
        }

        public bool ContainsStartTokenOf(char item)
        {
            return ContainerTokens.Any(t => t.StartToken == item.ToString());
        }

        public bool ContainsEndTokenOf(char item)
        {
            return ContainerTokens.Any(t => t.EndToken == item.ToString());
        }

        public bool IsKnownOperator(string item)
        {
            return Operators.Any(o => o == item);
        }

        public bool IsStartOfKnownOperator(string item)
        {
            return Operators.Any(o => o.StartsWith(item));
        }
    }
}