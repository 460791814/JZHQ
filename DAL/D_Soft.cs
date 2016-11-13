using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using DBUtility;
using System.Data.SqlClient;
using IDAL;

namespace DAL
{
    public class D_Soft : ID_Soft
    {
       /// <summary>
       /// 根据HASH值获取详情
       /// </summary>
       /// <param name="eSoft"></param>
       /// <returns></returns>
       public E_Soft SelectByHash(E_Soft eSoft)
       {
         
           E_Soft _eSoft = new E_Soft();
           string sql = @"SELECT 
                                   [Hash]
                                  ,[Name]
                                  ,[Length]
                                  ,[Hit]
                                  ,[MonthHit]
                                  ,[FileCount]
                                  ,[SoftType]
                                  ,[Details]
                                  ,[Area]
                                  ,[Publisher]
                                  ,[UpdateTime]
                              FROM [dbo].[T_Soft] WHERE Hash=@Hash ";

           using (SqlDataReader reader= SqlHelper.ExecuteReader(sql,new SqlParameter("@Hash",eSoft.Hash)))
           {
               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                     
                    
                       _eSoft.Hash = reader["Hash"].ToString();
                       _eSoft.Name = reader["Name"].ToString();
                       _eSoft.Length = reader["Length"].ToString();
                       _eSoft.Area = Convert.ToInt32(reader["Area"]);
                       _eSoft.Details = reader["Details"].ToString();
                       _eSoft.Hit = Convert.ToInt32(reader["Hit"]);
                       _eSoft.FileCount = Convert.ToInt32(reader["FileCount"]);
                       _eSoft.SoftType = Convert.ToInt32(reader["SoftType"]);
                       _eSoft.Publisher = reader["Publisher"].ToString();
                       _eSoft.UpdateTime =Convert.ToDateTime( reader["UpdateTime"]);
                     
                   }

               }
           }
           return _eSoft;
       }
       /// <summary>
       /// 获取资料列表
       /// </summary>
       /// <param name="eSoft"></param>
       /// <returns></returns>
       public List<E_Soft> SelectSoft(E_Soft eSoft,ref int total)
       {
           if (string.IsNullOrEmpty(eSoft.KeyWord))
           {
               return null;
           }
           List<E_Soft> list = new List<E_Soft>();
           string sql = @" select  ROW_NUMBER() OVER ( ORDER BY UpdateTime desc) AS RID,
                                  [Hash]
                                  ,[Name]
                                  ,[Length]
                                  ,[Hit]
                                  ,[MonthHit]
                               ,[FileCount]
                                  ,[SoftType]
                                
                                  ,[Area]
                                  ,[Publisher]
                                  ,[UpdateTime]
                              FROM [dbo].[T_Soft] where Name like '%'+@KeyWord +'%' ";

           string PageCountSqlstr = "SELECT COUNT(1) as RowsCount FROM (" + sql + ") AS CountList";
           var ObjCount = SqlHelper.ExecuteScalar(PageCountSqlstr, new SqlParameter("@KeyWord",eSoft.KeyWord));
           int RowCount = Convert.ToInt32(ObjCount);
           total = RowCount;
           int PageCount = RowCount /eSoft.PageSize ;
           if (RowCount % eSoft.PageSize > 0)
           {
               PageCount += 1;
           }

           if (eSoft.CurrentPage >= PageCount)
           {
               eSoft.CurrentPage = PageCount;
           }
           string PageSqlStr = "select * from ( " + sql + " ) as Temp_PageData where Temp_PageData.RID BETWEEN {0} AND {1}";
           PageSqlStr = string.Format(PageSqlStr, (eSoft.PageSize * (eSoft.CurrentPage - 1) + 1).ToString(), (eSoft.PageSize * eSoft.CurrentPage).ToString());

           using (SqlDataReader reader = SqlHelper.ExecuteReader(PageSqlStr, new SqlParameter("@KeyWord", eSoft.KeyWord)))
           {
               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       E_Soft _eSoft = new E_Soft();
                       _eSoft.Hash = reader["Hash"].ToString();
                       _eSoft.URL = reader["Hash"].ToString();
                       _eSoft.Name = reader["Name"].ToString();
                       _eSoft.Length = reader["Length"].ToString();
                       _eSoft.Area = Convert.ToInt32(reader["Area"]);
                       // _eSoft.Details = reader["Details"].ToString();
                       _eSoft.Hit = Convert.ToInt32(reader["Hit"]);

                       _eSoft.FileCount = Convert.ToInt32(reader["FileCount"]);
                       _eSoft.SoftType = Convert.ToInt32(reader["SoftType"]);
                       _eSoft.Publisher = reader["Publisher"].ToString();
                       _eSoft.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                       list.Add(_eSoft);
                   }

               }
           }
           return list;
       }
       /// <summary>
       /// 插入关键字表
       /// </summary>
       /// <param name="keyWord"></param>
       /// <returns></returns>
       public bool InsertInToKeyWord(E_KeyWord eKey) {
           string sql = @"INSERT INTO T_KeyWord
           ([KeyWord]
           ,[Hit]
           ,[IsSearch]
           ,[UpdateTime]
          )
     VALUES
           (@KeyWord
           ,@Hit
           ,@IsSearch,GETDATE())";
           SqlParameter[] para=new SqlParameter[]{
           new SqlParameter("@KeyWord",eKey.KeyWord),
           new SqlParameter("@Hit",eKey.Hit),
           new SqlParameter("@IsSearch",eKey.IsSearch)

           };
         int count=  SqlHelper.ExecuteNonQuery(sql, para.ToArray());
         if (count > 0)
         {
             return true;
         }
         else {
             return false;
         }
       }
       /// <summary>
       /// 检查关键字是否存在
       /// </summary>
       /// <param name="keyWord"></param>
       /// <returns></returns>
       public bool SelectKeyWord(E_KeyWord eKey)
       {
           string sql = @"select COUNT(*) from  T_KeyWord where KeyWord=@KeyWord";
           SqlParameter[] para = new SqlParameter[]{
           new SqlParameter("@KeyWord",eKey.KeyWord),
         

           };
           object count = SqlHelper.ExecuteScalar(sql, para.ToArray());

           if (Convert.ToInt32(count) > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 关键字Hit+1
       /// </summary>
       /// <param name="keyWord"></param>
       /// <returns></returns>
       public bool UpdateHitForKeyWord(E_KeyWord eKey)
       {
           string sql = @"update T_KeyWord set Hit=Hit+1,UpdateTime=GETDATE() where KeyWord=@KeyWord";
           SqlParameter[] para = new SqlParameter[]{
           new SqlParameter("@KeyWord",eKey.KeyWord),
         
           };
           int count = SqlHelper.ExecuteNonQuery(sql, para.ToArray());

           if (count > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }

       /// <summary>
       /// 统计资料下载次数
       /// </summary>
       /// <param name="hash"></param>
       /// <returns></returns>
       public bool UpdateHitForSoft(string hash)
       {
           string sql = @"update  dbo.T_Soft set Hit=Hit+1,UpdateTime=GETDATE() where Hash=@Hash";
           SqlParameter[] para = new SqlParameter[]{
           new SqlParameter("@Hash",hash),
         
           };
           int count = SqlHelper.ExecuteNonQuery(sql, para.ToArray());

           if (count > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 统计资料下载次数
       /// </summary>
       /// <param name="hash"></param>
       /// <returns></returns>
       public string GetSoftCount()
       {
           string sql = @"select count(*) from  dbo.T_Soft";

           object count = SqlHelper.ExecuteScalar(sql);
           return count.ToString();
 
       }
       public string GetSoftCountZuoRi()
       {
           string sql = @"select count(*) from [dbo].[T_Soft] where  datediff(day,[UpdateTime],getdate())=1 ";

           object count = SqlHelper.ExecuteScalar(sql);
           return count.ToString();

       }
       public string GetSoftCountJinRi()
       {
           string sql = @"select count(*) from [dbo].[T_Soft] where  datediff(day,[UpdateTime],getdate())=0 ";

           object count = SqlHelper.ExecuteScalar(sql);
           return count.ToString();

       }
       public bool GetKey()
       {
           string sql = @"SELECT [IsUse] FROM [dbo].[T_Key] where KeyName='xiaosu'";

           object count = ExecuteScalar(sql);

           if (Convert.ToBoolean(count))
           {
               return true;
           }
           else
           {
               return false;
           }
       }

       /// <summary>
       /// 获取资料列表
       /// </summary>
       /// <param name="eSoft"></param>
       /// <returns></returns>
       public List<E_Soft> SelectSoftTop100(string count)
       {
           List<E_Soft> list = new List<E_Soft>();
           string sql = @"SELECT top "+count+" [Hash],[Name],[Length] ,[Hit] ,[MonthHit],[FileCount] ,[SoftType] ,[Area],[Publisher] ,[UpdateTime] FROM [dbo].[T_Soft] order by UpdateTime desc ";

           using (SqlDataReader reader = SqlHelper.ExecuteReader(sql))
           {
               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       E_Soft _eSoft = new E_Soft();
                      
                       _eSoft.Hash = reader["Hash"].ToString();
                       _eSoft.URL = reader["Hash"].ToString();
                       _eSoft.Name = reader["Name"].ToString();
                       _eSoft.Length = reader["Length"].ToString();
                       _eSoft.Area = Convert.ToInt32(reader["Area"]);
                      // _eSoft.Details = reader["Details"].ToString();
                       _eSoft.Hit = Convert.ToInt32(reader["Hit"]);
                       
                       _eSoft.FileCount = Convert.ToInt32(reader["FileCount"]);
                       _eSoft.SoftType = Convert.ToInt32(reader["SoftType"]);
                       _eSoft.Publisher = reader["Publisher"].ToString();
                       _eSoft.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                       list.Add(_eSoft);
                   }

               }
           }
           return list;
       }

       /// <summary>
       /// 插入上传表
       /// </summary>
       /// <param name="keyWord"></param>
       /// <returns></returns>
       public bool InsertInToUpload(E_Soft eSoft)
       {
           string sql = @"INSERT INTO T_Upload
           ([Hash]
           ,[Name]
       
           ,[WriteTime]
          )
     VALUES
           (@Hash
           ,@Name
           ,GETDATE())";
           SqlParameter[] para = new SqlParameter[]{
           new SqlParameter("@Hash",eSoft.Hash),
           new SqlParameter("@Name",eSoft.Name)
        

           };
           int count = SqlHelper.ExecuteNonQuery(sql, para.ToArray());
           if (count > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }

       /// <summary>
       /// 获取上传列表
       /// </summary>
       /// <param name="eSoft"></param>
       /// <returns></returns>
       public List<E_Soft> SelectUploadTop100()
       {
           List<E_Soft> list = new List<E_Soft>();
           string sql = @"SELECT top 100  [Hash] ,[Name],WriteTime FROM [dbo].[T_Upload] order by WriteTime desc ";

           using (SqlDataReader reader = SqlHelper.ExecuteReader(sql))
           {
               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       E_Soft _eSoft = new E_Soft();

                       _eSoft.Hash = reader["Hash"].ToString();
                       _eSoft.URL = reader["Hash"].ToString() + "-0";
                       _eSoft.Name = reader["Name"].ToString();
                       _eSoft.UpdateTime = Convert.ToDateTime(reader["WriteTime"]);
                       list.Add(_eSoft);
                   }

               }
           }
           return list;
       }

       public static object ExecuteScalar(string sql, params SqlParameter[] pms)
       {
           using (SqlConnection conn = new SqlConnection("Data Source=115.28.213.122;Initial Catalog=Safe;User ID=jzhq;Password=qwertyuiop"))
           {
               using (SqlCommand cmd = new SqlCommand(sql, conn))
               {
                   cmd.Parameters.AddRange(pms);
                   conn.Open();
                   return cmd.ExecuteScalar();
               }
           }
       }


    }
}
