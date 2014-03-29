

 //  宁夏大学-张冬-BLOG  http://blog.163.com/zd4004/
 
 
 //  在ICTCLAS 基础上修改为ActiceX   因为主要代码不是我的  所以良心深感不安  但是 这个对于国内整个行业来说是重要的
 
 //  希望大家依照 ICTCLAS 原来的许可进行研究  用到商业用途的话 请先征求 ICTCLAS 开发者许可 
 
 //        宁夏大学  张冬 2007.4.7
 
 
// aa_xl_word.cpp : Caa_xl_wordApp 和 DLL 注册的实现。

#include "stdafx.h"
#include "aa_xl_word.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


Caa_xl_wordApp theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0x5DFAF7B7, 0xE60E, 0x4A7E, { 0xAB, 0xB1, 0x13, 0x81, 0xAD, 0x0, 0xEE, 0x2A } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;



// Caa_xl_wordApp::InitInstance - DLL 初始化

BOOL Caa_xl_wordApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: 在此添加您自己的模块初始化代码。
	}

	return bInit;
}



// Caa_xl_wordApp::ExitInstance - DLL 终止

int Caa_xl_wordApp::ExitInstance()
{
	// TODO: 在此添加您自己的模块终止代码。

	return COleControlModule::ExitInstance();
}



// DllRegisterServer - 将项添加到系统注册表

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - 将项从系统注册表中移除

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
