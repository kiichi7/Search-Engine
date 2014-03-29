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

namespace nSearch.ModelBuild.BuildZXL
{


    /// <summary>
    /// �洢���ݳ��̱仯
    /// </summary>
   public   struct HTM2SHORT
    {
        /// <summary>
        /// �ʵ� ��ǩ
        /// </summary>
        public ArrayList Dict1;

        /// <summary>
        /// �ʵ� ����
        /// </summary>
        public ArrayList Dict2;

        /// <summary>
        /// �任������
        /// </summary>
        public Hashtable HASH;


    }




    /// <summary>
    /// �������Զ�������
    /// 
    ///   �仯 ʹ�� <XL INDEX=T> ������ֱ�Ӵ�Title��ȡ�������� 
    /// 
    /// </summary>
   public  class ClassAuto
    {
       /// <summary>
       /// �ļ�ϵͳ·��
       /// </summary>
       private  string pFS = "";

       /// <summary>
       /// ����·��
       /// </summary>
       private  string pUrl = "";

       /// <summary>
       /// ģ��·��
       /// </summary>
       private  string pMod = "";

       /// <summary>
       /// �Ƿ�ֹͣ
       /// </summary>
       public bool IsStop = false;


       public void SetPath(string fs, string url, string mod)
       {
           pFS = fs;

           pUrl = url;

           pMod = mod;
       
       }

       /// <summary>
       /// ��ʼ���г���
       /// </summary>
       public void StartRun()
       {

           //�ļ�ϵͳ��ʼ��
           nSearch.FS.ClassFSMD.Init(pFS );
           nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�ϵͳ��ʼ��");

           //�ļ�ID���ձ�
           ArrayList mID = nSearch.FS.ClassFSMD.GetUrlList();

           IsStop = false;
 
           //�Ѿ����ɹ���ģ�������
           ArrayList BV = new ArrayList();


           // 1 �õ�Ŀ¼�µ��ļ�
           DirectoryInfo dir = new DirectoryInfo(pMod);
           foreach (FileInfo f in dir.GetFiles("*.a"))   //���������xmlΪ��չ�����ļ�   
           {
               String name = f.Name;         //nameΪ���ļ����µ��ļ����ƣ���f.FullNameΪȫ��  
               name = name.Substring(0, name.Length - 2);


               BV.Add(name); //�Ѿ����ɹ���ģ�������
           }


           // 1 ��ȡ�����б�
           Hashtable urlList = CaiYang(pUrl);

           

           // ���������б�
           foreach (DictionaryEntry nT in urlList)
           {


               if (IsStop == true)
               {
                   //ֹͣ����
                   return;
               }
               else
               {
                   // 2 �õ�һ����Ҫ��URL ����
                   ArrayList xbn = (ArrayList)nT.Value;
                   //3 ��Ų�������
                   Hashtable xnbDat = new Hashtable();

                   string NameMod = nT.Key.ToString();

                   //�Ѿ����ɹ���ģ�������
                   if (BV.Contains(NameMod) == true)
                   {
                       //�Ѿ����ɹ��� ����
                   }
                   else
                   {


                       //��¼�Ѿ����ɹ�ģ��Ĳ������
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

                                   //������ӵ�����

                                   xnbDat.Add(aurl, newDat);

                               }
                           }
                       }

                       if (xnbDat.Count > 2)
                       {
                           //����һ��ģ��
                           nSearch.DebugShow.ClassDebugShow.WriteLineF("��ʼ����һ��ģ�� �������ݸ�����" + xnbDat.Count.ToString());
                           CompModIt(pMod, xnbDat, NameMod);
                           nSearch.DebugShow.ClassDebugShow.WriteLineF("1��ģ�彨�����");
                       }
                       xbn = null;
                       xnbDat = null;
                       NameMod = "";
                   }
               }
           }


           nSearch.DebugShow.ClassDebugShow.WriteLineF("ȫ��ģ�彨�����");
       
       }



       /// <summary>
       /// ����һ��ģ��
       /// </summary>
       /// <param name="mpathc">ģ��·��</param>
       /// <param name="nm">����</param>
       /// <param name="modName">ģ������</param>
       private  void CompModIt(string mpathc, Hashtable nm,string modName)
       {

           nSearch.ClassLibraryHTML.ClassHTML mHtm = new nSearch.ClassLibraryHTML.ClassHTML();
       
             //����Ԥ����  �����������
           Hashtable  nT = new Hashtable ();
           nT.Clear();

           foreach (DictionaryEntry HI in nm)
           {

               string url = HI.Key.ToString();

               nSearch.DebugShow.onePage nnt_mp = (nSearch.DebugShow.onePage)mHtm.GetOnePage(HI.Value.ToString(), url);

                string HtmDat = nnt_mp.Body;

               //ȡ��Title��� Body �ֿ����� ���Ч��
               //Title�Զ���Ϊ��0�� ��������

               nT.Add(url, HtmDat);
                    
           }

           nSearch.DebugShow.ClassDebugShow.WriteLineF(" ���ݼ������ ");
             
           //�͵����������д���
           TMM(nT, mpathc+modName);

             //������
       
       
       
       
       }

       /// <summary>
       /// �õ���������
       /// </summary>
       /// <param name="path"></param>
       /// <returns></returns>
       private  Hashtable CaiYang(string path)
       {

           Hashtable VBF = new Hashtable();
           VBF.Clear();

           //��������Ч��  ȥ��һЩ����
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
                                   //ȥ��һ��������  ��֤��������
                           }
                           else
                           {
                               if (xxxx.Count >= 15)
                               {
                                   //����ֻ��20������ ��֤������������������̫��
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
       /// ����ģ���߳�
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




               //����ģ�幹����
               nSearch.ModelBuild.BuildZXL.ClassModelBuilder myBU = new nSearch.ModelBuild.BuildZXL.ClassModelBuilder();

               string ModelData = "";

               try
               {
                   ModelData = myBU.BuilderModel(TMMSOne2);

                   ModelData = Short2Long(ModelData, Short_DICT1, Short_DICT2);  //��ԭѹ��������

               }
               catch
               {
                   nSearch.DebugShow.ClassDebugShow.WriteLineF("--> ERROR �ų�����ģ��");
                //   nSearch.DebugShow.ClassDebugShow.WriteLineF;
                   return;
               }

               int nn = myBU.inStrNum(ModelData, "*");

               //ģ�潨������ ���������ݲ������ģ�洴������ ���迼��
               if (ModelData.Length < 150 | nn < 4)
               {
                   nSearch.DebugShow.ClassDebugShow.WriteLineF("--> �ų�����ģ��");
                   return;
               }

               ArrayList txtDat = EditOneModelTag(ModelData);

               ArrayList neWW = (ArrayList)txtDat.Clone();

               //*************************************************************************************
               //  ƥ��ģ��  
               //*************************************************************************************
               // ƥ��ģ��  ����ȫ��ƥ��ģ��      
               string[] pipeiEndData = new string[neWW.Count];  //ƥ��õ���ƥ�����б�����
               for (int i = 0; i < neWW.Count; i++)
               {
                   pipeiEndData[i] = "";
               }
               foreach (System.Collections.DictionaryEntry de2 in TMMSOne)
               {
                   //�����˲��� 
                   nSearch.ClassLibraryStruct.ClassUserModel m = new nSearch.ClassLibraryStruct.ClassUserModel();
                   //ѹ�����ģ��
                   m.TestModeL("", "", "", "", "", "", ModelData, "","","");
                   nSearch.ClassLibraryStruct.auto2dat k = m.getTagAndData(de2.Value.ToString());
                   Hashtable p = m.modelOneList;
                   foreach (System.Collections.DictionaryEntry de in p)
                   {
                       int pi = (int)de.Key;
                       pipeiEndData[pi] = pipeiEndData[pi] + de.Value.ToString().Trim();
                   }
               }

               string dataA = "";//�����õ�����

               string dataB = "";//����õ�����

               string TagTmp = "����"; //��¼��1���б�ʾ�� ���� 

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
                           //��ʾ������ǰ�� �޿���ʹ�õ����Ա�ʾ ��Ѵ� ȡ�������ݺϲ���ǰ��  <XX><TAGDATA INDEX=1></XX> <XX><TAGDATA INDEX=1><TAGDATA INDEX=2></XX>

                           //1 ȡ�������� <TAGDATA INDEX=1>  
                           if (TagTmp.Length > 0)
                           {
                               dataB = dataB + "<" + TagTmp + ">" + "<TAGDATA INDEX=" + i.ToString() + "/>" + "</" + TagTmp + ">" + "\r\n";
                           }
                       }
                   }
               }

              

               string x = SavePath;

               nSearch.ClassLibraryStruct.auto2dat v = new nSearch.ClassLibraryStruct.auto2dat();

               putFileData(x + ".a", "<TAGDATA INDEX=1/>");// ����A

               putFileData(x + ".b", dataB);// ���B

               putFileData(x + ".c", dataA);// ��Ҫ��ʾC

               putFileData(x + ".d", dataA);// ������ʾD

               putFileData(x + ".e", dataA);// �����õ�����E

               putFileData(x + ".f", dataE);// ��������б�F

               putFileData(x + ".h", ModelData);// �Զ����� ������H

               putFileData(x + ".t", "HTM");// �����ı������� 

               putFileData(x + ".m", "WEB");// �����

               putFileData(x + ".a1", "<TAGDATA INDEX=1/>");// ���һ
               putFileData(x + ".a2", "<TAGDATA INDEX=1/>");// ����

               nSearch.DebugShow.ClassDebugShow.WriteLineF("-ģ�潨���ɹ�->");

           }
           catch
           {
               nSearch.DebugShow.ClassDebugShow.WriteLineF("-ģ�潨��ʧ��->");
           }

       }

       /// <summary>
       /// �Ƿ�ȫΪ�������
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
       /// ��עģ��  
       /// </summary>
       /// <param name="c">һ��ģ������</param>
       /// <returns>��ע��ģ�����ֵ</returns>
       private ArrayList EditOneModelTag(string c)
       {
           ArrayList x = new ArrayList();  //��ÿ��*���б�ע
           x.Clear();

           string[] xxs = c.Split('*');

           int Len = 0;

           foreach (string a in xxs)
           {
               Len = Len + a.Length;
           }

           //�õ�ƽ������
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

           // 1 ��ģ�����ݰ���*˳�� �ָ�  ÿ��* ��Ӧ��ǰ����һ������ n(i)

           // 2 ȥ��n(i) ��ǩ���� ��Ϊ����   ȥ�� : ��  ���̫����8 �����


       }

       /// <summary>
       /// ��������е�HTML��ǩ
       /// </summary>
       /// <param name="dat"></param>
       /// <returns></returns>
       private string GetClearTag(string dat)
       {
           //</td></tr><tr><td class="main_text_left">�����֤:</td><td class="main_text">

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

           // 1 ȥ����ǩ 


           // 2 ȥ��: ��

           return dat;
       }


       /// <summary>
       /// ���ļ�
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
       /// д�ļ�
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
       /// д���Ѿ�ʹ�ù���һ�����ݡ�����xxx-->> ����������б� olgurl
       /// </summary>
       /// <param name="filename">�ļ���</param>
       /// <param name="data">����</param>
       /// <param name="isApp">�Ƿ�׷��ģʽ</param>
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
        /// �ѳ������ݱ�Ϊ�̵� �ʵ� �����ݱ�Ϊ��KC+ZD INX=0��  
        /// </summary>
        /// <param name="HM">HTML�����б�</param>
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

            //ȡ��ǩ
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
                                    shortDict1.Add(ae_str);  //�õ�һ�������Ĵʵ�
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }


            // ȡ��ǩ�� 
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
                                    shortDict2.Add(ae_str);  //�õ�һ�������Ĵʵ�
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }

            int short_int1 = shortDict1.Count;
            //�Դʵ䰴�ճ��Ƚ�������
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
            //�Դʵ䰴�ճ��Ƚ�������
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
                    a_onre = a_onre.Replace(shortDict2[u].ToString(), "��[" + u.ToString() + ")��");
                }

                new_HM.Add(de2.Key, a_onre);
            }

            cv.Dict1 = shortDict1;
            cv.Dict2 = shortDict2;
            cv.HASH = new_HM;

            return cv;
        }

        /// <summary>
        /// ��ԭѹ�����ĵ�������
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
                dat = dat.Replace("��[" + u.ToString() + ")��", dict2[u].ToString());
            }

            return dat;
        }


       private string CCxmlTag(string dat)
       {
           return "";
       }



       /// <summary>
       /// д�ļ�
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
