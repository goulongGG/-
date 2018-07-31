/*
 * @Description: 消息分发中心，负责所有消息的存储以及分发
 */
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSKCore
{
    public class NotificationController : Singleton<NotificationController>
    {
        public delegate void NotificationDelegate(Notification notific);
        /// 消息存放的容器
        private Dictionary<uint, NotificationDelegate> eventListeners = new Dictionary<uint, NotificationDelegate>();

        ///<summary>
        /// 添加注册事件监听，增加特定的委托
        ///</summary>
        ///<param name = "eventKey" >事件名</param>
        ///<param name = "listener" >委托</param>
        public void AddNotificationListener(uint eventKey, NotificationDelegate listener)
        {
            if (!HasEventListener(eventKey))
            {
                NotificationDelegate del = null; //定义方法
                eventListeners[eventKey] = del;// 给委托变量赋值
            }
            eventListeners[eventKey] += listener; //注册接收者的监听
        }

        ///<summary>
        /// 移除注册事件监听,取消特定的委托
        ///</summary>
        ///<param name = "eventKey" >事件名</param>
        ///<param name = "listener" >委托</param>
        public void RemoveEventListener(uint eventKey, NotificationDelegate listener)
        {
            if (!HasEventListener(eventKey))
                return;
            eventListeners[eventKey] -= listener; //析构指定接收者的监听
            if (eventListeners[eventKey] == null)
            {
                RemoveEventListener(eventKey);
            }
        }  
        ///<summary>
        /// 移除注册事件监听,移除所有的委托
        ///</summary>
        public void RemoveEventListener(uint eventKey)
        {
            eventListeners.Remove(eventKey);
        }
        /// <summary>
        /// 分发事件，需要知道发送者，具体消息的情况
        /// </summary>
        ///<param name="eventKey">事件Key
        ///<param name="sender">发送者
        ///<param name="param">封装后的通知内容
        public void SendNotification(uint eventKey, GameObject sender = null,params object[] param)
        {
            if (!HasEventListener(eventKey))
                return;
            eventListeners[eventKey](new Notification(sender,param));
        }

        /// <summary>
        /// 分发事件，不需要知道接送者
        /// </summary>
        ///<param name="eventKey">事件Key
        ///<param name="param">封装后的通知内容
        public void SendNotification(uint eventKey, params object[] param)
        {
            if (!HasEventListener(eventKey))
                return;
            eventListeners[eventKey](new Notification(null, param));
        }

        /// <summary>
        /// 是否存在指定事件的监听器
        /// </summary>
        public bool HasEventListener(uint eventKey)
        {
            return eventListeners.ContainsKey(eventKey);
        }
    }
}