using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace WinForms
{
    public partial class Eng : Form
    {
        List<string> str = new List<string>();
        List<string> ti = new List<string>();
        List<string> ti2 = new List<string>();
        string[] f = System.IO.Directory.GetFiles("D:/Web/Web_Homework/Eng", "*.txt");
        public Eng()
        {
            InitializeComponent();
        }
        private void search_pre()
        {
            string[] files = System.IO.Directory.GetFiles("D:/Web/Web_Homework/Eng_af", "*.txt");
            string text = "";
            string a = "";
            string line;
            int count = 0;
            foreach (string file in files)
            {
                StreamReader reader = new StreamReader(file);
                count = 0;
                text = "";
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                    if (count == 1) a = line;
                    else text += line;
                }
                saveIndex(a, text);
            }
        }
        private void saveIndex(string title, string content)
        {
            DirectoryInfo dir = new DirectoryInfo(@"D:\Web\WinForms\index");
            Lucene.Net.Store.Directory path = new SimpleFSDirectory(dir);
            bool isNew = false;
            if (!IndexReader.IndexExists(path))
            {
                isNew = true;
            }
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter iw = new IndexWriter(path, analyzer, isNew, IndexWriter.MaxFieldLength.UNLIMITED);
            Document document = new Document();
            document.Add(new Field("title", title, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("content", content, Field.Store.YES, Field.Index.ANALYZED));
            iw.AddDocument(document); 
            iw.Optimize(); 
            iw.Dispose();
        }
        private List<string> search(string keyWord)
        {
            DirectoryInfo dir = new DirectoryInfo(@"D:\Web\WinForms\index");
            Lucene.Net.Store.Directory path = new SimpleFSDirectory(dir);
            List<string> results = new List<string>();
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexSearcher searcher = new IndexSearcher(path);
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, 
                new string[] { "title", "content" }, analyzer);
            Query query = parser.Parse(keyWord);
            TopDocs td = searcher.Search(query, 50);
            foreach (ScoreDoc sd in td.ScoreDocs)
            {
                Document document = searcher.Doc(sd.Doc);
                string s = document.Get("title");
                int x = ti.FindIndex(x=>x.Equals(s));
                if(!results.Contains(f[x])) results.Add(f[x]);
            }
            searcher.Dispose();
            return results;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string key = textBox1.Text;
            string[] files = System.IO.Directory.GetFiles("D:/Web/Web_Homework/Eng_af", "*.txt");
            foreach (var s in files)
            {
                StreamReader reader = new StreamReader(s);
                ti.Add(reader.ReadLine());
            }
            //search_pre();
            str =search(key);
            foreach(var s in str)
            {
                StreamReader reader = new StreamReader(s);
                ti2.Add(reader.ReadLine());
            }
            listBox1.DataSource = ti2; 
        }
    }
}
