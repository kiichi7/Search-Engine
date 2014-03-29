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

namespace nSearch.FS
{
    /// <summary>
    /// �ļ�ϵͳ  ����
    /// </summary>
   public static  class ClassFSMD
    {

       private static ClassFS myFS = new ClassFS();

       /// <summary>
       /// ���ݶ�ȡ��  ����ʱ ����������������
       /// </summary>
       private static bool xl_lock_r =false;

       /// <summary>
       /// ����д����  ����ʱ ����������д����
       /// </summary>
       private static bool xl_lock_w = false;

      

       /// <summary>
       /// ��ʼ������ϵͳ
       /// </summary>
       /// <param name="path"></param>
       public static void Init(string path)
       {

           //����ϵͳ
           xl_lock_r = true;
           //����ϵͳ
           xl_lock_w = true;
           //��ʼ��ϵͳ
           myFS.InitData(path);

           //д�����
           xl_lock_r = false;
           //д�����
           xl_lock_w = false;
       }

       /// <summary>
       /// ��ȡһ������
       /// </summary>
       /// <param name="id">����ID</param>
       /// <returns></returns>
       public static oneHtmDat GetOneDat(int id)
       {
           oneHtmDat myTmp = new oneHtmDat();

           if (xl_lock_r == true)
           {
               return myTmp;
           }

           xl_lock_r = true; //����

            myTmp = myFS.GetData(id);

           xl_lock_r = false; //����

           return myTmp;
       }

       /// <summary>
       /// д��һ������
       /// </summary>
       /// <param name="url"></param>
       /// <param name="dat"></param>
       /// <returns></returns>
       public static bool PutOneDat(string url, string dat)
       {
           //���ݴ���д����״̬ ����д��
           if (xl_lock_w == true)
           {
               return false;
           }


           //����ϵͳ
           xl_lock_w = true;

           myFS.SaveData(url, dat); 

           //д�����
           xl_lock_w = false ;

           return true;
       
       }

       /// <summary>
       /// �õ�ID �б�
       /// </summary>
       /// <returns></returns>
       public static ArrayList GetUrlList()
       {
           ArrayList nnnn = myFS.UrlNameIndex;

           return nnnn;
       }


       public static int GetFileNum()
       {

           return myFS.FileNum;
       
       }

       /// <summary>
       /// �����˳�
       /// </summary>
       public static void SaveExit()
       {

           myFS.SaveCache();
       
       }

    }
}
