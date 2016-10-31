namespace ZenQL.Parsing
{
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
}