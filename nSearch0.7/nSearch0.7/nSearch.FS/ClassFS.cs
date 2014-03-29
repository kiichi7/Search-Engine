using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.FS
{
    /// <summary>
    /// 返回给调用者的数据
    /// </summary>
    public struct oneHtmDat
{
        public string url;

        public string HtmDat;

}


    struct oneFS
    {
        public string url;

        /// <summary>
        /// 所在文件
        /// </summary>
        public string file;
        /// <summary>
        /// 开始
        /// </summary>
        public int str;
        /// <summary>
        /// 长度 
        /// </summary>
        public int len;


        public string md5;
    }

    /// <summary>
    /// 文件系统   读取一条  保存一条  按照文件ID  不允许在写入时候读取  数据具有防止重复url 相同数据指向一处
    /// </summary>
    class ClassFS
    {


        /// <summary>
        /// 名称集合 单 URL
        /// </summary>
        private ArrayList FS_URL = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// 文件内容MD5集合  单MD5
        /// </summary>
        private ArrayList FS_D_MD5 = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// 数据内容索引集合 单  oneFS
        /// </summary>
        private  ArrayList FS_D_D = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// 对外输出的ID 与 URL 对应表
        /// </summary>
        public   ArrayList UrlNameIndex = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// 数据文件单元大小
        /// </summary>
        private  int mSize = 1024*1024*3;

        /// <summary>
        /// 数据路径 后面代 /\
        /// </summary>
        private string mDirPath = "";

        /// <summary>
        /// 编码类
        /// </summary>
        private  CNewNxuEncoding fs_nCode = new  CNewNxuEncoding(); 
         
        /// <summary>
        /// 数据缓存区 5M
        /// </summary>
        Byte[] fs_mCache = new byte[ 10];

        /// <summary>
        /// 存储指针
        /// </summary>
        int p_fs_mCache = 0;

        /// <summary>
        /// 数据缓存区  读取时使用 5M
        /// </summary>
        Byte[] fs_mCacheREAD = new byte[10];

        /// <summary>
        /// 文件读取缓存区当前的文件名称
        /// </summary>
        string fs_mCacheName = "";

        /// <summary>
        /// 当前的数据指针
        /// </summary>
        private ArrayList NowFile = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// 当前存储的数据文件 名称
        /// </summary>
        private string NowFileName = "";

        /// <summary>
        /// 文件个数 用来读取数据标示ID
        /// </summary>
        public int FileNum = 0;



        /// <summary>
        /// 读取一条数据
        /// </summary>
        /// <param name="id">序号</param>
        /// <returns></returns>
        public oneHtmDat GetData(int id)
        {
            oneHtmDat back= new oneHtmDat();

            nSearch.DebugShow.ClassDebugShow.WriteLine(" FileSystem GetData--> "+ id.ToString());

            if (id < FS_D_D.Count)
            {
                oneFS one = (oneFS)FS_D_D[id];

                if (fs_mCacheName == one.file)
                {
                    //当前缓冲区中的文件正好是需要的文件
                    //下一步处理
                }
                else
                {
                    //当前缓冲区的文件 不是需要的文件
                    //文件数据读入缓冲区
                    //下一步处理

                    FileStream cfs = new FileStream(mDirPath + one.file + ".TDB", FileMode.Open, FileAccess.Read);
                    BinaryReader cr = new BinaryReader(cfs);
                    byte[] mTmp = cr.ReadBytes((int)cfs.Length);

                    cr.Close();
                    cfs.Close();

                    fs_mCacheREAD = new byte[mTmp.Length];
                    fs_mCacheREAD = mTmp;
                    fs_mCacheName = one.file;

                }

                //从缓存区中读取数据

                byte[] tmpBack = new byte[one.len];

                for (int ii = one.str; ii < one.str + one.len; ii++)
                {
                    tmpBack[ii - one.str] = fs_mCacheREAD[ii];
                }

                string backString = fs_nCode.dbsDeCompress(tmpBack);

                back.HtmDat = backString;
                back.url = one.url;

                return back;

            }
            else
            {
                back.url = null;
                back.HtmDat = null;
                //ID 错误
                return back;
            }
        
        }


        /// <summary>
        /// 写入一条数据 
        /// </summary>
        /// <returns></returns>
        public bool SaveData(string url, string dat)
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine(" FileSystem SaveData--> " + url);

            if (FS_URL.Contains(url) == true)
            {
                return false;  //已经存在
            }

            //得到数据的MD5码
            string MD5_IT = getMD5name(dat).ToUpper();

            // if (FS_D_MD5.Contains(MD5_IT) == true)

            int id_one = -1; FS_D_MD5.BinarySearch(MD5_IT);

            for(int iii =0;iii<FS_D_MD5.Count ;iii++)
            {
                if (MD5_IT == (string)FS_D_MD5[iii])
                {
                    id_one = iii;
                    break;
                }

            }


            if (id_one > -1)
            {
                //已经存在

                oneFS one = (oneFS)FS_D_D[id_one];

                one.url = url;
                FS_URL.Add(url);
                FS_D_D.Add(one);
                FS_D_MD5.Add(one.md5);

                if (one.file == NowFileName)
                {
                    //存在于当前的未写入数据中  直接增加索引
                    //找到                
                    NowFile.Add(one);
                }
                else
                {
                    //存在于写入过的数据中
                    //写入文件索引  开始 +  结束  + Md5 + Url
                    StreamWriter writer = null;
                    try
                    {
                        writer = new StreamWriter(mDirPath + one.file +".IDX", true, System.Text.Encoding.GetEncoding("gb2312"));
                        writer.WriteLine(one.str.ToString() + "\t" + one.len.ToString() + "\t" + one.md5 + "\t" + url);
                        writer.Close();
                    }
                    catch (IOException e)
                    {
                        if (writer != null)
                            writer.Close();

                    }
                }
                return true;
            }
            else
            {
                //数据写入 缓存
                oneFS one3 = new oneFS();

                byte[] uuD = fs_nCode.dbsCompress(dat);


                if (uuD.Length + p_fs_mCache > mSize)
                {
                    //数据超过缓存  缓存先写入
                    SaveCache();

                    NowFileName = GetOneName();

                   
                }


                one3.file = NowFileName;
                one3.len = uuD.Length;
                one3.md5 = getMD5name(dat);
                one3.url = url;
                one3.str = p_fs_mCache;

                //压入缓存
                for (int ipp = 0; ipp < uuD.Length; ipp++)
                {
                    fs_mCache[p_fs_mCache + ipp] = uuD[ipp];

                }
                p_fs_mCache = p_fs_mCache + uuD.Length;

                FS_URL.Add(url);
                FS_D_D.Add(one3);
                FS_D_MD5.Add(one3.md5);
                NowFile.Add(one3);

                return true;
            }

        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        public void SaveCache()
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine(" FileSystem --> " +" 保存文件系统");

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(mDirPath + NowFileName + ".IDX", true, System.Text.Encoding.GetEncoding("gb2312"));

                //索引写入文件系统   // 开始 +  结束  + Md5 + Url
            foreach (oneFS xiu in NowFile)
            { 
                    writer.WriteLine(xiu.str.ToString()+"\t"+ xiu.len.ToString()+"\t"+xiu.md5+"\t"+xiu.url  );                     
            }
               
                writer.Close();
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }


          
            //数据写入文件
            //写入mCache 数据
            FileStream fs = new FileStream(mDirPath  + NowFileName + ".TDB", FileMode.CreateNew);

            BinaryWriter writer2 = null;

            try
            {
                writer2 = new BinaryWriter(fs);

                //写入硬盘
                writer2.Write(fs_mCache, 0, p_fs_mCache );
                writer2.Close();
                fs.Close();

            }
            catch
            {
                if (writer2 != null)
                    writer2.Close();

                if (fs != null)
                {
                    fs.Close();
                }
            }

            p_fs_mCache = 0;
            NowFile.Clear();
        }


        /// <summary>
        /// 得到数据的MD5名
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string getMD5name(string data)
        {
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data, "md5");
            return strMd5.ToUpper();
        }


        /// <summary>
        /// 加载原始数据
        /// </summary>
        public void InitData(string path)
        {
            if (path[path.Length - 1] != '\\')
            {
                mDirPath = path + "\\";
            }
            else
            {
                mDirPath = path;
            }

            NowFileName = GetOneName();
            p_fs_mCache = 0;
            NowFile.Clear();
            

            fs_mCache = new byte[mSize]; 
fs_mCacheREAD = new byte[mSize]; 




             /// <summary>
        /// 名称集合
        /// </summary>
         FS_URL.Clear();

        /// <summary>
        /// 文件内容MD5集合
        /// </summary>
         FS_D_MD5.Clear();

        /// <summary>
        /// 数据内容索引集合
        /// </summary>
         FS_D_D.Clear();


            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles("*.IDX"))   //遍历获得以xml为扩展名的文件   
            {
                String namepath = f.FullName;//name为该文件夹下的文件名称，如f.FullName为全名   
                string name2 = f.Name;   // 开始 +  结束  + Md5 + Url
                name2 = name2.Substring(0, name2.Length - 4);

             

                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(namepath, System.Text.Encoding.GetEncoding("gb2312"));
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        line = line.Trim();
                        if (line.IndexOf('\t') > 0)
                        {
                            string[] nTmp = line.Split('\t');// 开始 +  长度  + Md5 + Url

                            string t_1 = nTmp[0];//   开始  
                            string t_2 = nTmp[1];//   长度   
                            string t_3 = nTmp[2];//   Md5  
                            string t_4 = nTmp[3].Trim();//   Url
                            
                            oneFS oneTmp = new oneFS();

                            oneTmp.file = name2;
                            oneTmp.str = Int32.Parse(t_1);
                            oneTmp.len = Int32.Parse(t_2);
                            oneTmp.md5 = t_3;
                            oneTmp.url = t_4;

                            /// <summary>
                            /// 名称集合
                            /// </summary>
                            FS_URL.Add(t_4);

                            /// <summary>
                            /// 文件内容MD5集合
                            /// </summary>
                            FS_D_MD5.Add(t_3);

                            /// <summary>
                            /// 数据内容索引集合
                            /// </summary>
                            FS_D_D.Add(oneTmp);

                            //ID URL 对应表
                            UrlNameIndex.Add(t_4);

                            //文件个数
                            FileNum = FileNum + 1;
                        }
                    }
                    reader.Close();
                }
                catch (IOException e)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }







            }

        }

        /// <summary>
        /// 得到1个临时的名字
        /// </summary>
        /// <returns></returns>
        private string GetOneName()
        {
            System.Random newRA = new System.Random();

            return DateTime.Now.Date.ToShortDateString() + "@" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + "@" + newRA.NextDouble().ToString();

        }
    }
}
