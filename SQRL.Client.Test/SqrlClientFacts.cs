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
        private const string Data = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAcpxnSp7R6UCirlE6+1ppbAAAAAACAAAAAAAQZgAAAAEAACAAAACc2axZHtEblgfejVtkGpx8VTRdO+hWV3a9/GUrQLOTkwAAAAAOgAAAAAIAACAAAABTwrBCPa/IasNJRRF+odCiyUyLmaYZ2XC/YMsxEmFAlBAEAAC/Xi0BXBhrFonzoSHalNb51zSJYUVU4q1SOgCPcB1A5Gtd5e/7ZZt9q6Y1Hr6ILh9EOEuT0wtcsAyn+S6mypTVyU9d/unjS1MZ2g/zTnsxT4NlxptXTVZhI2gjW3KEVuCcWLjwmiDCIVE3HJ30lHP+ylicN6d8ZN3xMnZVtPLM1THm32ZKwd+jV2AeL587bzrMTMXRXtLfaWTbyNH2+U5Mf7FbfT7KFEaRqysTTSfS87TI1TF9f+piZqwaoT8Vu2041L68qZDsXanEqGJNymWpAfpYj0lkgzO9hljmTG5ovQVp0gCC12WrscPCNLypSOns4H2Opir91xurfRypTnVZx/QtI1LF3ShgNgokHjAy6PqzYTXxt+Bo8ntuN15ncu03fee4JJ7zUq99tWc88utJB5roCOXllR3U4cYKNdTH9PItVF9X0btnVbaHccA1hXvGaoaO7DvtbWIGrhV051ii8jYbwo7SprlCDAQ8qxLbHLh8WRrmzqRooliLlxOafuWmhldLVrGzV9yn5oLQEb7y5WxNSXOf2PfFQnhyKF8dLiPATtSowk6SUsYhfTYVL575LwwPhZ4fIVkGQAhglA+3DYPoCFHR7NvVKVP8CE1dDQfIFzkY1ynaF54dj4RWzmGH9tzjpMhYCj4zaIy/cX5t2h9FEz/WvcuFR6P3O8GA+5DtP9yQKaBKOXYDMd2ndsZ0he8EPwCy77g16+IlQ2jVWau06BzhICjfP6qaMBCi9I+LpIx1+0Cdyq2LBGZFFu94t4wJbgmhoq1dP7Pzf/udjNG/oyjQb5PquI7VtyCRsO3FobsWee65KzGG1RB9vWtjPG79GbYY0PtHoB+AjBUyOlCUWCRrLSWqBGBimvrSgqMic1E91hyEMJfKNx8MOOaa6mxz1QLJNpICi4W4ipy67sb+IeWMl+ry3wVwe7G6anUj58sKrePPuRgW1lHeBOvFoeOLzJ/qSUj73JqXNGLgWE2a2zy4pmk20kiMZWcl8E9WjNIOzh1ioRxTNyJbzOIINkqPCqx4GxlrrIF+jXmW2Dys8yUNdiMp9HyJZGmKZCykhOIs+It0hkvtSqEVsNv7vo3wiSfD7bMxjJbyG7y4eZW7JgQRsH2z6/wTvA2ImMLMRXgaYOT1Hn4varva3CsPRwxbsTRn57u9Ga/RbECvuNZDA6QqqnI2SS4/QmeICXkniYpsyHwWlHlBl8Q7r2NPb58b4AgOE4KCCE4DnvyIoibKfB5j58XSGXNuXFMs7JHklVSaxqstdlBBkkgJYYIc484yZe1mufIs5IxGr2BRV1v/ussPuJCkaHPtHyoWGkzjzlBk26KhEJomUSrSoYNpJ0ccLKN/mTCvxlvl1NICprhgJ6iCB6WTudTn1J5COEAAAABny/O4Lf2Gl892XZTNseTPTJQ5ZwyyXNzx6iKaWOyuYRar8CS9kmg1Jsm0UrBtYxpLQb2Oi7CNY2BUMqpAhdmp";
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