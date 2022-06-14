using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FoVInEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView FoV = (FieldOfView)target;

        Handles.color = Color.red;
        Handles.DrawWireArc(FoV.transform.position, Vector3.up, Vector3.forward, 360, FoV.viewRadius);

        Vector3 viewAngleA = FoV.AngleDirection(-FoV.viewAngle / 2, false);
        Vector3 viewAngleB = FoV.AngleDirection(FoV.viewAngle / 2, false);

        Handles.DrawLine(FoV.transform.position, FoV.transform.position + viewAngleA * FoV.viewRadius);
        Handles.DrawLine(FoV.transform.position, FoV.transform.position + viewAngleB * FoV.viewRadius);

        Handles.color = Color.green;
        foreach (Transform visibleTarget in FoV.targetDistanceList)
        {
            Handles.DrawLine(FoV.transform.position, visibleTarget.position);
        }
    }
}
