using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Unity;
using DataEntitys;
namespace UnitTestProject
{
    [TestClass]
    public class AddTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 10; i++)
            {
                DevelopUser user = UnitySingleton.UnityUserFacade.AddEntity(new DevelopUser()
                {
                    Name = "测试" + i,
                    DevelopAge = 5
                }).Result;
                Assert.IsTrue(user.Id > 0);
            }

        }
    }
}
