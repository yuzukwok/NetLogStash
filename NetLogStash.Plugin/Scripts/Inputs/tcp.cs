//css_reference System.Reactive.Interfaces.dll
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
using NetLogStash.Util;
using NetLogStash.Tcp;
using System.Reactive.Linq;
using System.Reactive;

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
        srv.Shutdown();
    }
    public event EventHandler<StrEventArgs> Rev;

    public override IObservable<Event> Execute()
    {
        return Observable.FromEventPattern<EventHandler<StrEventArgs>, StrEventArgs>(action => { Rev += action; }, 
            action => { Rev -= action; })           
           .SelectMany(ParserText);
    }
    AsyncTcpSocketServer srv;

    IEnumerable<Event> ParserText(EventPattern<StrEventArgs> e)
    {
       
        Event item=new Event();
        item.SetMember("str", e.EventArgs.Text);
        return new List<Event>() { item };
    }
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
        var dispatcher = new SimpleMessageDispatcher();
        dispatcher.Rev += (o,e) => {
           
            this.Rev(o, e);
        };
         srv = new AsyncTcpSocketServer(IPAddress.Parse(Host), Port, dispatcher, new AsyncTcpSocketServerConfiguration() { FrameBuilder = new LineBasedFrameBuilder(LineDelimiter.WINDOWS) });
        srv.Listen();
        
    }
    public class StrEventArgs : EventArgs
    {
        public string Text { get; set; }
    }
    public class SimpleMessageDispatcher : IAsyncTcpSocketServerMessageDispatcher
    {
        public event EventHandler<StrEventArgs> Rev;
        public async Task OnSessionStarted(AsyncTcpSocketSession session)
        {
           // Console.WriteLine(string.Format("TCP session {0} has connected {1}.", session.RemoteEndPoint, session));
            await Task.CompletedTask;
        }

        public async Task OnSessionDataReceived(AsyncTcpSocketSession session, byte[] data, int offset, int count)
        {
           

            var text = Encoding.UTF8.GetString(data, offset, count);
            Console.Write(string.Format("Client : {0} --> ", session.RemoteEndPoint));
            Console.WriteLine(string.Format("{0}", text));
            if (Rev != null)
            {
                
                Rev(null, new StrEventArgs() {  Text=text});
            }
            await session.SendAsync(Encoding.UTF8.GetBytes(text));

           
        }

        public async Task OnSessionClosed(AsyncTcpSocketSession session)
        {
           // Console.WriteLine(string.Format("TCP session {0} has disconnected.", session));
            await Task.CompletedTask;
        }
    }

}
