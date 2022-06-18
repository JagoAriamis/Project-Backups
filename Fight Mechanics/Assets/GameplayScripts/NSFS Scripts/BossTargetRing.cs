using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BossTargetRing : MonoBehaviour
{
    public float thetaScale = 0.01f;
    public float radius = 15f;
    public float width;
    
    [Range(6, 60)]
    public int lineCount;

    LineRenderer lineDraw;


    // Start is called before the first frame update
    void Start()
    {
        lineDraw = GetComponent<LineRenderer>();
        lineDraw.loop = true;
        Draw();
    }

    void Draw()
    {
        lineDraw.positionCount = lineCount;
        lineDraw.startWidth = width;

        float theta = (2f * Mathf.PI) / lineCount;
        float angle = 0f;

        for (int i = 0; i < lineCount; i++)
        {
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            lineDraw.SetPosition(i, new Vector3(x, 0, z));
            angle += theta;
        }
    }
}
