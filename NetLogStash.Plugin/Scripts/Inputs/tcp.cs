using NetLogStash.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetLogStash.Config;
using NetLogStash;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class tcpInput : AbstractInput, IDisposable
{
    private string _host = "0.0.0.0";
    public string Host
    {
        get { return _host; }
        set { _host = value; }
    }
    private int _port = 9999;
         
    public int Port
    {
        get { return _port; }
        set { _port = value; }
    }

    private string _codec = "line";

    public string Codec
    {
        get { return _codec; }
        set { _codec = value; }
    }

    public class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 1024;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Received data string.     
        public StringBuilder sb = new StringBuilder();
    }

    public void Dispose()
    {
        //throw new NotImplementedException();
        listener.Disconnect(false);
    }

    public override IObservable<Event> Execute()
    {
        return null;
    }
    Socket listener;
    public override void Initialize(string typename, Dictionary<string, ParaItem> paras)
    {
        //config
        if (paras.ContainsKey("port"))
        {
            Port = Int32.Parse(paras["port"].Values.FirstOrDefault());
        }
        if (paras.ContainsKey("host"))
        {
            Host =paras["host"].Values.FirstOrDefault();
        }

        Task.Factory.StartNew(() => { StartListening(); });
        

    }

    public  void StartListening()
    {
        // Data buffer for incoming data.     
        byte[] bytes = new Byte[1024];
       
        IPAddress ipAddress = IPAddress.Parse(Host);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);
        // Create a TCP/IP socket.     
         listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // Bind the socket to the local     
        //endpoint and listen for incoming connections.     
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (true)
            {
                // Set the event to nonsignaled state.     
                acceptDone.Reset();
                // Start an asynchronous socket to listen for connections.     
                Console.WriteLine("begin accept");
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                // Wait until a connection is made before continuing.     
                acceptDone.WaitOne();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
       
    }
    // Thread signal.     
    private static ManualResetEvent acceptDone = new ManualResetEvent(false);
    //private static ManualResetEvent receiveDone = new ManualResetEvent(false);
    void AcceptCallback(IAsyncResult iar)
    {
        acceptDone.Set();
        Console.WriteLine("end accept,begin receive");
        //还原传入的原始套接字
        Socket socket = (Socket)iar.AsyncState;
        //在原始套接字上调用EndAccept方法，返回新的套接字
        Socket handler = socket.EndAccept(iar);
        // Create the state object.     
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        
        //receiveDone.WaitOne();
    }
    public static void ReadCallback(IAsyncResult ar)
    {
        Console.WriteLine("end receive");
        String content = String.Empty;
        // Retrieve the state object and the handler socket     
        // from the asynchronous state object.     
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;
        // Read data from the client socket.     
        int bytesRead = handler.EndReceive(ar);
        //receiveDone.Set();
        if (bytesRead > 0)
        {
            // There might be more data, so store the data received so far.     
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
            // Check for end-of-file tag. If it is not there, read     
            // more data.     
            content = state.sb.ToString();
            if (content.IndexOf("\r\n") > -1)
            {
                // All the data has been read from the     
                // client. Display it on the console.     
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);

            }
            else
            {
                // Not all data received. Get more.     
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
        }
    }

}
