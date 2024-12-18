using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using SeleniumExtras.WaitHelpers;

namespace SeleniumKJuht
{
    [TestFixture]
    public class StartPageTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            // Pane siia oma enda pathi selle driveri jaoks
            var options = new FirefoxOptions();
            driver = new FirefoxDriver(@"C:\Users\krist\Downloads\SeleniumJuht\SeleniumKJuht\drivers", options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [Test]
        public void TestNavigation()
        {
            driver.Navigate().GoToUrl("https://localhost:5173");

            var loginButton = driver.FindElement(By.LinkText("Login"));
            loginButton.Click();
            Assert.That(driver.Url.Contains("/login"), Is.True, "Should navigate to desired location");

            driver.Navigate().Back();

            var registerButton = driver.FindElement(By.LinkText("Register"));
            registerButton.Click();
            Assert.That(driver.Url.Contains("/register"), Is.True);
        }


        [Test]
        public void TestContactForm()
        {
            driver.Navigate().GoToUrl("https://localhost:5173");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var nameInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Your Name']")));
            var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Your Email']")));
            var messageInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@placeholder='Your Message']")));
            var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Send Message']")));

            nameInput.Clear();
            emailInput.Clear();
            messageInput.Clear();

            nameInput.SendKeys("Test Nimi");
            emailInput.SendKeys("Nagger@Gmail.com");
            messageInput.SendKeys("Tervist. Soovin saata complainiga, et teie veeblieht on halb");

            submitButton.Click();

            var responseMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(), 'Thank you')]")));

            Assert.That(responseMessage.Displayed, Is.True, "Should show a response message");
            Assert.That(responseMessage.Text.Contains("Test Nimi"), Is.True, "Response should include the name");
        }

        [Test]
        public void TestReviewsSection()
        {
            driver.Navigate().GoToUrl("https://localhost:5173");

            var reviewCards = driver.FindElements(By.ClassName("review-card"));
            Assert.That(reviewCards.Count, Is.EqualTo(1));

            var reviewAuthor = reviewCards[0].FindElement(By.TagName("h3"));
            var reviewText = reviewCards[0].FindElement(By.TagName("p"));

            Assert.That(reviewAuthor.Text, Is.EqualTo("Pavlova Tchaikovski"));
            Assert.That(reviewText.Text, Is.EqualTo("\"This app has changed my financial life!\""));
        }

        [Test]
        public void TestFooterSocialLinks()
        {
            // Testib, et kas need lingid on olemas
            driver.Navigate().GoToUrl("https://localhost:5173");

            var socialLinks = driver.FindElements(By.CssSelector(".social-links a"));
            Assert.That(socialLinks.Count, Is.EqualTo(2));

            Assert.That(socialLinks[0].GetDomAttribute("href"),
                Is.EqualTo("https://twitter.com/ishowspeed"));
            Assert.That(socialLinks[1].GetDomAttribute("href"),
                Is.EqualTo("https://instagram.com/blacknigga"));

            Assert.That(socialLinks[0].GetDomAttribute("target"), Is.EqualTo("_blank"));
            Assert.That(socialLinks[0].GetDomAttribute("rel"), Is.EqualTo("noopener noreferrer"));
        }


        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}