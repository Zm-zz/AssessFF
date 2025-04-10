using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

namespace WeFrame
{
	public class ServerManager : SystemSingleton<ServerManager>
	{
		/// <summary>
        /// 服务器IP地址
        /// </summary>
		private string mServerIPStr = "127.0.0.1";

        /// <summary>
        /// 服务器端口号
        /// </summary>
        private int mServerPort = 8888;

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        private IPAddress mServerIP = null;

        /// <summary>
        /// 服务器IP结束节点
        /// </summary>
        private IPEndPoint mServerIPEP = null;

        /// <summary>
        /// 服务器套接字
        /// </summary>
        private Socket mServerSocket = null;

        /// <summary>
        /// 客户端套接字
        /// </summary>
        private Socket mClientSocket = null;
        
        /// <summary>
        /// 服务器线程
        /// </summary>
        private Thread mServerThread = null;

        /// <summary>
        /// 客户端线程
        /// </summary>
        private Thread mClientThread = null;

        /// <summary>
        /// 客户端套接字数组
        /// </summary>
        /// <typeparam name="Socket">套接字</typeparam>
        /// <returns></returns>
        private List<Socket> mClientSocketList = new List<Socket>();

        /// <summary>
        /// 开启连接
        /// </summary>
        public void OpenConnect()
        {
            try
            {
                mServerIP = IPAddress.Parse(mServerIPStr);
                mServerIPEP = new IPEndPoint(mServerIP, mServerPort);
                mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mServerSocket.Bind(mServerIPEP);
                mServerSocket.Listen(1000);

                mServerThread = new Thread(new ThreadStart(ListenClient));
                mServerThread.Start();
            }
            catch (SocketException e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            try
            {
                mServerSocket.Close();
            }
            catch (SocketException e)
            {
                Debug.Log(e.ToString());
            }
        }

        ///<summary>
        ///监听用户连接
        ///</summary>
        public void ListenClient()
        {
            while (true)
            {
                try
                {
                    mClientSocket = mServerSocket.Accept();
                    mClientSocketList.Add(mClientSocket);
                    mClientThread = new Thread(new ThreadStart(ReceiveMessage));
                    mClientThread.Start();
                }
                catch (SocketException e)
                {
                    Debug.Log(e.ToString());
                }
            }
        }

        ///<summary>
        ///发送信息
        ///</summary>
        public void SendMessage(string sendStr)
        {
            if (mClientSocket.Connected)
            {
                IPEndPoint ipep = (IPEndPoint)mClientSocket.RemoteEndPoint;
                sendStr = ipep.ToString() + sendStr;
                byte[] sendByteAry = Encoding.UTF8.GetBytes(sendStr);
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
                string receiveStr = Encoding.UTF8.GetString(receiveByteAry, 0, byteAryLength);
            }
        }
	}		
}
