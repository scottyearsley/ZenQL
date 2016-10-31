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
            return Operators.Any(o => o.StartsWith(item) && o.Length > 1);
        }
    }

    public class ContainerTokens
    {
        public string StartToken { get; }
        public string EndToken { get; }

        public ContainerTokens(string startToken, string endToken)
        {
            StartToken = startToken;
            EndToken = endToken;
        }
    }

    public class QueryTokenizer : IQueryTokenizer
    {
        private readonly QueryTokenizerConfig _config;
        private const char EmptyChar = ' ';
        private string _currentToken = null;
        private bool _inContainer;

        public QueryTokenizer(QueryTokenizerConfig config)
        {
            _config = config;
        }

        public string[] Tokenize(string query)
        {
            var tokens = new List<string>();

            if (query == null)
            {
                return tokens.ToArray();
            }

            for (int i = 0; i < query.Length; i++)
            {
                var item = query[i];
                var isLastChar = i == query.Length - 1;

                var result = ProcessChar(item, isLastChar);

                if (result != null)
                {
                    tokens.Add(result);
                }
            }

            return tokens.ToArray();
        }

        private string ProcessChar(char item, bool isLastChar)
        {
            if (_currentToken == null)
            {
                RefreshCurrentToken();
            }

            if (_currentToken.Length == 0 && _config.IsKnownOperator(item.ToString()))
            {
                var token = _currentToken;
                RefreshCurrentToken();
                return token;
            }

            if (_config.IsStartOfKnownOperator(item.ToString()))
            {
                var token = _currentToken;
                RefreshCurrentToken();
                _currentToken += item;
                return token;
            }

            // append char to current token string
            if (_inContainer || item != EmptyChar)
            {
                _currentToken += item;
            }

            if (_config.ContainsEndTokenOf(item) && _inContainer)
            {
                _inContainer = false;
                var token = _currentToken;
                RefreshCurrentToken();
                return token;
            }

            if (_config.ContainsStartTokenOf(item) && !_inContainer)
            {
                _inContainer = true;
            }

            // Token is complete, return
            if (_currentToken != "" && item == ' ' && !_inContainer)
            {
                var token = _currentToken;
                RefreshCurrentToken();
                return token;
            }

            if (IsLastToken(isLastChar))
            {
                return _currentToken;
            }

            return null;
        }

        private bool IsLastToken(bool isLastChar)
        {
            return _currentToken != null && (isLastChar && _currentToken.Length != 0);
        }

        private void RefreshCurrentToken()
        {
            _currentToken = "";
        }
    }
}
