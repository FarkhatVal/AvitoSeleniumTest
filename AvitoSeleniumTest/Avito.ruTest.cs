using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AvitoSeleniumTest;

public class AvitoRuTest
{
    private WebDriver driver;
    private WebDriverWait wait;
    const string Url = "https://www.avito.ru/";
    private  string ValuePriceFrom = "500000";
    private string ValuePriceTo = "1000000";
    
    [OneTimeSetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }
    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Quit();
    }

    [Test]
    public void Test1()
    {
        driver.Navigate().GoToUrl(Url);
        driver.Manage().Window.Maximize();
        var category = driver.FindElement(By.XPath("//*[@id='category']/option[3]"));
        category.Click();
        var priceFrom = driver.FindElement(By.XPath("//input[@data-marker='price/from']"));
        priceFrom.SendKeys(ValuePriceFrom);
        var priceTo = driver.FindElement(By.XPath("//input[@data-marker='price/to']"));
        priceTo.SendKeys(ValuePriceTo);
        var models = driver.FindElement(By.XPath("//input[@data-marker='params[110000]/suggest-input']"));
        models.Click();
        var model = driver.FindElement(By.XPath("//li[@data-marker='params[110000]/suggest-dropdown(5)']"));
        model.Click();
        var searchButton = driver.FindElement(By.XPath("//button[@data-marker='search-filters/submit-button']"));
        searchButton.Click();
        var searchReults = driver.FindElement(By.XPath("//div[@class='items-items-kAJAg']"));
       // ReadOnlyCollection<IWebElement> results =
          //  wait.Until(e => e.FindElements(By.XPath("//div[@data-marker='item']")));
        ReadOnlyCollection<IWebElement> a =
            wait.Until(e => e.FindElements(By.XPath("//span[@class= 'price-text-E1Y7h text-text-LurtD text-size-s-BxGpL']")));
        var count = a.Count;
        for (int i = 0; i < count; i++)
        {
            String text=a[i].Text;  
            string cleanText = Regex.Replace(text, "[^0-9]", "");
            Console.WriteLine(cleanText);
            int priceVolue = int.Parse(cleanText);
            Assert.True((int.Parse(ValuePriceFrom) <= priceVolue || priceVolue >= int.Parse(ValuePriceTo)), $"Стоимость не верная, стоиомть ровна {priceVolue}");
        }
    }
}