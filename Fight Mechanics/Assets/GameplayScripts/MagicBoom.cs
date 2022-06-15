using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoom : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        TimeOutDestroy(1f);
    }

    void TimeOutDestroy(float time)
    {
        Destroy(gameObject, time);
    }
}
