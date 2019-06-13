using FluentData;

namespace Services.DataAccess
{
    /// <summary>
    /// 数据库操作基类
    /// </summary>
    public abstract class BaseDataAccess
    {
        /// <summary>
        /// 数据库操作上下文
        /// </summary>
        protected IDbContext _dbContext;

        private readonly object _obj;

        public BaseDataAccess()
        {
            if (_dbContext == null)
            {
                _obj = new object();
                lock (_obj)
                {
                    if (_dbContext == null)
                    {
                        // dbConnectionString 配置文件数据库连接名称
                        // SqlServerProvider  表示要连接的Sqlserver数据库
                        _dbContext = new DbContext().ConnectionStringName("dbConnectionString", new SqlServerProvider());
                        //// 支持部分事件
                        //_dbContext.OnConnectionOpened(new Action<ConnectionEventArgs>((e) =>
                        //{
                        //    Console.WriteLine("打开连接");
                        //}));
                        //// 关闭连接
                        //_dbContext.OnConnectionClosed(new Action<ConnectionEventArgs>((e) =>
                        //{
                        //    Console.WriteLine("关闭连接");
                        //}));
                        //// 正在执行操作
                        //_dbContext.OnExecuting((args) =>
                        //{
                        //    string commandText = args.Command.CommandText;
                        //    Console.WriteLine(string.Format("正在执行{0},语句{1}", args.Command.CommandType, commandText));
                        //});
                        //// 操作执行完毕
                        //_dbContext.OnExecuted((args) =>
                        //{
                        //    Console.WriteLine(string.Format("执行{0}完毕", args.Command.CommandText));
                        //});
                        //// 捕获执行过程中的异常
                        //_dbContext.OnError((args) =>
                        //{
                        //    Console.WriteLine(string.Format("执行{0}发生错误，错误信息：{1}", args.Command.CommandText, args.Exception.Message));
                        //});
                    }
                }
            }
        }

        /// <summary>
        /// 异常方法名
        /// </summary>
        //public virtual string GetExceptionMethodName()
        //{
        //    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        //    return st.GetFrame(0).GetMethod().Name;
        //}
    }
}