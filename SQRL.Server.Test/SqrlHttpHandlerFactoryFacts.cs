using System;
using FluentAssertions;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlHttpHandlerFactoryFacts
    {
        public class GetHandler
        {
            private readonly SqrlHttpHandlerFactor _factory;

            public GetHandler()
            {
                _factory = new SqrlHttpHandlerFactor();
            }

            [Fact]
            public void ReturnsSqrlHandlerIfRequestTypeIsPost()
            {
                const string post = "POST";

                var handler = _factory.GetHandler(null, post, null, null);

                handler.Should().NotBeNull();
                handler.Should().BeOfType<SqrlHttpHandler>();
            }

            [Fact]
            public void ReturnsNullIfRequestTypeIsGet()
            {
                const string get = "GET";

                var handler = _factory.GetHandler(null, get, null, null);

                handler.Should().BeNull();
            }
        }
    }
}
