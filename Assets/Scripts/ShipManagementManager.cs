using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManagementManager : MonoBehaviour {

    GameManager gm;

    private GameObject CurrentOpenTab;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        CurrentOpenTab = GameObject.Find("MapWindow");
    }

    public void Submit()
    {
        if (GameManager.SelectedNode != null)
        {
            if (!GameManager.SelectedNode.visited)
            {
                gm.LoadShipVoyageScreen();
            }
        }
    }

    public void SwitchTab(GameObject Tab)
    {
        if (Tab == CurrentOpenTab)
            return;
        CurrentOpenTab.SetActive(false);
        CurrentOpenTab = Tab;
        CurrentOpenTab.SetActive(true);
    }
}
