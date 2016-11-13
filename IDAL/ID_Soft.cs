using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace IDAL
{
   public  interface ID_Soft
    {
        /// <summary>
        /// 根据HASH值获取详情
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        E_Soft SelectByHash(E_Soft eSoft);
       
        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        List<E_Soft> SelectSoft(E_Soft eSoft, ref int total);
        /// <summary>
        /// 插入关键字表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        bool InsertInToKeyWord(E_KeyWord eKey);
        /// <summary>
        /// 检查关键字是否存在
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        bool SelectKeyWord(E_KeyWord eKey);
        /// <summary>
        /// 关键字Hit+1
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        bool UpdateHitForKeyWord(E_KeyWord eKey);
        /// <summary>
        /// 统计资料下载次数
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool UpdateHitForSoft(string hash);
        string GetSoftCount();
        string GetSoftCountZuoRi();
        string GetSoftCountJinRi();

        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        List<E_Soft> SelectSoftTop100(string count);
        bool GetKey();

        /// <summary>
        /// 插入上传表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        bool InsertInToUpload(E_Soft eSoft);
        /// <summary>
        /// 获取上传列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        List<E_Soft> SelectUploadTop100();
    }
}
