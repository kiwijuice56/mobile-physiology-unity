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
    private double heartRateBpm;
    private double heartRateConfidence;
    private double heartRateKurtosis;

    private double breathRateBpm;
    private double breathRateConfidence;
    private double breathRateKurtosis;

    bool heartRateDone = false;
    bool breathRateDone = false;

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        accel.Add(Input.acceleration);
        gyro.Add(Input.gyro.rotationRate);

        Debug.Log(Input.acceleration);
        Debug.Log(Input.gyro.rotationRate);

        // Note: since some samples are chopped off while filtering, we need to 
        // collect slightly more than our target power-of-2 sample size

        // Filters are different size for each calculator

        // Once enough data is collected, calculate the heart rate
        if (accel.Count == HeartRateCalculator.GetActualSampleSize(2048)) 
        {
            (heartRateBpm, heartRateConfidence, heartRateKurtosis) = HeartRateCalculator.Analyze(accel, gyro);
            heartRateDone = true;
        }

        // Same for breath rate
        if (accel.Count == BreathingRateCalculator.GetActualSampleSize(2048))
        {
            (breathRateBpm, breathRateConfidence, breathRateKurtosis) = BreathingRateCalculator.Analyze(accel, gyro);
            breathRateDone = true;
        }

        // Once both values are calculated, display them
        if (heartRateDone && breathRateDone) 
        {
            output.GetComponent<Text>().text = $"HR: {heartRateBpm}\nKurtosis: {heartRateKurtosis}\nBR: {breathRateBpm}\nKurtosis: {breathRateKurtosis}";  
        }
    }
}
