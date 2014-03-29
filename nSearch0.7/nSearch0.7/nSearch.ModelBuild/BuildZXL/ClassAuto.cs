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

namespace nSearch.ModelBuild.BuildZXL
{


    /// <summary>
    /// 存储数据长短变化
    /// </summary>
   public   struct HTM2SHORT
    {
        /// <summary>
        /// 词典 标签
        /// </summary>
        public ArrayList Dict1;

        /// <summary>
        /// 词典 数据
        /// </summary>
        public ArrayList Dict2;

        /// <summary>
        /// 变换的数据
        /// </summary>
        public Hashtable HASH;


    }




    /// <summary>
    /// 过滤器自动处理器
    /// 
    ///   变化 使用 <XL INDEX=T> 来代表直接从Title中取出的数据 
    /// 
    /// </summary>
   public  class ClassAuto
    {
       /// <summary>
       /// 文件系统路径
       /// </summary>
       private  string pFS = "";

       /// <summary>
       /// 采样路径
       /// </summary>
       private  string pUrl = "";

       /// <summary>
       /// 模板路径
       /// </summary>
       private  string pMod = "";

       /// <summary>
       /// 是否停止
       /// </summary>
       public bool IsStop = false;


       public void SetPath(string fs, string url, string mod)
       {
           pFS = fs;

           pUrl = url;

           pMod = mod;
       
       }

       /// <summary>
       /// 开始运行程序
       /// </summary>
       public void StartRun()
       {

           //文件系统初始化
           nSearch.FS.ClassFSMD.Init(pFS );
           nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化");

           //文件ID对照表
           ArrayList mID = nSearch.FS.ClassFSMD.GetUrlList();

           IsStop = false;
 
           //已经生成过的模板的名称
           ArrayList BV = new ArrayList();


           // 1 得到目录下的文件
           DirectoryInfo dir = new DirectoryInfo(pMod);
           foreach (FileInfo f in dir.GetFiles("*.a"))   //遍历获得以xml为扩展名的文件   
           {
               String name = f.Name;         //name为该文件夹下的文件名称，如f.FullName为全名  
               name = name.Substring(0, name.Length - 2);


               BV.Add(name); //已经生成过的模板的名称
           }


           // 1 读取采样列表
           Hashtable urlList = CaiYang(pUrl);

           

           // 遍历采样列表
           foreach (DictionaryEntry nT in urlList)
           {


               if (IsStop == true)
               {
                   //停止生成
                   return;
               }
               else
               {
                   // 2 得到一个需要的URL 队列
                   ArrayList xbn = (ArrayList)nT.Value;
                   //3 存放采样数据
                   Hashtable xnbDat = new Hashtable();

                   string NameMod = nT.Key.ToString();

                   //已经生成过的模板的名称
                   if (BV.Contains(NameMod) == true)
                   {
                       //已经生成过的 不计
                   }
                   else
                   {


                       //记录已经生成过模板的采样结果
                    //   SaveFileData(pMod + "URL.OLD", NameMod);


                       foreach (string aurl in xbn)
                       {

                           if (xnbDat.Contains(aurl) == false)
                           {
                               int id = -1;

                               for (int uiy = 0; uiy < mID.Count; uiy++)
                               {
                                   if (aurl == mID[uiy].ToString())
                                   {
                                       id = uiy;
                                       break;
                                   }
                               }

                               if (id > -1)
                               {


                                   string newDat = nSearch.FS.ClassFSMD.GetOneDat(id).HtmDat;

                                   //数据添加到队列

                                   xnbDat.Add(aurl, newDat);

                               }
                           }
                       }

                       if (xnbDat.Count > 2)
                       {
                           //建立一个模板
                           nSearch.DebugShow.ClassDebugShow.WriteLineF("开始建立一个模板 采样数据个数：" + xnbDat.Count.ToString());
                           CompModIt(pMod, xnbDat, NameMod);
                           nSearch.DebugShow.ClassDebugShow.WriteLineF("1个模板建立完成");
                       }
                       xbn = null;
                       xnbDat = null;
                       NameMod = "";
                   }
               }
           }


           nSearch.DebugShow.ClassDebugShow.WriteLineF("全部模板建立完成");
       
       }



       /// <summary>
       /// 建立一个模板
       /// </summary>
       /// <param name="mpathc">模板路径</param>
       /// <param name="nm">数据</param>
       /// <param name="modName">模板名称</param>
       private  void CompModIt(string mpathc, Hashtable nm,string modName)
       {

           nSearch.ClassLibraryHTML.ClassHTML mHtm = new nSearch.ClassLibraryHTML.ClassHTML();
       
             //数据预处理  清理各个环节
           Hashtable  nT = new Hashtable ();
           nT.Clear();

           foreach (DictionaryEntry HI in nm)
           {

               string url = HI.Key.ToString();

               nSearch.DebugShow.onePage nnt_mp = (nSearch.DebugShow.onePage)mHtm.GetOnePage(HI.Value.ToString(), url);

                string HtmDat = nnt_mp.Body;

               //取出Title项和 Body 分开进行 提高效率
               //Title自动变为第0项 不做处理

               nT.Add(url, HtmDat);
                    
           }

           nSearch.DebugShow.ClassDebugShow.WriteLineF(" 数据加载完成 ");
             
           //送到处理器进行处理
           TMM(nT, mpathc+modName);

             //保存结果
       
       
       
       
       }

       /// <summary>
       /// 得到采样队列
       /// </summary>
       /// <param name="path"></param>
       /// <returns></returns>
       private  Hashtable CaiYang(string path)
       {

           Hashtable VBF = new Hashtable();
           VBF.Clear();

           //产生扰乱效果  去掉一些数据
           int ii = 0;

           StreamReader reader = null;
           try
           {
               reader = new StreamReader(path, System.Text.Encoding.GetEncoding("gb2312"));
               for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
               {
                   ii = ii + 1;

                   line = line.Trim();
                   if (line.Length > 0)
                   {
                       string[] STR = line.Split('\t');

                       string T1MD5 = STR[0];
                       string T2URL = STR[1];

                       if (VBF.Contains(T1MD5) == false)
                       {
                           ArrayList xxx = new ArrayList();
                           xxx.Add(T2URL);
                           VBF.Add(T1MD5, xxx);
                           xxx = null;
                       }
                       else
                       {
                           ArrayList xxxx = (ArrayList)VBF[T1MD5];
                           if (ii % 10 == 7 | ii % 10 == 4 | ii % 10 == 1 | ii % 10 == 9 | ii % 10 == 2)
                           {
                                   //去掉一下相连的  保证多样性能
                           }
                           else
                           {
                               if (xxxx.Count >= 15)
                               {
                                   //采样只采20个数据 保证分析的数据量不至于太大
                               }
                               else
                               {
                                   xxxx.Add(T2URL);
                                   VBF[T1MD5] = xxxx;
                                   xxxx = null;
                               }
                           }
                       }
                   }
               }
               reader.Close();
           }
           catch (IOException en)
           {
               nSearch.DebugShow.ClassDebugShow.WriteLineF(en.Message);
           }
           finally
           {
               if (reader != null)
                   reader.Close();
           }


           return VBF;

       
       }

       /// <summary>
       /// 建立模板线程
       /// </summary>
       private void TMM(Hashtable TMMSOne,string SavePath)
       {
           try
           {
              
               HTM2SHORT new_vc_tmm = Long2Short(TMMSOne);

               Hashtable TMMSOne2 = new_vc_tmm.HASH;
               ArrayList Short_DICT1 = new_vc_tmm.Dict1;
               ArrayList Short_DICT2 = new_vc_tmm.Dict2;

               string dataE = "";

               foreach (System.Collections.DictionaryEntry de2 in TMMSOne)
               {
                   dataE = dataE + de2.Key.ToString() + "\r\n";
               }




               //声明模板构建类
               nSearch.ModelBuild.BuildZXL.ClassModelBuilder myBU = new nSearch.ModelBuild.BuildZXL.ClassModelBuilder();

               string ModelData = "";

               try
               {
                   ModelData = myBU.BuilderModel(TMMSOne2);

                   ModelData = Short2Long(ModelData, Short_DICT1, Short_DICT2);  //还原压缩的数据

               }
               catch
               {
                   nSearch.DebugShow.ClassDebugShow.WriteLineF("--> ERROR 排除错误模版");
                //   nSearch.DebugShow.ClassDebugShow.WriteLineF;
                   return;
               }

               int nn = myBU.inStrNum(ModelData, "*");

               //模版建立错误 表明该数据不能完成模版创建工作 则不予考虑
               if (ModelData.Length < 150 | nn < 4)
               {
                   nSearch.DebugShow.ClassDebugShow.WriteLineF("--> 排除错误模版");
                   return;
               }

               ArrayList txtDat = EditOneModelTag(ModelData);

               ArrayList neWW = (ArrayList)txtDat.Clone();

               //*************************************************************************************
               //  匹配模版  
               //*************************************************************************************
               // 匹配模版  数据全部匹配模版      
               string[] pipeiEndData = new string[neWW.Count];  //匹配得到的匹配项列表数据
               for (int i = 0; i < neWW.Count; i++)
               {
                   pipeiEndData[i] = "";
               }
               foreach (System.Collections.DictionaryEntry de2 in TMMSOne)
               {
                   //建立滤波类 
                   nSearch.ClassLibraryStruct.ClassUserModel m = new nSearch.ClassLibraryStruct.ClassUserModel();
                   //压入测试模板
                   m.TestModeL("", "", "", "", "", "", ModelData, "","","");
                   nSearch.ClassLibraryStruct.auto2dat k = m.getTagAndData(de2.Value.ToString());
                   Hashtable p = m.modelOneList;
                   foreach (System.Collections.DictionaryEntry de in p)
                   {
                       int pi = (int)de.Key;
                       pipeiEndData[pi] = pipeiEndData[pi] + de.Value.ToString().Trim();
                   }
               }

               string dataA = "";//索引用的数据

               string dataB = "";//类聚用的数据

               string TagTmp = "属性"; //记录上1个有标示的 数据 

               for (int i = 0; i < neWW.Count; i++)
               {
                   if (pipeiEndData[i].Length > 0)
                   {
                       dataA = dataA + neWW[i].ToString() + ":" + "<TAGDATA INDEX=" + i.ToString() + "/>" + "\r\n";

                       if (neWW[i].ToString().Length > 1)
                       {
                           if (neWW[i].ToString().IndexOf(' ') == -1 & neWW[i].ToString().Length < 13)
                           {
                               dataB = dataB + "<" + neWW[i].ToString() + ">" + "<TAGDATA INDEX=" + i.ToString() + "/>" + "</" + neWW[i].ToString() + ">" + "\r\n";
                               TagTmp = neWW[i].ToString();
                           }
                           else
                           {
                               string[] xrr = neWW[i].ToString().Split(' ');
                               string TagII = xrr[xrr.Length - 1];
                               dataB = dataB + "<" + TagII + ">" + "<TAGDATA INDEX=" + i.ToString() + "/>" + "</" + TagII + ">" + "\r\n";
                               TagTmp = TagII;
                           }
                       }
                       else
                       {
                           //表示本属性前方 无可以使用的属性标示 则把此 取出的数据合并到前方  <XX><TAGDATA INDEX=1></XX> <XX><TAGDATA INDEX=1><TAGDATA INDEX=2></XX>

                           //1 取出包含的 <TAGDATA INDEX=1>  
                           if (TagTmp.Length > 0)
                           {
                               dataB = dataB + "<" + TagTmp + ">" + "<TAGDATA INDEX=" + i.ToString() + "/>" + "</" + TagTmp + ">" + "\r\n";
                           }
                       }
                   }
               }

              

               string x = SavePath;

               nSearch.ClassLibraryStruct.auto2dat v = new nSearch.ClassLibraryStruct.auto2dat();

               putFileData(x + ".a", "<TAGDATA INDEX=1/>");// 标题A

               putFileData(x + ".b", dataB);// 类聚B

               putFileData(x + ".c", dataA);// 简要显示C

               putFileData(x + ".d", dataA);// 快照显示D

               putFileData(x + ".e", dataA);// 索引用的数据E

               putFileData(x + ".f", dataE);// 采样结果列表F

               putFileData(x + ".h", ModelData);// 自动生成 过滤器H

               putFileData(x + ".t", "HTM");// 单独的标题数据 

               putFileData(x + ".m", "WEB");// 主类别

               putFileData(x + ".a1", "<TAGDATA INDEX=1/>");// 类别一
               putFileData(x + ".a2", "<TAGDATA INDEX=1/>");// 类别二

               nSearch.DebugShow.ClassDebugShow.WriteLineF("-模版建立成功->");

           }
           catch
           {
               nSearch.DebugShow.ClassDebugShow.WriteLineF("-模版建立失败->");
           }

       }

       /// <summary>
       /// 是否全为数字组成
       /// </summary>
       /// <param name="data"></param>
       /// <returns></returns>
       private bool isNum(string data)
       {
           if (data == null | data.Length == 0)
           {
               return false;
           }

           foreach (char a in data)
           {
               if (a < '0' | a > '9')
               {
                   return false;
               }
           }

           return true;
       }

       /// <summary>
       /// 标注模板  
       /// </summary>
       /// <param name="c">一个模版数据</param>
       /// <returns>标注的模板各个值</returns>
       private ArrayList EditOneModelTag(string c)
       {
           ArrayList x = new ArrayList();  //对每个*进行标注
           x.Clear();

           string[] xxs = c.Split('*');

           int Len = 0;

           foreach (string a in xxs)
           {
               Len = Len + a.Length;
           }

           //得到平均长度
           int OneLen = (int)Len / xxs.Length;

           for (int i = 0; i < xxs.Length; i++)
           {
               if (xxs[i].Length > 0 & OneLen < xxs[i].Length)
               {
                   string aaa = CCxmlTag(GetClearTag(xxs[i]));
                    
                   if (aaa.Length < 16 & isNum(aaa) == false)
                   {
                       x.Add(aaa);
                   }
                   else
                   {
                       x.Add("");
                   }
               }
               else
               {
                   x.Add("");
               }

           }

           return x;

           // 1 把模板数据按照*顺序 分割  每个* 对应其前部的一个部分 n(i)

           // 2 去掉n(i) 标签数据 作为属性   去掉 : 等  如果太长〉8 则放弃


       }

       /// <summary>
       /// 清除数据中的HTML标签
       /// </summary>
       /// <param name="dat"></param>
       /// <returns></returns>
       private string GetClearTag(string dat)
       {
           //</td></tr><tr><td class="main_text_left">身份验证:</td><td class="main_text">

           int Loop = dat.Length / 4;

           for (int i = 0; i < Loop; i++)
           {
               int a1 = dat.IndexOf('<');
               if (a1 == -1 | a1 == (dat.Length - 1))
               { }
               else
               {
                   int a2 = dat.IndexOf('>', a1 + 1);
                   if (a2 == -1)
                   { }
                   else
                   {
                       int aa = dat.Length;
                       // *<>*<>*
                       string dat1 = "";

                       if (a1 == 0)
                       {

                       }
                       else
                       {
                           dat1 = dat.Substring(0, a1);
                       }
                       string dat2 = dat.Substring(a2 + 1, aa - a2 - 1);
                       dat = dat1 + dat2;

                   }
               }


           }

           // 1 去掉标签 


           // 2 去掉: ：

           return dat;
       }


       /// <summary>
       /// 读文件
       /// </summary>
       /// <param name="filename"></param>
       /// <returns></returns>
       private   string getFileData(string filename)
       {

           StreamReader reader = null;
           string data = string.Empty;
           try
           {
               reader = new StreamReader(filename, System.Text.Encoding.GetEncoding("gb2312"));

               data = reader.ReadToEnd();

               reader.Close();
               return data;
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

           return "";


       }

       /// <summary>
       /// 写文件
       /// </summary>
       /// <param name="filename"></param>
       /// <param name="data"></param>
       private void putFileData(string filename, string data)
       {


           StreamWriter writer = null;
           try
           {
               writer = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("gb2312"));
               writer.Write(data);
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

       }


       /// <summary>
       /// 写入已经使用过的一条数据　　　xxx-->> 相对于设置列表 olgurl
       /// </summary>
       /// <param name="filename">文件名</param>
       /// <param name="data">数据</param>
       /// <param name="isApp">是否追加模式</param>
       private  void putmOLDMODELSOURCEFileData(string okPath, string data)
       {
           StreamWriter writer = null;
           try
           {
               writer = new StreamWriter(okPath, true, System.Text.Encoding.GetEncoding("gb2312"));
               //  writer.Write(data);
               writer.WriteLine(data);
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

       }
 
 
         /// <summary>
        /// 把长的数据变为短的 词典 把数据变为《KC+ZD INX=0》  
        /// </summary>
        /// <param name="HM">HTML数据列表</param>
        /// <returns></returns>
        private HTM2SHORT Long2Short(Hashtable HM)
        {
            HTM2SHORT cv = new HTM2SHORT();

            ArrayList i_i_1 = new ArrayList();
            i_i_1.Clear();
            Hashtable i_i_2 = new Hashtable();
            i_i_2.Clear();

            cv.Dict1 = i_i_1;
            cv.Dict2 = i_i_1;
            cv.HASH = i_i_2;

            ArrayList shortDict1 = new ArrayList();
            shortDict1.Clear();

            ArrayList shortDict2 = new ArrayList();
            shortDict2.Clear();

            //取标签
            foreach (System.Collections.DictionaryEntry de2 in HM)
            {

                string ShortHMONE = de2.Value.ToString();
                for (int i = 0; i < ShortHMONE.Length - 2; i++)
                {
                    int ae_1 = ShortHMONE.IndexOf('<', i);
                    int ae_2 = ShortHMONE.IndexOf('>', ae_1 + 1);
                    int ae_3 = ShortHMONE.IndexOf('<', ae_1 + 1);

                    if (ae_1 == -1 | ae_2 == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (ae_3 < ae_2)
                        {
                            i = ae_1 + 1;
                        }
                        else
                        {
                            string ae_str = ShortHMONE.Substring(ae_1, ae_2 - ae_1 + 1);
                            if (ae_str.Length < 10)
                            {
                            }
                            else
                            {
                                if (shortDict1.Contains(ae_str) == false)
                                {
                                    shortDict1.Add(ae_str);  //得到一个公共的词典
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }


            // 取标签外 
            foreach (System.Collections.DictionaryEntry de2 in HM)
            {

                string ShortHMONE = de2.Value.ToString();
                for (int i = 0; i < ShortHMONE.Length - 2; i++)
                {
                    int ae_1 = ShortHMONE.IndexOf('>', i);
                    int ae_2 = ShortHMONE.IndexOf('<', ae_1 + 1);
                    int ae_3 = ShortHMONE.IndexOf('>', ae_1 + 1);

                    if (ae_1 == -1 | ae_2 == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (ae_3 < ae_2)
                        {
                            i = ae_1 + 1;
                        }
                        else
                        {
                            string ae_str = ShortHMONE.Substring(ae_1 + 1, ae_2 - ae_1 - 1);
                            if (ae_str.Length < 10)
                            {
                            }
                            else
                            {
                                if (shortDict2.Contains(ae_str) == false)
                                {
                                    shortDict2.Add(ae_str);  //得到一个公共的词典
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }

            int short_int1 = shortDict1.Count;
            //对词典按照长度进行排序
            string[] ae_ss1 = new string[short_int1];
            for (int i = 0; i < short_int1; i++)
            {
                ae_ss1[i] = shortDict1[i].ToString();
            }

            for (int i = 0; i < short_int1; i++)
            {
                for (int j = i; j < short_int1; j++)
                {
                    if (ae_ss1[i].Length < ae_ss1[j].Length)
                    {
                        string tmp_one = ae_ss1[j];
                        ae_ss1[j] = ae_ss1[i];
                        ae_ss1[i] = tmp_one;
                    }
                }
            }
            shortDict1.Clear();


            int short_int2 = shortDict2.Count;
            //对词典按照长度进行排序
            string[] ae_ss2 = new string[short_int2];
            for (int i = 0; i < short_int2; i++)
            {
                ae_ss2[i] = shortDict2[i].ToString();
            }

            for (int i = 0; i < short_int2; i++)
            {
                for (int j = i; j < short_int2; j++)
                {
                    if (ae_ss2[i].Length < ae_ss2[j].Length)
                    {
                        string tmp_one = ae_ss2[j];
                        ae_ss2[j] = ae_ss2[i];
                        ae_ss2[i] = tmp_one;
                    }
                }
            }
            shortDict2.Clear();

            for (int i = 0; i < short_int1; i++)
            {
                shortDict1.Add(ae_ss1[i]);
            }

            for (int i = 0; i < short_int2; i++)
            {
                shortDict2.Add(ae_ss2[i]);
            }

            Hashtable new_HM = new Hashtable();
            new_HM.Clear();

            foreach (System.Collections.DictionaryEntry de2 in HM)
            {
                string a_onre = de2.Value.ToString();
                for (int u = 0; u < short_int1; u++)
                {
                    a_onre = a_onre.Replace(shortDict1[u].ToString(), "<[" + u.ToString() + ")>");
                }

                for (int u = 0; u < short_int2; u++)
                {
                    a_onre = a_onre.Replace(shortDict2[u].ToString(), "《[" + u.ToString() + ")》");
                }

                new_HM.Add(de2.Key, a_onre);
            }

            cv.Dict1 = shortDict1;
            cv.Dict2 = shortDict2;
            cv.HASH = new_HM;

            return cv;
        }

        /// <summary>
        /// 还原压缩过的单个数据
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string Short2Long(string dat, ArrayList dict1, ArrayList dict2)
        {
            for (int u = 0; u < dict1.Count; u++)
            {
                dat = dat.Replace("<[" + u.ToString() + ")>", dict1[u].ToString());
            }

            for (int u = 0; u < dict2.Count; u++)
            {
                dat = dat.Replace("《[" + u.ToString() + ")》", dict2[u].ToString());
            }

            return dat;
        }


       private string CCxmlTag(string dat)
       {
           return "";
       }



       /// <summary>
       /// 写文件
       /// </summary>
       /// <param name="filename"></param>
       /// <param name="data"></param>
       private void SaveFileData(string filename, string data)
       {


           StreamWriter writer = null;
           try
           {
               writer = new StreamWriter(filename, true , System.Text.Encoding.GetEncoding("gb2312"));
               writer.Write(data);
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

       }


    }
}
