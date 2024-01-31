using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace NetworkAPI
{
    public class NetworkComm
    {
        public delegate void MsgHandler(string message);
        public event MsgHandler MsgReceived;

        public void sendMessage(string message)
        {
            IPAddress mcastAddress = IPAddress.Parse("230.0.0.1");
            int mcastPort = 11000;
            IPEndPoint endPoint;
            Socket mcastSocket = null;

            try
            {
                Debug.Log("Sending message: " + message);
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                endPoint = new IPEndPoint(mcastAddress, mcastPort);
                mcastSocket.SendTo(Encoding.ASCII.GetBytes(message), endPoint);
                Debug.Log("Message sent to " + endPoint.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError("Send message error: " + e.Message);
            }
            finally
            {
                mcastSocket?.Close();
            }
        }

        public void ReceiveMessages()
        {
            IPAddress mcastAddress = IPAddress.Parse("230.0.0.1");
            int mcastPort = 11000;
            Socket mcastSocket = null;
            MulticastOption mcastOption = null;

            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIP = IPAddress.Any;
                EndPoint localEP = new IPEndPoint(localIP, mcastPort);
                mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                mcastSocket.Bind(localEP);

                mcastOption = new MulticastOption(mcastAddress, localIP);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);

                Debug.Log("Started receiving messages.");

                while (true)
                {
                    byte[] bytes = new byte[1000]; // Increase the buffer size if needed
                    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, mcastPort);
                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);
                    string receivedMessage = Encoding.ASCII.GetString(bytes).TrimEnd('\0');
                    Debug.Log("Received message from " + remoteEP.ToString() + ": " + receivedMessage);

                    MsgReceived?.Invoke(receivedMessage);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Receive message error: " + e.Message);
            }
            finally
            {
                mcastSocket?.Close();
            }
        }
    }
}


//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace NetworkAPI
//{
//    public class NetworkComm
//    {
//        public delegate void MsgHandler(string message);
//        public event MsgHandler MsgReceived;
//        public void sendMessage(String message)
//        {
//            IPAddress mcastAddress;
//            int mcastPort;
//            Socket mcastSocket = null;
//            mcastAddress = IPAddress.Parse("230.0.0.1");
//            mcastPort = 11000;
//            IPEndPoint endPoint;

//            try
//            {
//                mcastSocket = new Socket(AddressFamily.InterNetwork,
//                               SocketType.Dgram,
//                               ProtocolType.Udp);

//                //Send multicast packets to the listener.
//                endPoint = new IPEndPoint(mcastAddress, mcastPort);
//                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Hello Multicast Listener"), endPoint);
//                Debug.Log("Message Sent");

//            }
//            catch (Exception e)
//            {
//                Debug.Log("\n" + e.ToString());
//            }

//            mcastSocket.Close();
//        }
//        public void ReceiveMessages()
//        {
//            IPAddress mcastAddress;
//            int mcastPort;
//            Socket mcastSocket = null;
//            MulticastOption mcastOption = null;
//            mcastAddress = IPAddress.Parse("230.0.0.1");
//            mcastPort = 11000;
//            try
//            {
//                mcastSocket = new Socket(AddressFamily.InterNetwork,
//                                         SocketType.Dgram,
//                                         ProtocolType.Udp);
//                IPAddress localIP = IPAddress.Any;
//                EndPoint localEP = (EndPoint)new IPEndPoint(localIP, mcastPort);
//                mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
//                mcastSocket.Bind(localEP);
//                mcastOption = new MulticastOption(mcastAddress, localIP);
//                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
//                                            SocketOptionName.AddMembership,
//                                            mcastOption);

//                bool done = false;
//                byte[] bytes = new Byte[100];
//                IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
//                EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

//                while (!done)
//                {
//                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);
//                    String message = "Received broadcast from: " + remoteEP.ToString() + "  " +
//                      Encoding.ASCII.GetString(bytes, 0, bytes.Length);
//                    //Debug.Log(message);

//                    MsgReceived(message);
//                }

//                mcastSocket.Close();
//            }

//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }
//    }
//}
