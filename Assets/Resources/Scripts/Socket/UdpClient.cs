
 
using UnityEngine;  
using UnityEngine.UI;  

using System.Collections;  
//引入库  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  
using System;
using BSKCore;
  
public class UdpClient : MonoBehaviour  
{  
    // string editString="CCDD"; //编辑框文字   
    [SerializeField] private Text _Info;
    // [SerializeField] private Text _IP;	
	// [SerializeField] private Button _SetUpServer;
	// [SerializeField] private Button _ShutDownServer;
	[SerializeField] private Button _SendAngelData;
	[SerializeField] private InputField _SendAngleData;

	// [SerializeField] private InputField _SenAVectorData;


  
    //以下默认都是私有的成员  
    Socket socket; //目标socket  
    EndPoint serverEnd; //服务端  
    IPEndPoint ipEnd; //服务端端口  
    string recvStr; //接收的字符串  
    // string sendStr; //发送的字符串  
    byte[] recvData=new byte[1024]; //接收的数据，必须为字节  
    byte[] sendData=new byte[1024]; //发送的数据，必须为字节  
    int recvLen; //接收的数据长度  
    Thread connectThread; //连接线程  
  
    void Start () 
	{
		// _Info.text = "Start...";	
		// _IP.text = GameConst.IP + ":" + GameConst.Port;
		_SendAngelData.onClick.AddListener(SendAngelData);
        InitSocket();
	}
	// private void CallBack(Notification notific)
	// {
	// 	string messege = (string)notific.param[0];
	// 	bool isSendAngle = (bool)notific.param[1];
	// 	if(isSendAngle)
	// 	{
	// 	   string[] words = messege.Split('/');	
	// 	   _Info.text = words[0] + "角度："	+ words[1] + "˚"+ "角速度：" + words[2] + "rad/s" + "\n" ;
	// 	}
    //     else
    //     {
    //         _Info.text = messege + "\n" + _Info.text;
    //     }
	// }

	private void SendAngelData()
	{
		string data = _SendAngleData.text;
		SocketSend(data);
        Debug.Log(data);
		// ClientSeverManger.Instance.ServerSendToClient(seriesAngelData +"/" + angelVeData);
	}

    void InitSocket()  
    {  
        //定义连接的服务器ip和端口，可以是本机ip，局域网，互联网  
        ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8001);   
        //定义套接字类型,在主线程中定义  
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);  
        //定义服务端  
        IPEndPoint sender =  new IPEndPoint(IPAddress.Any,0);  
        serverEnd = (EndPoint)sender;  
        print("waiting for sending UDP dgram");  
  
        //建立初始连接，这句非常重要，第一次连接初始化了serverEnd后面才能收到消息  
        // SocketSend("FF 04 00 FC FF F7 F7 37 0F FD 37 01 00 60 60 19 BD DB 04 04 00 0D 00 00 00 00 0F B5 FD 03 00 F9 FF FE");  
        SocketSend("FF 07 00 FD FF F4 F7 3F 0F 61 38 01 00 60 60 46 BD DB 04 04 00 0D 00 00 00 00 16 48 00 F0 FF 03 00 FE");  
  
        //开启一个线程连接，必须的，否则主线程卡死  
        connectThread = new Thread(new ThreadStart(SocketReceive));  
        connectThread.Start();  
    }  

    public static byte[] HexStringToByteArray(string s)  
        {  
            if (s.Length == 0)  
                throw new Exception("将16进制字符串转换成字节数组时出错，错误信息：被转换的字符串长度为0。");  
            s = s.Replace(" ", "");  
            byte[] buffer = new byte[s.Length / 2];  
            for (int i = 0; i < s.Length; i += 2)  
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);  
            return buffer;  
        }  
  
    void SocketSend(string sendStr)  
    {  
        sendData = new byte[1024];  
        //数据类型转换  
        sendData = HexStringToByteArray(sendStr);  
        _Info.text = sendStr;
        //发送给指定服务端  
        socket.SendTo(sendData,sendData.Length,SocketFlags.None,ipEnd);  
    }  
  
    //服务器接收  
    void SocketReceive()  
    {  
        //进入接收循环  
        while(true)  
        {  
            //对data清零  
            recvData=new byte[1024];  
            //获取客户端，获取服务端端数据，用引用给服务端赋值，实际上服务端已经定义好并不需要赋值  
            recvLen=socket.ReceiveFrom(recvData,ref serverEnd);  
            print("message from: "+ serverEnd.ToString()); //打印服务端信息  
            //输出接收到的数据  
            recvStr = BitConverter.ToString(recvData,0,recvLen);  
            print(recvStr);  
        }  
    }  
  
    //连接关闭  
    void SocketQuit()  
    {  
        //关闭线程  
        if(connectThread!=null)  
        {  
            connectThread.Interrupt();  
            connectThread.Abort();  
        }  
        //最后关闭socket  
        if(socket!=null)  
            socket.Close();  
    }  
  
    // Use this for initialization  
  
    void OnGUI()  
    {  
        // editString=GUI.TextField(new Rect(10,10,100,20),editString);  
        // if(GUI.Button(new Rect(10,30,60,20),"send"))  
        //     SocketSend(editString);  
    }  
  
    // Update is called once per frame  
    void Update()  
    {  
  
    }  
  
    void OnApplicationQuit()  
    {  
        SocketQuit();  
    }  
}  
