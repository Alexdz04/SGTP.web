using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; 
using OpenQA.Selenium.Edge;
using System;
using System.IO; 
using NUnit.Framework;
using SeleniumExtras.WaitHelpers; 

namespace SGTP.Tests
{
    public class TaskTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); 
        }

        [Test]
        public void AddTaskTest()
        {
            
            driver.Navigate().GoToUrl("https://localhost:7024/Tasks");

            
            IWebElement addNewTaskButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Agregar Nueva Tarea")));
            addNewTaskButton.Click();

            
            driver.FindElement(By.Id("Title")).SendKeys("Nueva Tarea Selenium");
            driver.FindElement(By.Id("Description")).SendKeys("Esta es una tarea agregada mediante Selenium");
            driver.FindElement(By.Id("DueDate")).SendKeys("12/08/2024");  

            
            IWebElement addTaskButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[type='submit']")));
            addTaskButton.Click();

            
            wait.Until(d => d.Url == "https://localhost:7024/");

            
            var tasksTable = wait.Until(d =>
            {
                try
                {
                    var table = d.FindElement(By.Id("TasksTable"));
                    return table.Displayed ? table : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            
            Assert.That(tasksTable != null && tasksTable.Text.Contains("Nueva Tarea Selenium"), Is.True);

            
            TakeScreenshot("AddTaskTest");
        }




        [TearDown]
        public void TearDown()
        {
            
            driver.Quit();
        }

        private void TakeScreenshot(string testName)
        {
            
            string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string filePath = Path.Combine(screenshotsDir, $"{testName}_{DateTime.Now:yyyyMMddHHmmss}.png");
            screenshot.SaveAsFile(filePath);

            Console.WriteLine($"Captura de pantalla guardada en: {filePath}");
        }
    }
}
