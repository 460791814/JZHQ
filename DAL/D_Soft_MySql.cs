using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

using System.Data;
using Maticsoft.DBUtility;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using IDAL;


namespace DAL
{
    public class D_Soft_MySql : ID_Soft
    {
        /// <summary>
        /// 根据HASH值获取详情
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public E_Soft SelectByHash(E_Soft eSoft)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Hash,Name,Length,Details,SoftType,FileCount,Hit,Area,Publisher,UpdateTime from t_soft ");
            strSql.Append(" where Hash=@Hash");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Hash", MySqlDbType.VarChar)
			};
            parameters[0].Value = eSoft.Hash;

            E_Soft model = new E_Soft();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM t_soft ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from t_soft T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectSoft(E_Soft eSoft, ref int total)
        {
           // MySqlCommand cmd;
         //   MySqlConnection myConn = new MySqlConnection("server=198.211.33.215;user id=root; password=root!@#$%^&*(); database=hash; pooling=false;port=3306");
            if (string.IsNullOrEmpty(eSoft.KeyWord))
            {
                return null;
            }


            List<E_Soft> list = new List<E_Soft>();
 

            string PageCountSqlstr = "SELECT COUNT(1) FROM t_soft where Name like concat ('%',@KeyWord ,'%')";
            //cmd = myConn.CreateCommand();
            //cmd.CommandText = PageCountSqlstr;
            //cmd.Parameters.Add(new MySqlParameter("@KeyWord", eSoft.KeyWord));
            //myConn.Open();
           var ObjCount = DbHelperMySQL.GetSingle(PageCountSqlstr, new MySqlParameter("@KeyWord", eSoft.KeyWord));
            //var ObjCount = cmd.ExecuteScalar();
            int RowCount = Convert.ToInt32(ObjCount);
            total = RowCount;
            int PageCount = RowCount / eSoft.PageSize;
            if (RowCount % eSoft.PageSize > 0)
            {
                PageCount += 1;
            }

            if (eSoft.CurrentPage >= PageCount)
            {
                eSoft.CurrentPage = PageCount;
            }
            if (eSoft.CurrentPage <= 0)
            {
                eSoft.CurrentPage = 1;
            }
            string PageSqlStr = "SELECT Hash,Name,Length,Area,Hit ,FileCount,SoftType,Publisher,UpdateTime FROM t_soft  where Name like concat ('%',@KeyWord ,'%') LIMIT {0},{1}";
            PageSqlStr = string.Format(PageSqlStr, (eSoft.PageSize * (eSoft.CurrentPage - 1) + 1).ToString(), (eSoft.PageSize * eSoft.CurrentPage).ToString());
            //cmd.CommandText = PageSqlStr;
            //cmd.Parameters.Clear();
            //cmd.Parameters.Add(new MySqlParameter("@KeyWord", eSoft.KeyWord));
            using (MySqlDataReader reader = DbHelperMySQL.ExecuteReader(PageSqlStr, new MySqlParameter("@KeyWord", eSoft.KeyWord)))
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


                        _eSoft.Hit = Convert.ToInt32(reader["Hit"]);

                        _eSoft.FileCount = Convert.ToInt32(reader["FileCount"]);
                        _eSoft.SoftType = Convert.ToInt32(reader["SoftType"]);

                        _eSoft.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        list.Add(_eSoft);
                    }

                }
            }
            //myConn.Close();
            return list;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public E_Soft DataRowToModel(DataRow row)
        {
            E_Soft model = new E_Soft();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["Hash"] != null)
                {
                    model.Hash = row["Hash"].ToString();
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Length"] != null)
                {
                    model.Length = row["Length"].ToString();
                }
                if (row["Details"] != null)
                {
                    model.Details = row["Details"].ToString();
                }
                if (row["SoftType"] != null && row["SoftType"].ToString() != "")
                {
                    model.SoftType = int.Parse(row["SoftType"].ToString());
                }
                if (row["FileCount"] != null && row["FileCount"].ToString() != "")
                {
                    model.FileCount = int.Parse(row["FileCount"].ToString());
                }
                if (row["Hit"] != null && row["Hit"].ToString() != "")
                {
                    model.Hit = int.Parse(row["Hit"].ToString());
                }
                if (row["Area"] != null && row["Area"].ToString() != "")
                {
                    model.Area = int.Parse(row["Area"].ToString());
                }
                if (row["Publisher"] != null)
                {
                    model.Publisher = row["Publisher"].ToString();
                }
                if (row["UpdateTime"] != null && row["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获取资料列表
        /// </summary>
        /// <param name="eSoft"></param>
        /// <returns></returns>
        public List<E_Soft> SelectSoftTop100(string count)
        {
            List<E_Soft> list = new List<E_Soft>();
            string sql = @"SELECT   Hash,Name,Length,Area,Hit ,FileCount,SoftType,Publisher,UpdateTime FROM  t_soft order by updatetime desc  LIMIT 0, " + count;

            using (MySqlDataReader reader = DbHelperMySQL.ExecuteReader(sql))
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
        public bool InsertInToKeyWord(E_KeyWord model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into t_keyword(");
            strSql.Append("KeyWord,Hit,UpdateTime)");
            strSql.Append(" values (");
            strSql.Append("@KeyWord,@Hit,@UpdateTime)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@KeyWord", MySqlDbType.VarChar,100),
					new MySqlParameter("@Hit", MySqlDbType.Int32,11),
					new MySqlParameter("@UpdateTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.KeyWord;
            parameters[1].Value = model.Hit;
            parameters[2].Value = model.UpdateTime;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from t_keyword");
            strSql.Append(" where KeyWord=@KeyWord");
            MySqlParameter[] parameters = {
					new MySqlParameter("@KeyWord", MySqlDbType.VarChar)
			};
            parameters[0].Value = eKey.KeyWord;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 关键字Hit+1
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool UpdateHitForKeyWord(E_KeyWord model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update t_keyword set ");
    
            strSql.Append("Hit=Hit+1,");
            strSql.Append("UpdateTime=@UpdateTime");
            strSql.Append(" where KeyWord=@KeyWord");
            MySqlParameter[] parameters = {
					new MySqlParameter("@KeyWord", MySqlDbType.VarChar,100),
                      new MySqlParameter("@UpdateTime", MySqlDbType.DateTime)
                                          };
            parameters[0].Value = model.KeyWord;
            parameters[1].Value = DateTime.Now;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
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
            string sql = @"update  T_Soft set Hit=Hit+1,UpdateTime=@UpdateTime where Hash=@Hash";
            MySqlParameter[] para = new MySqlParameter[]{
                new MySqlParameter("@Hash", MySqlDbType.VarChar),
          new MySqlParameter("@UpdateTime", MySqlDbType.DateTime)
           };
            para[0].Value = hash;
            para[1].Value = DateTime.Now;
            int count = DbHelperMySQL.ExecuteSql(sql, para.ToArray());

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
            string sql = @"select count(*) from  t_soft";

            object count = DbHelperMySQL.GetSingle(sql);
            return count.ToString();

        }
        public string GetSoftCountZuoRi()
        {
            string sql = @"select count(*) from t_soft where  TO_DAYS(NOW()) – TO_DAYS(UpdateTime) = 1 ";

            object count = DbHelperMySQL.GetSingle(sql);
            return count.ToString();

        }
        public string GetSoftCountJinRi()
        {
            string sql = @"select count(*) from t_soft where  to_days(UpdateTime)=to_days(now())";

            object count = DbHelperMySQL.GetSingle(sql);
            return count.ToString();

        }



        public bool GetKey()
        {
            throw new NotImplementedException();
        }

        public bool InsertInToUpload(E_Soft eSoft)
        {
            throw new NotImplementedException();
        }

        public List<E_Soft> SelectUploadTop100()
        {
            throw new NotImplementedException();
        }
    }
}
