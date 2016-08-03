using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dos.ORM;
using WebSocketMvcUI.DAL;

namespace WebSocketMvcUI.BLL
{
    public class BLLBase<T> where T : Entity
    {
        DALBase<T> dal = new DALBase<T>();
        public int Insert(T model)
        {
            return dal.Insert(model);
        }

        public bool Update(T model)
        {
            return dal.Update(model);
        }

        public bool Delete(WhereClip clip)
        {
            return dal.Delete(clip);
        }

        public List<T> GetAllList(WhereClip clip)
        {
            return dal.GetAllList(clip);
        }

        public List<T> GetListByFieldName(params Field[] fields)
        {
            return dal.GetListByFieldName(fields);
        }

        public DataTable GetAllTableBy(WhereClip clip)
        {
            return dal.GetAllTableBy(clip);
        }

        public T GetModel(WhereClip clip)
        {
            return dal.GetModel(clip);
        }


        public DataTable GetTableBySql(string sql)
        {
            return dal.GetTableBySql(sql);
        }
        public bool InsertTrans<U>(T model, List<U> list) where U : Entity
        {
            return dal.InsertTrans<U>(model, list);
        }


        public DataTable GetTableByLeft<U>(WhereClip clip, WhereClip onWhere, params Field[] fields) where U : Entity
        {
            return dal.GetTableByLeft<U>(clip, onWhere, fields);
        }
    }
}