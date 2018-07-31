/*
 * @Description: 消息类，建立消息机制 ---- 解耦合
 */
using UnityEngine;
using System;
namespace BSKCore
{
    public class Notification
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public GameObject sender;

        /// <summary>
        /// 消息内容,消息允许携带的参数
        /// </summary>
        
        public object[] param;

        /// <summary>
        /// 构造函数 （初始化）
        /// </summary>
        ///<param name="sender">通知发送者
        ///<param name="param">通知内容
        public Notification(GameObject sender,object[] param)
        {
            this.sender = sender;
            this.param = param;
        }　　　　
        public Notification()
        {

        }
    }
  
}