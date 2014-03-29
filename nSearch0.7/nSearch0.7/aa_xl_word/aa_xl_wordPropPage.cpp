// aa_xl_wordPropPage.cpp : Caa_xl_wordPropPage 属性页类的实现。



 //  宁夏大学-张冬-BLOG  http://blog.163.com/zd4004/
 
 
 //  在ICTCLAS 基础上修改为ActiceX   因为主要代码不是我的  所以良心深感不安  但是 这个对于国内整个行业来说是重要的
 
 //  希望大家依照 ICTCLAS 原来的许可进行研究  用到商业用途的话 请先征求 ICTCLAS 开发者许可 
 
 //        宁夏大学  张冬 2007.4.7
 
 



#include "stdafx.h"
#include "aa_xl_word.h"
#include "aa_xl_wordPropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(Caa_xl_wordPropPage, COlePropertyPage)



// 消息映射

BEGIN_MESSAGE_MAP(Caa_xl_wordPropPage, COlePropertyPage)
END_MESSAGE_MAP()



// 初始化类工厂和 guid

IMPLEMENT_OLECREATE_EX(Caa_xl_wordPropPage, "AA_XL_WORD.aa_xl_wordPropPage.1",
	0x54f07742, 0x1dd0, 0x4187, 0x83, 0x2c, 0xe, 0xc7, 0xc, 0x57, 0x16, 0x53)



// Caa_xl_wordPropPage::Caa_xl_wordPropPageFactory::UpdateRegistry -
// 添加或移除 Caa_xl_wordPropPage 的系统注册表项

BOOL Caa_xl_wordPropPage::Caa_xl_wordPropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_AA_XL_WORD_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// Caa_xl_wordPropPage::Caa_xl_wordPropPage - 构造函数

Caa_xl_wordPropPage::Caa_xl_wordPropPage() :
	COlePropertyPage(IDD, IDS_AA_XL_WORD_PPG_CAPTION)
{
}



// Caa_xl_wordPropPage::DoDataExchange - 在页和属性间移动数据

void Caa_xl_wordPropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// Caa_xl_wordPropPage 消息处理程序
