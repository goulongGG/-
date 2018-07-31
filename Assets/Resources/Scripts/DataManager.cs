
using System;
using System.Collections;  
using System.Text;
using BSKCore;
using UnityEngine;
public class AngleDataModel
{
    public float angle;
    public float angle_velocity;
}
public class DataManager : Singleton<DataManager>
{
    private const int ANGLE_RANGE = 360;
    private const int ANGLE_VELOCITY_RANGE = 250;    
    private const int BYTE_RANGE = 32768; 
    private const int ANGLE_HIGH_BYTE_POS = 3;   
    private const int ANGLE_LOW_BYTE_POS = 4;  
    private const int ANGVLE_HIGH_BYTE_POS = 13;   
    private const int ANGVLE_LOW_BYTE_POS = 14;    
    public DataManager()
    {
    }
    /// <summary>
	/// 字节转十六进制字符串
	/// </summary>
    public string ByteArrayToHexStringNoBlank(byte[] data)
    {
        StringBuilder sbu = new StringBuilder(data.Length * 3);
        foreach (byte mByte in data)
            sbu.Append(Convert.ToString(mByte, 16).PadLeft(2, '0'));
        return sbu.ToString().ToUpper();
    }

    /// <summary>
    /// 十六进制字符串转字节
    /// </summary>
    public byte[] HexStringToByteArray(string s)
    {
        if (s.Length == 0)
        {
            throw new Exception("将16进制字符串转换成字节数组时出错，错误信息：被转换的字符串长度为0。");
        }
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;
    }

    /// <summary>
    /// 数据解析传输
    /// </summary>
    public void SolveByteToData(byte[] data)
    {
        byte[] angleByte = new byte[]{data[ANGLE_HIGH_BYTE_POS],data[ANGLE_LOW_BYTE_POS]};
        int angleData = ByteArrayToInt16(angleByte);
 
        byte[] angVleByte = new byte[]{data[ANGVLE_HIGH_BYTE_POS],data[ANGVLE_LOW_BYTE_POS]};
        int angVelData = ByteArrayToInt16(angVleByte);

        //角度、角速度解析为float型 赋予model 传输数据
        AngleDataModel model = new AngleDataModel();
        model.angle = SolveAngleData(angleData);
        model.angle_velocity = SolveAngVelData(angVelData);

        Debug.Log("角度：" + model.angle);
        Debug.Log("角速度：" + model.angle_velocity);  

        NotificationController.Instance.SendNotification(NotificationConst.ANGLE_AND_ANGLEVEL,model);
    }

    /// <summary>
    /// 两字节转16进制 16进制转int
    /// </summary>
    private int ByteArrayToInt16(byte[] byteArray)
    {
        string Str16 = ByteArrayToHexStringNoBlank(byteArray);
        Debug.Log("十六进制：" + Str16);
        
        int data = Convert.ToInt16(Str16, 16);
        return data;
    }

    /// <summary>
    /// 角度解析
    /// </summary>
    private float SolveAngleData(int angleData)
    {
        return (float)angleData / BYTE_RANGE * ANGLE_RANGE;
    }

    /// <summary>
    /// 角速度解析
    /// </summary>
    private float SolveAngVelData(int angleVeloData)
    {
        return (float)angleVeloData / BYTE_RANGE * ANGLE_VELOCITY_RANGE;
    }

    /// <summary>  
    /// 计算按位异或校验和（返回校验和值）  
    /// 不清楚你们怎么校验的，反正 XOR 异或校验就是这个方法，从给我的数据，校验出来的结果与第34字节对不上
    /// </summary>  
    /// <param name="Cmd">命令数组</param>  
    /// <returns>校验和值</returns>  
    public byte GetXOR(byte[] Cmd)
    {
        byte check = (byte)(Cmd[0] ^ Cmd[1]);
        for (int i = 2; i < Cmd.Length; i++)
        {
            check = (byte)(check ^ Cmd[i]);
        }
        return check;
    }

}