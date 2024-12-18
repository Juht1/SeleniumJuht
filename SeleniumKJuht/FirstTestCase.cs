using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System;
using NUnit.Framework;


namespace SeleniumKJuht
{
    [TestFixture]
    public class FirstTestCase
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        [SetUp]
        public void SetUp()
        {
            driver = new FirefoxDriver("C:\\Users\\opilane\\source\\repos\\SeleniumKJuht\\SeleniumKJuht\\drivers"); // VÕid puttida oma isiliku pathi oma driveri jaoks
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [Test]
        public void TestHiddenLayers()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/hiddenlayers");

            var btn = driver.FindElement(By.Id("greenButton"));
            btn.Click();

            try
            {
                btn.Click();
                Assert.Fail("The green button should be disabled after the first click");
            }
            catch (WebDriverException)
            {
                Assert.Pass("Check passed, as the green button cannot be clicked twice.");
            }
        }

        [Test]
        public void TestClick()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/click");

            var nupp = driver.FindElement(By.Id("badButton"));

            new Actions(driver).MoveToElement(nupp).Click().Perform();

            string updatedClass = nupp.GetAttribute("class");
            Assert.That(updatedClass.Contains("btn-success"), Is.True, "The button did not change to green after being clicked.");
        }

        [Test]
        public void TestInput()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/textinput");

            var tekst = driver.FindElement(By.Id("newButtonName"));
            var nupp = driver.FindElement(By.Id("updatingButton"));

            new Actions(driver).Click(tekst).SendKeys("Nagger").Perform();
            nupp.Click();

            Assert.That(nupp.Text, Is.EqualTo("Nagger"), "Button text did not update.");
        }

        [Test]
        public void TestAlerts()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/alerts");

            var alrt = driver.FindElement(By.Id("alertButton"));
            alrt.Click();
            driver.SwitchTo().Alert().Accept();

            var cnfrm = driver.FindElement(By.Id("confirmButton"));
            cnfrm.Click();
            Thread.Sleep(1000);
            driver.SwitchTo().Alert().Accept();

            var prmt = driver.FindElement(By.Id("promptButton"));
            prmt.Click();
            Thread.Sleep(1000);
            var Alert = driver.SwitchTo().Alert();
            Alert.SendKeys("Cats");
            Alert.Accept();
        }

        [Test]
        public void TestProgressBar()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/progressbar");

            var startButton = driver.FindElement(By.Id("startButton"));
            startButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            int progressValue = 0;
            wait.Until(d =>
            {
                var progressBar = driver.FindElement(By.Id("progressBar"));
                string progressText = progressBar.Text.Replace("%", "");
                progressValue = int.Parse(progressText);
                return progressValue >= 73;
            });

            if (progressValue >= 75)
            {
                var stopButton = driver.FindElement(By.Id("stopButton"));
                stopButton.Click();
            }

            var finalProgressBar = driver.FindElement(By.Id("progressBar"));
            string finalProgressText = finalProgressBar.Text.Replace("%", "");
            int finalProgressValue = int.Parse(finalProgressText);

            Assert.That(finalProgressValue, Is.EqualTo(75).Within(5),
                        $"Expected the progress bar to be close to 75%, but it was {finalProgressValue}%.");
        }

        [Test]
        public void TestAjaxData()
        {
            driver.Navigate().GoToUrl("http://www.uitestingplayground.com/ajax");

            var ajax = driver.FindElement(By.Id("ajaxButton"));
            ajax.Click();

            wait.Until(d => !d.FindElement(By.Id("spinner")).Displayed);

            var label = wait.Until(d => d.FindElement(By.CssSelector("#content .bg-success")));
            label.Click();

            string labelText = label.Text;
            Assert.That(labelText, Is.Not.Empty, "The label text should not be empty after AJAX request.");
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

    }
}