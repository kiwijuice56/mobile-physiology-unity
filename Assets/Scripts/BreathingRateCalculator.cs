using UnityEngine;
using System.Collections.Generic;

// Calculates breathing rate from gyroscope and accelerometer samples

public class BreathingRateCalculator : MonoBehaviour
{
    private const int DetrendWindowSize = 128;

    public static double Analyze(List<Vector3> accel, List<Vector3> gyro)
    {
        int sampleSize = accel.Count;

        // Load data into arrays
        double[][] data = new double[6][];
        for (int i = 0; i < 6; i++)
        {
            data[i] = new double[sampleSize];
        }
        for (int i = 0; i < sampleSize; i++)
        {
            data[0][i] = accel[i].x;
            data[1][i] = accel[i].y;
            data[2][i] = accel[i].z;
            data[3][i] = gyro[i].x;
            data[4][i] = gyro[i].y;
            data[5][i] = gyro[i].z;
        }


        for (int i = 0; i < 6; i++)
        {
            PreprocessSignal(data[i]);
        }

        // Run ICA (using external C# Accord library) 
        data = SignalHelper.IndependentComponentAnalysis(data, sampleSize - LowPassRespirationFilter.Length);

        // Run FFT (using external C# Accord library) to find the strongest signal within respiration rate ranges
        for (int i = 0; i < 6; i++)
        {
            data[i] = SignalHelper.FastFourierTransform(data[i], data[i].Length);
        }


        double maxConfidence = 0.0;
        double maxConfidenceFrequency = 0.0;
        for (int i = 0; i < 6; i++)
        {
            int index = SignalHelper.ExtractRate(data[i], 8.0, 45.0);
            if (data[i][index] >= maxConfidence)
            {
                maxConfidence = data[i][index];
                maxConfidenceFrequency = 60.0 / data[i].Length * index;
            }
        }
        return maxConfidenceFrequency * 60.0;
    }

    private static void PreprocessSignal(double[] signal)
    {
        // Use a sliding window average to detrend the samples
        double[] detrendedSignal = SignalHelper.Detrend(signal, DetrendWindowSize);

        // Set mean and variance to 0 (z-scoring)
        SignalHelper.Normalize(detrendedSignal);

        // [not in paper] Use a low pass filter to isolate signals < 1 Hz
        double[] filteredSignal = SignalHelper.ApplyFirFilter(detrendedSignal, LowPassRespirationFilter);
        filteredSignal.CopyTo(signal, 0);
    }

    public static int GetActualSampleSize(int outputSampleSize)
    {
        return outputSampleSize + LowPassRespirationFilter.Length;
    }

    public static readonly double[] LowPassRespirationFilter = {
      -0.0045644380812448395,
      -0.00042392147736656203,
      -0.00039921600949020575,
      -0.0003410784893278681,
      -0.000246559170922285,
      -0.00011254431170462093,
      0.00006433750801042243,
      0.0002870933492719704,
      0.000558700907206804,
      0.0008818359747837215,
      0.0012587683624311434,
      0.0016910582068537412,
      0.0021856908690502572,
      0.002726710900242062,
      0.0033400842043236107,
      0.0040111517348650795,
      0.004739077342511737,
      0.005524253099514465,
      0.006366047547594455,
      0.007263122811066336,
      0.008212361291143212,
      0.009210018111155885,
      0.010251426837018239,
      0.011331469979433535,
      0.01244362364387908,
      0.013584873647048157,
      0.01474370531018102,
      0.015917964027590856,
      0.01709913746351596,
      0.01827867257835206,
      0.01944880098525925,
      0.020601194890077695,
      0.02172846305781673,
      0.022822065011098805,
      0.0238738289199096,
      0.02487547673817073,
      0.025819341620558257,
      0.026697029918866366,
      0.02750356254696736,
      0.028230666791627817,
      0.028873750993741547,
      0.029427571925871056,
      0.029887019803322225,
      0.030248790890860813,
      0.030508789150617766,
      0.030665772386255268,
      0.03071819701905401,
      0.030665772386255268,
      0.030508789150617766,
      0.030248790890860813,
      0.029887019803322225,
      0.029427571925871056,
      0.028873750993741547,
      0.028230666791627817,
      0.02750356254696736,
      0.026697029918866366,
      0.025819341620558257,
      0.02487547673817073,
      0.0238738289199096,
      0.022822065011098805,
      0.02172846305781673,
      0.020601194890077695,
      0.01944880098525925,
      0.01827867257835206,
      0.01709913746351596,
      0.015917964027590856,
      0.01474370531018102,
      0.013584873647048157,
      0.01244362364387908,
      0.011331469979433535,
      0.010251426837018239,
      0.009210018111155885,
      0.008212361291143212,
      0.007263122811066336,
      0.006366047547594455,
      0.005524253099514465,
      0.004739077342511737,
      0.0040111517348650795,
      0.0033400842043236107,
      0.002726710900242062,
      0.0021856908690502572,
      0.0016910582068537412,
      0.0012587683624311434,
      0.0008818359747837215,
      0.000558700907206804,
      0.0002870933492719704,
      0.00006433750801042243,
      -0.00011254431170462093,
      -0.000246559170922285,
      -0.0003410784893278681,
      -0.00039921600949020575,
      -0.00042392147736656203,
      -0.0045644380812448395
    };
}
