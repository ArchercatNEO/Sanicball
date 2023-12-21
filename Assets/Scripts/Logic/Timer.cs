using UnityEngine;

public class Timer
{
    private bool isRunning = false;
    private float start = 0;
    private float runningTime = 0;

    public float Now()
    {
        if (!isRunning) return runningTime;
        return Time.time - start + runningTime;
    }

    public void Start()
    {
        isRunning = true;
        start = Time.time;
    }

    public void Stop()
    {
        isRunning = false;
        runningTime += Time.time - start;
    }

    public bool Finished(float time)
    {
        return isRunning && Now() > time;
    }

    public void Reset()
    {
        isRunning = false;
        start = 0;
        runningTime = 0;
    }

    public void AddTime(float time)
    {
        runningTime += time;
    }
}
