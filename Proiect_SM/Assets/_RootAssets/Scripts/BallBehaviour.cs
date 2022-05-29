using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public static BallBehaviour Instance { get; private set; }

    [SerializeField] private GameObject coinParticles;
    
    private Vector3 initialSpawn;
    private Rigidbody rb;


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
        initialSpawn = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Coin":
                //BluetoothManager.Instance.SendBluetoothMessage(BluetoothManager.COIN_COLLECT_MSG_CODE);
                Instantiate(coinParticles, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                GameManager.Instance.CollectCoin();
                break;
            
            case "Respawn":
                Respawn();
                break;
        }
        
    }

    public void Respawn()
    {
        transform.position = initialSpawn;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}
