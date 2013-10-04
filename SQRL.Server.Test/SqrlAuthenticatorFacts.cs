using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlAuthenticatorFacts
    {
        public class Verify
        {
            [Fact]
            public void ThrowsIfUrlIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(null, SampleData.Signature, SampleData.PublicKey);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("url");
            }

            [Fact]
            public void ThrowsIfSignatureIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(SampleData.Url, null, SampleData.PublicKey);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("signature");
            }

            [Fact]
            public void ThrowsIfPublicKeyIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(SampleData.Url, SampleData.Signature, null);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("publicKey");
            }

            [Fact]
            public void ValidSignatureReturnsTrue()
            {
                bool actual = SqrlAuthenticator.Verify(SampleData.Url, SampleData.Signature, SampleData.PublicKey);

                actual.Should().BeTrue();
            }

            [Fact]
            public void InvalidUrlReturnsFalse()
            {
                bool actual = SqrlAuthenticator.Verify(SampleData.Url + "B", SampleData.Signature, SampleData.PublicKey);

                actual.Should().BeFalse();
            }

            [Fact]
            public void InvalidSignatureReturnsFalse()
            {
                var sig = Convert.FromBase64String(SampleData.Signature);
                sig[0]++;
                bool actual = SqrlAuthenticator.Verify(SampleData.Url, Convert.ToBase64String(sig), SampleData.PublicKey);

                actual.Should().BeFalse();
            }

            [Fact]
            public void InvalidPublicKeyReturnsFalse()
            {
                var key = Convert.FromBase64String(SampleData.PublicKey);
                key[0]++;

                bool actual = SqrlAuthenticator.Verify(SampleData.Url, SampleData.Signature, Convert.ToBase64String(key));

                actual.Should().BeFalse();
            }
        }
    }
}