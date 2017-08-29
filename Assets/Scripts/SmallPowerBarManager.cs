using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallPowerBarManager : MonoBehaviour {

    public PowerValues powerValue;

    private Text value;

	// Use this for initialization
	void Start () {
        value = transform.Find("Value").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        value.text = GameManager.GetPowerValue(powerValue) + "";
	}
}
