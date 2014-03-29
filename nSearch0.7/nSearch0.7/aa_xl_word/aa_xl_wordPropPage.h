#pragma once

// aa_xl_wordPropPage.h : Caa_xl_wordPropPage 属性页类的声明。


// Caa_xl_wordPropPage : 有关实现的信息，请参阅 aa_xl_wordPropPage.cpp。

class Caa_xl_wordPropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(Caa_xl_wordPropPage)
	DECLARE_OLECREATE_EX(Caa_xl_wordPropPage)

// 构造函数
public:
	Caa_xl_wordPropPage();

// 对话框数据
	enum { IDD = IDD_PROPPAGE_AA_XL_WORD };

// 实现
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 消息映射
protected:
	DECLARE_MESSAGE_MAP()
};

