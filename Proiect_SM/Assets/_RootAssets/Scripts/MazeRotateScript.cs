using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float multiplicationSliderGyro = 40f;

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
        StartCoroutine(RotateCoroutine());
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        // sliderX.value += verticalInput * manualRotationSpeed * Time.deltaTime;
        // sliderZ.value += -horizontalInput * manualRotationSpeed * Time.deltaTime;

        

        // sliderX.value = -BluetoothManager.Instance.Gyroscope.y * multiplicationSliderGyro;
        // sliderZ.value = -BluetoothManager.Instance.Gyroscope.x * multiplicationSliderGyro;
        
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

    private IEnumerator RotateCoroutine()
    {
        var x = -BluetoothManager.Instance.Gyroscope.y;
        var z = -BluetoothManager.Instance.Gyroscope.x;

        // var xValues = new List<float>();
        // var zValues = new List<float>();
        while (true)
        {
            // var dx = -BluetoothManager.Instance.Gyroscope.y;
            // var dz = -BluetoothManager.Instance.Gyroscope.x;

            // xValues.Add(dx);
            // zValues.Add(dz);

            //print(Mathf.Abs(x - dx));
            //print(Mathf.Abs(z - dz));
            
            var dx = -BluetoothManager.Instance.Gyroscope.y;
            var dz = -BluetoothManager.Instance.Gyroscope.x;
            
            
             x += dx;
             x /= 2;
         
             z += dz;
             z /= 2;
             
            
            // sliderX.value += x;
            // sliderZ.value += z;
            
            yield return new WaitForSecondsRealtime(0.02f);
            
            // for (var i = 1; i < xValues.Count; i++)
            // {
            //     if (Mathf.Abs(xValues[i - 1] - xValues[i]) > 0.4f)
            //     {
            //         continue;
            //     }
            //
            //     x += xValues[i];
            //     x /= 2f;
            //
            // }
            //
            // for (var i = 1; i < zValues.Count; i++)
            // {
            //     if (Mathf.Abs(zValues[i - 1] - zValues[i]) > 0.4f)
            //     {
            //         continue;
            //     }
            //     
            //     z += zValues[i];
            //     z /= 2f;
            //     
            // }
            
            sliderX.value = x * multiplicationSliderGyro;
            sliderZ.value = z * multiplicationSliderGyro;

        }
    }
    
}
