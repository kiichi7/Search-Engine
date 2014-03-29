/*
 *创建索引的类，索引优化。。
 */

using System.IO;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Net;
using System.Net.Sockets;


namespace Indexer
{
    /// <summary>
    /// Summary description for Indexer.
    /// </summary>
    public class IntranetIndexer
    {
        private IndexWriter writer;
        private string docRootDirectory;
        private string pattern;
        private string localhostIp;


        /// <summary>
        /// Creates a new index in <c>directory</c>. Overwrites the existing index in that directory.
        /// </summary>
        /// <param name="directory">Path to index (will be created if not existing).</param>
        public IntranetIndexer(string directory)
        {
            writer = new IndexWriter(directory, new StandardAnalyzer(), true);
            writer.SetUseCompoundFile(true);
        }
        /// <summary>
        /// 建立内存索引。。。
        /// </summary>
        /// <param name="ramdir">内存索引</param>
        public IntranetIndexer(Lucene.Net.Store.Directory ramdir)
        {
            writer = new IndexWriter(ramdir, new StandardAnalyzer(), true);
            writer.SetUseCompoundFile(true);
        }

        public void AddIndexWriters(IndexWriter writer, Lucene.Net.Store.Directory ramdir)
        {
            if (writer!=null || ramdir!=null)
            {
            writer.AddIndexes(new Lucene.Net.Store.Directory[] { ramdir });
            }
            
        }

        /// <summary>
        /// Add HTML files from <c>directory</c> and its subdirectories that match <c>pattern</c>.
        /// </summary>
        /// <param name="directory">Directory with the HTML files.</param>
        /// <param name="pattern">Search pattern, e.g. <c>"*.html"</c></param>
        public void AddDirectory(DirectoryInfo directory, string pattern)
        {
            GetIP();
            this.docRootDirectory = directory.FullName;
            this.pattern = pattern;

            addSubDirectory(directory);
        }

        private void addSubDirectory(DirectoryInfo directory)
        {

            foreach (FileInfo fi in directory.GetFiles(pattern))
            {
                AddHtmlDocument(fi.FullName);
            }
            foreach (DirectoryInfo di in directory.GetDirectories())
            {
                addSubDirectory(di);
            }
        }

        protected void GetIP()   //获取本地IP   
        {
            //IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddr = ipHost.AddressList[0];

            IPAddress[] ipAddr=Dns.GetHostAddresses(Dns.GetHostName());

             localhostIp = ipAddr[0].ToString();
            
        }


        /// <summary>
        /// Loads, parses and indexes an HTML file.
        /// </summary>
        /// <param name="path"></param>
        public void AddHtmlDocument(string path)
        {
            Document doc = new Document();

            string html;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                html = sr.ReadToEnd();
            }

            int relativePathStartsAt = this.docRootDirectory.EndsWith("\\") ? this.docRootDirectory.Length : this.docRootDirectory.Length + 1;
            string relativePath = path.Substring(relativePathStartsAt);

doc.Add(new Field("text",parseHtml(html),Lucene.Net.Documents.Field.Store.YES,Lucene.Net.Documents.Field.Index.TOKENIZED));
doc.Add(new Field("path", relativePath, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.TOKENIZED));
doc.Add(new Field("fullpath", "http://" + localhostIp + "/" + new System.IO.DirectoryInfo(this.docRootDirectory).Name + "/" + relativePath, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.TOKENIZED));
doc.Add(new Field("keywords", KeyWordparseHtml(html), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.TOKENIZED));
doc.Add(new Field("description", DescriptionparseHtml(html), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.TOKENIZED));
doc.Add(new Field("title", getTitle(html),Lucene.Net.Documents.Field.Store.YES,Lucene.Net.Documents.Field.Index.TOKENIZED));
            writer.AddDocument(doc);
        }

       
        /// <summary>
        /// Very simple, inefficient, and memory consuming HTML parser. Take a look at Demo/HtmlParser in DotLucene package for a better HTML parser.
        /// </summary>
        /// <param name="html">HTML document</param>
        /// <returns>Plain text.</returns>
        private string parseHtml(string html)
        {
            string temp = Regex.Replace(html, "<[^>]*>", "");
            return temp.Replace("&nbsp;", " ");
        }
        /// <summary>
        /// 返回keywords的内容
        /// </summary>
        /// <param name="html">html文档字符串</param>
        /// <returns></returns>
        private string KeyWordparseHtml(string html)
        {
            Match m = Regex.Match(html, "<meta name=\"keywords\" content=\"([^<]*)\" />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (m.Groups.Count == 2)
                return m.Groups[1].Value;
            return "(unknown KeyWord)";
        }
        /// <summary>
        /// 返回description的内容
        /// </summary>
        /// <param name="html">html文档字符串</param>
        /// <returns></returns>
        private string DescriptionparseHtml(string html)
        {
            Match m = Regex.Match(html, "<meta name=\"description\" content=\"([^<]*)\" />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (m.Groups.Count == 2)
                return m.Groups[1].Value;
            return "(unknown Description)";
        }
        /// <summary>
        /// Finds a title of HTML file. Doesn't work if the title spans two or more lines.
        /// </summary>
        /// <param name="html">HTML document.</param>
        /// <returns>Title string.</returns>
        private string getTitle(string html)
        {
            Match m = Regex.Match(html, "<title>([^<]*)</title>", RegexOptions.IgnoreCase|RegexOptions.Multiline);
            if (m.Groups.Count == 2)
                return m.Groups[1].Value;
            return "(unknown Title)";
        }

        /// <summary>
        /// Optimizes and save the index.
        /// </summary>
        public void Close()
        {
            writer.Optimize();
            writer.Close();
        }


    }
}
