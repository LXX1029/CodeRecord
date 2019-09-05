using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Services.Unity.UnityContainerManager;
using DataEntitys;
namespace UnitTestProject
{
    [TestClass]
    public class RemoveTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool result1 = UnityUserFacade.RemoveEntity(20261).Result;
            Assert.IsTrue(result1);

            DevelopUser removeUser = UnityUserFacade.GetEntity(20260).Result;
            bool result2 = UnityUserFacade.RemoveEntity(removeUser).Result;
            Assert.IsTrue(result2);

            
            
        }
    }
}
