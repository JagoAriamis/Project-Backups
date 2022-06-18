using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossMechanics : MonoBehaviour
{
    public GameObject magicBoom;

    bool mechSelected = false;

    public Slider CastBar;
    TextMeshProUGUI SkillText;

    FieldOfView FoV;

    [HideInInspector]
    public float targetRing;

    [HideInInspector]
    public int mechNum = 0;

    public delegate void NearsightFarsight();
    public NearsightFarsight handle;

    // Start is called before the first frame update
    void Start()
    {
        SkillText = GameObject.Find("Skill Text").GetComponent<TextMeshProUGUI>();
        CastBar.gameObject.SetActive(false);
        FoV = GetComponent<FieldOfView>();
        targetRing = FoV.viewRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (mechSelected == false)
        {
            MechanicSelect();
        }

        if (handle != null)
        {
            handle();
        }
    }

    void MechanicSelect()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        mechNum = 0;

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

        Transform target1 = FoV.targetDistanceList[0];
        Transform target2 = FoV.targetDistanceList[1];

        Instantiate(magicBoom, new Vector3(target1.position.x, 0, target1.position.z), Quaternion.identity);

        Instantiate(magicBoom, new Vector3(target2.position.x, 0, target2.position.z), Quaternion.identity);

        Debug.Log(target1.position);
        Debug.Log(target2.position);

        mechSelected = false;
        handle -= Nearsight;
    }

    void Farsight()
    {
        Debug.Log("Cast Farsight");
        CastBar.gameObject.SetActive(false);
        CastBarReset();

        Transform last = FoV.targetDistanceList[FoV.targetDistanceList.Count - 1];
        Transform secondToLast = FoV.targetDistanceList[FoV.targetDistanceList.Count - 2];

        Instantiate(magicBoom, new Vector3(last.position.x, 0, last.position.z), Quaternion.identity);
        Instantiate(magicBoom, new Vector3(secondToLast.position.x, 0, secondToLast.position.z), Quaternion.identity);

        Debug.Log(last.position);
        Debug.Log(secondToLast.position);

        mechSelected = false;
        handle -= Farsight;
    }

    IEnumerator NearsightCast()
    {
        SkillText.SetText("Nearsight");
        mechNum = 1;
        CastBar.gameObject.SetActive(true);
        StartCoroutine(CastBarFill(5f));
        yield return new WaitForSeconds(5);
        handle += Nearsight;
    }

    IEnumerator FarsightCast()
    {
        SkillText.SetText("Farsight");
        mechNum = 2;
        CastBar.gameObject.SetActive(true);
        StartCoroutine(CastBarFill(5f));
        yield return new WaitForSeconds(5);
        handle += Farsight;
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
