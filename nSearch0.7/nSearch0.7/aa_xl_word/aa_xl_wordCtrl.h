#pragma once

// aa_xl_wordCtrl.h : Caa_xl_wordCtrl ActiveX 控件类的声明。


// Caa_xl_wordCtrl : 有关实现的信息，请参阅 aa_xl_wordCtrl.cpp。

class Caa_xl_wordCtrl : public COleControl
{
	DECLARE_DYNCREATE(Caa_xl_wordCtrl)

// 构造函数
public:
	Caa_xl_wordCtrl();

// 重写
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();

    virtual void   Replace(char   *strIn,   char   *   f_c,   char   *   r_c);

// 实现
protected:
	~Caa_xl_wordCtrl();

	DECLARE_OLECREATE_EX(Caa_xl_wordCtrl)    // 类工厂和 guid
	DECLARE_OLETYPELIB(Caa_xl_wordCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(Caa_xl_wordCtrl)     // 属性页 ID
	DECLARE_OLECTLTYPE(Caa_xl_wordCtrl)		// 类型名称和杂项状态

// 消息映射
	DECLARE_MESSAGE_MAP()


afx_msg BSTR aa_X_word_ocx(BSTR a) ;
// 调度映射
	DECLARE_DISPATCH_MAP()



	afx_msg void AboutBox();

// 事件映射
	DECLARE_EVENT_MAP()

// 调度和事件 ID
public:
	enum {
	};
};

