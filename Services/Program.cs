using Services.EFCodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*自动进行数据库迁移*/
                Console.WriteLine("开始迁移数据库");
                using (RecordContext recordContext = new RecordContext())
                {
                    if (!recordContext.Database.Exists())
                    {
                        recordContext.Database.Create();
                    }
                    recordContext.Database.Initialize(true);
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
