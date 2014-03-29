using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.FS
{
    /// <summary>
    /// ���ظ������ߵ�����
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
        /// �����ļ�
        /// </summary>
        public string file;
        /// <summary>
        /// ��ʼ
        /// </summary>
        public int str;
        /// <summary>
        /// ���� 
        /// </summary>
        public int len;


        public string md5;
    }

    /// <summary>
    /// �ļ�ϵͳ   ��ȡһ��  ����һ��  �����ļ�ID  ��������д��ʱ���ȡ  ���ݾ��з�ֹ�ظ�url ��ͬ����ָ��һ��
    /// </summary>
    class ClassFS
    {


        /// <summary>
        /// ���Ƽ��� �� URL
        /// </summary>
        private ArrayList FS_URL = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// �ļ�����MD5����  ��MD5
        /// </summary>
        private ArrayList FS_D_MD5 = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// ���������������� ��  oneFS
        /// </summary>
        private  ArrayList FS_D_D = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// ���������ID �� URL ��Ӧ��
        /// </summary>
        public   ArrayList UrlNameIndex = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// �����ļ���Ԫ��С
        /// </summary>
        private  int mSize = 1024*1024*3;

        /// <summary>
        /// ����·�� ����� /\
        /// </summary>
        private string mDirPath = "";

        /// <summary>
        /// ������
        /// </summary>
        private  CNewNxuEncoding fs_nCode = new  CNewNxuEncoding(); 
         
        /// <summary>
        /// ���ݻ����� 5M
        /// </summary>
        Byte[] fs_mCache = new byte[ 10];

        /// <summary>
        /// �洢ָ��
        /// </summary>
        int p_fs_mCache = 0;

        /// <summary>
        /// ���ݻ�����  ��ȡʱʹ�� 5M
        /// </summary>
        Byte[] fs_mCacheREAD = new byte[10];

        /// <summary>
        /// �ļ���ȡ��������ǰ���ļ�����
        /// </summary>
        string fs_mCacheName = "";

        /// <summary>
        /// ��ǰ������ָ��
        /// </summary>
        private ArrayList NowFile = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// ��ǰ�洢�������ļ� ����
        /// </summary>
        private string NowFileName = "";

        /// <summary>
        /// �ļ����� ������ȡ���ݱ�ʾID
        /// </summary>
        public int FileNum = 0;



        /// <summary>
        /// ��ȡһ������
        /// </summary>
        /// <param name="id">���</param>
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
                    //��ǰ�������е��ļ���������Ҫ���ļ�
                    //��һ������
                }
                else
                {
                    //��ǰ���������ļ� ������Ҫ���ļ�
                    //�ļ����ݶ��뻺����
                    //��һ������

                    FileStream cfs = new FileStream(mDirPath + one.file + ".TDB", FileMode.Open, FileAccess.Read);
                    BinaryReader cr = new BinaryReader(cfs);
                    byte[] mTmp = cr.ReadBytes((int)cfs.Length);

                    cr.Close();
                    cfs.Close();

                    fs_mCacheREAD = new byte[mTmp.Length];
                    fs_mCacheREAD = mTmp;
                    fs_mCacheName = one.file;

                }

                //�ӻ������ж�ȡ����

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
                //ID ����
                return back;
            }
        
        }


        /// <summary>
        /// д��һ������ 
        /// </summary>
        /// <returns></returns>
        public bool SaveData(string url, string dat)
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine(" FileSystem SaveData--> " + url);

            if (FS_URL.Contains(url) == true)
            {
                return false;  //�Ѿ�����
            }

            //�õ����ݵ�MD5��
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
                //�Ѿ�����

                oneFS one = (oneFS)FS_D_D[id_one];

                one.url = url;
                FS_URL.Add(url);
                FS_D_D.Add(one);
                FS_D_MD5.Add(one.md5);

                if (one.file == NowFileName)
                {
                    //�����ڵ�ǰ��δд��������  ֱ����������
                    //�ҵ�                
                    NowFile.Add(one);
                }
                else
                {
                    //������д�����������
                    //д���ļ�����  ��ʼ +  ����  + Md5 + Url
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
                //����д�� ����
                oneFS one3 = new oneFS();

                byte[] uuD = fs_nCode.dbsCompress(dat);


                if (uuD.Length + p_fs_mCache > mSize)
                {
                    //���ݳ�������  ������д��
                    SaveCache();

                    NowFileName = GetOneName();

                   
                }


                one3.file = NowFileName;
                one3.len = uuD.Length;
                one3.md5 = getMD5name(dat);
                one3.url = url;
                one3.str = p_fs_mCache;

                //ѹ�뻺��
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
        /// ���滺��
        /// </summary>
        public void SaveCache()
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine(" FileSystem --> " +" �����ļ�ϵͳ");

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(mDirPath + NowFileName + ".IDX", true, System.Text.Encoding.GetEncoding("gb2312"));

                //����д���ļ�ϵͳ   // ��ʼ +  ����  + Md5 + Url
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


          
            //����д���ļ�
            //д��mCache ����
            FileStream fs = new FileStream(mDirPath  + NowFileName + ".TDB", FileMode.CreateNew);

            BinaryWriter writer2 = null;

            try
            {
                writer2 = new BinaryWriter(fs);

                //д��Ӳ��
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
        /// �õ����ݵ�MD5��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string getMD5name(string data)
        {
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data, "md5");
            return strMd5.ToUpper();
        }


        /// <summary>
        /// ����ԭʼ����
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
        /// ���Ƽ���
        /// </summary>
         FS_URL.Clear();

        /// <summary>
        /// �ļ�����MD5����
        /// </summary>
         FS_D_MD5.Clear();

        /// <summary>
        /// ����������������
        /// </summary>
         FS_D_D.Clear();


            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles("*.IDX"))   //���������xmlΪ��չ�����ļ�   
            {
                String namepath = f.FullName;//nameΪ���ļ����µ��ļ����ƣ���f.FullNameΪȫ��   
                string name2 = f.Name;   // ��ʼ +  ����  + Md5 + Url
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
                            string[] nTmp = line.Split('\t');// ��ʼ +  ����  + Md5 + Url

                            string t_1 = nTmp[0];//   ��ʼ  
                            string t_2 = nTmp[1];//   ����   
                            string t_3 = nTmp[2];//   Md5  
                            string t_4 = nTmp[3].Trim();//   Url
                            
                            oneFS oneTmp = new oneFS();

                            oneTmp.file = name2;
                            oneTmp.str = Int32.Parse(t_1);
                            oneTmp.len = Int32.Parse(t_2);
                            oneTmp.md5 = t_3;
                            oneTmp.url = t_4;

                            /// <summary>
                            /// ���Ƽ���
                            /// </summary>
                            FS_URL.Add(t_4);

                            /// <summary>
                            /// �ļ�����MD5����
                            /// </summary>
                            FS_D_MD5.Add(t_3);

                            /// <summary>
                            /// ����������������
                            /// </summary>
                            FS_D_D.Add(oneTmp);

                            //ID URL ��Ӧ��
                            UrlNameIndex.Add(t_4);

                            //�ļ�����
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
        /// �õ�1����ʱ������
        /// </summary>
        /// <returns></returns>
        private string GetOneName()
        {
            System.Random newRA = new System.Random();

            return DateTime.Now.Date.ToShortDateString() + "@" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + "@" + newRA.NextDouble().ToString();

        }
    }
}
