/*
 * @Description: 传递的消息，这个是消息类中的具体消息种类
 */
using System;
namespace BSKCore
{
    public class NotiEventArgs : EventArgs
    {
        public object[] ParamArray
        {
            get; set;
        }
}
}