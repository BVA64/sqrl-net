using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace SQRL.Client.Test
{
    public class SqrlClientFacts
    {
        private const string Name = "Identity #1";
        private const string Password = "password1";
        private const string Data = "AAEAAAD/////AQAAAAAAAAAMAgAAAEJTUVJMLkNsaWVudCwgVmVyc2lvbj0xLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAACJTUVJMLkNsaWVudC5JZGVudGl0eStJZGVudGl0eVN0b3JlBAAAABo8TWFzdGVyS2V5PmtfX0JhY2tpbmdGaWVsZBU8U2FsdD5rX19CYWNraW5nRmllbGQZPFZlcmlmaWVyPmtfX0JhY2tpbmdGaWVsZBs8SXRlcmF0aW9ucz5rX19CYWNraW5nRmllbGQHBwcAAgICCAIAAAAJAwAAAAkEAAAACQUAAAAABAAADwMAAAAgAAAAAt4uR4dxlOuP5uVFRscJPa6hyz+nBAakfKF3sPt3EujGDwQAAAAAAQAAAg3IPB/gmAr9Ep0ffogLu+Zf4C7XkE3gElTTGbaH1ygLZA3yWJSgS6ThAGh6GAqPuOoB3KSx3x6EVUxtf4cppHEGaaNB0RXb/4HcCwo8YaJCcpd/XZIa6IKI1bZgT/ZFAmOoZXLw856dIizpfUgksCedsNEBAnqmQCu3ngdBbB5vH1cFDHUsVQV18LNc0vBvDZ1N7JPzPcbhqQgYnY0mZR77DJBD4snqG+6PbYtyJC51BNhz8jDhxmUyO3GGpOXkPcq07KKPz65WdYFp2Qivx9eKI5RoesJGa1DomrhSCkncxIrNnXLtMEVe16zXTqStrPJ5/eR2F3P6bXqM6hclUm8PBQAAABAAAAACigEz0k36O6eK20xtU4WB7AsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        private static readonly Identity Identity;

        static SqrlClientFacts()
        {
            var mock = new Mock<IIdentityStorageProvider>();
            mock.Setup(x => x.Load(Name)).Returns(Data);
            Identity.StorageProvider = mock.Object;
            Identity = Identity.Open(Name, Password);
        }

         public class Constructor
         {
             [Fact]
             public void ThrowsArgumentNullIfIdentityIsNull()
             {
                 Action act = () => new SqrlClient(null);

                 act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("identity");
             } 
         }

        public class Process
        {
            private readonly SqrlClient _client = new SqrlClient(Identity);

            static Process()
            {
                new SqrlClientFacts();
            }

            [Fact]
            public void ThrowsArgumentNullIfUrlIsNull()
            {
                Action act = () => _client.Process(null);

                act.ShouldThrow<ArgumentNullException>()
                   .And.ParamName.Should().Be("url");
            }

            [Fact]
            public void ThrowsArgumentNullIfUrlIsMalformed()
            {
                Action act = () => _client.Process("sq\\l:/loc./bad$form://");

                act.ShouldThrow<UriFormatException>();
            }
        }
    }
}