using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ZenQL.Parsing.Specs
{
    public class QueryTokenizerSpecs
    {
        public abstract class TokenizerSpec
        {
            protected IReadOnlyCollection<string> Run(string query)
            {
                var config = new QueryTokenizerConfig(
                    new []
                    {
                        new ContainerTokens("\"", "\""),
                        new ContainerTokens("'", "'"),
                    },
                    new [] { "=", "!=", ">", ">=", "<", "<=", "~", "!~" }
                );
                var sut = new QueryTokenizer(config);
                return sut.Tokenize(query);
            }
        }

        public class SinglePart : TokenizerSpec
        {
            [Test]
            public void ShouldReturnSingleToken()
            {
                const string expected = "name";
                var result = Run(expected).ToArray();

                Assert.That(result, Has.Length.EqualTo(1));
                Assert.That(result[0], Is.EqualTo(expected));
            }
        }

        public class EmptyQuery : TokenizerSpec
        {
            [Test]
            public void ShouldReturnNoTokens()
            {
                var result = Run("   ");

                Assert.That(result, Has.Count.EqualTo(0));
            }
        }

        public class NullQuery : TokenizerSpec
        {
            [Test]
            public void ShouldReturnNoTokens()
            {
                var result = Run(null);

                Assert.That(result, Has.Count.EqualTo(0));
            }
        }

        public class SinglePartAndOperator : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name =");

                Assert.That(result, Has.Count.EqualTo(2));
            }
        }

        public class TwoPartsAndOperator : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name = test").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("test"));
            }
        }

        public class TwoPartsAndOperatorExtraWhitespace : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run(" name   = test ").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("test"));
            }
        }

        public class PropertyInDoubleQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("\"First name\" = Ken").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("\"First name\""));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("Ken"));
            }
        }

        public class PropertyInSingleQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("'First name' = Ken").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("'First name'"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("Ken"));
            }
        }

        public class ValueInDoubleQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name = \"Ken Dodd\"").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("\"Ken Dodd\""));
            }
        }

        public class ValueInSingleQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name = 'Ken Dodd'").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("'Ken Dodd'"));
            }
        }

        public class OperatorCharBunchedWithNoQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name=Ken").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("Ken"));
            }
        }

        public class OperatorCharBunchedWithQuotes : TokenizerSpec
        {
            [Test]
            public void ShouldReturnTokens()
            {
                var result = Run("name='Ken dodd'").ToArray();

                Assert.That(result, Has.Length.EqualTo(3));
                Assert.That(result[0], Is.EqualTo("name"));
                Assert.That(result[1], Is.EqualTo("="));
                Assert.That(result[2], Is.EqualTo("Ken"));
            }
        }
    }
}
