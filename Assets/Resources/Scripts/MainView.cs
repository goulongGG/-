using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;
using BSKCore; 
  
public class MainView : MonoBehaviour 
{	[SerializeField] private Text _AngleText;
	[SerializeField] private Text _AngularVeclocityText;    
    [SerializeField] private Button _ReturnViewButton;    
    [SerializeField] private GameObject _WaterPool;    
    [SerializeField] private InputField _IpPort;
    [SerializeField] private Button _InitSocketButton;
    private bool updateAngle = false;   
    private AngleDataModel angleDataModel = new AngleDataModel();
    private UdpServer udpServer = new UdpServer();

    void Awake()
    {
    }
	void Start()
    {
        _ReturnViewButton.onClick.RemoveAllListeners();
        _ReturnViewButton.onClick.AddListener(Return3DView);
        _InitSocketButton.onClick.RemoveAllListeners();
        _InitSocketButton.onClick.AddListener(InitSocket);
        NotificationController.Instance.AddNotificationListener(NotificationConst.ANGLE_AND_ANGLEVEL,AngleAndAngleVel);
    }

    private void AngleAndAngleVel(Notification noti)
    {
        angleDataModel = (AngleDataModel)noti.param[0];
        updateAngle = true;
    }
    
    private void InitSocket()
    {
        if (_IpPort.text != "")
        {
            udpServer.InitSocket(int.Parse(_IpPort.text));
        }
        else
        {
            udpServer.InitSocket(GameConst.Port);
        }
    }
    
    private void Return3DView()
    {
        _WaterPool.transform.localPosition = Vector3.zero; 
        _WaterPool.transform.localEulerAngles = Vector3.zero; 
    }

    void Update()
    {
        if (updateAngle)
        {
            updateAngle = false;
            _AngleText.text = "横倾角：" + angleDataModel.angle + "˚";
            _AngularVeclocityText.text = "角速度：" + angleDataModel.angle_velocity + " ˚/s";
        }

    }

    void OnApplicationQuit()  
    {  
        udpServer.SocketQuit();  
    }  
}
