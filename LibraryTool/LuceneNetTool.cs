using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Model;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Analysis.PanGu;
using System.Xml.Linq;
using System.Xml;


namespace LibraryTool
{
    public class LuceneNetTool 
    {
     
        /// <summary>
        /// 创建索引
        /// </summary>
        public  void CreateIndexByData(List<E_Soft> list)
        {
            string indexPath = Utils.MapPath("~/IndexData");//索引文档保存位置          
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

        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        public  List<E_Soft> SearchFromIndexData()
        {
            string indexPath = Utils.MapPath("~/IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            PhraseQuery query = new PhraseQuery();
            //把用户输入的关键字进行分词
            //foreach (string word in Common.SplitContent.SplitWords(Request.QueryString["SearchKey"]))
            //{
            //    query.Add(new Term("content", word));
            //}
            //query.Add(new Term("content", "C#"));//多个查询条件时 为且的关系
            query.Add(new Term("Name", "0416"));
            query.SetSlop(100); //指定关键词相隔最大距离

            //TopScoreDocCollector盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;

            //展示数据实体对象集合
            List<E_Soft> eSoftResult = new List<E_Soft>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document


                E_Soft eSoft = new E_Soft();
                eSoft.Name = doc.Get("Name");
                eSoft.Details = doc.Get("Details");//未使用高亮
                //搜索关键字高亮显示 使用盘古提供高亮插件
              //  eSoft.ContentDescription = System.Data.Common.SplitContent.HightLight(Request.QueryString["SearchKey"], doc.Get("content"));
                eSoft.Hash = doc.Get("Hash");
                eSoftResult.Add(eSoft);
            }
            return eSoftResult;
        }

        public string GetSoftInro(string str)
        {
            List<E_Soft> list = StrToXml(str);
            StringBuilder intro = new StringBuilder();
            if (list.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    intro.Append(list[i].Name +"<br/>");
                }
            }
            else {
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
    }
}
