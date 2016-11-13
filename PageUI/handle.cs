using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Model;
using Lucene.Net.Documents;
using LibraryTool;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Analysis.PanGu;
using PanGu;
using System.Diagnostics;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace PageUI
{
    public class Handel
    {
        /**
      * 对搜索返回的前n条结果进行分页显示
      * @param keyWord       查询关键词
      * @param pageSize      每页显示记录数
      * @param currentPage   当前页 
      * @throws ParseException
      * @throws CorruptIndexException
      * @throws IOException
      */
        public List<E_Soft> PageQuery(String keyWord, int pageSize, int currentPage, ref int total)
        {
          
            string indexPath = Utils.MapPath("~/IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            BooleanQuery UniteQuery = new BooleanQuery();
            if (!string.IsNullOrEmpty(keyWord))
            {
                //TermQuery q2 = new TermQuery(new Term("Name", "*"+keyWord+"*"));
              Query query1 = new FuzzyQuery(new Term("Name",  keyWord ));
                //UniteQuery.Add(q2, Lucene.Net.Search.BooleanClause.Occur.MUST);
              UniteQuery.Add(new WildcardQuery(new Term("Name", "*" + keyWord + "*")), BooleanClause.Occur.SHOULD);
                UniteQuery.Add(query1, BooleanClause.Occur.SHOULD);
            }

            PhraseQuery query = new PhraseQuery();
            //把用户输入的关键字进行分词
            string[] keyWords = GetKeyWordsSplitBySpace(keyWord, new PanGuTokenizer()).Split('|');

            ICollection<WordInfo> words = new PanGuTokenizer().SegmentToWordInfos(keyWord);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                //TermQuery q1 = new TermQuery(new Term("Name", word.Word));
                Query q1 = new FuzzyQuery(new Term("Name", keyWord));
                UniteQuery.Add(q1, Lucene.Net.Search.BooleanClause.Occur.SHOULD);

               // TermQuery q2 = new TermQuery(new Term("Details", word.Word));
               // UniteQuery.Add(q2, Lucene.Net.Search.BooleanClause.Occur.SHOULD);
                query.Add(new Term("Name", word.Word));
            }

            //query.Add(new Term("content", "C#"));//多个查询条件时 为且的关系
           
           // query.SetSlop(100); //指定关键词相隔最大距离
          
            Sort sort = new Sort(new SortField("UpdateTime", SortField.DOC, true));//按照UpdateTime字段排序，false表示升序,true表示逆序


            //可按多个属性排序
            //Sort sort1 = new Sort(new SortField("price",SortField.FLOAT),new SortField("id",SortField.FLOAT));

            //排序搜索,返回符合条件的前10条记录. TopFieldDocs是TopDocs的子类
           // TopFieldDocs topFieldDocs = searcher.Search(query, 10, sort);
            //搜索结果总数量
          //  int totalCount = topFieldDocs.totalHits;
            // 搜索的结果列表
        //    ScoreDoc[] scoreDocs = topFieldDocs.scoreDocs; 
     
       

            //TopScoreDocCollector盛放查询结果的容器
          
            Stopwatch watch = new Stopwatch();
            watch.Start();
          

            TopDocs topDocs = searcher.Search(UniteQuery, null, 1000, sort);//根据query查询条件进行查询，查询结果放入collector容器
            string sb = "FuzzyQuery搜索完成,共用时：" + watch.Elapsed.Hours + "时 " + watch.Elapsed.Minutes + "分 " + watch.Elapsed.Seconds + "秒 " + watch.Elapsed.Milliseconds + "毫秒";

            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
            ScoreDoc[] docs = topDocs.scoreDocs;
            //查询起始记录位置

            total = topDocs.totalHits;
            int begin = pageSize * (currentPage - 1);

            //查询终止记录位置

            int end = Math.Min(begin + pageSize, total);
            //展示数据实体对象集合
            List<E_Soft> eSoftResult = new List<E_Soft>();
            for (int i = begin; i < end; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document


                E_Soft eSoft = new E_Soft();
                //eSoft.Name = doc.Get("Name");
                eSoft.Name = PanGuFenCi.HightLight(keyWord, doc.Get("Name"));
               // eSoft.Details = doc.Get("Details");//未使用高亮
                //搜索关键字高亮显示 使用盘古提供高亮插件
                eSoft.Details = PanGuFenCi.HightLight(keyWord, doc.Get("Details"));
                eSoft.Hash = doc.Get("Hash");
                eSoft.SoftType =Convert.ToInt32( doc.Get("SoftType"));
                eSoft.Length = doc.Get("Length");
                eSoft.FileCount = Convert.ToInt32(doc.Get("FileCount"));
                eSoft.Hit = Convert.ToInt32(doc.Get("Hit"));
                eSoft.UpdateTime =Convert.ToDateTime( doc.Get("UpdateTime"));
                eSoft.URL = doc.Get("Hash");
                eSoftResult.Add(eSoft);
            }
            return eSoftResult;

        }
        public  string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}| ", word.Word);
            }
            return result.ToString().TrimEnd('|');
        }

        /// <summary>
        /// 生成翻页代码
        /// </summary>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="sLinkUrl">跳转地址</param>
        /// <returns></returns>
        public  string ShowPageV(int totalPageCount, int pageSize, int currentPageIndex, string sLinkUrl)
        {
            int proPage = currentPageIndex - 1;
            int nextPage = proPage + 2;
            if (proPage < 1)
            {
                proPage = 1;
            }
            if (nextPage > totalPageCount)
            {
                nextPage = totalPageCount;
            }
            if (totalPageCount < 1 || pageSize < 1)
            {
                return "";
            }
            int start = currentPageIndex - (int)(Math.Ceiling(Convert.ToDouble(pageSize / 2)) - 1);
            if (pageSize < totalPageCount)
            {
                if (start < 1)
                {
                    start = 1;
                }
                else if (start + pageSize > totalPageCount)
                {
                    start = totalPageCount - pageSize + 1;
                }
            }
            else
            {
                start = 1;
            }
            // int end = start + pageSize - 1;
            int end = start + 10;
            if (end > totalPageCount)
            {
                end = totalPageCount;
            }
            StringBuilder newNumberStr = new StringBuilder();
            if (currentPageIndex <= 1)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\"  class=\"shouye\">首页</a><a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"next\">上一页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}/1\" class=\"shouye\">首页</a><a href=\"{0}/{1}\" class=\"next\">上一页</a>", sLinkUrl, proPage);
            }
            for (var i = start; i <= end; i++)
            {

                if (i == currentPageIndex)
                {
                    newNumberStr.AppendFormat("<a title='" + i + "' href=\"javascript:void(0);\" class=\"se\">" + i + "</a>");
                }
                else
                {
                    newNumberStr.AppendFormat("<a title='" + i + "' href=\"{0}/{1}\">" + i + "</a>", sLinkUrl, i);
                }
            }
            if (currentPageIndex == totalPageCount)
            {
                newNumberStr.Append("<a href=\"javascript:void(0);\" disabled=\"disabled\" class=\"next\">下一页</a>");
            }
            else
            {
                newNumberStr.AppendFormat("<a href=\"{0}/{1}\" class=\"next\">下一页</a>", sLinkUrl, nextPage);
            }
         //   newNumberStr.AppendFormat("<a href=\"javascript:void(0);\" class=\"next\">共{0}页</a>", totalPageCount);
            if (totalPageCount > 1)
            {
                return newNumberStr.ToString();
            }
            else
            {
                return "";
            }
        }



    }
}
