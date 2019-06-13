namespace Services.Migrations
{
    using DataEntitys;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Services.EFCodeFirst.RecordContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Services.EFCodeFirst.RecordContext context)

        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            try
            {
                // ��ʼ��DevelopUsers Ĭ������
                context.DevelopUsers.AddOrUpdate(new DevelopUser()
                {
                    Id = 1,
                    Name = "admin",
                    Pwd = Common.UtilityHelper.MD5Encrypt("0"),
                });


                //��ʼ��DevelopUsers��Ӧ��Ȩ��
                DevelopFun[] funList = new DevelopFun[]
                {
                new DevelopFun{Id=1,Name="���˵�",ImageIndex=0,ParentID=0},
                new DevelopFun{Id=2,Name="������",ImageIndex=0,ParentID=0},
                new DevelopFun{Id=3,Name="ͳ��",ImageIndex=0,ParentID=0},
                new DevelopFun{Id=4,Name="����",ImageIndex=0,ParentID=0},
                new DevelopFun{Id=5,Name="�û�����",ImageIndex=7,ParentID=4},
                new DevelopFun{Id=6,Name="��������",ImageIndex=6,ParentID=4},
                new DevelopFun{Id=7,Name="����",ImageIndex=0,ParentID=1},
                new DevelopFun{Id=8,Name="�޸�",ImageIndex=4,ParentID=1},
                new DevelopFun{Id=9,Name="ɾ��",ImageIndex=1,ParentID=1},
                new DevelopFun{Id=10,Name="��ѯͳ��",ImageIndex=9,ParentID=3},
                new DevelopFun{Id=11,Name="������",ImageIndex=8,ParentID=2},
                new DevelopFun{Id=12,Name="��ӡ",ImageIndex=10,ParentID=1},
                };
                context.DevelopFuns.AddOrUpdate(funList.ToArray());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
