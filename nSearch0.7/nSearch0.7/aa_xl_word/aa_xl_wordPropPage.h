#pragma once

// aa_xl_wordPropPage.h : Caa_xl_wordPropPage ����ҳ���������


// Caa_xl_wordPropPage : �й�ʵ�ֵ���Ϣ������� aa_xl_wordPropPage.cpp��

class Caa_xl_wordPropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(Caa_xl_wordPropPage)
	DECLARE_OLECREATE_EX(Caa_xl_wordPropPage)

// ���캯��
public:
	Caa_xl_wordPropPage();

// �Ի�������
	enum { IDD = IDD_PROPPAGE_AA_XL_WORD };

// ʵ��
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ��Ϣӳ��
protected:
	DECLARE_MESSAGE_MAP()
};

