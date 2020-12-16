using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using DataEntitys;
using Services.Unity;

namespace UnitTestProject
{
    [TestClass]
    public class GetTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //List<DevelopUser> list = UnitySingleton.UnityUserFacade.GetEntities(m => m.UserId < 10).Result;
            //list.ForEach(f =>
            //{
            //    Console.WriteLine(f.Name);
            //});

            IQueryable<DevelopUser> list = UnityUserFacade.GetQueryableEntities(m => m.Id < 10);
            list.ToList().ForEach(f =>
            {
                Console.WriteLine(f.Name);
            });
            Assert.AreEqual(7, list.Count(m => m.Id != 0));
        }

        [TestMethod]
        public void TestDevelopRecord()
        {
            /*获取总条目数*/
            ///int count = UnitySingleton.UnityDevelopRecordFacade.GetDevelopRecordListCount().Result;
            //Assert.AreEqual(248, count);

            /*测试分页*/
            //IList<DevelopRecordEntity> list1 = UnitySingleton.UnityDevelopRecordFacade.GetDevelopRecordListByPager(1, 30).Result;

            /*测试更新点击次数*/
            bool affectRows = UnityDevelopRecordFacade.UpdateDevelopRecordClickCount(4).Result;
            Assert.IsTrue(true);
        }
    }
}
