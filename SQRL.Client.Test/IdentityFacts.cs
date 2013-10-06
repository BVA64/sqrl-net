using System;
using System.Security;
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
        private const string Data = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAcpxnSp7R6UCirlE6+1ppbAAAAAACAAAAAAAQZgAAAAEAACAAAAALPzlB7DOFR8bw+aR9hYvH0xvJMSTgcsbOyJFUqFxsEAAAAAAOgAAAAAIAACAAAAAQYA2RErg5MAVzqbX67F8DEWtmcJtPkPnqQBzgHpZB+xAEAACEFkiLIG2AFR04t4rcJPhFwt6tVmR5cZCokcJb1HYCGjjzRwpNkUzkSrc+WzE4xDFrEwz5jyfhIIJOm+TNwVJYIssNNn0BzujPwa6K5hS/bheTg8o3vbLatHlHoj7QdRN2bJMc4ykIjL5KR9SQl2HYL7ee+kNTmx7a355zCU6B7N10AyHZt1CXGmVt5PJPjC4Y4S4p9rCltMaVYGkVO/bOGHZAxpOa5JzVpiKvw63XQS4nT09Vsf0qmANOkp52C9m98otNJeadlC0qEax6mwouMsm3pK8q040O/VJx8RSWlzPiHK/z/Mh2G6c/QpbFeO9TSMKPxC9DGwU0sdjFkezygRDN7QSfN2LN8fR+cRLzeVXeCSGWOwEEWeLRM0GgxClbp7c1zN96u8EXUYT/uPca7ggij5hI5qOMwY/aD29tcfJiFyZ7W1lCpqisGjmEb3s/bAWYzjSr+T1se+RU7NT2MOGAN0HCAcpgQkp7hMKVzHUXNZwAxaKhBHCWDhFHds0u4IFRhoQ6zeOGcuD9ITXCyED2DWpKT0loO3gETTGokaP9MvnVfqjkHuG6BGsKD2MzPebZ+EvIt3c7cr5Ke469Sj55zguv4xiGO9JsLbGnjQYW3fUn1BSgxjc5rUzRN1fMQEpkZDCNRTsIS+onnMkSlsDkKbyJdT7HCRC3QFivn7u9h/4PVizeMhuCWYf28tYzkS4c4tMstD4qw2PgB2RblljGhzflT82rxSzjTkFy5EZrfCJmEvR2JTcrzYDoxgAL1qMX0dboHXVM7LObQ71Pcu7zbKwwadXusELiezlQiZc0kKcuW4TvPSGrKUKcg7BUBTsgRvLASE69s+cB3VezAveEO53Z3gYP7ig6G/N1GGKu3aLph8wQe0NJ7muDyfK04vdIdJn2aiypdqjOdkdWtiAHgmBWlIAd9T9H/V5fh9jq+UKQxDOxigPpKF7xWFqxcZzU9NUkvm+P9vyCaBK3v3TvJYQ1aoyQPstf7lL+O/fDqOVToMN8GiHJhySack8PQSYi61Z/gUUN8ULhqyfBhGAHRrm0xe+UmSpcIxWOd7+Zjei1AagSXjMmDigGKRLlRYQdJgCy806oDxXKdrAo1tPAX5psRJ/o5VjXWIRAF27VKQvrPHG7q0cAzJsuULEx2mcFAqQrhDjqJUynb0nkiW8YTDoDdNg1gcutx9fzfVT/U/zYi0c7rbLB6z4TnBORkfvhJWgc6SnsBqTF7oJmlZpzBkVyj7092GHZhC69fGVTpU4R/JjpcE9/JKvixaBYTqHdcpaAqLR0Fu+2W3QcXId4VBT3Zv4+VwgzFb7D0wr9juC5j1T7M2KyqGWNfWEhJj5kpQGQpDikfwoHMXI0mDY6RFOIEAe/WrWTq+ZFq0AAAABnO23zZ5M4VB+XH2+qRBK6D80L7DRwh+qWM3LXdGS09mVBmr3fBQIZ9+Qng6bnWJzBmKjfEJI5CaxzVP9WMYRM";
        private static readonly SecureString SecurePassword = new SecureString();

        static IdentityFacts()
        {
            Identity.StorageProvider = new Mock<IIdentityStorageProvider>().Object;
            foreach (var c in Password)
            {
                SecurePassword.AppendChar(c);
            }
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
                Identity id = Identity.CreateNew(Name, SecurePassword, Entropy);

                id.Should().NotBeNull();
            }

            [Fact]
            public void ThrowsArgumentNullIfNameIsNull()
            {
                Identity id;
                Action act = () => id = Identity.CreateNew(null, SecurePassword, Entropy);

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
                Action act = () => id = Identity.CreateNew(Name, SecurePassword, null);

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

                Identity id = Identity.Open(Name, SecurePassword);

                id.Should().NotBeNull();
            }

            [Fact]
            public void ThrowsArgumentNullIfNameIsNull()
            {
                Identity id;
                Action act = () => id = Identity.Open(null, SecurePassword);

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
                    Action act = () => id = Identity.Open(Name, SecurePassword);

                    act.ShouldThrow<InvalidOperationException>();
                }
                finally
                {
                    Identity.StorageProvider = old;
                }
            }

            [Fact]
            public void CanRoundTrip()
            {
                string data = null;
                var mock = Mock.Get(Identity.StorageProvider);
                mock.Setup(x => x.Load(Name)).Returns(() => data);
                mock.Setup(x => x.Save(Name, It.IsAny<string>()))
                    .Callback((Action<string, string>) ((name, d) => data = d));

                var id = Identity.CreateNew(Name, SecurePassword, Entropy);
                var expected = id.GetSitePrivateKey(Domain);

                id = Identity.Open(Name, SecurePassword);
                var actual = id.GetSitePrivateKey(Domain);

                expected.Should().BeEquivalentTo(actual);
            }
        } 
    }
}