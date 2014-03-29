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

namespace nSearch.UrlMain
{

    /// <summary>
    /// URL��������
    /// </summary>
    public static  class ClassSTURL
    {
        private static ClassUrlMain newUrl = new ClassUrlMain();


        /// <summary>
        /// ���ݶ�ȡ��  ����ʱ ����������������
        /// </summary>
        private static bool xl_lock_r = false;

        /// <summary>
        /// ����д����  ����ʱ ����������д����
        /// </summary>
        private static bool xl_lock_w = false;






        /// <summary>
        /// ��ʼ��
        /// </summary>
        public static void Init( string path)
        {
            //����ϵͳ
            xl_lock_r = true;
            //����ϵͳ
            xl_lock_w = true;
          

            newUrl.InitClassUrlMain(path);



 
            //д�����
            xl_lock_r = false;
            //д�����
            xl_lock_w = false;

        }

        /// <summary>
        /// ��ȡһ��URL
        /// </summary>
        public static string  GetOneUrl()
        {
            if (xl_lock_r == true)
            {
                return null;
            }

            xl_lock_r = true; //����

        

       




    string     btr =   newUrl.GetOneUrl();

     xl_lock_r = false; //����

    return btr;




        }


        /// <summary>
        /// ѹ��һ��URL
        /// </summary>
        public static void PutOneUrl(string url)
        {

            //���ݴ���д����״̬ ����д��
            if (xl_lock_w == true)
            {
                return ;
            }


            //����ϵͳ
            xl_lock_w = true;

             newUrl.PutOneUrl(url);        

            //д�����
            xl_lock_w = false;


        }

        /// <summary>
        /// ѹ��һ��URL
        /// </summary>
        public static void RePutOneUrl(string url)
        {

            //���ݴ���д����״̬ ����д��
            if (xl_lock_w == true)
            {
                return  ;
            }


            //����ϵͳ
            xl_lock_w = true;

              newUrl.RePutOneUrl(url);     

            //д�����
            xl_lock_w = false;


        }

        /// <summary>
        /// �õ��ڲ����ͳ��
        /// </summary>
        /// <returns></returns>
        public static string GetTongji()
        {

            return newUrl.GetTongji();
        
        }

        /// <summary>
        /// �����˳� 
        /// </summary>
        public static void SaveExit()
        {
            newUrl.ExitSave();
        
        }


    }
}
