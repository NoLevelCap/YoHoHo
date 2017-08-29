using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour {

    public InventoryManager _IM;
    public GameObject[] Windows;

	// Use this for initialization
	void Start () {
        _IM = FindObjectOfType<InventoryManager>();
        CloseAllWindows();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseAllWindows()
    {
        for (int i = 0; i < Windows.Length; i++)
        {
            Windows[i].SetActive(false);
        }
    }

    public void OpenWindow(GameObject WindowToOpen)
    {
        WindowToOpen.SetActive(true);
    }

    public void Submit()
    {
        GameManager.gm.EquippedFire = (FireComponent) _IM.FireSlot.GetItem();
        GameManager.gm.EquippedFlank = (FlankComponent)_IM.FlankSlot.GetItem();
        GameManager.gm.EquippedFlee = (FleeComponent)_IM.FleeSlot.GetItem();

        GameManager.gm.LoadShipVoyageScreen();
    }
}
