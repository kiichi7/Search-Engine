using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Net;
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
    /// ����ģ��
    /// </summary>
     class ClassModelBuilder
    {
        private static Random raV = new Random();

        /// <summary>
        /// ȡ����ƥ���ַ����Ƿ�����Ҫ��
        /// </summary>
        /// <param name="data"></param>
        /// <returns>�������ҡ������ɶ� </returns>
        private bool xStrItOK(string data)
        {
            if ((data.IndexOf("<") == -1) || (data.IndexOf(">") == -1) || (inStrNum(data, "<") != inStrNum(data, ">")))
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// ĳ���ַ��ڴ��еĸ���
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
        /// �������ݽ���һ����ҳģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        public string BuilderModel(Hashtable mHTMs2)
        {

            Hashtable mHTMs = new Hashtable();
            mHTMs.Clear();
            foreach (System.Collections.DictionaryEntry de in mHTMs2)
            {
                //ԭʼ��������Ҫ�޸��Ա�����ڱ�����ͬ����� ��ʶ�����
                string NewHTM =  de.Value.ToString();
                mHTMs.Add(de.Key, NewHTM);
            }


            Console.Write("..");

            //�õ���̵Ĵ�
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

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 4)         //����ַ�����С��5  >=��
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


                            if (a1 != '<')   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }


                            char a2 = oneModel_ss[st + h - 1];

                            if (a2 != '>')   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }


                            string onestr = oneModel.Substring(st, h);

                            //    if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            //   {
                            //       goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            //   }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                // if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                if (de1.Value.ToString().IndexOf(onestr) == -1)
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            ////nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
                            Hashtable mtmpp = new Hashtable();
                            mtmpp.Clear();

                            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
                            {
                                mtmpp.Add(de2.Key, de2.Value.ToString().Replace(onestr, "*" + tdictI.ToString() + "*"));
                            }

                            mHTMs = mtmpp;

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA1>>  "+cckc);
            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��cckc�е�*0*��ȡ����
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

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                        mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                        mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");

                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                        mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");


                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");
                        mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");

                        /*
                        try
                        {


                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                            mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");

                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                            mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");

                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                            mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");


                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");
                            mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<" + uu.ToString() + ">");

                        }
                        catch
                        {

                            for (int uu_i = 0; uu_i < 800; uu_i++)
                            {
                                mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + "><" + uu.ToString() + ">", "<��3-" + uu.ToString() + "��>");
                                mkc = mkc.Replace("<" + uu.ToString() + "><" + uu.ToString() + ">", "<��2-" + uu.ToString() + "��>");

                                mkc = mkc.Replace("<��3-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                                mkc = mkc.Replace("<��2-" + uu.ToString() + "��>", "<" + uu.ToString() + ">");
                            }

                        }
                        */
                    }



                    return mkc;
                }

                //1 ȡ�õ�һ��*  ȡ�õ�2��*  ��ȡ��һ���͵�2�� ֮�������  �ض��ַ���
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
        /// ��2��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aassBuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:


            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict2>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA2>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��3��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass3BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);

                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict3>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA3>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��4��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass4BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict4>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA4>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass5BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��5��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass5BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict5>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA5>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass6BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��6��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass6BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict6>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA6>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass7BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��7��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass7BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-Dict7>>  " + tdictI.ToString() + "  || " + onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                //nSearch.DebugShow.ClassDebugShow.WriteLineF("M-DATA7>>  " + cckc);
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass8BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��8��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass8BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass9BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��9��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass9BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass10BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��10��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass10BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass11BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��11��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass11BuilderModel(Hashtable mHTMs)
        {

            Console.Write("..");
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass12BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��12��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass12BuilderModel(Hashtable mHTMs)
        {
            Console.Write("..");

            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass13BuilderModel(mHTMs);

            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��13��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass13BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass14BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��14��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass14BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass15BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��15��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass15BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass16BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��16��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass16BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass17BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��17��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass17BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass18BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��18��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass18BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass19BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��19��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass19BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass20BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��20��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass20BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass21BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��21��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass21BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass22BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��22��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass22BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass23BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��23��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass23BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass24BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��24��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass24BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass25BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��25��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass25BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass26BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��26��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass26BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass27BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��27��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass27BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass28BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��28��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass28BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass29BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��29��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass29BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass30BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��30��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass30BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass31BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��31��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass31BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass32BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��32��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass32BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass33BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��33��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass33BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass34BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��34��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass34BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass35BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��35��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass35BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass36BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��36��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass36BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass37BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��37��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass37BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass38BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��38��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass38BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass39BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��39��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass39BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            // string mmoo = aass40BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��40��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass40BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass41BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��41��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass41BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;

            //��ģ��õ�һ����Ŀģ��
            //string mmoo = aass42BuilderModel(mHTMs);
            string mmoo = GetComDataEND(mHTMs, tdict);

            if (mmoo.Length == 0)
            {
                //��ģ��õ�һ����Ŀģ��
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
        /// ��42��������ģ�彨��һ������ģ��
        /// </summary>
        /// <param name="mHTMs"></param>
        /// <returns></returns>
        private string aass42BuilderModel(Hashtable mHTMs)
        {
            //�õ���̵Ĵ�
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
                if (oneModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneModel;

        cXStart:

            //�õ�ƥ����ʱģ���ԭʼ����
            int onemLong = oneModel.Length;

            Hashtable tdict = new Hashtable();
            int tdictI = 0; //�ֵ����
            tdict.Clear();

            Hashtable oneModels = new Hashtable();
            oneModels.Clear();
            oneModels.Add(oneModel, "0");  //��Ƭ�����ͬ �Ͳ��ü���Ѱ�����     

            //�����ʱ��Ƭ
            Hashtable newModels = new Hashtable();
            newModels.Clear();

            oneModel = "";

        NewStart: //��ʼ����ƥ�䴮

            if (oneModel.Length > 0)
            {
                oneModels.Remove(oneModel);       //�Ƴ��ɵ��ַ���  ѹ��ֽ����ַ���
            }

            foreach (System.Collections.DictionaryEntry de in newModels)    //�����µ���Ƭ��
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
                        if (h < 3)         //����ַ�����С��5  >=��
                        { goto nextcmd2; }

                        int st = 0;
                        while (true)
                        {
                            if (st + h > oneModel.Length)
                            { goto nextcmd1; }
                            string a1 = oneModel.Substring(st, 1);
                            string a2 = oneModel.Substring(st + h - 1, 1);
                            if ((a1 != "<") | (a2 != ">"))   //�ж��Ƿ�Ϸ� ���� <  >
                            {
                                goto nextcmd3;
                            }

                            string onestr = oneModel.Substring(st, h);
                            if ((onestr.Substring(0, 1) != "<") | (onestr.Substring(onestr.Length - 1, 1) != ">") | (onestr.IndexOf(">") == -1) | (onestr.IndexOf("<") == -1))
                            {
                                goto nextcmd3;         //ȡ��ͷ��ĩβ��<>�е�����
                            }
                            //��������  �Ƿ����ȫ������
                            foreach (System.Collections.DictionaryEntry de1 in mHTMs)
                            {
                                if ((de1.Value.ToString().IndexOf(onestr) == -1) & (de1.Value.ToString().Length > 0))
                                {
                                    goto nextcmd3;
                                }
                            }
                            //������������ڵ����� ѹ���ֵ�  
                            tdict.Add(tdictI, onestr);
                            //�����滻
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

                            tdictI = tdictI + 1;  //�ֵ����

                            //oneModel  ��ȡ   ȡ����Ƭ ѹ�� newModels
                            string oneModelTmp = oneModel.Replace(onestr, "*");
                            string[] mykc = oneModelTmp.Split('*');
                            foreach (string ddee in mykc)
                            {
                                if ((ddee.Length > 4) & (newModels.Contains(ddee) == false))
                                {
                                    newModels.Add(ddee, "0");
                                }
                            }
                            goto NewStart;             //��Ϊ��Ƭ�仯  �������¿�ʼɨ��

                        nextcmd3:
                            st = st + 1;
                        }
                    nextcmd1:
                        h = h - 1;
                    }
                }
            nextcmd2: ;
                goto NewStart;  //���α�������  ȥ�����ַ���   
            }

            //mHTMs  �����µ� ��ҳ  ֻ���� *0*
            Hashtable mtmppC = new Hashtable();
            mtmppC.Clear();
            foreach (System.Collections.DictionaryEntry de2 in mHTMs)
            {
                string cckc = de2.Value.ToString();
                //��cckc�е�*0*��ȡ����
                mtmppC.Add(de2.Key, getXnX(de2.Value.ToString()));
                // mtmppC.Add(de2.Key, de2.Value.ToString());

            }

            mHTMs = mtmppC;




            string mm = "";    //�洢�Ƚϵ�ֵ  ����ʱ��Ϊģ��
            bool mmT = true;  //�Ƿ�����
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
                return "";       //ϵͳ������ �޷�����ģ��
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
        /// �ܽ� �õ���
        /// </summary>
        /// <param name="mud"></param>
        /// <returns></returns>
        private string GetComDataEND(Hashtable mud, Hashtable kdict)
        {

            Console.Write("..");

            // <1><2><3><4><5>
            //�õ���Ĵ�
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

            //�Ƿ�ȫ����ͬ  ���� �򷵻�һ��ģ��
            foreach (System.Collections.DictionaryEntry de in mud)
            {
                if (oneLModel != de.Value.ToString())                    //����ͬ
                {
                    if (de.Value.ToString().IndexOf(oneLModel) == -1)    //������
                    {
                        goto cXStart;
                    }
                }
            }
            return oneLModel;

        cXStart:

            long iEndSD = getNumC(oneLModel);
            //long iiStOP = (long)Math.Pow(2, iEnd);

            //�����Ҫƥ�������̫�� ��ƥ��
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
                        //string myII = addOneGG(de.Value.ToString(),iv);//�õ�һ�����ܽ��
                        string mI = Convert.ToString(iiStOP + iv, 2);

                        myII = string.Empty;

                        //����mi ����ʾ  �õ� �µ� mdat 
                        for (long i = 1; i < mI.Length; i++)
                        {
                            if (mI.Substring((int)i, 1) == "1")
                            {
                                myII = myII + myGdata[i];  //�õ�����
                            }
                        }

                        if (myII.Length > 0)
                        {
                            if (myDB.Contains(myII) == false)//û�еĻ�������
                            {
                                myDB.Add(myII, 1);                   //����һ������   ���ִ���  ͬһ��Դ�г��ֶ�� ���ټ�¼
                                cmyDB.Add(myII, de.Key.ToString());  //��¼����������Դ
                            }
                            else
                            {   //���û��仯һ�� ��¼�����Ÿı�һ��
                                if (cmyDB[myII].ToString() != de.Key.ToString())
                                {
                                    myDB[myII] = Int32.Parse(myDB[myII].ToString()) + 1;     //����һ������   ���ִ���  ͬһ��Դ�г��ֶ�� ���ټ�¼
                                    cmyDB[myII] = de.Key.ToString();  //��¼����������Դ        
                                }
                            }
                        }

                    }
                }
            }

            // 1 �ϲ�����myII ��ͬ����Ŀ
            Hashtable as1 = new Hashtable();
            as1.Clear();

            foreach (System.Collections.DictionaryEntry de in myDB)
            {
                if (Int32.Parse(de.Value.ToString()) >= mud.Count)
                {
                    as1.Add(de.Key, "0");
                }
            }


            //�õ���Ĵ�  

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

            //�õ���Ĵ�   ����
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


            //����DICT��ԭ   
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


            //�õ���Ĵ�  ͬ������ȡ�����

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
        /// �õ�һ�����ܽ��
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="ii">��� ת��2����</param>
        /// <returns></returns>
        private string addOneGG(string[] Gdata, string ii)  //LENX  Ԫ����Ŀ 
        {

            Console.Write("..");

            // long  LENX = getNumC(data);

            // if (ii > LENX)
            // {
            //     return "";        //���Ȳ��ʺϴ����� �� ����
            // }

            // ii=ii + (long)Math.Pow(2,LENX);   //Ϊ��������λ ��Ҫ��ǰ��+1

            // string mi = Convert.ToString(ii,2);
            string mi = ii;
            // data =<1><2><3><4><5><6><7>
            // string[] myGdata = data.Split('<');

            // for(int c=1;c<myGdata.Length;c++)
            // {
            //     myGdata[c] = "<" + myGdata[c];                                          
            // }


            string mdat = "";

            //����mi ����ʾ  �õ� �µ� mdat 
            for (long i = 1; i < mi.Length; i++)
            {
                if (mi.Substring((int)i, 1) == "1")
                {
                    mdat = mdat + Gdata[i];  //�õ�����
                }
            }

            //  if (mdat.Length < 6)
            //  {
            //      return "";
            //  }

            return mdat;
        }
        /// <summary>
        /// �õ� *�Ĵ�С
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
        /// �ı����� <ZDKC0>" + data1 + "</ZDKC0> �ڱ���ı�  �Ա������������ȫ��ͬʱ��ʶ�� 
        /// ��ô 0 λ�þͿ϶�Ϊ������
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