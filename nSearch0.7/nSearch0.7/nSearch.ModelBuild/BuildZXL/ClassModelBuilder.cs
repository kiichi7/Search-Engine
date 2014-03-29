using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Net;
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
    /// 建立模板
    /// </summary>
     class ClassModelBuilder
    {
        private static Random raV = new Random();

        /// <summary>
        /// 取出的匹配字符串是否满足要求
        /// </summary>
        /// <param name="data"></param>
        /// <returns>包含　且　《》成对 </returns>
        private bool xStrItOK(string data)
        {
            if ((data.IndexOf("<") == -1) || (data.IndexOf(">") == -1) || (inStrNum(data, "<") != inStrNum(data, ">")))
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// 某个字符在串中的个数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int inStrNum(string data, string one)
        {
            string c = data.Replace(one, "");

            int cx = data.Length - c.Length;

            return cx;
        }




        /// <summary>
        /// 根据数据建立一个网页模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        public string BuilderModel(Hashtable mHTMs2)
        {

            Hashtable mHTMs = new Hashtable();
            mHTMs.Clear();
            foreach (System.Collections.DictionaryEntry de in mHTMs2)
            {
                //原始的数据需要修改以便可以在标题相同的情况 下识别标题
                string NewHTM =  de.Value.ToString();
                mHTMs.Add(de.Key, NewHTM);
            }


            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();

                char[] oneModel_ss = oneModel.ToCharArray();


                if (oneModel.Length > 4)
                {

                    int h = oneModel.Length;

                    if ((5 * h > onemLong) & (onemLong > 9000))
                    {
                        h = onemLong / 5;
                    }

                    if (h > 512)
                    {
                        h = 512;
                    }

                    while (true)
                    {
                        if (h < 4)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }

                            ///////**********************************************
                            //  string a1= oneModel.Substring(st, 1);
                            //   string a2= oneModel.Substring(st+h-1, 1);
                            ////////////*****************************************

                            char a1 = oneModel_ss[st];


                            if (a1 != '<')   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }


                            char a2 = oneModel_ss[st + h - 1];

                            if (a2 != '>')   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }


                            string onestr = oneModel.Substring(st, h);

                            //    if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            //   {
                            //       goto nextcmd3;         //取开头和末尾在<>中的数据
                            //   }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                // if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                if (de1.Value.ToString().IndexOf(onestr) == -1)
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            ////nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA1>>  "+cckc);
            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aassBuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            string mo = mmoo;

            if (mo.Length == 0)
            {

                return "";
            }
            else
            {
                Hashtable mytpp = new Hashtable();
                mytpp.Clear();

                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mo = mo.Replace("*" + dec.Key.ToString() + "*", "<kc*Kc>" + dec.Value.ToString() + "<kc*Kc>");


                }

                mo = mo.Replace("<kc*Kc><kc*Kc><kc*Kc>", "<kc*Kc>");
                mo = mo.Replace("<kc*Kc><kc*Kc>", "<kc*Kc>");
                mo = mo.Replace("<kc*Kc>", "*");

                return mo;


                // ArrayList mm = new ArrayList();


            }

        }
        /// <summary>
        /// 把cckc中的*0*提取出来
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string getXnX(string data)
        {
            if (data.IndexOf("*") == -1)
            {
                return "";
            }

            string mkc = "";

            int ii = 0;

            while (true)
            {
                ii = ii + 1;

                if (data.IndexOf("*") == -1)
                {
                    // mkc = mkc.Replace("###", "#");
                    // mkc = mkc.Replace("##", "#");
                    for (int uu = 0; uu < 100; uu++)
                    {

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                        mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                        mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                        mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");


                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");

                        /*
                        try
                        {


                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                            mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");

                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                            mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");

                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                            mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");


                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");

                        }
                        catch
                        {

                            for (int uu_i = 0; uu_i < 800; uu_i++)
                            {
                                mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<《3-" + uu.ToString() + "》>");
                                mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<《2-" + uu.ToString() + "》>");

                                mkc = mkc.Replace("<《3-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                                mkc = mkc.Replace("<《2-" + uu.ToString() + "》>", "<" + uu.ToString() + ">");
                            }

                        }
                        */
                    }



                    return mkc;
                }

                //1 取得第一个*  取得第2个*  截取第一个和第2个 之间的数据  截断字符串
                int a1 = data.IndexOf("*");
                int a2 = data.IndexOf("*", a1 + 1);
                // string aa = data.Substring(a1, a2 - a1);
                // mkc = mkc + aa;
                string aa = data.Substring(a1 + 1, a2 - a1 - 1);
                //  mkc = mkc + "#<"+aa+">#";
                mkc = mkc + "<" + aa + ">";

                data = data.Substring(a2 + 1, data.Length - a2 - 1);
            }


        }
        /// <summary>
        /// 《2》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aassBuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:


            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict2>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA2>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }
            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN2>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《3》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass3BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);

                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict3>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    try
                                    {
                                        mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                    }
                                    catch
                                    {
                                        return "";
                                    }
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA3>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass4BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN3>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《4》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass4BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict4>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }


                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA4>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass5BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }


            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN4>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《5》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass5BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict5>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA5>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass6BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN5>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《6》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass6BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict6>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA6>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass7BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN6>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《7》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass7BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict7>>  " + tdictI.ToString() + "  || " + onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA7>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass8BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN7>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《8》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass8BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass9BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }


            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("TRETURN8>>  " + mmoo);
                return mmoo;

            }

        }
        /// <summary>
        /// 《9》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass9BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass10BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        //10
        /// <summary>
        /// 《10》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass10BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }

                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass11BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        //11
        /// <summary>
        /// 《11》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass11BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }

                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass12BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //12
        ////////////////////////////////////////////////////////////////////////////////
        //12
        /// <summary>
        /// 《12》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass12BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass13BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //13
        ////////////////////////////////////////////////////////////////////////////////
        //13
        /// <summary>
        /// 《13》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass13BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass14BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //14
        ////////////////////////////////////////////////////////////////////////////////
        //14
        /// <summary>
        /// 《14》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass14BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass15BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //15
        ////////////////////////////////////////////////////////////////////////////////
        //15
        /// <summary>
        /// 《15》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass15BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass16BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //16
        ////////////////////////////////////////////////////////////////////////////////
        //16
        /// <summary>
        /// 《16》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass16BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass17BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //17
        ////////////////////////////////////////////////////////////////////////////////
        //17
        /// <summary>
        /// 《17》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass17BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass18BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //18
        ////////////////////////////////////////////////////////////////////////////////
        //18
        /// <summary>
        /// 《18》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass18BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass19BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //19
        ////////////////////////////////////////////////////////////////////////////////
        //19
        /// <summary>
        /// 《19》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass19BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass20BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //20
        ////////////////////////////////////////////////////////////////////////////////
        //20
        /// <summary>
        /// 《20》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass20BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass21BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //21
        ////////////////////////////////////////////////////////////////////////////////
        //21
        /// <summary>
        /// 《21》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass21BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass22BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //22
        ////////////////////////////////////////////////////////////////////////////////
        //22
        /// <summary>
        /// 《22》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass22BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass23BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //23
        ////////////////////////////////////////////////////////////////////////////////
        //23
        /// <summary>
        /// 《23》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass23BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass24BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //24
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《24》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass24BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass25BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //25
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《25》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass25BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass26BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //26
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《26》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass26BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass27BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //27
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《27》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass27BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass28BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //28
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《28》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass28BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass29BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //29
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《29》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass29BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass30BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }
            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //30
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《30》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass30BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass31BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //31
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《31》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass31BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass32BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //32
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《32》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass32BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass33BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //33
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《33》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass33BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //   mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass34BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //34
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《34》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass34BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass35BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //35
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《35》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass35BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass36BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //36
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《36》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass36BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass37BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //37
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《37》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass37BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //  mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass38BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //38
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《38》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass38BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass39BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //39
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《39》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass39BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            // string mmoo = aass40BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //40
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《40》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass40BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass41BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        ////////////////////////////////////////////////////////////////////////////////
        //41
        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 《41》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass41BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                // mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //从模板得到一个项目模板
            //string mmoo = aass42BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //从模板得到一个项目模板
                mmoo = aass3BuilderModel(mHTMs);
            }

            mmoo = mmoo.Replace("<", "*");
            mmoo = mmoo.Replace(">", "*");

            if (mmoo.Length == 0)
            {

                return "";
            }
            else
            {
                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());

                }

                return mmoo;

            }

        }
        /// <summary>
        /// 《42》根据子模板建立一个超级模板
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass42BuilderModel(Hashtable mHTMs)
        {
            //得到最短的串
            string oneModel = "";

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel.Length == 0)
                {
                    oneModel = de.Value.ToString();
                }
                else
                {
                    if (oneModel.Length > de.Value.ToString().Length)
                    {
                        oneModel = de.Value.ToString();
                    }
                }
            }

            foreach (System.Collections.DictionaryEntry de in mHTMs)
            {
                if (oneModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //得到匹配临时模板的原始长度
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //字典序号
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //碎片如果相同 就不用继续寻找添加     

            //存放临时碎片
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //开始遍历匹配串

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //移除旧的字符串  压入分解后的字符串
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //加入新的碎片项
            {
                if (oneModels.Contains(de.Key) == false)
                {
                    oneModels.Add(de.Key, "0");
                }
            }
            newModels.Clear();

            foreach (System.Collections.DictionaryEntry de in oneModels)
            {
                oneModel = de.Key.ToString();
                if (oneModel.Length > 2)
                {

                    int h = oneModel.Length;


                    while (true)
                    {
                        if (h < 3)         //最短字符不能小于5  >=四
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //判断是否合法 含有 <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //取开头和末尾在<>中的数据
                            }
                            //遍历数据  是否符合全部数据
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //符合所有项都存在的条件 压入字典  
                            tdict.Add(tdictI, onestr);
                            //数据替换
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                //mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                                string tmp_str = de2.Value.ToString();
                                if (tmp_str == null)
                                {
                                }
                                else
                                {
                                    mtmpp.Add(de2.Key, tmp_str.Replace(onestr, "*" + tdictI.ToString() + "*"));
                                }
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //字典序号

                            //oneModel  截取   取出碎片 压入 newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //因为碎片变化  所以重新开始扫描

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //本次遍历结束  去掉本字符串   
            }

            //mHTMs  处理新的 网页  只留下 *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //把cckc中的*0*提取出来
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;




            string mm = "";    //存储比较的值  收敛时即为模板
            bool mmT = true;  //是否收敛
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                if (mm.Length == 0)
                {
                    mm = de2.Value.ToString();
                }
                else
                {
                    if (mm != de2.Value.ToString())
                    {
                        mmT = false;
                        break;
                    }
                }
            }

            if (mmT == false)
            {
                return "";       //系统不收敛 无法生成模板
            }
            else
            {

                foreach (System.Collections.DictionaryEntry dec in tdict)
                {
                    mm = mm.Replace("<" + dec.Key.ToString() + ">", dec.Value.ToString());

                }



                return mm;
            }

        }
        /// <summary>
        /// 总结 得到串
        /// </summary>
        /// <param name="mud"></param>
        /// <returns></returns>
        private string GetComDataEND(Hashtable mud, Hashtable kdict)
        {

            Console.Write("..");

            // <1><2><3><4><5>
            //得到最长的串
            string oneLModel = "";
            foreach (System.Collections.DictionaryEntry de in mud)
            {
                if (oneLModel.Length == 0)
                {
                    oneLModel = de.Value.ToString();
                }
                else
                {
                    if (oneLModel.Length < de.Value.ToString().Length)
                    {
                        oneLModel = de.Value.ToString();
                    }
                }
            }

            //是否全部相同  若是 则返回一个模板
            foreach (System.Collections.DictionaryEntry de in mud)
            {
                if (oneLModel != de.Value.ToString())                    //不相同
                {
                    if (de.Value.ToString().IndexOf(oneLModel) == -1)    //不包含
                    {
                        goto cXStart;
                    }
                }
            }
            return oneLModel;

        cXStart:

            long iEndSD = getNumC(oneLModel);
            //long iiStOP = (long)Math.Pow(2, iEnd);

            //如果需要匹配的数据太长 则不匹配
            if (iEndSD > 20)
            {
                return "";
            }

            Hashtable myDB = new Hashtable();
            myDB.Clear();

            Hashtable cmyDB = new Hashtable();
            cmyDB.Clear();

            foreach (System.Collections.DictionaryEntry de in mud)
            {
                if (de.Value.ToString().Length > 0)
                {
                    long iEnd = getNumC(de.Value.ToString());
                    long iiStOP = (long)Math.Pow(2, iEnd);

                    string[] myGdata = de.Value.ToString().Split('<');

                    for (int c = 1; c < myGdata.Length; c++)
                    {
                        myGdata[c] = "<" + myGdata[c];
                    }


                    long mmPP = getNumC(de.Value.ToString()) + 1;
                    string myII = "";

                    for (long iv = iiStOP - 1; iv > 4; iv--)
                    {
                        //string myII = addOneGG(de.Value.ToString(),iv);//得到一个可能结果
                        string mI = Convert.ToString(iiStOP + iv, 2);

                        myII = string.Empty;

                        //按照mi 的显示  得到 新的 mdat 
                        for (long i = 1; i < mI.Length; i++)
                        {
                            if (mI.Substring((int)i, 1) == "1")
                            {
                                myII = myII + myGdata[i];  //得到数据
                            }
                        }

                        if (myII.Length > 0)
                        {
                            if (myDB.Contains(myII) == false)//没有的话就增加
                            {
                                myDB.Add(myII, 1);                   //增加一个数据   出现次数  同一个源中出现多次 不再记录
                                cmyDB.Add(myII, de.Key.ToString());  //记录此条数据来源
                            }
                            else
                            {   //当用户变化一次 记录次数才改变一次
                                if (cmyDB[myII].ToString() != de.Key.ToString())
                                {
                                    myDB[myII] = Int32.Parse(myDB[myII].ToString()) + 1;     //增加一个数据   出现次数  同一个源中出现多次 不再记录
                                    cmyDB[myII] = de.Key.ToString();  //记录此条数据来源        
                                }
                            }
                        }

                    }
                }
            }

            // 1 合并所有myII 相同的项目
            Hashtable as1 = new Hashtable();
            as1.Clear();

            foreach (System.Collections.DictionaryEntry de in myDB)
            {
                if (Int32.Parse(de.Value.ToString()) >= mud.Count)
                {
                    as1.Add(de.Key, "0");
                }
            }


            //得到最长的串  

            string as1LModel = "";
            foreach (System.Collections.DictionaryEntry de in as1)
            {
                if (as1LModel.Length == 0)
                {
                    as1LModel = de.Key.ToString();
                }
                else
                {
                    if (getNumC(as1LModel) < getNumC(de.Key.ToString()))
                    {
                        as1LModel = de.Key.ToString();
                    }
                }
            }

            //得到最长的串   队列
            Hashtable myLongStr = new Hashtable();
            myLongStr.Clear();
            long Xnml = getNumC(as1LModel);
            foreach (System.Collections.DictionaryEntry de in as1)
            {
                if (Xnml == getNumC(de.Key.ToString()))
                {
                    myLongStr.Add(de.Key.ToString(), "0");
                }
            }


            //根据DICT还原   
            Hashtable mDictHY = new Hashtable();
            mDictHY.Clear();

            foreach (System.Collections.DictionaryEntry de in myLongStr)
            {
                string mmoo = de.Key.ToString();
                mmoo = mmoo.Replace("<", "*");
                mmoo = mmoo.Replace(">", "*");
                foreach (System.Collections.DictionaryEntry dec in kdict)
                {
                    mmoo = mmoo.Replace("*" + dec.Key.ToString() + "*", dec.Value.ToString());
                }

                if (mDictHY.Contains(mmoo) == false)
                {
                    mDictHY.Add(mmoo, de.Key.ToString());
                }
            }


            //得到最长的串  同类项中取出最长的

            string ENDBACKX = "";
            string ENDBACK2 = "";

            foreach (System.Collections.DictionaryEntry de in mDictHY)
            {
                if (ENDBACKX.Length == 0)
                {
                    ENDBACKX = de.Key.ToString();
                    ENDBACK2 = de.Value.ToString();
                }
                else
                {
                    if (getNumC(ENDBACKX) < getNumC(de.Key.ToString()))
                    {
                        ENDBACKX = de.Key.ToString();
                        ENDBACK2 = de.Value.ToString();
                    }
                }
            }



            return ENDBACK2;
        }
        /// <summary>
        /// 得到一个可能结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="ii">序号 转换2进制</param>
        /// <returns></returns>
        private string addOneGG(string[] Gdata, string ii)  //LENX  元素数目 
        {

            Console.Write("..");

            // long  LENX = getNumC(data);

            // if (ii > LENX)
            // {
            //     return "";        //长度不适合此数据 则 返回
            // }

            // ii=ii + (long)Math.Pow(2,LENX);   //为保持数据位 需要在前面+1

            // string mi = Convert.ToString(ii,2);
            string mi = ii;
            // data =<1><2><3><4><5><6><7>
            // string[] myGdata = data.Split('<');

            // for(int c=1;c<myGdata.Length;c++)
            // {
            //     myGdata[c] = "<" + myGdata[c];                                          
            // }


            string mdat = "";

            //按照mi 的显示  得到 新的 mdat 
            for (long i = 1; i < mi.Length; i++)
            {
                if (mi.Substring((int)i, 1) == "1")
                {
                    mdat = mdat + Gdata[i];  //得到数据
                }
            }

            //  if (mdat.Length < 6)
            //  {
            //      return "";
            //  }

            return mdat;
        }
        /// <summary>
        /// 得到 *的大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private long getNumC(string data)
        {
            return data.Length - data.Replace("<", "").Length;

        }

    }
}


/*
 
 
 
        /// <summary>
        /// 改变数据 <ZDKC0>" + data1 + "</ZDKC0> 在标题改变  以便标题栏能在完全相同时被识别 
        /// 那么 0 位置就肯定为标题兰
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string RW_HTMLDB(string data)
        {

            int a1 = data.IndexOf("<ZDKC0>");
            int a2 = data.IndexOf("</ZDKC0>");
            int a3 = data.IndexOf("<ZDbody>");
            int a4 = data.IndexOf("</ZDbody>");

            int a5 = data.IndexOf(">", a3 + 1);

            string data1 = "";
            string data2 = "";


            int nowTitle = raV.Next(25, 984562);

            try
            {

                if (a1 > 0 & a2 > 0 & a2 > a1)
                {
                    data1 = data.Substring(a1 + 7, a2 - a1 - 7);
                }

                if (a3 > 0 & a5 > 0 & a5 > a3)
                {
                    data2 = data.Substring(a5 + 1, a4 - a5 - 1);
                }

                data1 = data1.Replace("*", "#");
                data2 = data2.Replace("*", "#");

                return " <ZDKC0>" + nowTitle.ToString() + "_" + data1.Length.ToString() + "</ZDKC0> <ZDbody>" + data2 + "</ZDbody>";
            }
            catch
            {

                return " <ZDKC0>" + nowTitle.ToString() + "_" + data.Length.ToString() + "</ZDKC0> <ZDbody>" + data + "</ZDbody> ";
            }

        }

 
 
 
 
 
 
 
 
 
 
 */