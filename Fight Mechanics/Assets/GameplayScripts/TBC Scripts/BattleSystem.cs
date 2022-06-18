using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, VICTORY, DEFEAT }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject player;
    public GameObject enemy;

    UnitInfo playerUnit;
    UnitInfo enemyUnit;

    public TextMeshProUGUI battleText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Animator playerHit;

    public Animator enemyHit;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = player;
        playerUnit = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGO = enemy;
        enemyUnit = enemyGO.GetComponent<UnitInfo>();

        battleText.text = "Here comes a new challenger! " + enemyUnit.unitName + " is here to throw down!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        battleText.text = "Choose your action: ";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerHeal());
    }
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
       
        enemyHit.enabled = true;
        enemyHit.Play("HitAnim");

        battleText.text = "Super effective!";

        yield return new WaitForSeconds(0.5f);
        enemyHit.enabled = false;

        yield return new WaitForSeconds(3f);

        if (isDead)
        {
            state = BattleState.VICTORY;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(3);

        playerHUD.SetHP(playerUnit.currentHP);
        battleText.text = "You feel invigorated once more!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        battleText.text = enemyUnit.unitName + " is making their move..";
        yield return new WaitForSeconds(5f);

        battleText.text = enemyUnit.unitName + " makes a wild attack!";
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        playerHit.enabled = true;
        playerHit.Play("HitAnim");

        yield return new WaitForSeconds(0.5f);

        playerHit.enabled = false;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.DEFEAT;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.VICTORY)
        {
            battleText.text = "KO! You win!";
        }
        else if (state == BattleState.DEFEAT)
        {
            battleText.text = "Mission failed! We'll get 'em next time..";
        }

    }
}
