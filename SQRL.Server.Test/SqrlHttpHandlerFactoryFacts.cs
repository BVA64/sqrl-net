using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlHttpHandlerFactoryFacts
    {
        public class GetHandler
        {
            private readonly SqrlHttpHandlerFactory _factory;

            public GetHandler()
            {
                _factory = new SqrlHttpHandlerFactory();
            }

            [Fact]
            public void ReturnsSqrlHandlerIfRequestTypeIsGet()
            {
                const string get = "GET";

                var handler = _factory.GetHandler(null, get, null, null);

                handler.Should().NotBeNull();
                handler.Should().BeOfType<SqrlHttpHandler>();
            }

            [Fact]
            public void ReturnsNullIfRequestTypeIsPost()
            {
                const string post = "POST";

                var handler = _factory.GetHandler(null, post, null, null);

                handler.Should().BeNull();
            }
        }
    }
}
