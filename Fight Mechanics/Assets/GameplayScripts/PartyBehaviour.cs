using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBehaviour : MonoBehaviour
{
    public GameObject[] Tanks;
    public GameObject[] Healers;
    public GameObject[] DPS;

    BossMechanics boss;
    FieldOfView FoV;
    BossTargetRing hitBox;

    Vector3 bossPos;
    Vector3 fleePos;
    public float safeDis = 224f;

    [SerializeField]
    bool fleeCalculationIsDone;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("BossEmpty").GetComponent<BossMechanics>();
        FoV = GameObject.Find("BossEmpty").GetComponent<FieldOfView>();
        hitBox = GameObject.Find("BossEmpty").GetComponent<BossTargetRing>();
        bossPos = boss.transform.position;
        fleeCalculationIsDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        fleeCalculationIsDone = false;

        TankSpacing();

        Debug.Log("The flee pos is " + fleePos);

        if (boss.mechNum == 1)
        {
            TanksIn();
        }

        if (boss.mechNum == 2)
        {
            TanksOut();
        }

        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        
        if (!fleeCalculationIsDone)
        {
            fleePos = CalculateFleePos(hitBox.radius);
            fleeCalculationIsDone = true;
        }
    }

    void TanksIn()
    {
        foreach (GameObject tank in Tanks)
        {
            Vector3 directionToMove = Vector3.MoveTowards(tank.transform.position, bossPos, 5f * Time.deltaTime);
            tank.transform.position = directionToMove;
        }

        foreach (GameObject healer in Healers)
        {
            if (FoV.DistanceToTarget(healer.transform.position, bossPos) >= safeDis)
            {
                healer.transform.position = healer.transform.position;
            }
            else
            {
                Vector3 directionToMove = Vector3.MoveTowards(healer.transform.position, fleePos, 5f * Time.deltaTime);
                healer.transform.position = directionToMove;
            }
        }

        foreach (GameObject dps in DPS)
        {
            if (FoV.DistanceToTarget(dps.transform.position, bossPos) >= safeDis)
            {
                dps.transform.position = dps.transform.position;
            }
            else
            {
                Vector3 directionToMove = Vector3.MoveTowards(dps.transform.position, fleePos, 6f * Time.deltaTime);
                dps.transform.position = directionToMove;
            }
        }
    }

    void TanksOut()
    {
        foreach (GameObject tank in Tanks)
        {
            if (FoV.DistanceToTarget(tank.transform.position, bossPos) >= safeDis)
            {
                tank.transform.position = tank.transform.position;
            }
            else
            {
                Vector3 directionToMove = Vector3.MoveTowards(tank.transform.position, fleePos, 5f * Time.deltaTime);
                tank.transform.position = directionToMove;
            }
        }

        foreach (GameObject healer in Healers)
        {
            Vector3 directionToMove = Vector3.MoveTowards(healer.transform.position, bossPos, 5f * Time.deltaTime);
            healer.transform.position = directionToMove;
        }

        foreach (GameObject dps in DPS)
        {
            Vector3 directionToMove = Vector3.MoveTowards(dps.transform.position, bossPos, 6f * Time.deltaTime);
            dps.transform.position = directionToMove;
        }
    }

    void TankSpacing()
    {
        float distance = 5f;

        GameObject tank1 = Tanks[0];
        GameObject tank2 = Tanks[1];

        tank1.transform.position = (tank1.transform.position - tank2.transform.position).normalized * distance + tank2.transform.position;
        tank2.transform.position = (tank2.transform.position - tank1.transform.position).normalized * distance + tank1.transform.position;

    }

    Vector3 CalculateFleePos(float radius)
    {
        float theta = 2.0f * Mathf.PI;
        float angle = Random.Range(0f, theta);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(x, 1, z);
    }
}
