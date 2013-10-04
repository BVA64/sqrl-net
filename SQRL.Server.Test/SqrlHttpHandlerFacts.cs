using System;
using System.Net;
using System.Web;
using FluentAssertions;
using Moq;
using Xunit;

namespace SQRL.Server.Test
{
    public class SqrlHttpHandlerFacts
    {
        public class ProcessRequest
        {
            private readonly SqrlHttpHandler _handler;
            private readonly HttpContextBase _context;

            public ProcessRequest()
            {
                _handler = new SqrlHttpHandler();
                _context = MvcMockHelpers.FakeHttpContext();
            }

            [Fact]
            public void SetsResponseStatusCode200()
            {
                var response = Mock.Get(_context.Response);
                response.SetupProperty(ctx => ctx.StatusCode);

                _handler.ProcessRequest(_context);
                
                _context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        public class IsReusable
        {
            [Fact]
            public void ReturnsFalse()
            {
                var handler = new SqrlHttpHandler();

                bool reusable = handler.IsReusable;

                reusable.Should().BeFalse();
            }
        }
    }
}