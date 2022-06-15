using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBehaviour : MonoBehaviour
{
    public GameObject[] Tanks;
    public GameObject[] Healers;
    public GameObject[] DPS;

    BossMechanics bossMechanics;

    // Start is called before the first frame update
    void Start()
    {
        bossMechanics = GameObject.Find("Boss").GetComponent<BossMechanics>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
