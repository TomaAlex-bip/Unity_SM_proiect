using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class BluetoothManager : MonoBehaviour
{
    public static BluetoothManager Instance { get; private set; }
    
    public Vector2 Gyroscope { get; private set; }

    public const string COIN_COLLECT_MSG_CODE = "coin";
    public const string GAME_OVER_MSG_CODE = "over";
    public const string WIN_MSG_CODE = "win";
    
    [SerializeField] private string serverHost = "127.0.0.1";
    [SerializeField] private int serverPort = 50000;

    
    private byte[] buffer = new byte[1024];

    private SocketAsyncEventArgs socketAsyncEventArgs;

    private Socket socket;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        // socketAsyncEventArgs = new SocketAsyncEventArgs();
        // socketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
        

        // socketAsyncEventArgs.Completed += (sender, eventArgs) =>
        // {
        //     if (eventArgs.LastOperation == SocketAsyncOperation.Receive)
        //     {
        //         ReceiveData(eventArgs);
        //     }
        // };
        
        
        socket = new Socket(SocketType.Stream, ProtocolType.Unspecified);
        socket.Connect(serverHost, serverPort);
        
        var state = new StateObject
        {
            workSocket = socket
        };
        socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,  
            new AsyncCallback(ReadCallback), state);  

        // socket.ReceiveAsync(socketAsyncEventArgs);
    }

    private void ReadCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject) ar.AsyncState;  
        Socket handler = state.workSocket;
        
        // Read data from the client socket.  
        int read = handler.EndReceive(ar);  
  
        // Data was read from the client socket.  
        if (read > 0)
        {  
            var data = (Encoding.ASCII.GetString(state.buffer,0,read));
            state.sb.Append(data);


            ProcessData(state.sb);
            
            
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,  
                new AsyncCallback(ReadCallback), state);  
        }
        
    }

    private void ProcessData(StringBuilder sb)
    {
        var data = sb.ToString().Split(new string[]{"\r\n"}, StringSplitOptions.None);

        
        
        foreach (var d in data)
        {
            // Debug.Log(d);
            try
            {
                var axes = d.Split('~');
                // Debug.Log($"x: {axes[0]} | z: {axes[1]}");
                
                var x = float.Parse(axes[0].Replace('.', ','));
                var z = float.Parse(axes[1].Replace('.', ','));

                Gyroscope = new Vector2(x, z);

            }
            catch
            {
                continue;
            }
        }
    }

    [ContextMenu("SendMessage")]
    public void SendBluetoothMessage(string message = "led")
    {
        var msg = Encoding.UTF8.GetBytes(message);
        socket.Send(msg);
    }


    // private void ReceiveData(SocketAsyncEventArgs eventArgs)
    // {
    //     while (eventArgs.SocketError == SocketError.Success)
    //     {
    //         Debug.Log($"Received: {Encoding.UTF8.GetString(eventArgs.Buffer)}");
    //     }
    //     // else
    //     // {
    //     //     throw new SocketException((int) eventArgs.SocketError);
    //     // }
    //     
    // }
    
}

public class StateObject
{  
    public Socket workSocket = null;  
    public const int BufferSize = 1024;  
    public byte[] buffer = new byte[BufferSize];  
    public StringBuilder sb = new StringBuilder();  
} 
