﻿using System;
using Services.EFCodeFirst;

namespace Services
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                /*自动进行数据库迁移*/
                Console.WriteLine("开始迁移数据库");
                using (var context = new RecordContext())
                {
                    context.Database.Initialize(true);
                    Console.WriteLine("数据库迁移完毕");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error:{ex.Message}");
            }

            Console.WriteLine(".......按任意键退出");
            Console.ReadKey();
        }
    }
}
