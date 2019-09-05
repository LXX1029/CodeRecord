using System.Threading.Tasks;
using DataEntitys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Services.Unity.UnityContainerManager;
namespace UnitTestProject
{
    [TestClass]
    public class UpdateTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {

            for (int i = 0; i < 5; i++)
            {
                await Task.Run(async () =>
                {
                    // 测试用户更新
                    DevelopUser updateUser = await UnityUserFacade.GetEntity(2);
                    updateUser.Name = $"第{ i + 1}次  测试更新";
                    await UnityUserFacade.UpdateEntity(updateUser);
                });
                //Assert.AreEqual(true, );
                //Assert.IsTrue(updatedUser.Name == "测试更新");

                await Task.Run(async () =>
                {
                    // 测试用户更新
                    DevelopUser updateUser = await UnityUserFacade.GetEntity(2);
                    updateUser.Name = $"第{ i + 5}次  测试更新";
                    await UnityUserFacade.UpdateEntity(updateUser);
                });
            }
            System.Console.ReadKey();
            //using (RecordContext context = new RecordContext())
            //{
            //    DevelopUser user = context.DevelopUsers.Find(2);
            //    for (int i = 0; i < 100; i++)
            //    {
            //        user.Name = $"x{i + 1}";
            //        context.SaveChanges();

            //        user.Name = $"x{i + 2}";
            //        context.SaveChanges();
            //    }
            //}
        }
    }
}
