// aa_xl_wordPropPage.cpp : Caa_xl_wordPropPage ����ҳ���ʵ�֡�



 //  ���Ĵ�ѧ-�Ŷ�-BLOG  http://blog.163.com/zd4004/
 
 
 //  ��ICTCLAS �������޸�ΪActiceX   ��Ϊ��Ҫ���벻���ҵ�  ����������в���  ���� ������ڹ���������ҵ��˵����Ҫ��
 
 //  ϣ��������� ICTCLAS ԭ������ɽ����о�  �õ���ҵ��;�Ļ� �������� ICTCLAS ��������� 
 
 //        ���Ĵ�ѧ  �Ŷ� 2007.4.7
 
 



#include "stdafx.h"
#include "aa_xl_word.h"
#include "aa_xl_wordPropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(Caa_xl_wordPropPage, COlePropertyPage)



// ��Ϣӳ��

BEGIN_MESSAGE_MAP(Caa_xl_wordPropPage, COlePropertyPage)
END_MESSAGE_MAP()



// ��ʼ���๤���� guid

IMPLEMENT_OLECREATE_EX(Caa_xl_wordPropPage, "AA_XL_WORD.aa_xl_wordPropPage.1",
	0x54f07742, 0x1dd0, 0x4187, 0x83, 0x2c, 0xe, 0xc7, 0xc, 0x57, 0x16, 0x53)



// Caa_xl_wordPropPage::Caa_xl_wordPropPageFactory::UpdateRegistry -
// ��ӻ��Ƴ� Caa_xl_wordPropPage ��ϵͳע�����

BOOL Caa_xl_wordPropPage::Caa_xl_wordPropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_AA_XL_WORD_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// Caa_xl_wordPropPage::Caa_xl_wordPropPage - ���캯��

Caa_xl_wordPropPage::Caa_xl_wordPropPage() :
	COlePropertyPage(IDD, IDS_AA_XL_WORD_PPG_CAPTION)
{
}



// Caa_xl_wordPropPage::DoDataExchange - ��ҳ�����Լ��ƶ�����

void Caa_xl_wordPropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// Caa_xl_wordPropPage ��Ϣ�������
