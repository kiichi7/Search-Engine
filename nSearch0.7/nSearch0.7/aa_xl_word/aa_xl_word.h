#pragma once

// aa_xl_word.h : aa_xl_word.DLL ����ͷ�ļ�

#if !defined( __AFXCTL_H__ )
#error "�ڰ������ļ�֮ǰ������afxctl.h��"
#endif

#include "resource.h"       // ������


// Caa_xl_wordApp : �й�ʵ�ֵ���Ϣ������� aa_xl_word.cpp��

class Caa_xl_wordApp : public COleControlModule
{
public:
	BOOL InitInstance();
	int ExitInstance();
};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;

