using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Framework;

    public class SocketClient : IDisposable
    {
        public Security m_security;
        Socket m_socket;
        byte[] m_buffer = new byte[4096];
        public uint SessionId { get; internal set; }
        public bool IsSpawned;

        public Socket GetSocket() => m_socket;

        public string ServerIp { get; internal set; }
        public ushort ServerPort { get; internal set; }
        public string Username { get; internal set; }
        public string Password { get; internal set; }

        public delegate void dOnReceive(Packet p);
        public event dOnReceive onReceiveHandlers;

        private DateTime LatestPacketTime = DateTime.Now;

        // AsyncTimer PingTimer;

        public object m_lock;

        System.Timers.Timer packetTimer;

        System.Timers.Timer PingTimer;

        const double TIMEOUT = 4000; // milliseconds

        public Security GetSecurity() { return m_security; }

        public SocketClient(string ip, ushort port, string username, string password)
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_security = new Security();
            m_security.ChangeIdentity("ServiceManager", 0);
            ServerIp = ip;
            ServerPort = port;
            Username = username;
            Password = password;

            m_lock = new object();

            //onReceiveHandlers += new dOnReceive(onReceive());

            ElapsedEventHandler ping = (a, b) =>
           {
               m_security.Send(new Packet(Opcode.General.PING));
               DoSend();
           };

            PingTimer = new System.Timers.Timer(TIMEOUT);
            PingTimer.Elapsed += ping;
            PingTimer.Enabled = false;

            ElapsedEventHandler packet = (a, b) =>
            {
                PostRecv(null, null);
            };

            packetTimer = new System.Timers.Timer(100);
            packetTimer.Elapsed += packet;
            packetTimer.Enabled = false;

            Connect(ip, port);
        }
        public SocketClient(string ip, ushort port)
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_security = new Security();
            ServerIp = ip;
            ServerPort = port;

            m_lock = new object();

            ElapsedEventHandler ping = (a, b) =>
            {
                m_security.Send(new Packet(Opcode.General.PING));
                DoSend();
            };

            PingTimer = new System.Timers.Timer(TIMEOUT);
            PingTimer.Elapsed += ping;
            PingTimer.Enabled = false;

            ElapsedEventHandler packet = (a, b) =>
            {
                PostRecv(null, null);
            };

            packetTimer = new System.Timers.Timer(10);
            packetTimer.Elapsed += packet;
            packetTimer.Enabled = false;
        }


        public virtual void Dispose()
        {
            //try
            //{
            //    packetTimer.Abort();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //m_ReceiverTimer.Dispose();
            packetTimer.Dispose();
            PingTimer.Dispose();
            m_socket.Dispose();
            m_security.Dispose();
        }

        public void Connect(string ip, ushort port)
        {
            m_socket.Connect(ip, port);

            //m_ReceiverTimer = new AsyncTimer(PostRecv);
            //m_ReceiverTimer.Start(0, 10, false);

            packetTimer.Enabled = true;
            PingTimer.Enabled = true;
        }

        public void Connect()
        {
            m_socket.Connect(ServerIp, ServerPort);
            packetTimer.Enabled = true;
            PingTimer.Enabled = true;
        }

        void PostRecv(object sender, object state)
        {
            lock (m_lock)
            {
                //var msec2 = DateTime.Now.Subtract(LatestPacketTime);
                /*if ((int)msec2.TotalSeconds >= 4)
                {
                    m_security.Send(new Packet(Opcode.General.PING));
                    DoSend();
                }*/

                try
                {
                    m_socket.BeginReceive(m_buffer, 0, m_buffer.Length, 0, endRecv, null);
                }
                catch (System.Exception ex)
                {
                    Dispose();
                }
            }
        }
        public void DoSend()
        {
            try
            {
                var going = m_security.TransferOutgoing();
                if (going != null)
                    foreach (var pkt in going)
                    {
                        Console.WriteLine("Send [0x{0:X2}] [{1} bytes]\n{2}", pkt.Value.Opcode, pkt.Value.GetBytes().Length, Utility.HexDump(pkt.Value.GetBytes()));

                        m_socket.Send(pkt.Key.Buffer);
                        //LatestPacketTime = DateTime.Now;
                        PingTimer.Interval = TIMEOUT;
                    }
            }
            catch { }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        void endRecv(IAsyncResult ar)
        {
            //try
            {
                int size = m_socket.EndReceive(ar);
                if (size > 0)
                {
                    m_security.Recv(m_buffer, 0, size);
                    var coming = m_security.TransferIncoming();
                    if (coming != null)
                        foreach (var pkt in coming)
                        {
                            Packet _pkt = pkt;
                            //onReceiveHandlers.Invoke(new Packet(pkt));
                            onReceive(_pkt);
                        }
                    DoSend();
                    PostRecv(null, null);
                }
                else
                {
                    Console.WriteLine("disconnected.");
                    m_socket.Dispose();
                }
            }
            /*catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
                m_socket.Dispose();
            }*/
        }

        void onReceive(Packet pkt)
        {
            Console.WriteLine("Recv [0x{0:X2}] [{1} bytes]\n{2}", pkt.Opcode, pkt.GetBytes().Length, Utility.HexDump(pkt.GetBytes()));

            switch (pkt.Opcode)
            {

                default:
                    break;

                case 0x2001:
                    {
                        var p = new Packet(Opcode.Global.Request.LOGIN, false);
                        p.WriteAscii("x3nx1a1");
                        p.WriteAscii(CreateMD5("a5158940"));
                        p.WriteAscii(CreateMD5(""));
                        p.WriteUInt16(24);
                        p.WriteUInt16(0);

                        m_security.Send(p);
                    }
                    break;


                case 0xb001:
                    {
                    }
                    break;


                case 0xb012:
                    {
                        GetSecurity().Send(new Packet(0x7204));
                    }
                    break;

                /* case 0xb001:
                     {
                         GetSecurity().Send(new Packet(0x7012));
                         GetSecurity().Send(new Packet(0x7003));
                         GetSecurity().Send(new Packet(0x7002));
                     }
                     break;*/
            }

        }
    }
}
