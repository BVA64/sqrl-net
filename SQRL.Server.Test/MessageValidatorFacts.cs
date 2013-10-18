using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace SQRL.Server.Test
{
    public class MessageValidatorFacts
    {
        public class Validate
        {
            private readonly MessageValidator _validator = new MessageValidator();

            private SqrlMessage _message = new SqrlMessage
                {
                    Uri = new Uri(SampleData.Url),
                    SignatureBase64 = SampleData.Signature,
                    PublicKeyBase64 = SampleData.PublicKey,
                    Version = "1"
                };

            private readonly Mock<ISqrlAuthenticationHandlerFactory> _mockFactory = new Mock<ISqrlAuthenticationHandlerFactory>();
            private readonly Mock<ISqrlAuthenticationHandler> _mockHandler = new Mock<ISqrlAuthenticationHandler>();

            public Validate()
            {
                _mockFactory.Setup(m => m.Create()).Returns(_mockHandler.Object);
                _mockHandler.Setup(m => m.VerifySession(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
                
                SqrlConfig.AuthenticationHandlerFactory = _mockFactory.Object;
            }

            [Fact]
            public void ValidSignatureReturnsSucceeds()
            {
                _mockHandler.Setup(
                    m => m.AuthenticateSession(SampleData.PublicKey, It.IsAny<string>(), It.IsAny<string>()));

                _validator.Validate(_message);

                _mockHandler.VerifyAll();
            }

            [Fact]
            public void InvalidUrlReturnsThrows()
            {
                _message.Uri = new Uri(_message.Uri + "B");

                Action act = () => _validator.Validate(_message);

                act.ShouldThrow<Exception>()
                   .And.Message.Should().Be("Signature verification failed.");
            }

            [Fact]
            public void InvalidSignatureReturnsThrows()
            {
                var sig = Convert.FromBase64String(SampleData.Signature);
                sig[0] ^= 0x01;
                _message.SignatureBase64 = Convert.ToBase64String(sig);

                Action act = () => _validator.Validate(_message);

                act.ShouldThrow<Exception>()
                   .And.Message.Should().Be("Signature verification failed.");
            }

            [Fact]
            public void InvalidPublicKeyReturnsThrows()
            {
                var key = Convert.FromBase64String(SampleData.PublicKey);
                key[0] ^= 0x01;
                _message.PublicKeyBase64= Convert.ToBase64String(key);

                Action act = () => _validator.Validate(_message);

                act.ShouldThrow<Exception>()
                   .And.Message.Should().Be("Signature verification failed.");
            }
        } 
    }
}