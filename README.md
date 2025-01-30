# mobile-physiology-unity

Simple port of heart rate (HR) and breathing rate (BR) calculator using gyroscope and accelerometer data originally written in Godot.

See `Assets/Scripts/Sensor.cs` for the main script. The algorithm is written in `Assets/Scripts/SignalHelper.cs`, `Assets/Scripts/HeartRateCalculator.cs`, and `Assets/Scripts/BreathingRateCalculator.cs`. These scripts require the Accord.net library (included in the plugins folder, version 3.8.0). Run `Assets/Scenes/MainScene.unity` to see the algorithm in action. Once the project is running,
wait ~30 seconds for the sensor to collect data and run the algorithm. Results will be displayed on screen.

Please contact `ealfaro@mit.edu` for any questions or issues.