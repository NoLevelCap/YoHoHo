using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailManager : MonoBehaviour {

    public int[] activepoints;
    public RectTransform Ship;
    public Vector2 SPosition;
    public RectTransform InfoTab;

    private ScrollingBackground background;
    private float ProgressBarAmount;
    private int lastIcon;
    private EventManager _EventManager;
    private CombatManager _Combat;


    private static bool Paused;

    [Range(0, 1)]
    public float Range;

    // Use this for initialization
    void Start () {
        GameObject sh = GameObject.Find("SmallShip");
        Ship = sh.GetComponent<RectTransform>();
        SPosition = Ship.anchoredPosition;
        background = FindObjectOfType<ScrollingBackground>();
        ProgressBarAmount = (float) GameObject.Find("ProgressBarDetail").transform.Find("Nodes").childCount;

        _EventManager = GetComponent<EventManager>();
        _Combat = GetComponent<CombatManager>();
    }

    void OnAwake()
    {
        
    }
	
	void Update () {
        if (!Paused) {
            Move();
        }

        Ship.anchoredPosition = SPosition + new Vector2(1012,0) * Range;
	}


    public void Move()
    {
        Range += GameManager.ShipSpeed * Time.deltaTime;
        background.Offset = Range;

        if (Range >= 1)
        {
            Range = 1;
            GameManager.SelectedNode.visited = true;
            GameManager.gm.LoadShipManagementScreen();

            _Combat.SubmitPlayerShipData();
            return;
        }

        CheckForEvent();
    }

    

    private void CheckForEvent()
    {
        if (Range > (1f / (ProgressBarAmount-1)) * (lastIcon))
        {
            bool EventFire = IntArrayContains(activepoints, lastIcon);
            lastIcon += 1;

            if (lastIcon != 1) {
                OnMarkerPart(EventFire);
            }
        }
    }

    public void SkipToNextLocation()
    {

    }

    public static bool IntArrayContains(int[] i, int a)
    {
        for (int b = 0; b < i.Length; b++)
        {
            if (i[b] == a)
            {
                return true;
            }
        }
        return false;
    }

    public static void PauseMovement()
    {
        Paused = true;
    }

    public static void UnPauseMovement()
    {
        Paused = false;
    }

    void OnMarkerPart(bool Event)
    {
        if (Event)
        {
            PauseMovement();
            _EventManager.FireEvent();
        } else if(Random.value < 1f)
        {
            PauseMovement();
            _Combat.StartCombat();
        }
    }
}
