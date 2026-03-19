using Core.Drivers;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDriverFactory(
		this IServiceCollection services,
		Action<Dictionary<string, IDriverCreator>> configure)
	{
		var creators = new Dictionary<string, IDriverCreator>();
		configure(creators);
		services.AddSingleton<IWebDriverFactory>(new DriverFactory(creators));
		return services;
	}

	public static Dictionary<string, IDriverCreator> AddChrome(
		this Dictionary<string, IDriverCreator> creators)
	{
		creators["chrome"] = new ChromeDriverCreator();
		return creators;
	}

	public static Dictionary<string, IDriverCreator> AddFirefox(
		this Dictionary<string, IDriverCreator> creators)
	{
		creators["firefox"] = new FirefoxDriverCreator();
		return creators;
	}
}
