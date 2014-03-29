

 //  宁夏大学-张冬-BLOG  http://blog.163.com/zd4004/
 
 
 //  在ICTCLAS 基础上修改为ActiceX   因为主要代码不是我的  所以良心深感不安  但是 这个对于国内整个行业来说是重要的
 
 //  希望大家依照 ICTCLAS 原来的许可进行研究  用到商业用途的话 请先征求 ICTCLAS 开发者许可 
 
 //        宁夏大学  张冬 2007.4.7
 
 
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


// stdafx.h : 标准系统包含文件的包含文件，
// 或是常用但不常更改的项目特定的包含文件
//
  // CResult *m_Result=new CResult();
 
  CResult m_Result ;
// TODO: 在此处引用程序要求的附加头文件
// ClientServ.cpp : 定义控制台应用程序的入口点。
//

bool q=false;

//0 空闲
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

	m_Result.m_nOutputFormat=0;  // 北大  973   XML
	m_Result.m_nOperateType=2;   //标注  分割  1级   2级


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