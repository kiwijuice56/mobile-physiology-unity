using UnityEngine;
using System.Collections.Generic;

public class Sensor : MonoBehaviour
{
    public GameObject output;
    public List<Vector3> accel = new List<Vector3>();
    public List<Vector3> gyro = new List<Vector3>();

    void Start()
    {

    }

    void FixedUpdate()
    {
        accel.Add(Input.acceleration);
        gyro.Add(Input.gyro.rotationRate);

        // Once enough data is collected, calculate the heart rate
        if (accel.Count == HeartRateAlgorithm.GetActualSampleSize(1024)) 
        {
            double heart_rate_bpm = HeartRateAlgorithm.Analyze(accel, gyro);
            print(heart_rate_bpm);
        }
    }
}
