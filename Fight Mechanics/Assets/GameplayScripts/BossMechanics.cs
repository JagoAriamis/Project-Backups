using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossMechanics : MonoBehaviour
{
    GameObject boss;

    bool mechSelected = false;

    public Slider CastBar;
    TextMeshProUGUI SkillText;

    FieldOfView FoV;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Boss");
        SkillText = GameObject.Find("Skill Text").GetComponent<TextMeshProUGUI>();
        CastBar.gameObject.SetActive(false);
        FoV = GetComponent<FieldOfView>();
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
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

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
        CastBar.gameObject.SetActive(false);
        CastBarReset();

        Debug.Log(FoV.targetDistanceList[0].position);
        Debug.Log(FoV.targetDistanceList[1].position);

        mechSelected = false;
    }

    void Farsight()
    {
        Debug.Log("Cast Farsight");
        CastBar.gameObject.SetActive(false);
        CastBarReset();

        Transform last = FoV.targetDistanceList[FoV.targetDistanceList.Count - 1];
        Transform secondToLast = FoV.targetDistanceList[FoV.targetDistanceList.Count - 2];

        Debug.Log(last.position);
        Debug.Log(secondToLast.position);

        mechSelected = false;
    }

    IEnumerator NearsightCast()
    {
        SkillText.SetText("Nearsight");
        CastBar.gameObject.SetActive(true);
        StartCoroutine(CastBarFill(5f));
        yield return new WaitForSeconds(5);
        Nearsight();
    }

    IEnumerator FarsightCast()
    {
        SkillText.SetText("Farsight");
        CastBar.gameObject.SetActive(true);
        StartCoroutine(CastBarFill(5f));
        yield return new WaitForSeconds(5);
        Farsight();
    }

    IEnumerator CastBarFill(float castTime)
    {
        float fillTime = 0f;

        while (fillTime < castTime)
        {
            fillTime += Time.deltaTime;
            CastBar.value = Mathf.Lerp(CastBar.minValue, CastBar.maxValue, fillTime / castTime);
            yield return null;
        }
    }

    void CastBarReset()
    {
        CastBar.value = CastBar.minValue;
    }
}
