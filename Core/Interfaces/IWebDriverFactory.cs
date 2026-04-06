using OpenQA.Selenium;

namespace Core.Interfaces;

public interface IWebDriverFactory
{
	ThreadLocal<IWebDriver> GetDriver(string browserName);
	void QuitDriver(string browserName);
}
