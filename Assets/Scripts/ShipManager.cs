using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    private CombatManager _CombatManager;
    private SailManager _SailManager;
    private Camera _MainCam;
    private RectTransform _InfoArea;
    private InfoTabManager _InfoTabMan;

    private int Health, MaxHealth;
    private int Cannons, Sails, Hull, Cooks;
    public float shift;
    public bool flanked;

    private bool player;

    public FireComponent FireComponent;
    public FlankComponent FlankComponent;
    public FleeComponent FleeComponent;


    // Use this for initialization
    void Start () {
        _CombatManager = FindObjectOfType<CombatManager>();
        _SailManager = FindObjectOfType<SailManager>();
        _MainCam = FindObjectOfType<Camera>();
        _InfoTabMan = _SailManager.InfoTab.GetComponent<InfoTabManager>();
        _InfoArea = _SailManager.InfoTab.parent.parent.GetComponent<RectTransform>();

        if (gameObject.tag == "Player")
        {
            SetComponents();
            UpdateValues();
            player = true;
        }

        
    }

    // Update is called once per frame
    void Update () {
        transform.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Sin(Time.time + shift) * 5f);

        if (GameManager.ValueChange && player)
            UpdateValues();
    }

    void SetComponents()
    {
        FireComponent = GameManager.gm.EquippedFire;
        FlankComponent = GameManager.gm.EquippedFlank;
        FleeComponent = GameManager.gm.EquippedFlee;
    }

    void UpdateValues()
    {
        Cannons = GameManager.GetPowerValue(PowerValues.Cannon);
        Sails = GameManager.GetPowerValue(PowerValues.Sail);
        Cooks = GameManager.GetPowerValue(PowerValues.Cook);
        Hull = GameManager.GetPowerValue(PowerValues.Hull);
    }

    void Die()
    {
        _CombatManager.KillShip(this);
    }

    public void ChangeHealth(int amount)
    {
        Debug.Log("Health " + Health + " to " + (Health+amount));
        Health += amount;

        if (Health > MaxHealth || MaxHealth == 0)
            MaxHealth = Health;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void SetHealth(int amount)
    {
        Health = amount;

        if (Health > MaxHealth || MaxHealth == 0)
            MaxHealth = Health;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
    }

    public void SetCannons(int amount)
    {
        Cannons = amount;
    }

    public void SetSails(int amount)
    {
        Sails = amount;
    }

    public void SetHull(int amount)
    {
        Hull = amount;
    }

    public void SetCooks(int amount)
    {
        Cooks = amount;
    }

    public int GetCannons()
    {
        return Cannons;
    }

    public int GetSails()
    {
        return Sails;
    }

    public int GetCooks()
    {
        return Cooks;
    }

    public int GetHull()
    {
        return Hull;
    }

    public int GetHealth()
    {
        return Health;
    }

    private void OnMouseOver()
    {
        if(!_SailManager.InfoTab.gameObject.activeSelf)
            _SailManager.InfoTab.gameObject.SetActive(true);

        Vector2 MouseViewport = _MainCam.ScreenToViewportPoint(Input.mousePosition);

        _SailManager.InfoTab.anchoredPosition = new Vector2(
             ((MouseViewport.x * _InfoArea.sizeDelta.x)),
             ((MouseViewport.y * _InfoArea.sizeDelta.y)));

        _SailManager.InfoTab.anchoredPosition -= new Vector2(
            Mathf.Max(0, _SailManager.InfoTab.anchoredPosition.x + _SailManager.InfoTab.sizeDelta.x - _InfoArea.sizeDelta.x),
            Mathf.Max(0, _SailManager.InfoTab.anchoredPosition.y + _SailManager.InfoTab.sizeDelta.y - _InfoArea.sizeDelta.y));

        _InfoTabMan.HBar.fillAmount = (float) Health / MaxHealth;
        _InfoTabMan.HealthOver.text = _InfoTabMan.HealthUnder.text = Health + "/" + MaxHealth;
        _InfoTabMan.Sails.text = Sails.ToString();
        _InfoTabMan.Cannons.text = Cannons.ToString();
        _InfoTabMan.Cooks.text = Cooks.ToString();
        _InfoTabMan.Hull.text = Hull.ToString();
    }

    private void OnMouseExit()
    {
        _SailManager.InfoTab.gameObject.SetActive(false);
    }
}
