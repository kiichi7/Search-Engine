#include "..\\Utility\\Utility.h"

#define WORD_MAXLENGTH 100
#define CHANGETIME 100

#define  T_SINGLE  20//SINGLE byte
#define  T_DELIMITER T_SINGLE+1//delimiter
#define  T_CHINESE   T_SINGLE+2//Chinese Char
#define  T_NUM       T_SINGLE+3//数字
#define  T_OTHER     T_SINGLE+4//Other

class CNorm
{
public:
	CNorm();
	virtual ~CNorm();
	 char NumOut[10][3];
	 int ctype[CHANGETIME];
	 bool IsAllAlabNum(char *sWord);//判断是否全是阿拉伯数字
	 char sSameTypeString[CHANGETIME][WORD_MAXLENGTH];//存储一个词的子段
	 bool SentenceNorm(char* senten, char* ResultSen);//对句子规范化
	 bool Processing(char* NumWord,int* time);//对数词进行分段处理
	 bool ReadWord(CString s,char* word,char* tag,int* lenth);//从句子种读取一个词
	 bool ChangeWord(char* sWord,char* sTag,char* sResult,bool type);//对一个数词进行码读或值读处理
	 bool ConvertNumber(char* sWord,char* Result,bool type);//对一个数字段仅仅包含数字，做码读或值读处理
	 int GetCharType(unsigned char *sChar);//得到一个字符串的类型
	 bool GetSameType(char* sChar,int* lenth,int* type);//得到相同类型的字符串长度。

};