

 //  ���Ĵ�ѧ-�Ŷ�-BLOG  http://blog.163.com/zd4004/
 
 
 //  ��ICTCLAS �������޸�ΪActiceX   ��Ϊ��Ҫ���벻���ҵ�  ����������в���  ���� ������ڹ���������ҵ��˵����Ҫ��
 
 //  ϣ��������� ICTCLAS ԭ������ɽ����о�  �õ���ҵ��;�Ļ� �������� ICTCLAS ��������� 
 
 //        ���Ĵ�ѧ  �Ŷ� 2007.4.7
 
 
#include "stdafx.h"
#include "MRST_NEW.h"


// tagDlg.cpp : implementation file
//
//#include "stdafx.h"
//#include "tag.h"
#include "tagDlg.h"
#include "Utility\\Utility.h"

//#include "Pytag\\cservice.h"
//#include "Pytag\\sockcom.h"

#include <time.h>//Calculate the time
#include  <io.h>
#include  <stdio.h>
#include  <stdlib.h>

#include <winsock2.h>

#pragma once

#include <iostream>
#include <tchar.h>
#include <windows.h>
#include <process.h>

//#ifdef _DEBUG
//#define new DEBUG_NEW
//#undef THIS_FILE
//static char THIS_FILE[] = __FILE__;

//#endif
#include <comutil.h>
  #include <comdef.h>
   #include <stdio.h>
#include <locale.h>
#include <windows.h>

#define NO_FLAGS_SET 0

#define PORT (u_short) 19800
#define MAXBUFLEN 1024


// stdafx.h : ��׼ϵͳ�����ļ��İ����ļ���
// ���ǳ��õ��������ĵ���Ŀ�ض��İ����ļ�
//
  // CResult *m_Result=new CResult();
 
  CResult m_Result ;
// TODO: �ڴ˴����ó���Ҫ��ĸ���ͷ�ļ�
// ClientServ.cpp : �������̨Ӧ�ó������ڵ㡣
//

bool q=false;

//0 ����
 bool qmmm=false;

//int _tmain(int argc, _TCHAR* argv[])


MRST_NEW::MRST_NEW(void)
{
}

MRST_NEW::~MRST_NEW(void)
{
}

  BSTR   MRST_NEW::GetXowrdIt(BSTR aXX)
{


	char* ax = _com_util::ConvertBSTRToString(aXX); 

	m_Result.m_nOutputFormat=0;  // ����  973   XML
	m_Result.m_nOperateType=2;   //��ע  �ָ�  1��   2��


  char sResult[4096];

  try
  {
   //m_Result.ParagraphProcessing((char *)(LPCTSTR)recvBuf,sResult);
 m_Result.ParagraphProcessing(ax,sResult);

 BSTR bstrText = _com_util::ConvertStringToBSTR(sResult);


    return bstrText ;
  }
   catch(char* errMes)
  {

      return aXX;
   }


}