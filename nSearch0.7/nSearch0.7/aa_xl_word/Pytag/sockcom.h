#ifndef    __SOCK_COMMON_H
#define    __SOCK_COMMON_H

#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <winsock2.h>

#define MAX_LINE_SIZE	1024



typedef struct tag_BufferSocket
{
	SOCKET Socket;
	char *ReadPtr;
	char ReadBuf[MAX_LINE_SIZE];
	int ReadCount;
} BufferSocket;



/***************************************************************************************************************
 sockerror 用来输出网络函数的错误信息
***************************************************************************************************************/
void sockerror(const char *format, ...);

/***************************************************************************************************************
 Constructsockaddr 用来从根据地址strAddr和端口strPort自动构造好一个地址机构
 ***************************************************************************************************************/
int ConstructSockaddr(SOCKADDR_IN* paddr_in,char * strAddr,char * strPort);

/***************************************************************************************************************
  WriteSocket 用来从网络中写n个字节
 ***************************************************************************************************************/
int WriteSocket(SOCKET s,char FAR *buf,int n);

void InitBufferSocket(BufferSocket* pSock);
int ReadCharFromBufferSocket(BufferSocket* pSock,char *ptr);
int	ReadLineFromBufferSocket(BufferSocket*,char* szBuf,int nBufSize);

#endif   //		__SOCK_COMMON_H









#define MAX_LINE_SIZE	1024

/***************************************************************************************************************
 sockerror 用来输出网络函数的错误信息
***************************************************************************************************************/
void sockerror(const char *format, ...)
{
	int errno;
	va_list	args;

	va_start(args, format);
	fprintf(stderr, format, args);
	va_end(args);

	errno = WSAGetLastError();

	switch (errno)
	{
		case WSAEADDRINUSE:
			fprintf(stderr,"WSAEADDRINUSE:The specified address is already in use.");
			break;
		case WSAEADDRNOTAVAIL:
			fprintf(stderr,"WSAEADDRNOTAVAIL:The specified address is not available from the local machine.");
			break;
		case WSAEAFNOSUPPORT:
			fprintf(stderr,"WSAEAFNOSUPPORT:Addresses in the specified family cannot be used with this socket.");
			break;
		case WSAECONNREFUSED:
			fprintf(stderr,"WSAECONNREFUSED:The attempt to connect was forcefully rejected.");
			break;
		case WSAEDESTADDRREQ:
			fprintf(stderr,"WSAEDESTADDRREQ:A destination address is required.");
			break;
		case WSAEFAULT:
			fprintf(stderr,"WSAEFAULT:The lpSockAddrLen argument is incorrect.");
			break;
		case WSAEINVAL:
			fprintf(stderr,"WSAEINVAL:The socket is already bound to an address.");
			break;
		case WSAEISCONN:
			fprintf(stderr,"WSAEISCONN:The socket is already connected.");
			break;
		case WSAEMFILE:
			fprintf(stderr,"WSAEMFILE:No more file descriptors are available.");
			break;
		case WSAENETUNREACH:
			fprintf(stderr,"WSAENETUNREACH:The network cannot be reached from this host at this time.");
			break;
		case WSAENOBUFS:
			fprintf(stderr,"WSAENOBUFS:No buffer space is available. The socket cannot be connected.");
			break;
		case WSAENOTCONN:
			fprintf(stderr,"WSAENOTCONN:The socket is not connected.");
			break;
		case WSAENOTSOCK:
			fprintf(stderr,"WSAENOTSOCK:The descriptor is a file, not a socket.");
			break;
		case WSAETIMEDOUT:
			fprintf(stderr,"WSAETIMEDOUT:The attempt to connect timed out without establishing a connection. ");
			break;
		default:
			fprintf(stderr,"WSAEError: Unknown! ");
			break;	
	}
	fprintf(stderr,"\n");
}

/***************************************************************************************************************
 Constructsockaddr 用来从根据地址strAddr和端口strPort自动构造好一个地址机构
 他会自动判断strAddr是域名还是IP地址，然后进行相应转换
 ***************************************************************************************************************/
int ConstructSockaddr(SOCKADDR_IN* paddr_in,char * strAddr,char * strPort)
{
	unsigned short port;
	struct hostent* phostent;

	port = atoi(strPort);
	if (port == 0)
		return 0;

	paddr_in->sin_family = AF_INET;
	paddr_in->sin_port = htons(port);   
	paddr_in->sin_addr.s_addr = inet_addr(strAddr);
	
	if(paddr_in->sin_addr.s_addr != INADDR_NONE)
		return 1;

	phostent = gethostbyname(strAddr);
	if (phostent == NULL)
	{
		sockerror("Resove name %s error!",strAddr);
		return 0;
	}

	paddr_in->sin_addr = *((IN_ADDR*) phostent->h_addr);
	if(paddr_in->sin_addr.s_addr != INADDR_NONE)
		return 1;
	
	return 0;
}

/***************************************************************************************************************
  WriteSocket 用来从网络中写n个字节
 ***************************************************************************************************************/
int WriteSocket(SOCKET s,char FAR *buf,int n)
{
	int count = 0;
	int sc;
	while(count < n)
	{
		sc = send(s,buf + count,n - count,0);
		if(sc < 0)
			return sc;
		if(sc == 0)
			Sleep(100);
		count += sc;
	}
	return count;
}


void InitBufferSocket(BufferSocket* pSock)
{
	pSock->ReadCount = 0;
}

/***************************************************************************************************************
  readline 用来从网络中读取一行,先将数据读取到Buffer中，再从Buffer中读取一行
 ***************************************************************************************************************/
int ReadCharFromBufferSocket(BufferSocket* pSock,char *ptr)
{
	if(pSock->ReadCount <= 0)
	{
		pSock->ReadCount = recv(pSock->Socket,pSock->ReadBuf,MAX_LINE_SIZE,0);
		if(pSock->ReadCount <= 0)		// EOF or SOCKET_ERROR
			return pSock->ReadCount;
		
		pSock->ReadPtr = pSock->ReadBuf;
	}

	pSock->ReadCount--;
	*ptr = *pSock->ReadPtr++;
	return 1;
}

/***************************************************************************************************************
  readline 用来从网络中读取一行,先将数据读取到Buffer中，再从Buffer中读取一行
 ***************************************************************************************************************/
int	ReadLineFromBufferSocket(BufferSocket* pSock,char* szBuf,int nBufSize)
{
	char c,*ptr;
	int n,rc;

	ptr = szBuf;
	for(n = 1;n < nBufSize;n++)
	{
		rc = ReadCharFromBufferSocket(pSock,&c);
		if(rc == 1)
		{
			*ptr++ = c;
			if(c == '\n')
				break;
		}
		else		// EOF or SOCKET_ERROR
		{
			if(n==1)  // no data read
				return 0;
			else
				break;
		}
	}

	ptr--;
	*ptr = '\0';
	return n;
}



