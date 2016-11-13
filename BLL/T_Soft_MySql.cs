using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using DAL;
using System.Data;

namespace BLL
{
   public  class T_Soft_MySql
    {
       D_Soft_MySql dal = new D_Soft_MySql();
        /// <summary>
        /// 根据HASH值获取详情
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public E_Soft SelectByHash(E_Soft eSoft)
        {
            return dal.SelectByHash(eSoft);
        }
               /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
        }
        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectSoft(E_Soft eSoft, ref int total)
        {
            return dal.SelectSoft(eSoft, ref total);
        }
               /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectSoftTop100(string count)
        {
            return dal.SelectSoftTop100(count);
        }
    }
}
