using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Unity;
using DataEntitys;
namespace UnitTestProject
{
    [TestClass]
    public class RemoveTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool result1 = UnitySingleton.UnityUserFacade.RemoveEntity(20261).Result;
            Assert.IsTrue(result1);

            DevelopUser removeUser = UnitySingleton.UnityUserFacade.GetEntity(20260).Result;
            bool result2 = UnitySingleton.UnityUserFacade.RemoveEntity(removeUser).Result;
            Assert.IsTrue(result2);

            
            
        }
    }
}
