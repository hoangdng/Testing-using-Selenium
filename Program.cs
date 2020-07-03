using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeleniumTest
{
    class Program
    {

        IWebDriver driver = new ChromeDriver();


        [SetUp]
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://localhost:44331/Identity/Account/Register");
        }

        [Test]
        public void Test1()
        {
            var textfieldUsername = driver.FindElement(By.XPath("//input[@type='email']"));
            var textfieldPassword = driver.FindElement(By.Id("Input_Password"));
            var textfieldPasswordConfirm = driver.FindElement(By.Id("Input_ConfirmPassword"));
            var buttonSignUp = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(),'Đăng ký')]"));
            Assert.IsTrue(buttonSignUp.Displayed && textfieldUsername.Displayed && textfieldPasswordConfirm.Displayed
                && textfieldPassword.Displayed, "Element is not displayed");
        }

        [Test]
        public void Test2()
        {
            //Test link direct to Contact Page
            var linkContact = driver.FindElement(By.XPath("//a[contains(text(),'Liên hệ')]"));
            linkContact.Click();

            string expectedURL = "https://localhost:44331/Home/Contact";
            string actualURL = driver.Url;
            Assert.AreEqual(actualURL, expectedURL);

            //Test link direct to register Facebook page
            driver.Navigate().GoToUrl("https://localhost:44331/Identity/Account/Register");
            var linkRegisterViaFacebook = driver.FindElement(By.XPath("//button[@type='submit' and @value='Facebook']"));
            linkRegisterViaFacebook.Click();

            string expectedFacebookURL = "https://www.facebook.com/login.php?skip_api_login";
            bool isMatchFacebookURL = driver.Url.Contains(expectedFacebookURL);
            Assert.IsTrue(isMatchFacebookURL, "Cannot direct to register via facebook page");

            //Test link direct to register by Google page
            driver.Navigate().GoToUrl("https://localhost:44331/Identity/Account/Register");
            var linkRegisterViaGoogle = driver.FindElement(By.XPath("//button[@type='submit' and @value='Google']"));
            linkRegisterViaGoogle.Click();

            string expectedGoogleURL = "https://accounts.google.com/signin/oauth/identifier";
            bool isMatchGoogleURL = driver.Url.Contains(expectedGoogleURL);
            Assert.IsTrue(isMatchGoogleURL, "Cannot direct to register via google page");
            driver.Navigate().GoToUrl("https://localhost:44331/Identity/Account/Register");
        }

        [Test]
        public void Test3()
        {
            var textfieldUsername = driver.FindElement(By.XPath("//input[@type='email']"));
            textfieldUsername.SendKeys("TestEmail@gmail.com");

            var textfieldPassword = driver.FindElement(By.Id("Input_Password"));
            textfieldPassword.SendKeys("123456");

            var textfieldPasswordConfirm = driver.FindElement(By.Id("Input_ConfirmPassword"));
            textfieldPasswordConfirm.SendKeys("123456");

            var buttonSignUp = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(),'Đăng ký')]"));
            buttonSignUp.Click();

            string expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "TestEmail@gmail.com";
            string actualURL = driver.Url;
            Assert.AreEqual(expectedURL, actualURL);
        }

        [Test]
        public void Test4()
        {
            // 1 _ wrong email format
            var textfieldUsername = driver.FindElement(By.XPath("//input[@type='email']"));
            textfieldUsername.SendKeys("TestEmailgmail.com");

            var textfieldPassword = driver.FindElement(By.Id("Input_Password"));
            textfieldPassword.SendKeys("123456");

            var textfieldPasswordConfirm = driver.FindElement(By.Id("Input_ConfirmPassword"));
            textfieldPasswordConfirm.SendKeys("123456");

            var buttonSignUp = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(),'Đăng ký')]"));
            buttonSignUp.Click();

            string expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "TestEmailgmail.com";
            string actualURL = driver.Url;
            Assert.AreNotEqual(expectedURL, actualURL);

            // 2 _ wrong email format
            textfieldUsername.Clear();
            textfieldUsername.SendKeys("TestEmail@");

            buttonSignUp.Click();

            expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "TestEmail@";
            actualURL = driver.Url;
            Assert.AreNotEqual(expectedURL, actualURL);

            // 3 _ wrong email format
            textfieldUsername.Clear();
            textfieldUsername.SendKeys("@gmail.com");

            buttonSignUp.Click();

            expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "@gmail.com";
            actualURL = driver.Url;
            Assert.AreNotEqual(expectedURL, actualURL);

            // 4 _ wrong password length
            textfieldUsername.Clear();
            textfieldUsername.SendKeys("TestingEmail@gmail.com");

            textfieldPassword.Clear();
            textfieldPassword.SendKeys("12345");

            textfieldPasswordConfirm.Clear();
            textfieldPasswordConfirm.SendKeys("12345");

            buttonSignUp.Click();

            expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "TestingEmail@gmail.com";
            actualURL = driver.Url;
            Assert.AreNotEqual(expectedURL, actualURL);

            // 5 _ wrong confirm password
            textfieldUsername.Clear();
            textfieldUsername.SendKeys("TestingEmail@gmail.com");

            textfieldPassword.Clear();
            textfieldPassword.SendKeys("123456");

            textfieldPasswordConfirm.Clear();
            textfieldPasswordConfirm.SendKeys("1234567");

            buttonSignUp.Click();

            expectedURL = "https://localhost:44331/Identity/Account/RegisterConfirmation?email=" + "TestingEmail@gmail.com";
            actualURL = driver.Url;
            Assert.AreNotEqual(expectedURL, actualURL);
        }

        [TearDown]
        public void CleanUp()
        {
            //driver.Close();
        }
    }
}
