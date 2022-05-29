using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class BluetoothManager : MonoBehaviour
{
    public static BluetoothManager Instance { get; private set; }

    public const string COIN_COLLECT_MSG_CODE = "coin";
    public const string GAME_OVER_MSG_CODE = "over";
    public const string WIN_MSG_CODE = "win";
    
    [SerializeField] private string serverHost = "127.0.0.1";
    [SerializeField] private int serverPort = 50000;

    
    private byte[] buffer = new byte[1024];

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

        socket = new Socket(SocketType.Stream, ProtocolType.Unspecified);

        socket.Connect(serverHost, serverPort);

        // StartCoroutine(ReceiveData());

    }

    [ContextMenu("SendMessage")]
    public void SendBluetoothMessage(string message = "led")
    {
        var msg = Encoding.UTF8.GetBytes(message);
        socket.Send(msg);
    }


    private IEnumerator ReceiveData()
    {
        while (true)
        {
            var l = socket.Receive(buffer);
            
            print(l);
            
            if (l > 0)
            {
                print(Encoding.UTF8.GetString(buffer));
            }
            
            yield return null;
        }
    }
    
}
