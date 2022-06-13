using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanics : MonoBehaviour
{
    GameObject boss;

    bool mechSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (mechSelected == false)
        {
            MechanicSelect();
        }
    }

    void MechanicSelect()
    {
        int mechanicSelector = Random.Range(0, 2);

        if (mechanicSelector == 1)
        {
            mechSelected = true;
            StartCoroutine(NearsightCast());
        }
        else
        {
            mechSelected = true;
            StartCoroutine(FarsightCast());
        }
    }

    void Nearsight()
    {
        Debug.Log("Cast Nearsight");
    }

    void Farsight()
    {
        Debug.Log("Cast Farsight");
    }

    IEnumerator NearsightCast()
    {
        Debug.Log("Nearsight");
        yield return new WaitForSeconds(5);
        Nearsight();
    }

    IEnumerator FarsightCast()
    {
        Debug.Log("Farsight");
        yield return new WaitForSeconds(5);
        Farsight();
    }
}
