using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlMessageFacts
    {
         public class NonceBytes
         {
             [Fact]
             public void ThrowsWhenUriIsNull()
             {
                 string str;
                 var msg = new SqrlMessage {Uri = null};
                 Action act = () => str = msg.Nonce;

                 act.ShouldThrow<InvalidOperationException>();
             }

             [Fact]
             public void ReturnsTheNonceFromUri()
             {
                 var msg = new SqrlMessage {Uri = new Uri(SampleData.Url)};

                 string actual = msg.Nonce;

                 actual.Should().BeEquivalentTo(SampleData.Nonce);
             }

             [Fact]
             public void ReturnsEmptyStringWhenNoNonceIsFound()
             {
                 var msg = new SqrlMessage {Uri = new Uri("http://example.com")};

                 string nonce = msg.Nonce;
                 
                 nonce.Should().NotBeNull().And.BeEmpty();
             }
         }

        public class SignatureBytes
        {
            [Fact]
            public void ReturnsTheSignatureBytes()
            {
                var expected = Convert.FromBase64String(SampleData.Signature);
                var msg = new SqrlMessage {SignatureBase64 = SampleData.Signature};

                byte[] actual = msg.SignatureBytes;

                actual.Should().BeEquivalentTo(expected);
            } 
        }

        public class PublicKeyBytes
        {
            [Fact]
            public void ReturnsThePublicKeyBytes()
            {
                var expected = Convert.FromBase64String(SampleData.PublicKey);
                var msg = new SqrlMessage {PublicKeyBase64 = SampleData.PublicKey};

                byte[] actual = msg.PublicKeyBytes;

                actual.Should().BeEquivalentTo(expected);
            } 
        }
    }
}