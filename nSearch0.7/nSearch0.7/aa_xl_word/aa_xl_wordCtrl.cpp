

 //  宁夏大学-张冬-BLOG  http://blog.163.com/zd4004/
 
 
 //  在ICTCLAS 基础上修改为ActiceX   因为主要代码不是我的  所以良心深感不安  但是 这个对于国内整个行业来说是重要的
 
 //  希望大家依照 ICTCLAS 原来的许可进行研究  用到商业用途的话 请先征求 ICTCLAS 开发者许可 
 
 //        宁夏大学  张冬 2007.4.7
 
 
// aa_xl_wordCtrl.cpp : Caa_xl_wordCtrl ActiveX 控件类的实现。

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



// 消息映射

BEGIN_MESSAGE_MAP(Caa_xl_wordCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()



// 调度映射

BEGIN_DISPATCH_MAP(Caa_xl_wordCtrl, COleControl)
	DISP_FUNCTION(Caa_xl_wordCtrl, "aa_X_word_ocx", aa_X_word_ocx, VT_BSTR,  VTS_BSTR)
	DISP_FUNCTION_ID(Caa_xl_wordCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()



// 事件映射

BEGIN_EVENT_MAP(Caa_xl_wordCtrl, COleControl)
END_EVENT_MAP()



// 属性页

// TODO: 按需要添加更多属性页。请记住增加计数!
BEGIN_PROPPAGEIDS(Caa_xl_wordCtrl, 1)
	PROPPAGEID(Caa_xl_wordPropPage::guid)
END_PROPPAGEIDS(Caa_xl_wordCtrl)



// 初始化类工厂和 guid

IMPLEMENT_OLECREATE_EX(Caa_xl_wordCtrl, "AA_XL_WORD.aa_xl_wordCtrl.1",
	0x39655597, 0xbed9, 0x4ee9, 0x99, 0x70, 0x65, 0xf8, 0x11, 0xe8, 0x14, 0x4a)



// 键入库 ID 和版本

IMPLEMENT_OLETYPELIB(Caa_xl_wordCtrl, _tlid, _wVerMajor, _wVerMinor)



// 接口 ID

const IID BASED_CODE IID_Daa_xl_word =
		{ 0x6EEC6F7E, 0xCB2E, 0x45CB, { 0xAC, 0xD, 0x6F, 0xB3, 0x7C, 0x79, 0x25, 0xD4 } };
const IID BASED_CODE IID_Daa_xl_wordEvents =
		{ 0xF9498B14, 0xEB18, 0x4E58, { 0xA0, 0x30, 0x49, 0x52, 0x51, 0xC8, 0xAD, 0xE0 } };



// 控件类型信息

static const DWORD BASED_CODE _dwaa_xl_wordOleMisc =
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(Caa_xl_wordCtrl, IDS_AA_XL_WORD, _dwaa_xl_wordOleMisc)



// Caa_xl_wordCtrl::Caa_xl_wordCtrlFactory::UpdateRegistry -
// 添加或移除 Caa_xl_wordCtrl 的系统注册表项

BOOL Caa_xl_wordCtrl::Caa_xl_wordCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: 验证您的控件是否符合单元模型线程处理规则。
	// 有关更多信息，请参考 MFC 技术说明 64。
	// 如果您的控件不符合单元模型规则，则
	// 必须修改如下代码，将第六个参数从
	// afxRegApartmentThreading 改为 0。

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



// Caa_xl_wordCtrl::Caa_xl_wordCtrl - 构造函数

Caa_xl_wordCtrl::Caa_xl_wordCtrl()
{
	InitializeIIDs(&IID_Daa_xl_word, &IID_Daa_xl_wordEvents);
	// TODO: 在此初始化控件的实例数据。
}



// Caa_xl_wordCtrl::~Caa_xl_wordCtrl - 析构函数

Caa_xl_wordCtrl::~Caa_xl_wordCtrl()
{
	// TODO: 在此清理控件的实例数据。
}



// Caa_xl_wordCtrl::OnDraw - 绘图函数

void Caa_xl_wordCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

	// TODO: 用您自己的绘图代码替换下面的代码。
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(WHITE_BRUSH)));
	pdc->Ellipse(rcBounds);
}



// Caa_xl_wordCtrl::DoPropExchange - 持久性支持

void Caa_xl_wordCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: 为每个持久的自定义属性调用 PX_ 函数。
}



// Caa_xl_wordCtrl::GetControlFlags -
// 自定义 MFC 的 ActiveX 控件实现的标志。
//
DWORD Caa_xl_wordCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();


	// 不用创建窗口即可激活控件。
	// TODO: 编写控件的消息处理程序时，在使用
	//		m_hWnd 成员变量之前应首先检查它的值是否
	//		非空。
	dwFlags |= windowlessActivate;
	return dwFlags;
}



// Caa_xl_wordCtrl::OnResetState - 将控件重置为默认状态

void Caa_xl_wordCtrl::OnResetState()
{
	COleControl::OnResetState();  // 重置 DoPropExchange 中找到的默认值

	// TODO: 在此重置任意其他控件状态。
}



// Caa_xl_wordCtrl::AboutBox - 向用户显示“关于”框

void Caa_xl_wordCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_AA_XL_WORD);
	dlgAbout.DoModal();
}



// Caa_xl_wordCtrl 消息处理程序
//BSTR COcxtestCtrl::testocx(BSTR a) 
BSTR  Caa_xl_wordCtrl::aa_X_word_ocx(BSTR a) 
{


            /*aa慡0
ab扡1
ac捡2
ad摡3
ae敡4
af晡5
ag条6
ah桡7
ai楡8
aj橡9
zz 空z
             * */

//截取  每3位一段




 char* ax = _com_util::ConvertBSTRToString(a); 

char* one1 ="慡"; char* onea1 ="0"; 
 Replace(ax,one1,onea1);

 char* one2 ="扡"; char* onea2 ="1"; 
 Replace(ax,one2,onea2);

 char* one3 ="捡"; char* onea3 ="2"; 
 Replace(ax,one3,onea3);

 char* one4 ="摡"; char* onea4 ="3"; 
 Replace(ax,one4,onea4);

 char* one5 ="敡"; char* onea5 ="4"; 
 Replace(ax,one5,onea5);

 char* one6 ="晡"; char* onea6 ="5"; 
 Replace(ax,one6,onea6);

 char* one7 ="条"; char* onea7 ="6"; 
 Replace(ax,one7,onea7);

 char* one8 ="桡"; char* onea8 ="7"; 
 Replace(ax,one8,onea8);

 char* one9 ="楡"; char* onea9 ="8"; 
 Replace(ax,one9,onea9);

 char* one10 ="橡"; char* onea10 ="9"; 
 Replace(ax,one10,onea10);

 char* one11 ="空"; char* onea11 ="z"; 
 Replace(ax,one11,onea11);

// 获得字符数
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