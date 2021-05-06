using System.Threading.Tasks;
using Xunit;
using Moq;
using DIPS.Fhir.Service.Configuration;

namespace TestDIPS.Fhir.Service
{
    public class CentralConfigurationTests
    {
        private const string SomeRelease = "SomeRelease";
        private const string SomeDomain = "SomeDomain";

        private CentralConfiguration CentralConfiguration;
        private Mock<IConfigurationLoader> ConfigurationLoaderMock;
        private Mock<IEnvironment> EnvironmentMock;



        public CentralConfigurationTests()
        {
            ConfigurationLoaderMock = new Mock<IConfigurationLoader>();
            
            EnvironmentMock = new Mock<IEnvironment>();
            EnvironmentMock.Setup(e => e.GetEnvironmentVariable("RELEASE_NAME")).Returns(SomeRelease);
            EnvironmentMock.Setup(e => e.GetEnvironmentVariable("HOST_NAME")).Returns(SomeDomain);

            CentralConfiguration = new CentralConfiguration(ConfigurationLoaderMock.Object, EnvironmentMock.Object);            
        }

        [Fact]
        public async Task GetSecurityServiceUrl_WithoutTemplate_ReturnsCorrectUrl()
        {
            var configuration = new Configuration()
            {
                SecurityServiceUri = "https://dips-ehr-security"
            };
            ConfigurationLoaderMock.Setup(c => c.Load()).Returns(Task.FromResult(configuration));

            var securityServiceUrl = await CentralConfiguration.GetSecurityServiceUrl();
            Assert.Equal("https://dips-ehr-security", securityServiceUrl);
        }

        [Fact]
        public async Task GetSecurityServiceUrl_WithReleaseNameTemplate_ReturnsResolvedUrl()
        {
            var configuration = new Configuration()
            {
                SecurityServiceUri = "https://RELEASE-NAME.dips-ehr-security"
            };
            ConfigurationLoaderMock.Setup(c => c.Load()).Returns(Task.FromResult(configuration));

            var securityServiceUrl = await CentralConfiguration.GetSecurityServiceUrl();
            Assert.Equal("https://SomeRelease.dips-ehr-security", securityServiceUrl);
        }

        [Fact]
        public async Task GetSecurityServiceUrl_WithDomainTemplate_ReturnsResolvedUrl()
        {
            var configuration = new Configuration()
            {
                SecurityServiceUri = "https://dips-ehr-security.DOMAIN"
            };
            ConfigurationLoaderMock.Setup(c => c.Load()).Returns(Task.FromResult(configuration));

            var securityServiceUrl = await CentralConfiguration.GetSecurityServiceUrl();
            Assert.Equal("https://dips-ehr-security.SomeDomain", securityServiceUrl);
        }
    }
}
