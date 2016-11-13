using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Model;
using System.Xml;
using System.IO;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.PanGu;
using BLL;

namespace CreateLuceneService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer1;  //计时器
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = 3000;  //设置计时器事件间隔执行时间              
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            timer1.Enabled = true;
        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
        }
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            T_Soft tSoft = new T_Soft();
            //执行SQL语句或其他操作    
            List<E_Soft> list = tSoft.SelectSoft(new E_Soft());
          CreateIndexByData(list);
        }
        public void CreateIndexByData(List<E_Soft> list)
        {
            string indexPath = MapPath("~/IndexData");//索引文档保存位置          
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);


            //--------------------------------遍历数据源 将数据转换成为文档对象 存入索引库
            foreach (var soft in list)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档

                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)SoftType
                document.Add(new Field("Hash", soft.Hash.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据
                document.Add(new Field("SoftType", soft.SoftType.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询


                document.Add(new Field("Name", soft.Name, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离

                document.Add(new Field("Details", GetSoftInro(soft.Details), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document); //文档写入索引库
                writer.Optimize(5);

            }
            writer.Close();//会自动解锁
            directory.Close(); //不要忘了Close，否则索引结果搜不到
        }
        public string GetSoftInro(string str)
        {
            List<E_Soft> list = StrToXml(str);
            StringBuilder intro = new StringBuilder();
            if (list.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    intro.Append(list[i].Name + "<br/>");
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    intro.Append(list[i].Name + "<br/>");
                }
            }
            return intro.ToString();
        }

        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<E_Soft> StrToXml(string str)
        {
            // byte[] buff = System.Text.Encoding.Default.GetBytes(str);


            //stream.Write(buff, 0, buff.Length);
            List<E_Soft> list = new List<E_Soft>();
            XmlDocument xdoc = new XmlDocument();

            xdoc.LoadXml(str);

            foreach (XmlElement birthday in xdoc.DocumentElement.ChildNodes)
            {
                E_Soft eSoft = new E_Soft();
                string name = birthday.SelectSingleNode("name").InnerText;
                string length = birthday.SelectSingleNode("length").InnerText;
                eSoft.Name = name + "&nbsp&nbsp&nbsp&nbsp&nbsp" + length;
                list.Add(eSoft);
            }
            return list;
        }
        public static string MapPath(string strPath) 
        { 
                strPath = strPath.Replace("/", "\\"); 
                if (strPath.StartsWith("\\")) 
                { 
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\'); 
                    strPath = strPath.TrimStart('\\'); 
                } 
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath); 
            } 
    }
}
