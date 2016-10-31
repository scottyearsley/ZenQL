using System.Collections.Generic;

namespace ZenQL.Parsing
{
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

        public IReadOnlyCollection<string> Tokenize(string query)
        {
            var tokens = new List<string>();

            if (query == null)
            {
                return tokens.AsReadOnly();
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

            return tokens.AsReadOnly();
        }

        private string ProcessChar(char item, bool isLastChar)
        {
            if (_currentToken == null)
            {
                RefreshCurrentToken();
            }

            if (_currentToken.Length > 1 && _config.IsStartOfKnownOperator(item.ToString()))
            {
                var token = _currentToken;
                RefreshCurrentToken();
                _currentToken += item;
                return token;
            }

            if (_config.IsKnownOperator(_currentToken))
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

            if (_config.IsKnownOperator(_currentToken))
            {
                var token = _currentToken;
                RefreshCurrentToken();
                return token;
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
