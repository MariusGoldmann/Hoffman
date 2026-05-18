using System;
using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField] private float hitStopTime = 0.07f;
    [SerializeField] private bool  isWaiting;

    public void Stop()
    {
        if (isWaiting)
        {
            return;
        }
        Time.timeScale = 0;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1;
        isWaiting = false;
    }
}
