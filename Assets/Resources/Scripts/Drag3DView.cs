using UnityEngine;
using System.Collections;

public class Drag3DView : MonoBehaviour 
{

	private int MouseWheelSensitivity = 1;
	private int MouseZoomMin = -10;
	private int MouseZoomMax = 1;
	private float normalDistance = 3;
	private float xSpeed = 250.0f;
	private float ySpeed = 120.0f;
	private float mouse_x = 0.0f;
	private float mouse_y = 0.0f;
	private Vector3 CameraTarget;
	void Start () 
	{
		transform.localEulerAngles = Vector3.zero;
	}
	
	void Update ()
	{
        if (Input.GetMouseButton(0)) // 鼠标左键调整x,z视角
        {
            mouse_x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            mouse_y += Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            transform.rotation = Quaternion.Euler(mouse_x, transform.rotation.y, mouse_y);
        }
        if (Input.GetMouseButton(1)) // 鼠标右键键调整x,y视角
        {
            mouse_x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            mouse_y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            transform.rotation = Quaternion.Euler(mouse_y, mouse_x, transform.rotation.z);;
        }
		if (Input.GetAxis("Mouse ScrollWheel") != 0) //鼠标滚动缩小放大
		{
            normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
            normalDistance = Mathf.Clamp(normalDistance, MouseZoomMin, MouseZoomMax);
            Vector3 mPosition = 3 * new Vector3(normalDistance, 0.0f, 0.0F);
            transform.localPosition = mPosition;
        }
	}
}