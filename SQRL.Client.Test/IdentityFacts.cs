using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace SQRL.Client.Test
{
    public class IdentityFacts
    {
        private static readonly byte[] Entropy = new byte[]
            {
                0x37, 0x2e, 0xfc, 0xf9, 0xb4, 0x0b, 0x35, 0xc2
                , 0x11, 0x5b, 0x13, 0x46, 0x90, 0x3d, 0x2e, 0xf4
                , 0x2f, 0xce, 0xd4, 0x6f, 0x08, 0x46, 0xe7, 0x25
                , 0x7b, 0xb1, 0x56, 0xd3, 0xd7, 0xb3, 0x0d, 0x3f
            };

        private const string Name = "Identity #1";
        private const string Password = "password1";
        private const string Domain = "example.com";
        private const string Data = "AAEAAAD/////AQAAAAAAAAAMAgAAAEJTUVJMLkNsaWVudCwgVmVyc2lvbj0xLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAACJTUVJMLkNsaWVudC5JZGVudGl0eStJZGVudGl0eVN0b3JlBAAAABo8TWFzdGVyS2V5PmtfX0JhY2tpbmdGaWVsZBU8U2FsdD5rX19CYWNraW5nRmllbGQZPFZlcmlmaWVyPmtfX0JhY2tpbmdGaWVsZBs8SXRlcmF0aW9ucz5rX19CYWNraW5nRmllbGQHBwcAAgICCAIAAAAJAwAAAAkEAAAACQUAAAAABAAADwMAAAAgAAAAAt4uR4dxlOuP5uVFRscJPa6hyz+nBAakfKF3sPt3EujGDwQAAAAAAQAAAg3IPB/gmAr9Ep0ffogLu+Zf4C7XkE3gElTTGbaH1ygLZA3yWJSgS6ThAGh6GAqPuOoB3KSx3x6EVUxtf4cppHEGaaNB0RXb/4HcCwo8YaJCcpd/XZIa6IKI1bZgT/ZFAmOoZXLw856dIizpfUgksCedsNEBAnqmQCu3ngdBbB5vH1cFDHUsVQV18LNc0vBvDZ1N7JPzPcbhqQgYnY0mZR77DJBD4snqG+6PbYtyJC51BNhz8jDhxmUyO3GGpOXkPcq07KKPz65WdYFp2Qivx9eKI5RoesJGa1DomrhSCkncxIrNnXLtMEVe16zXTqStrPJ5/eR2F3P6bXqM6hclUm8PBQAAABAAAAACigEz0k36O6eK20xtU4WB7AsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";

        static IdentityFacts()
        {
            Identity.StorageProvider = new Mock<IIdentityStorageProvider>().Object;
        }

        public class CreateNew
        {
            static CreateNew()
            {
                new IdentityFacts();
            }

            [Fact]
            public void CreatesNewIdentity()
            {
                Identity id = Identity.CreateNew(Name, Password, Entropy);

                id.Should().NotBeNull();
            }

            [Fact]
            public void ThrowsArgumentNullIfNameIsNull()
            {
                Identity id;
                Action act = () => id = Identity.CreateNew(null, Password, Entropy);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("name");
            }

            [Fact]
            public void ThrowsArgumentNullIfPasswordIsNull()
            {
                Identity id;
                Action act = () => id = Identity.CreateNew(Name, null, Entropy);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("password");
            }

            [Fact]
            public void ThrowsArgumentNullIfEntropyIsNull()
            {
                Identity id;
                Action act = () => id = Identity.CreateNew(Name, Password, null);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("entropy");
            }
        } 

        public class Open
        {
            static Open()
            {
                new IdentityFacts();
            }

            [Fact]
            public void CreatesNewIdentity()
            {
                Mock.Get(Identity.StorageProvider)
                    .Setup(x => x.Load(Name)).Returns(Data);

                Identity id = Identity.Open(Name, Password);

                id.Should().NotBeNull();
            }

            [Fact]
            public void ThrowsArgumentNullIfNameIsNull()
            {
                Identity id;
                Action act = () => id = Identity.Open(null, Password);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("name");
            }

            [Fact]
            public void ThrowsArgumentNullIfPasswordIsNull()
            {
                Identity id;
                Action act = () => id = Identity.Open(Name, null);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("password");
            }

            [Fact]
            public void ThrowsInvalidOperationIfStorageProviderIsNull()
            {
                IIdentityStorageProvider old = Identity.StorageProvider;
                Identity.StorageProvider = null;

                try
                {
                    Identity id;
                    Action act = () => id = Identity.Open(Name, Password);

                    act.ShouldThrow<InvalidOperationException>();
                }
                finally
                {
                    Identity.StorageProvider = old;
                }
            }

            [Fact]
            public void ThrowsExceptionIfPasswordIsIncorrect()
            {
                Mock.Get(Identity.StorageProvider)
                    .Setup(x => x.Load(Name)).Returns(Data);
                Identity id;

                Action act = () => id = Identity.Open(Name, "wrongpassword");

                act.ShouldThrow<Exception>()
                   .And.Message.Should().Be("Invalid password.");
            }

            [Fact]
            public void CanRoundTrip()
            {
                string data = null;
                var mock = Mock.Get(Identity.StorageProvider);
                mock.Setup(x => x.Load(Name)).Returns(() => data);
                mock.Setup(x => x.Save(Name, It.IsAny<string>()))
                    .Callback((Action<string, string>) ((name, d) => data = d));

                var id = Identity.CreateNew(Name, Password, Entropy);
                var expected = id.GetSitePrivateKey(Domain);

                id = Identity.Open(Name, Password);
                var actual = id.GetSitePrivateKey(Domain);

                expected.Should().BeEquivalentTo(actual);
            }
        } 
    }
}