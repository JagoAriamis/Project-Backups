using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpBar;

    public void SetHUD(UnitInfo unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpBar.value = hp;
    }
}
