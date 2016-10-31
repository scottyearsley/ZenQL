using System.Collections.Generic;

namespace ZenQL.Parsing
{
    /// <summary>
    /// Query tokenizer.
    /// </summary>
    public interface IQueryTokenizer
    {
        /// <summary>
        /// Parses a query into key tokens.
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns>A readonly collection of tokens.</returns>
        IReadOnlyCollection<string> Tokenize(string query);
    }
}