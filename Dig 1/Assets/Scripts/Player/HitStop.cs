using System;
using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    float hitStopTime = 0.07f;
    bool isWaiting;
    public void Stop()
    {
        if (isWaiting)
        {
            return;
        }
        Time.timeScale = 0;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1;
        isWaiting = false;
    }
}
