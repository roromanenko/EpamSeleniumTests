using Core.Configuration;

namespace Core.Interfaces;

public interface ITestConfigurationProvider
{
	TestConfiguration GetConfiguration();
}
