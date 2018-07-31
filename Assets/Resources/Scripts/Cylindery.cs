using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BSKCore; 

public class Cylindery : MonoBehaviour 
{

	private bool updateAngle = false;   
    private AngleDataModel angleDataModel = new AngleDataModel();
	void Start () 
	{
        NotificationController.Instance.AddNotificationListener(NotificationConst.ANGLE_AND_ANGLEVEL,Rotation);		
	}
	
	private void Rotation(Notification noti)
    {
        angleDataModel = (AngleDataModel)noti.param[0];
        updateAngle = true;
    }
    void Update()
    {
        if (updateAngle)
        {
            updateAngle = false;
            transform.localEulerAngles = new Vector3(0, 0, angleDataModel.angle);
        }
    }
}
