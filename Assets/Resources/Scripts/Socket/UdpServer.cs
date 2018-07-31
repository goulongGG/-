using UnityEngine;  
using System.Collections;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  
using System;
  
public class UdpServer 
{  
    //以下默认都是私有的成员  
    Socket socket; //目标socket  
    EndPoint clientEnd; //客户端  
    IPEndPoint ipEnd; //侦听端口  
    string recvStr; //接收的字符串  
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
    int recvLen; //接收的数据长度  
    Thread connectThread; //连接线程  
  
    public void InitSocket(int port)  
    {  
          
        ipEnd = new IPEndPoint(IPAddress.Any, port);  

        socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);  

        socket.Bind(ipEnd);  
   
        IPEndPoint sender = new IPEndPoint(IPAddress.Any,0);  
        clientEnd = (EndPoint)sender;  
        Debug.Log("server had Inited and the port: " + port);  

        connectThread = new Thread(new ThreadStart(SocketReceive));  
        connectThread.Start();  
    }  

    /// <summary>
	/// 服务器发送数据
	/// </summary>
    private void SocketSend(string sendStr)  
    {  
        sendData = new byte[1024];  
        sendData =  DataManager.Instance.HexStringToByteArray(sendStr);  
        socket.SendTo(sendData,sendData.Length,SocketFlags.None,clientEnd);  
    }  
    
  
    /// <summary>
	/// 服务器接收数据
	/// </summary>
    private void SocketReceive()  
    {  
        //进入接收循环  
        while(true)  
        {  
            //对data清零  
            recvData = new byte[1024];  
            //获取客户端，获取客户端数据，用引用给客户端赋值  
            recvLen = socket.ReceiveFrom(recvData,ref clientEnd);  
            Debug.Log("message from: "+clientEnd.ToString()); //打印客户端信息    

            recvStr = DataManager.Instance.ByteArrayToHexStringNoBlank(recvData);
            Debug.Log(recvStr);  
            
            byte[] data = new byte[33];
            Array.Copy(recvData,0,data,0,33);//复制0-32字节

            Debug.Log("校验和：" + DataManager.Instance.GetXOR(data));
            Debug.Log("33字节：" + recvData[33]);

            //if(DataManager.Instance.GetXOR(data) != recvData[33]) continue; //如果校验你们校验正确的话，就使用这行代码

            DataManager.Instance.SolveByteToData(data);
           
        }  
    }

  
  
    //连接关闭  
    public void SocketQuit()  
    {  
        //关闭线程  
        if(connectThread != null)  
        {  
            connectThread.Interrupt();  
            connectThread.Abort();  
        }  
        //最后关闭socket  
        if(socket != null)  
            socket.Close();  
        Debug.Log("disconnect");  
    }  
    
}  