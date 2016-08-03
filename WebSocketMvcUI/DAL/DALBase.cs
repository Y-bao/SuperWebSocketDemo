using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Dos.ORM;

namespace WebSocketMvcUI.DAL
{
    public class DALBase<T> where T : Entity
    {
        private static DbSession _db = null;

        private static object objLocker = new object();


        /// <summary>
        /// SqlserverDbsession
        /// </summary>
        public static DbSession DB
        {

            get
            {
                if (_db == null)
                {
                    lock (objLocker)
                    {
                        if (_db == null)
                        {
                            string strSql = "data source=.;Database=ZKPT_DB;uid=sa;pwd=sa;";
                            //string strSql = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString.Trim();
                            _db = new DbSession(DatabaseType.SqlServer, strSql);
                        }
                    }
                }
                return _db;
            }
        }

        #region 数据库基本操作
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(T model)
        {
            //model.Attach();
            return DB.Insert(model);
        }

        /// <summary> 
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public bool Update(T model)
        {
            model.Attach();
            return DB.Update(model) > 0;
        }


        public int Update<T>(Dictionary<Field, object> fieldValue, WhereClip where) where T : Entity
        {
            return DB.Update<T>(fieldValue, where);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public bool Delete(WhereClip clip)
        {
            return DB.Delete<T>(clip) > 0;
        }

        /// <summary>
        /// 得到一个实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetModel(WhereClip clip)
        {
            return DB.From<T>().Where(clip).First();
        }
        /// <summary>
        /// 获得所有
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllList(WhereClip clip)
        {
            return DB.From<T>().Where(clip).ToList() as List<T>;
        }
        /// <summary>
        /// 返回指定列的查询结果
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> GetListByFieldName(params Field[] fields)
        {
            return DB.From<T>().Select(fields).ToList() as List<T>;
        }
        /// <summary>
        /// 根据条件获得数据列表；
        /// </summary>
        /// <param name="clip"></param>
        /// <returns>DataTable</returns>
        public DataTable GetAllTableBy(WhereClip clip)
        {
            return DB.From<T>().Where(clip).ToDataTable() as DataTable;
            //return DB.FromSql("").ToTable() as DataTable;
        }




        public DataTable GetTableBySql(string sql)
        {
            return DB.FromSql(sql).ToDataTable() as DataTable;
        }

        
        /// <summary>
        /// 获取主从表联合查询
        /// </summary>
        /// <typeparam name="U">外键表</typeparam>
        /// <param name="clip">条件</param>
        /// <param name="onWhere">链接条件</param>
        /// <param name="fields">查询的字段</param>
        /// <returns></returns>
        public DataTable GetTableByLeft<U>(WhereClip clip, WhereClip onWhere, Field[] fields) where U : Entity
        {
            return DB.From<T>().LeftJoin<U>(onWhere).Where(clip).Select(fields).ToDataTable() as DataTable;
        }

        /// 主外表插入事务方法
        /// </summary>
        /// <typeparam name="U">从表实体</typeparam>
        /// <param name="model">主表实体</param>
        /// <param name="list">从表实体列表</param>
        /// <returns></returns>
        public bool InsertTrans<U>(T model, List<U> list) where U : Entity
        {
            var trans = DB.BeginTransaction();
            try
            {
                if (list.Count <= 0)
                {
                    return false;
                }
                model.Attach();
                trans.Insert(model);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Attach();
                    trans.Insert(list[i]);
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
            }
            finally
            {
                trans.Dispose();
            }
            return false;
        }



        #endregion
    }
}