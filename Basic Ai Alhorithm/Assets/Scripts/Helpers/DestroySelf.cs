using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void StartCounting(float durationInSeconds)
    {
        Destroy(gameObject, durationInSeconds);
    }
}
