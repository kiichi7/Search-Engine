

 //  ���Ĵ�ѧ-�Ŷ�-BLOG  http://blog.163.com/zd4004/
 
 
 //  ��ICTCLAS �������޸�ΪActiceX   ��Ϊ��Ҫ���벻���ҵ�  ����������в���  ���� ������ڹ���������ҵ��˵����Ҫ��
 
 //  ϣ��������� ICTCLAS ԭ������ɽ����о�  �õ���ҵ��;�Ļ� �������� ICTCLAS ��������� 
 
 //        ���Ĵ�ѧ  �Ŷ� 2007.4.7
 
 
// aa_xl_word.cpp : Caa_xl_wordApp �� DLL ע���ʵ�֡�

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



// Caa_xl_wordApp::InitInstance - DLL ��ʼ��

BOOL Caa_xl_wordApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: �ڴ�������Լ���ģ���ʼ�����롣
	}

	return bInit;
}



// Caa_xl_wordApp::ExitInstance - DLL ��ֹ

int Caa_xl_wordApp::ExitInstance()
{
	// TODO: �ڴ�������Լ���ģ����ֹ���롣

	return COleControlModule::ExitInstance();
}



// DllRegisterServer - ������ӵ�ϵͳע���

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - �����ϵͳע������Ƴ�

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
