using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// Collects data samples and calculates HR and BR

public class Sensor : MonoBehaviour
{
    // Reference to a GameObject with a Text component
    public GameObject output;

    // Data samples
    private List<Vector3> accel = new List<Vector3>();
    private List<Vector3> gyro = new List<Vector3>();
    
    // Output values
    private double heart_rate_bpm;
    private double breath_rate_bpm;

    bool heart_rate_done = false;
    bool breath_rate_done = false;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        accel.Add(Input.acceleration);
        gyro.Add(Input.gyro.rotationRate);

        // Note: since some samples are chopped off while filtering, we need to 
        // collect slightly more than our target power-of-2 sample size

        // Filters are different size for each calculator

        // Once enough data is collected, calculate the heart rate
        if (accel.Count == HeartRateCalculator.GetActualSampleSize(2048)) 
        {
            heart_rate_bpm = HeartRateCalculator.Analyze(accel, gyro);
            heart_rate_done = true;
        }

        // Same for breath rate
        if (accel.Count == BreathingRateCalculator.GetActualSampleSize(2048))
        {
            breath_rate_bpm = BreathingRateCalculator.Analyze(accel, gyro);
            breath_rate_done = true;
        }

        // Once both values are calculated, display them
        if (heart_rate_done && breath_rate_done) 
        {
            output.GetComponent<Text>().text = $"HR: {heart_rate_bpm}\nBR: {breath_rate_bpm}";  
        }
    }
}
