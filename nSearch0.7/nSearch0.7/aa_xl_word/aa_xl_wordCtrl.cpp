

 //  ���Ĵ�ѧ-�Ŷ�-BLOG  http://blog.163.com/zd4004/
 
 
 //  ��ICTCLAS �������޸�ΪActiceX   ��Ϊ��Ҫ���벻���ҵ�  ����������в���  ���� ������ڹ���������ҵ��˵����Ҫ��
 
 //  ϣ��������� ICTCLAS ԭ������ɽ����о�  �õ���ҵ��;�Ļ� �������� ICTCLAS ��������� 
 
 //        ���Ĵ�ѧ  �Ŷ� 2007.4.7
 
 
// aa_xl_wordCtrl.cpp : Caa_xl_wordCtrl ActiveX �ؼ����ʵ�֡�

#include "stdafx.h"
#include "aa_xl_word.h"
#include "aa_xl_wordCtrl.h"
#include "aa_xl_wordPropPage.h"

#include <comutil.h>
  #include <comdef.h>

   #include <stdio.h>
#include <locale.h>
#include <windows.h>

#include "MRST_NEW.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(Caa_xl_wordCtrl, COleControl)



// ��Ϣӳ��

BEGIN_MESSAGE_MAP(Caa_xl_wordCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()



// ����ӳ��

BEGIN_DISPATCH_MAP(Caa_xl_wordCtrl, COleControl)
	DISP_FUNCTION(Caa_xl_wordCtrl, "aa_X_word_ocx", aa_X_word_ocx, VT_BSTR,  VTS_BSTR)
	DISP_FUNCTION_ID(Caa_xl_wordCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()



// �¼�ӳ��

BEGIN_EVENT_MAP(Caa_xl_wordCtrl, COleControl)
END_EVENT_MAP()



// ����ҳ

// TODO: ����Ҫ��Ӹ�������ҳ�����ס���Ӽ���!
BEGIN_PROPPAGEIDS(Caa_xl_wordCtrl, 1)
	PROPPAGEID(Caa_xl_wordPropPage::guid)
END_PROPPAGEIDS(Caa_xl_wordCtrl)



// ��ʼ���๤���� guid

IMPLEMENT_OLECREATE_EX(Caa_xl_wordCtrl, "AA_XL_WORD.aa_xl_wordCtrl.1",
	0x39655597, 0xbed9, 0x4ee9, 0x99, 0x70, 0x65, 0xf8, 0x11, 0xe8, 0x14, 0x4a)



// ����� ID �Ͱ汾

IMPLEMENT_OLETYPELIB(Caa_xl_wordCtrl, _tlid, _wVerMajor, _wVerMinor)



// �ӿ� ID

const IID BASED_CODE IID_Daa_xl_word =
		{ 0x6EEC6F7E, 0xCB2E, 0x45CB, { 0xAC, 0xD, 0x6F, 0xB3, 0x7C, 0x79, 0x25, 0xD4 } };
const IID BASED_CODE IID_Daa_xl_wordEvents =
		{ 0xF9498B14, 0xEB18, 0x4E58, { 0xA0, 0x30, 0x49, 0x52, 0x51, 0xC8, 0xAD, 0xE0 } };



// �ؼ�������Ϣ

static const DWORD BASED_CODE _dwaa_xl_wordOleMisc =
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(Caa_xl_wordCtrl, IDS_AA_XL_WORD, _dwaa_xl_wordOleMisc)



// Caa_xl_wordCtrl::Caa_xl_wordCtrlFactory::UpdateRegistry -
// ��ӻ��Ƴ� Caa_xl_wordCtrl ��ϵͳע�����

BOOL Caa_xl_wordCtrl::Caa_xl_wordCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: ��֤���Ŀؼ��Ƿ���ϵ�Ԫģ���̴߳������
	// �йظ�����Ϣ����ο� MFC ����˵�� 64��
	// ������Ŀؼ������ϵ�Ԫģ�͹�����
	// �����޸����´��룬��������������
	// afxRegApartmentThreading ��Ϊ 0��

	if (bRegister)
		return AfxOleRegisterControlClass(
			AfxGetInstanceHandle(),
			m_clsid,
			m_lpszProgID,
			IDS_AA_XL_WORD,
			IDB_AA_XL_WORD,
			afxRegApartmentThreading,
			_dwaa_xl_wordOleMisc,
			_tlid,
			_wVerMajor,
			_wVerMinor);
	else
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}



// Caa_xl_wordCtrl::Caa_xl_wordCtrl - ���캯��

Caa_xl_wordCtrl::Caa_xl_wordCtrl()
{
	InitializeIIDs(&IID_Daa_xl_word, &IID_Daa_xl_wordEvents);
	// TODO: �ڴ˳�ʼ���ؼ���ʵ�����ݡ�
}



// Caa_xl_wordCtrl::~Caa_xl_wordCtrl - ��������

Caa_xl_wordCtrl::~Caa_xl_wordCtrl()
{
	// TODO: �ڴ�����ؼ���ʵ�����ݡ�
}



// Caa_xl_wordCtrl::OnDraw - ��ͼ����

void Caa_xl_wordCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

	// TODO: �����Լ��Ļ�ͼ�����滻����Ĵ��롣
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(WHITE_BRUSH)));
	pdc->Ellipse(rcBounds);
}



// Caa_xl_wordCtrl::DoPropExchange - �־���֧��

void Caa_xl_wordCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: Ϊÿ���־õ��Զ������Ե��� PX_ ������
}



// Caa_xl_wordCtrl::GetControlFlags -
// �Զ��� MFC �� ActiveX �ؼ�ʵ�ֵı�־��
//
DWORD Caa_xl_wordCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();


	// ���ô������ڼ��ɼ���ؼ���
	// TODO: ��д�ؼ�����Ϣ�������ʱ����ʹ��
	//		m_hWnd ��Ա����֮ǰӦ���ȼ������ֵ�Ƿ�
	//		�ǿա�
	dwFlags |= windowlessActivate;
	return dwFlags;
}



// Caa_xl_wordCtrl::OnResetState - ���ؼ�����ΪĬ��״̬

void Caa_xl_wordCtrl::OnResetState()
{
	COleControl::OnResetState();  // ���� DoPropExchange ���ҵ���Ĭ��ֵ

	// TODO: �ڴ��������������ؼ�״̬��
}



// Caa_xl_wordCtrl::AboutBox - ���û���ʾ�����ڡ���

void Caa_xl_wordCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_AA_XL_WORD);
	dlgAbout.DoModal();
}



// Caa_xl_wordCtrl ��Ϣ�������
//BSTR COcxtestCtrl::testocx(BSTR a) 
BSTR  Caa_xl_wordCtrl::aa_X_word_ocx(BSTR a) 
{


            /*aa�S0
ab�L1
ac��2
ad��3
ae��4
af��5
ag��6
ah��7
ai�@8
aj��9
zz ��z
             * */

//��ȡ  ÿ3λһ��




 char* ax = _com_util::ConvertBSTRToString(a); 

char* one1 ="�S"; char* onea1 ="0"; 
 Replace(ax,one1,onea1);

 char* one2 ="�L"; char* onea2 ="1"; 
 Replace(ax,one2,onea2);

 char* one3 ="��"; char* onea3 ="2"; 
 Replace(ax,one3,onea3);

 char* one4 ="��"; char* onea4 ="3"; 
 Replace(ax,one4,onea4);

 char* one5 ="��"; char* onea5 ="4"; 
 Replace(ax,one5,onea5);

 char* one6 ="��"; char* onea6 ="5"; 
 Replace(ax,one6,onea6);

 char* one7 ="��"; char* onea7 ="6"; 
 Replace(ax,one7,onea7);

 char* one8 ="��"; char* onea8 ="7"; 
 Replace(ax,one8,onea8);

 char* one9 ="�@"; char* onea9 ="8"; 
 Replace(ax,one9,onea9);

 char* one10 ="��"; char* onea10 ="9"; 
 Replace(ax,one10,onea10);

 char* one11 ="��"; char* onea11 ="z"; 
 Replace(ax,one11,onea11);

// ����ַ���
   int xInt = SysStringLen(a);

 byte *strIt   =   new   byte[xInt/3];   

 int strItone = 0;

 char *UII ;

while(*ax!='z')
{
	
  // *ax;
char x1 = *ax;
 ax ++ ;
int ix1=(int)x1-48;

char x2 = *ax;
 ax ++ ;
 int ix2=(int)x2-48;

char x3 = *ax;
 ax ++ ;
int ix3=(int)x3-48;

  int n; 

 n = ix1*100+ix2*10+ix3;

//char It =(char)n;

 strIt[strItone] = n;

 strItone  = strItone +1;

}

strIt[strItone] = 0;


CString   tt;   
 
  tt.Format(_T("%s"),strIt);   
  char   *kk=tt.GetBuffer(tt.GetLength());   

 BSTR bstrText = 	 CString(kk).AllocSysString();

 // return bstrText;

 // delete[] strIt; 

 BSTR bstrText2 = MRST_NEW::GetXowrdIt( bstrText);

        return bstrText2;


}
         


void  Caa_xl_wordCtrl::Replace(char   *strIn,   char   *   f_c,   char   *   r_c){  

  int   sl   =   (int)strlen(strIn),   fl   =   (int)strlen(f_c),   rl   =   (int)strlen(r_c),   pl,   tl;  
  char   *p   =   strstr(strIn,   f_c);  
  while(p){  
  pl   =   (int)strlen(p);  
  tl   =   sl   -   pl;  
  memset(strIn+tl,   0x0,   fl);  
  memmove(strIn+tl+rl,   &strIn[tl+fl],   pl);  
  memcpy(strIn+tl,   r_c,   rl);  
  strIn   +=   tl   +   rl;  
  sl   =   (int)strlen(strIn);  
  p   =   strstr(strIn,   f_c);  
  }  

  } 