using System;
using System.Collections.Generic;
using System.Text;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.ClassLibraryHTML
{




    public class ClassHTML
    {

        private  ClassTXT2IDAT mClassTXT2IDAT = new ClassTXT2IDAT();
        private ClassTagClear mClassTagClear = new ClassTagClear();



        /// <summary>
        /// ������ Html��� ���� �� Ϊ�ɾ��ı�
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GetClearCode(string dat,bool isClearBD)
        {
            string mct = mClassTXT2IDAT.GetOneGoodData2(dat, isClearBD);

            return mct;
        }


        /// <summary>
        /// ����һ������õ�HTML���� onePage �ṹ�� Title = ���ı�  Body = �ɾ���HTML����   
        /// 
        /// ֻȡ����TITLE �͡�BODY  �е�����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public nSearch.DebugShow.onePage GetOnePage(string data, string UrlX)
        {
          //  nSearch.ClassLibraryStruct.onePage VC = new nSearch.ClassLibraryStruct.onePage();

            nSearch.DebugShow.onePage VC = new nSearch.DebugShow.onePage();

            int a1 = data.IndexOf("<title>");
            int a2 = data.IndexOf("</title>");
            int a3 = data.IndexOf("<body");
            int a4 = data.IndexOf("</body>");

            int a5 = data.IndexOf(">", a3 + 1);

            string data1 = "";
            string data2 = "";

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

                VC.Title =mClassTXT2IDAT.GetOneGoodData2( data1,true);
                VC.Body = mClassTagClear.HTML2CLEAR2( data2);
                VC.Num = 0;
               
            }
            catch
            {

                VC.Title = "[]";
                VC.Body =mClassTagClear.HTML2CLEAR2( data);
                VC.Num = 0;
         
            }

            return VC;
        
        }




    }
}


/*
 
 
 
         /// <summary>
        /// �����ǩΪ�ɾ���HTML  �ݲ��������·����URL����
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GetClearTagX(string dat,string url)
        {
            string mct = mClassTagClear.HTML2CLEAR(dat);

            return mct;
                   
        }

 
 
 
 
 
 
 
 
 */