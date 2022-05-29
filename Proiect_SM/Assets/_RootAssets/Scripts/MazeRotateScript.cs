using UnityEngine;
using UnityEngine.UI;

public class MazeRotateScript : MonoBehaviour
{
    public static MazeRotateScript Instance { get; private set; }
    
    [SerializeField] private Transform mazePivot;

    [SerializeField] private Slider sliderX;
    [SerializeField] private Slider sliderZ;
    
    [SerializeField] private float maxAngle = 10;

    [SerializeField] private float manualRotationSpeed = 20f;
    [SerializeField] private float pivotRotationSpeed = 5f;

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

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        sliderX.value += verticalInput * manualRotationSpeed * Time.deltaTime;
        sliderZ.value += -horizontalInput * manualRotationSpeed * Time.deltaTime;
        
        RotateXZAxis(sliderX.value, sliderZ.value);
    }

    public void ResetAngle()
    {
        mazePivot.rotation = Quaternion.identity;
    }

    private void RotateXZAxis(float x, float z)
    {
        var desiredRotation = Quaternion.Euler(
            Mathf.Clamp(x, -maxAngle, maxAngle), 
            mazePivot.rotation.eulerAngles.y, 
            Mathf.Clamp(z, -maxAngle, maxAngle));
        
        mazePivot.rotation = Quaternion.Lerp(mazePivot.rotation, desiredRotation, pivotRotationSpeed * Time.deltaTime);
    }
    
    
}
