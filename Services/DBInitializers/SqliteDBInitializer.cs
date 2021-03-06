﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SQLite.EF6.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataEntitys;
using Services.EFCodeFirst;
using SQLite.CodeFirst;

namespace Services.Migrations
{
    public sealed class SqliteDBInitializer : DbMigrationsConfiguration<RecordContext>
    {
        public SqliteDBInitializer()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(RecordContext context)
        {
            try
            {
                // 初始化DevelopUsers 默认数据
                context.DevelopUsers.AddOrUpdate(new DevelopUser()
                {
                    Id = 1,
                    Name = "admin",
                    Pwd = Common.UtilityHelper.MD5Encrypt("0"),
                });

                // 初始化DevelopUsers对应的权限
                DevelopFun[] funList = new DevelopFun[]
                {
                new DevelopFun{Id = 1, Name="主菜单",ImageIndex=0,ParentID=0 },
                new DevelopFun{Id = 2, Name="版块管理",ImageIndex=0,ParentID=0 },
                new DevelopFun{Id=3, Name="统计",ImageIndex=0,ParentID=0 },
                new DevelopFun{Id=4,Name="设置",ImageIndex=0,ParentID=0 },
                new DevelopFun{Id=5,Name="用户设置",ImageIndex=7,ParentID=4 },
                new DevelopFun{Id=6,Name="其它设置",ImageIndex=6,ParentID=4 },
                new DevelopFun{Id=7,Name="新增",ImageIndex=0,ParentID=1 },
                new DevelopFun{Id=8,Name="修改",ImageIndex=4,ParentID=1 },
                new DevelopFun{Id=9,Name="删除",ImageIndex=1,ParentID=1 },
                new DevelopFun{Id=10,Name="查询统计", ImageIndex=9,ParentID=3 },
                new DevelopFun{Id=11,Name="版块管理",ImageIndex=8,ParentID=2 },
                new DevelopFun{Id=12,Name="打印",ImageIndex=10,ParentID=1 },
                };
                context.DevelopFuns.AddOrUpdate(funList.ToArray());
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                if (ex is DbEntityValidationException dbEntity)
                {
                    dbEntity.EntityValidationErrors.ToList().ForEach(f =>
                    {
                        f.ValidationErrors.ToList().ForEach(m =>
                        {
                            Console.WriteLine(m.ErrorMessage);
                        });
                    });
                }

                MsgHelper.ShowError(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        //public SqliteConfiguration(DbModelBuilder modelBuilder)
        //   : base(modelBuilder, true)
        //{
        //}

        //protected override void Seed(RecordContext context)
        //{
        //    try
        //    {
        //        // 初始化DevelopUsers 默认数据
        //        context.DevelopUsers.AddOrUpdate(new DevelopUser()
        //        {
        //            Id = 1,
        //            Name = "admin",
        //            Pwd = Common.UtilityHelper.MD5Encrypt("0"),
        //        });

        //        // 初始化DevelopUsers对应的权限
        //        DevelopFun[] funList = new DevelopFun[]
        //        {
        //        new DevelopFun{Id = 1, Name="主菜单",ImageIndex=0,ParentID=0 },
        //        new DevelopFun{Id = 2, Name="版块管理",ImageIndex=0,ParentID=0 },
        //        new DevelopFun{Id=3, Name="统计",ImageIndex=0,ParentID=0 },
        //        new DevelopFun{Id=4,Name="设置",ImageIndex=0,ParentID=0 },
        //        new DevelopFun{Id=5,Name="用户设置",ImageIndex=7,ParentID=4 },
        //        new DevelopFun{Id=6,Name="其它设置",ImageIndex=6,ParentID=4 },
        //        new DevelopFun{Id=7,Name="新增",ImageIndex=0,ParentID=1 },
        //        new DevelopFun{Id=8,Name="修改",ImageIndex=4,ParentID=1 },
        //        new DevelopFun{Id=9,Name="删除",ImageIndex=1,ParentID=1 },
        //        new DevelopFun{Id=10,Name="查询统计", ImageIndex=9,ParentID=3 },
        //        new DevelopFun{Id=11,Name="版块管理",ImageIndex=8,ParentID=2 },
        //        new DevelopFun{Id=12,Name="打印",ImageIndex=10,ParentID=1 },
        //        };
        //        context.DevelopFuns.AddOrUpdate(funList.ToArray());
        //        context.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is DbEntityValidationException dbEntity)
        //        {
        //            dbEntity.EntityValidationErrors.ToList().ForEach(f =>
        //            {
        //                f.ValidationErrors.ToList().ForEach(m =>
        //                {
        //                    Console.WriteLine(m.ErrorMessage);
        //                });
        //            });
        //        }

        //        MsgHelper.ShowError(ex.Message);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }
}
