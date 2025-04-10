using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

namespace WeFrame
{
    public enum NetworkEventType
    {
        License_Check = 1
    }

    public class BaseClientData { }

    public class BaseServerData { }

    public class NetworkEventData : BaseEventData
    {
        protected string mJsonData = null;

        public string JsonData
        {
            get { return mJsonData; }
        }

        public NetworkEventData(string data)
        {
            if (data != null) 
            {
                mJsonData = data;
            }
        }
    }

	public class ClientManager : SystemSingleton<ClientManager>
	{
        /// <summary>
        /// 服务器IP地址字符串
        /// </summary>
		private string mServerIPData = "139.196.203.46";

        /// <summary>
        /// 服务器端口号
        /// </summary>
        private int mServerPort = 8011;

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        private IPAddress mServerIP = null;

        /// <summary>
        /// 服务器IP结束节点
        /// </summary>
        private IPEndPoint mServerIPEP = null;

        /// <summary>
        /// 客户端套接字
        /// </summary>
        private Socket mClientSocket = null;

        /// <summary>
        /// 客户端线程
        /// </summary>
        private Thread mClientThread = null;

        public bool IsConnect
        {
            get { return mClientSocket.Connected; }
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        public void OpenConnect()
        {
            CLog.InputOperate("连接许可证服务器");

            try
            {
                mServerIP = IPAddress.Parse(mServerIPData);
                mServerIPEP = new IPEndPoint(mServerIP, mServerPort);
                mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mClientSocket.Connect(mServerIPEP);

                mClientThread = new Thread(new ThreadStart(ReceiveMessage));
                mClientThread.Start();

                CLog.InputRight();
            }
            catch (SocketException e)
            {
                CLog.InputWrong(e.ToString());
            }

            CLog.Output();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            try
            {
                mClientSocket.Close();
                mClientThread.Abort();
            }
            catch (SocketException e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="message">消息字符串</param>
        public void SendMessage(NetworkEventType type, string message)
        {
            SendMessage((int)type, message);

            CLog.InputOperate("发送客户端消息");
            CLog.InputTarget("类型", type.ToString());
            CLog.InputTarget("消息", message);
            CLog.Output();
        }

        private void SendMessage(int index, string sendData)
        {
            if (mClientSocket.Connected)
            {
                byte id = 0xCA;
                int count = sendData.Length + 4;
                byte high = (byte)((count & 0xFF00) >> 8);
                byte low = (byte)(count & 0x00FF);
                byte[] jsonByteAry = Encoding.UTF8.GetBytes(sendData);
                byte[] sendByteAry = new byte[count];
                sendByteAry[0] = id;
                sendByteAry[1] = high;
                sendByteAry[2] = low;
                sendByteAry[3] = (byte)index;
                jsonByteAry.CopyTo(sendByteAry, 4);
                mClientSocket.Send(sendByteAry, sendByteAry.Length, 0); 
            }        
        }        
 
        ///<summary>
        ///接收信息
        ///</summary>
        private void ReceiveMessage()
        {
            while (true)
            {
                byte[] receiveByteAry = new byte[1024];
                int byteAryLength = mClientSocket.Receive(receiveByteAry, receiveByteAry.Length, 0);
                if (byteAryLength <= 0) break;

                byte[] jsonByteAry = new byte[1020];
                Array.ConstrainedCopy(receiveByteAry, 4, jsonByteAry, 0, 1020);
                string receiveData = Encoding.UTF8.GetString(jsonByteAry, 0, byteAryLength - 4);

                int index = (int)receiveByteAry[3];
                NetworkEventType type = (NetworkEventType)index;
                NetworkEventData data = new NetworkEventData(receiveData);

                ThreadEventManager.Self.InvokeEvent(type, data);

                CLog.InputOperate("接收服务器消息");
                CLog.InputTarget("类型", type.ToString());
                CLog.InputTarget("消息", receiveData);
                CLog.Output();
            }
        }
	}		
}
