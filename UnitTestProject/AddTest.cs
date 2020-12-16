using System;
using System.Diagnostics;
using DataEntitys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Unity;

namespace UnitTestProject
{
    [TestClass]
    public class AddTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                DevelopUser user = UnityUserFacade.AddEntity(new DevelopUser()
                {
                    Name = "测试" + i,
                    DevelopAge = 5,
                    Pwd = "1111"
                }).Result;
                Assert.IsTrue(user.Id > 0);
            }
            sw.Stop();
            Console.WriteLine($"time:{sw.Elapsed.TotalSeconds}");
        }
    }
}
