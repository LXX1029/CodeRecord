using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Unity;
using DataEntitys;
using System.Threading.Tasks;
using Services.EFCodeFirst;

namespace UnitTestProject
{
    [TestClass]
    public class UpdateTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    // 测试用户更新
            //    DevelopUser updateUser = await UnitySingleton.UnityUserFacade.GetEntity(20270);
            //    updateUser.Name = $"第{ i + 1}次  测试更新";
            //    DevelopUser updatedUser = await UnitySingleton.UnityUserFacade.UpdateEntity(updateUser);
            //    //Assert.AreEqual(true, );
            //    //Assert.IsTrue(updatedUser.Name == "测试更新");
            //}

            using (RecordContext context = new RecordContext())
            {
                DevelopUser user = context.DevelopUsers.Find(20270);
                for (int i = 0; i < 100; i++)
                {
                    user.Name = $"x{i + 1}";
                    context.SaveChanges();

                    user.Name = $"x{i + 2}";
                    context.SaveChanges();
                }


            }
        }
    }
}
