using OpenQA.Selenium;

namespace Core.Interfaces;

public interface IDriverCreator
{
	ThreadLocal<IWebDriver> Create();
}
