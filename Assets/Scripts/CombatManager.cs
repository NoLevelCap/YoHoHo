using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    private static Text CombatLogText;

    public GameObject EnemyShipPrefab;

    private GameObject CombatUI, CombatLog;
    private ShipManager EnemyShip, PlayerShip, KilledShip;

    private float shipRotateSpeed = 1f;
    private bool shipMoving, playersTurn, inCombat, shipDying, fleeing;
    private float startMovingTime, lastTurnTime;

    private float duration = 4f, turnDuration;

    public Vector3 finalLoc, startLoc;

    private Vector3 finalMoveLoc = new Vector3(4.67f, -2f, 0f);

	// Use this for initialization
	void Start () {
        CombatUI = GameObject.Find("Combat UI");
        PlayerShip = GameObject.Find("Ship").GetComponent<ShipManager>();
        CombatLog = GameObject.Find("CombatLog");
        CombatLogText = CombatLog.GetComponentInChildren<Text>();

        CombatLogText.text = "";

        CombatLog.SetActive(false);
        HideCombatUI();

        LoadPlayerShipData();
    }

    public void LoadPlayerShipData() {

        PlayerShip.SetMaxHealth(100 + 20 * GameManager.GetPowerValue(PowerValues.Hull));
        PlayerShip.SetHealth(GameManager.PlayerShipHealth);
    }

    public void SubmitPlayerShipData()
    {
        GameManager.PlayerShipHealth = PlayerShip.GetHealth();
    }

    // Update is called once per frame
    void Update () {
        if (inCombat)
            CombatLoop();
    }

    void CombatLoop()
    {
        if (shipDying)
        {
            KilledShip.transform.localPosition = Vector3.LerpUnclamped(startLoc, finalLoc, (Time.time - startMovingTime) / duration);

            if ((Time.time - startMovingTime) / duration >= 1f)
            {
                shipDying = false;

                EndCombat();
            }
        }
        else if (fleeing)
        {
            EnemyShip.transform.localPosition = Vector3.LerpUnclamped(startLoc, finalLoc, (Time.time - startMovingTime) / duration);

            if ((Time.time - startMovingTime) / duration >= 1f)
            {
                fleeing = false;

                EndCombat();
            }
        }
        else if (shipMoving)
        {
            EnemyShip.transform.localPosition = Vector3.LerpUnclamped(startLoc, finalLoc, (Time.time - startMovingTime) / duration);

            if ((Time.time - startMovingTime) / duration >= 1f)
            {
                shipMoving = false;
                shipRotateSpeed = 1f;

                lastTurnTime = Time.time;
                turnDuration = Random.Range(1, 5);

                CombatLog.SetActive(true);
                ShowCombatUI();
            }
        }
        else if ((Time.time - lastTurnTime) > turnDuration && !playersTurn)
        {
            EnemyTurn();
        }

    }

    

    void EnemyTurn()
    {
        ShowCombatUI();

        if (Random.value < 0.25f && EnemyShip.GetHealth() > 30)
        {
            EnemyShip.FlankComponent.Fire(EnemyShip, PlayerShip);

        } else if (Random.value < 0.5f && EnemyShip.GetHealth() < 30)
        {
            EnemyShip.FleeComponent.Fire(EnemyShip, PlayerShip);
        } else
        {
            EnemyShip.FireComponent.Fire(EnemyShip, PlayerShip);
        }

        SwitchTurn();
    }

    void ShowCombatUI()
    {
        CombatUI.SetActive(true);
    }

    void HideCombatUI()
    {
        CombatUI.SetActive(false);
    }

    public void LoadEnemyShip()
    {
        GameObject EShip = Instantiate<GameObject>(EnemyShipPrefab);
        EnemyShip = EShip.GetComponent<ShipManager>();

        int nHealth = Mathf.FloorToInt((Random.value * 10 * (GameManager.GetPowerValue(PowerValues.Cannon) / 12.0f) * 0.7f) + 5) * 10;
        EnemyShip.ChangeHealth(nHealth);

        int nAttack = Mathf.Min(Mathf.Max(Mathf.FloorToInt(((Random.value * 6) + ((GameManager.GetPowerValue(PowerValues.Cannon) + GameManager.GetPowerValue(PowerValues.Hull)) / 2) - 4)), 1), 12);
        EnemyShip.SetCannons(nAttack);

        int nSpeed = Mathf.FloorToInt((Random.value * 6) + 3);
        EnemyShip.SetSails(nSpeed);

        startLoc = EShip.transform.localPosition;
        finalLoc = new Vector3(finalMoveLoc.x, finalMoveLoc.y, startLoc.z);
    }

    public void StartCombat()
    {
        CombatLogText.text = "";

        shipMoving = true;
        inCombat = true;
        playersTurn = true;
        startMovingTime = Time.time;

        LoadEnemyShip();

        shipRotateSpeed = 1f;

        PostToCombatLog("An enemy ship approaches!");
    }


    public void EndCombat()
    {
        inCombat = false;

        CombatLog.SetActive(false);
        HideCombatUI();

        SailManager.UnPauseMovement();

        if (KilledShip == PlayerShip)
        {
            Debug.Log("Game is Lost");
            Application.Quit();
            Debug.Break();
        }

        Destroy(KilledShip.gameObject);
    }

    public void KillShip(ShipManager killship)
    {
        shipDying = true;
        KilledShip = killship;

        startLoc = KilledShip.transform.localPosition;
        finalLoc = startLoc - new Vector3(0, 7.5f);

        startMovingTime = Time.time;
    }

    public void Fire()
    {
        
        PlayerShip.FireComponent.Fire(PlayerShip, EnemyShip);
        SwitchTurn();
    }

    public void Flank()
    {
        PlayerShip.FlankComponent.Fire(PlayerShip, EnemyShip);
        SwitchTurn();
    }

    public void Flee()
    {
        PlayerShip.FleeComponent.Fire(PlayerShip, EnemyShip);
        SwitchTurn();
    }

    public void ActiveFlee()
    {
        fleeing = true;
        finalLoc = EnemyShipPrefab.transform.localPosition;
        startLoc = finalMoveLoc + (Vector3.forward * finalLoc.z);
        startMovingTime = Time.time;
        KilledShip = EnemyShip;
    }

    public void SwitchTurn()
    {
        playersTurn = !playersTurn;

        if (playersTurn)
            CombatUI.SetActive(true);
        else
            CombatUI.SetActive(false);


        turnDuration = Random.Range(1, 5);
        lastTurnTime = Time.time;
    }

    public static void PostToCombatLog(string text)
    {
        CombatLogText.text += "\n" + text;
    }
}
