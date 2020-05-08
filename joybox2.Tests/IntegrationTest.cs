using Funq;
using ServiceStack;
using NUnit.Framework;
using joybox2.ServiceInterface;
using joybox2.ServiceModel;

namespace joybox2.Tests
{
    public class IntegrationTest
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(IntegrationTest), typeof(ApiServices).Assembly) { }

            public override void Configure(Container container)
            {
            }
        }

        public IntegrationTest()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

        [Test]
        public void Can_call_GetCategories_Service()
        {
            var client = CreateClient();

            var response = client.Get(new GetCategories());

            Assert.That(response.Data, Is.Not.Empty);
        }
    }
}