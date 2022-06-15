using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBehaviour : MonoBehaviour
{
    public GameObject[] Tanks;
    public GameObject[] Healers;
    public GameObject[] DPS;

    public GameObject boss;

    public delegate void PartyReaction();
    public PartyReaction partyReaction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        partyReaction();
    }

    void TanksIn()
    {
        for (int i = 0; i < 8; i++)
        {
            Tanks[i].transform.position = boss.transform.position - Tanks[i].transform.position.normalized * 5f;
            Healers[i].transform.position = Healers[i].transform.position - boss.transform.position.normalized * 5f;
            DPS[i].transform.position = DPS[i].transform.position - boss.transform.position.normalized * 5f;
        }
    }

    void TanksOut()
    {
        for (int i = 0; i < 8; i++)
        {
            Tanks[i].transform.position = Tanks[i].transform.position - boss.transform.position.normalized * 5f; 
            Healers[i].transform.position = boss.transform.position - Healers[i].transform.position.normalized * 5f;
            DPS[i].transform.position = boss.transform.position - DPS[i].transform.position.normalized * 5f;
        }
    }
}
