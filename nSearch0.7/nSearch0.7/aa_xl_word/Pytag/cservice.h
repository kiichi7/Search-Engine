#include <time.h>
#include <process.h>
#include <conio.h>
#include <windows.h>

#include "StdAfx.h"
#include "sockcom.h"


#define USER_PORT 19800
#define BUFFER_SIZE 100

unsigned __stdcall HttpThread(void *  p);
unsigned long hThreadHandle;
unsigned uThreadID;

long GetIt(char *word,char *back);                           // �������� 
int mainListen();                                 //����������
void GetNewWordNetCN(BufferSocket BufSock);       //�õ���������   ��������  ��������


int mainListen()
{
	WSADATA wsaData;
	SOCKET serverSocket,newConnection;
	SOCKADDR_IN serverAddr,clientAddr;

	int Ret,clientAddrLen;

	//��ʼ��Winsock Dll
	if((Ret=WSAStartup(MAKEWORD(2,2),&wsaData))!=0)
	{
		printf("WSAStartup failed with error %d\n", Ret);
		return 0;
	}

	//������������socket
	if((serverSocket=socket(AF_INET, SOCK_STREAM, 0))==INVALID_SOCKET)
	{
		printf("Create socket error\n");
		WSACleanup();
		return 0;
	}

	//����������SOCKADDR_IN�ṹ
	serverAddr.sin_family=AF_INET;
	serverAddr.sin_addr.s_addr=htonl(INADDR_ANY);
	serverAddr.sin_port=htons(USER_PORT);
    
	//����������socket��ָ��IP��ַ�Ͷ˿ڰ�
	if(bind(serverSocket,(SOCKADDR *)&serverAddr,sizeof(serverAddr))==SOCKET_ERROR)
	{
		int temp;
		temp=WSAGetLastError();
		printf("Bind error!\n");
		closesocket(serverSocket);
		WSACleanup();
		return 0;
	}

	//ָ����������serverSocketΪ����ģʽ
	if(listen(serverSocket,5)!=0)
	{
		printf("Listen error!\n");
		closesocket(serverSocket);
		WSACleanup();
		return 0;
	}

	printf("Init...!\n");
	clientAddrLen=sizeof(clientAddr);

	//��ȡ����

  
	//while(!kbhit())
	while(true)
	{
		if((newConnection=accept(serverSocket,(sockaddr FAR*)&clientAddr,&clientAddrLen))==INVALID_SOCKET)
		{
			printf("Connect failed.Please try again!\n");
			continue;
		}
		hThreadHandle = _beginthreadex(NULL , 0 , HttpThread , (void *)&newConnection , 0 , &uThreadID);
		CloseHandle((HANDLE) hThreadHandle);
	}
	closesocket(serverSocket);
	WSACleanup();
	return 0;
}


unsigned __stdcall HttpThread(void *  p)
{
	BufferSocket BufSock;
	InitBufferSocket(&BufSock);
	BufSock.Socket = *((SOCKET *) p);
	GetNewWordNetCN(BufSock);
	closesocket(BufSock.Socket);
	return 0;
}

//�õ��ִʽ��
void GetNewWordNetCN(BufferSocket BufSock)    
{
	char buf[BUFFER_SIZE],*NotFind="null";
	int Ret;
	Ret = ReadLineFromBufferSocket(&BufSock , buf , 1024);     //��ȡ����
	buf[Ret-2]='\0';
    
	//printf("%s\n",buf);
	char *backData;   //��������

	long index=GetIt(buf,backData);
	//printf("SSS:");
    //printf("%s\n",MwordNwt[index+1]);

	if(index!=-999)
	{
		if((Ret=WriteSocket(BufSock.Socket,backData,strlen(backData))) < 0 )
			printf("Send failed.You can try again!\n");
	}
	else
	{
		if((Ret=WriteSocket(BufSock.Socket,NotFind,strlen(NotFind))) < 0 )
			printf("Send failed.You can try again!\n");
	}
	return;
}

//�õ��ִʽ��
long GetIt(char *word,char *back)                 
{


    
    return -999;
}




