using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using DAL;
using IDAL;
using System.Configuration;


namespace BLL
{
    public class T_Soft
    {
        ID_Soft dal = null;
        public T_Soft()
        {
            if (ConfigurationManager.AppSettings["IsMySql"] == "1")
            {
                dal = new D_Soft_MySql();
            }
            else
            {
                 dal = new D_Soft();
            }
        }



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
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectSoft(E_Soft eSoft, ref int total)
        {
            return dal.SelectSoft(eSoft, ref total);
        }
        /// <summary>
        /// 插入关键字表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool InsertInToKeyWord(E_KeyWord eKey)
        {

            return dal.InsertInToKeyWord(eKey);
        }
        /// <summary>
        /// 检查关键字是否存在
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool SelectKeyWord(E_KeyWord eKey)
        {
            return dal.SelectKeyWord(eKey);
        }
        /// <summary>
        /// 关键字Hit+1
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool UpdateHitForKeyWord(E_KeyWord eKey)
        {
            return dal.UpdateHitForKeyWord(eKey);
        }
        /// <summary>
        /// 统计资料下载次数
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool UpdateHitForSoft(string hash)
        {
            return dal.UpdateHitForSoft(hash);
        }
        public string GetSoftCount()
        {
            return dal.GetSoftCount();

        }
        public string GetSoftCountZuoRi()
        {
            return dal.GetSoftCountZuoRi();

        }
        public string GetSoftCountJinRi()
        {
            return dal.GetSoftCountJinRi();

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
        public bool GetKey()
        {
            return dal.GetKey();
        }

        /// <summary>
        /// 插入上传表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool InsertInToUpload(E_Soft eSoft)
        {
            return dal.InsertInToUpload(eSoft);
        }
        /// <summary>
        /// 获取上传列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectUploadTop100()
        {
            return dal.SelectUploadTop100();
        }
    }
}
