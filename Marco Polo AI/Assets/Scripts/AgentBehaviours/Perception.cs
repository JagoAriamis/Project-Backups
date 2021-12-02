using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Perception : MonoBehaviour
{
    public Dictionary<GameObject, MemoryRecord> MemoryMap = new Dictionary<GameObject, MemoryRecord>();

    //For debugging purposes
    public GameObject[] SensedObjects;
    public MemoryRecord[] SensedRecord;

    //Clears all current FoVs
    public void ClearFoV()
    {
        foreach (KeyValuePair<GameObject, MemoryRecord> Memory in MemoryMap)
            Memory.Value.WithinFoV = false;
    }

    public void AddMemory(GameObject TargetDesu)
    {
        // Create a new memory record
        MemoryRecord record = new MemoryRecord(DateTime.Now, TargetDesu.transform.position, true);

        // Check if there's already a a previous memory record for this target
        if (MemoryMap.ContainsKey(TargetDesu))
        {
            // Overwrite it if true
            MemoryMap[TargetDesu] = record;
        }

        else
        {
            // Add it if false
            MemoryMap.Add(TargetDesu, record);
        }
    }

    // This update is for debugging. It exposes values in the inspector to see if they work
    void Update()
    {
        SensedObjects = new GameObject[MemoryMap.Keys.Count];
        SensedRecord = new MemoryRecord[MemoryMap.Values.Count];
        MemoryMap.Keys.CopyTo(SensedObjects, 0);
        MemoryMap.Values.CopyTo(SensedRecord, 0);
    }
}

[Serializable]
public class MemoryRecord
{
    [SerializeField]
    public DateTime TimeLastSensed; // Time when the target was last sensed

    [SerializeField]
    public Vector3 LastSensedPos; // Pos where the target was sensed

    [SerializeField]
    public bool WithinFoV; // Checks whether target is currently in FoV

    [SerializeField]
    public string TimeLastSenseStr; // For debugging, convert DateTime to string to view in inspector


    public MemoryRecord()
    {
        TimeLastSensed = DateTime.MinValue;
        LastSensedPos = Vector3.zero;
        WithinFoV = false;
    }

    public MemoryRecord(DateTime TimeDesu, Vector3 PosDesu, bool FoVDesu)
    {
        TimeLastSensed = TimeDesu;
        LastSensedPos = PosDesu;
        WithinFoV = FoVDesu;
        TimeLastSenseStr = TimeDesu.ToLongTimeString();
    }

}
