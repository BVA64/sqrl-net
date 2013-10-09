using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlMessageFacts
    {
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