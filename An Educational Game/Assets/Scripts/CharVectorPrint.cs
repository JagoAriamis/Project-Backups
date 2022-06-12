using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharVectorPrint : MonoBehaviour
{
    public GameObject player;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerRotationx = player.transform.eulerAngles.x;
        float playerRotationy = player.transform.eulerAngles.y;
        float playerRotationz = player.transform.eulerAngles.z;
        text.text = "Position = (X: " + player.transform.position.x.ToString("F0") + ", Y: " + player.transform.position.y.ToString("F0") + ", Z: " + player.transform.position.z.ToString("F0") + ")" +
            "\n" + "Rotation (Deg) = (X: " + playerRotationx + ", Y: " + playerRotationy.ToString("F0") + ", Z: " + playerRotationz + ")";
    }
}
