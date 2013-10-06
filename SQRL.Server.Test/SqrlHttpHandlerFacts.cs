using System;
using System.Collections.Specialized;
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
                InitializeForm();
                
                var response = Mock.Get(_context.Response);
                response.SetupProperty(ctx => ctx.StatusCode);

                _handler.ProcessRequest(_context);
                
                _context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }

            private void InitializeForm()
            {
                var request = Mock.Get(_context.Request);
                request.Setup(r => r.Form).Returns(new NameValueCollection());
                request.Setup(r => r.Url).Returns(new Uri(SampleData.Url));
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