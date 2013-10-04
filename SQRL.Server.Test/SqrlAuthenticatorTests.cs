using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlAuthenticatorTests
    {
        public class Verify
        {
            private const string SampleUrl = "sqrl://example.com/sqrl?ABABABABABABABABA";
            private const string SampleSignature = "1u8roTU8lCMTyO5nD5wZWC1BXfI8rAKhtL+my7Cg6Iymaa+sX4pB2Fxp1hiv4u1FwJGIv57P6GLnsYI6gh0HDHNxcmw6Ly9leGFtcGxlLmNvbS9zcXJsP0FCQUJBQkFCQUJBQkFCQUJB";
            private const string SamplePublicKey = "bIg2mYMSUex88y3+UEkCoqOemZPIpGbTXLNWgGSr/JI=";
            private const string SamplePrivateKey = "xua3skX9oroME46sBXYC3Aj6QSg3yxiJkkrCiTxtHApsiDaZgxJR7HzzLf5QSQKio56Zk8ikZtNcs1aAZKv8kg==";

            [Fact]
            public void ThrowsIfUrlIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(null, SampleSignature, SamplePublicKey);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("url");
            }

            [Fact]
            public void ThrowsIfSignatureIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(SampleUrl, null, SamplePublicKey);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("signature");
            }

            [Fact]
            public void ThrowsIfPublicKeyIsNull()
            {
                Action act = () => SqrlAuthenticator.Verify(SampleUrl, SampleSignature, null);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("publicKey");
            }

            [Fact]
            public void ValidSignatureReturnsTrue()
            {
                bool actual = SqrlAuthenticator.Verify(SampleUrl, SampleSignature, SamplePublicKey);

                actual.Should().BeTrue();
            }

            [Fact]
            public void InvalidUrlReturnsFalse()
            {
                bool actual = SqrlAuthenticator.Verify(SampleUrl + "B", SampleSignature, SamplePublicKey);

                actual.Should().BeFalse();
            }

            [Fact]
            public void InvalidSignatureReturnsFalse()
            {
                var sig = Convert.FromBase64String(SampleSignature);
                sig[0]++;
                bool actual = SqrlAuthenticator.Verify(SampleUrl, Convert.ToBase64String(sig), SamplePublicKey);

                actual.Should().BeFalse();
            }

            [Fact]
            public void InvalidPublicKeyReturnsFalse()
            {
                var key = Convert.FromBase64String(SamplePublicKey);
                key[0]++;

                bool actual = SqrlAuthenticator.Verify(SampleUrl, SampleSignature, Convert.ToBase64String(key));

                actual.Should().BeFalse();
            }
        }
    }
}