using Contracts;
using DataEntitys;
using Services.DataAccess;
using System;
using System.Collections.Generic;

namespace Services
{
    public class CodeRecordService : BaseDataAccess, ICodeRecord
    {
        #region 单例模式

        private static CodeRecordService _codeRecordService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static CodeRecordService GetInstance()
        {
            if (_codeRecordService == null)
            {
                _obj = new object();
                lock (_obj)
                {
                    if (_codeRecordService == null)
                    {
                        _codeRecordService = new CodeRecordService();
                    }
                }
            }
            return _codeRecordService;
        }

        #endregion 单例模式

        #region 记录操作

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool AddDevelopRecord(DevelopRecord record)
        {
            return _dbContext.Insert<DevelopRecord>("DevelopRecord", record).AutoMap(x => x.Id).ExecuteReturnLastId<int>() > 0 ? true : false;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteDevelop(int Id)
        {
            return _dbContext.Delete("DevelopRecord").Where("Id", Id).Execute() > 0 ? true : false;
        }

        /// <summary>
        /// 根据id 获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DevelopRecord GetDevelopRecordById(int id)
        {
            return _dbContext.Sql("select * from DevelopRecord where id = @0", id).QuerySingle<DevelopRecord>();
        }

        /// <summary>
        /// 获取记录集合(弃用)
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public IList<DevelopRecordEntity> GetDevelopRecordList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _dbContext.StoredProcedure("proc_GetDevelopRecord").QueryMany<DevelopRecordEntity>();
            }
            return null;
        }

        /// <summary>
        /// 批量 加载数据
        /// </summary>
        /// <param name="pageIndex">页</param>
        /// <param name="pageCount">每页数量</param>
        public IList<DevelopRecordEntity> GetDevelopRecordListByPager(int pageIndex, int pageCount)
        {
            return _dbContext.StoredProcedure("proc_GetDevelopRecordPager")
                 .Parameter("pageIndex", pageIndex)
                 .Parameter("pageCount", pageCount).QueryMany<DevelopRecordEntity>();
        }

        /// <summary>
        /// 批量加载数据
        /// </summary>
        /// <param name="startId">起始Id</param>
        /// <param name="stepId">终止Id</param>
        /// <returns></returns>
        public IList<DevelopRecordEntity> GetDevelopRecordListByStep(int startId, int stepId)
        {
            return _dbContext.StoredProcedure("proc_GetDevelopRecord")
                 .Parameter("startId", startId)
                 .Parameter("endId", stepId).QueryMany<DevelopRecordEntity>();
        }

        /// <summary>
        /// 获取总记录数量
        /// </summary>
        /// <returns></returns>
        public int GetDevelopRecordListCount()
        {
            return _dbContext.Sql("select count(id) from DevelopRecord").QuerySingle<int>();
        }

        /// <summary>
        /// 获取最大Develop Id 值
        /// </summary>
        public int GetMaxDevelopId()
        {
            return _dbContext.Sql("select max(Id) from DevelopRecord").QuerySingle<int>();
        }

        /// <summary>
        /// 更新developrecord
        /// </summary>
        /// <param name="record">原DevelopRecord对象</param>
        public bool UpdateDevelopRecord(DevelopRecord record)
        {
            DevelopRecord developRecord = _dbContext.Sql("select * from  DevelopRecord where Id=@0", record.Id).QuerySingle<DevelopRecord>();
            developRecord.Title = record.Title;
            developRecord.Desc = record.Desc;
            developRecord.TypeId = record.TypeId;
            developRecord.UpdatedTime = record.UpdatedTime;
            developRecord.Picture = record.Picture;
            if (record.Zip != null && record.Zip.Length > 0)
            {
                developRecord.Zip = record.Zip;
            }
            return _dbContext.Update<DevelopRecord>("DevelopRecord", developRecord)
                .AutoMap(x => x.Id)
                .Where(x => x.Id).Execute() > 0 ? true : false;
        }

        #endregion 记录操作

        #region 记录类型操作

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type">要添加的类型</param>
        public bool AddDevelopType(DevelopType type)
        {
            return _dbContext.Insert<DevelopType>("DevelopType", type).AutoMap(x => x.Id).ExecuteReturnLastId<int>() > 0 ? true : false;
        }

        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        public bool DeleteDeveloptype(int typeId)
        {
            return _dbContext.Delete("DevelopType").Where("Id", typeId).Execute() > 0 ? true : false;
        }

        /// <summary>
        /// 根据Id 获取类型对象
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public DevelopType GetDevelopTypeById(int id)
        {
            return _dbContext.Select<DevelopType>(string.Format("select * from DevelopType where Id={0}", id)).QuerySingle();
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        /// <returns></returns>
        public IList<DevelopType> GetDevelopTypeList()
        {
            return _dbContext.Select<DevelopType>(" * ").From("DevelopType").OrderBy(" UpdatedTime Desc").QueryMany();
        }

        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="name">类型名称</param>
        /// <param name="parentId">父Id</param>
        public IList<DevelopType> GetDevelopTypeListByFilter(string name, int parentId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return _dbContext.Select<DevelopType>("select * from DevelopType").QueryMany();
            }
            return _dbContext.Sql("select * from DevelopType where Name=@0 and ParentId = @1").Parameters(name, parentId).QueryMany<DevelopType>();
        }

        /// <summary>
        /// 获取最大类型Id
        /// </summary>
        public int GetMaxDevelopTypeId()
        {
            return _dbContext.Sql("select Max(Id) from DevelopType").QuerySingle<int>();
        }

        /// <summary>
        /// 更新记录点击次数
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public int UpdateDevelopRecordClickCount(int recordId)
        {
            return _dbContext.Sql(string.Format("update DevelopRecord set ClickCount+=1 where Id={0}", recordId), null).Execute();
        }

        /// <summary>
        /// 更新类型
        /// </summary>
        /// <param name="type">原类型对象</param>
        public bool UpdateDeveloptype(DevelopType type)
        {
            DevelopType developType = _dbContext.Sql("select * from DevelopType where Id=@0", type.Id).QuerySingle<DevelopType>();
            developType.Name = type.Name;
            developType.UpdatedTime = DateTime.Now;
            return _dbContext.Update<DevelopType>("DevelopType", developType).AutoMap(x => x.Id).Where(x => x.Id).Execute() > 0 ? true : false;
        }

        #endregion 记录类型操作

        #region 统计

        public IList<ClickCountReportEntity> GetClickCountReport()
        {
            return _dbContext.StoredProcedure("proc_ClickCountReport").QueryMany<ClickCountReportEntity>();
        }

        #endregion 统计

        #region 人员操作

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="user">user对象</param>
        public bool AddUser(DevelopUser user)
        {
            return _dbContext.Insert("DevelopUser", user)
    .AutoMap(x => x.Id)
    .ExecuteReturnLastId<int>() > 0 ? true : false;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        public bool DeleteUser(int userId)
        {
            return _dbContext.Delete("DevelopUser").Where("Id", userId).Execute() > 0 ? true : false;
        }

        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pwd">密码</param>
        public DevelopUser GetDevelopUser(string name, string pwd)
        {
            return _dbContext.Sql(@"select * from DevelopUser where Name=@0 and Pwd=@1")
                .Parameters(name, pwd).QuerySingle<DevelopUser>();
        }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId">userId</param>
        public DevelopUser GetDevelopUserByUserId(int userId)
        {
            return _dbContext.Sql("select * from DevelopUser where id=@0", userId).QuerySingle<DevelopUser>();
        }

        /// <summary>
        /// 加载所有用户
        /// </summary>
        /// <returns></returns>
        public IList<DevelopUser> GetDevelopUserList()
        {
            return _dbContext.Select<DevelopUser>(" * ").From("DevelopUser").OrderBy("Id desc").QueryMany();
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user">原User对象</param>
        /// <returns></returns>
        public bool UpdateUser(DevelopUser user)
        {
            DevelopUser developUser = _dbContext.Sql("select * from DevelopUser where Id=@0", user.Id).QuerySingle<DevelopUser>();
            developUser.Name = user.Name;
            developUser.Sex = user.Sex;
            developUser.Pwd = user.Pwd;
            developUser.DevelopAge = user.DevelopAge;
            return _dbContext.Update("DevelopUser", developUser).AutoMap(x => x.Id).Where(w => w.Id).Execute() > 0 ? true : false;
        }

        #endregion 人员操作

        #region 功能项操作

        public IList<DevelopFun> GetDevelopFunList()
        {
            return _dbContext.Sql("select * from DevelopFun order by Id asc").QueryMany<DevelopFun>();
        }

        #endregion 功能项操作

        #region 权限功能操作

        public bool AddDevelopPowerFun(DevelopPowerFun developPowerFun)
        {
            return false;
        }

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        public IList<DevelopPowerFun> GetDevelopPowerFunByUserId(int userId)
        {
            return _dbContext.Sql("select * from DevelopPowerFun where UserId=@0 order by FunId desc", userId).QueryMany<DevelopPowerFun>();
        }

        public IList<DevelopPowerFun> GetDevelopPowerFunList()
        {
            return null;
        }

        /// <summary>
        /// 获取用户权限视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IList<View_DevelopUserPowerFun> GetViewDevelopUserPowerFunByUserId(int userId)
        {
            return _dbContext.Sql("select * from View_DevelopUserPowerFun where UserId=@0 order by FunId asc", userId).QueryMany<View_DevelopUserPowerFun>();
        }

        /// <summary>
        /// 设置用权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="funId">功能Id</param>
        /// <param name="isEnabled">是否启用</param>
        public bool SetDevelopPowerFun(int userId, int funId, bool isEnabled)
        {
            return _dbContext.StoredProcedure("proc_SaveUserFun")
                .Parameter("UserId", userId)
                .Parameter("FunId", funId)
                .Parameter("IsEnable", isEnabled).Execute() > 0 ? true : false;
        }

        #endregion 权限功能操作
    }
}