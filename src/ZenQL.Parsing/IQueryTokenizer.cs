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
        /// <returns>an array of tokens.</returns>
        string[] Tokenize(string query);
    }
}