using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */


namespace nSearch.DebugShow
{

    /// <summary>
    /// ������Ϣ��ʾ  nSearch.DebugShow.ClassDebugShow.WriteLine("");
    /// </summary>
    public static class ClassDebugShow
    {

        private static StringBuilder show_string = new StringBuilder();

        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        public static  bool IsShow = true;


        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        public static void SetIsShow(bool isShowB)
        {
             IsShow = isShowB;
        }

        /// <summary>
        /// ��ȡ��ʾ������
        /// </summary>
        /// <returns></returns>
        public static string showf()
        {
            if (IsShow == false) { return ""; }

            string show_string2 = show_string.ToString();
            show_string.Remove(0, show_string.Length);


            return show_string2;
        }

        /// <summary>
        /// �趨Ҫ��ʾ�����ݵ�������
        /// </summary>
        /// <param name="dat"></param>
        public static void WriteLine(string dat)
        {
            if (IsShow == false) { return  ; }

           

       //     System.Diagnostics.Debug.WriteLine(dat );

       //     Console.WriteLine(dat);


            if (show_string.Length > 1024 * 128)
            {//������δ�ܼ�ʱ��ȡ�Ļ� ��������
                show_string.Remove(0, show_string.Length);
            }

            show_string.AppendLine(dat+"   "+ Environment.TickCount.ToString());
            


        }

        /// <summary>
        /// �趨Ҫ��ʾ�����ݵ�������
        /// </summary>
        /// <param name="dat"></param>
        public static void WriteLineF(string dat)
        {
             //   System.Diagnostics.Debug.WriteLine(dat );

             //    Console.WriteLine(dat);

            if (IsShow == false) { return  ; }

            if (show_string.Length > 1024 * 128)
            {   //������δ�ܼ�ʱ��ȡ�Ļ� ��������
                show_string.Remove(0, show_string.Length);
            }

                show_string.AppendLine(dat);
              //  System.Diagnostics.Debug.WriteLine(dat);
           


        }

    }
}
