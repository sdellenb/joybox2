using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using joybox2.ServiceInterface;
using joybox2.ServiceModel;

namespace joybox2.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<ApiServices>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Can_call_ApiServices()
        {
            var service = appHost.Container.Resolve<ApiServices>();

            var response = (CategoriesResponse)service.GetJson(new GetCategory { Id = 1 });

            Assert.That(response.Data, Is.Not.Empty);
        }
    }
}
