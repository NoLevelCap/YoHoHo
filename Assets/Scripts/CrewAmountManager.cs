using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewAmountManager : MonoBehaviour {

    private Text CrewUsage, CrewTotal;

	// Use this for initialization
	void Start () {
        Text[] t = GetComponentsInChildren<Text>();
        CrewUsage = t[0];
        CrewTotal = t[2];
	}
	
	// Update is called once per frame
	void Update () {
        CrewUsage.text = GameManager.CrewUsage + "";
        CrewTotal.text = GameManager.CrewAmount + "";
	}
}
