using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform child in transform)
        {
            child.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Sin(Time.time + child.localPosition.x * .1f) * 30f);
        }
	}
}
