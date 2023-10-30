using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

public class Read_Webpage_Datas : MonoBehaviour
{
    public InputField identity;
    public InputField password;

    string el;
    
    void Start()
    {
        //IWebDriver driver = new ChromeDriver(Application.dataPath + "/Packages/Selenium.WebDriver.ChromeDriver.118.0.5993.7000/driver/win32/");
        IWebDriver driver = new ChromeDriver("C:/Users/burak/Desktop/Projects/ExampleCodes/Unity Examples/earth_orbit_test/Packages/Selenium.WebDriver.ChromeDriver.118.0.5993.7000/driver/win32/");
        driver.Navigate().GoToUrl("https://www.space-track.org/auth/login");
        driver.FindElement(By.Id("identity")).SendKeys("burakberzener@gmail.com");
        driver.FindElement(By.Id("password")).SendKeys("SpaceBoy1034...");
        driver.FindElement(By.Id("password")).Submit();
        driver.Navigate().GoToUrl("https://www.space-track.org/basicspacedata/query/class/tle_latest/ORDINAL/1/EPOCH/%3Enow-30/orderby/NORAD_CAT_ID/format/tle");
        el = Convert.ToString(driver.FindElement(By.TagName("body")).GetAttribute("innerText"));
        PlayerPrefs.SetString("Name", el);
        //Debug.Log(PlayerPrefs.GetString("Name"));
        driver.Quit();
        SceneManager.LoadScene("MainMenu");
    }
}