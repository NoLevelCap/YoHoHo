using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollingBackground : MonoBehaviour {

    Material BackTexture;

    [Range(0f, 1f)]
    public float Offset;

	// Use this for initialization
	void Start () {
        BackTexture = GetComponent<MeshRenderer>().sharedMaterial;
        DisplayOffset();
    }
	
	// Update is called once per frame
	void Update () {
        DisplayOffset();
    }

    private void DisplayOffset()
    {
        BackTexture.mainTextureOffset = Vector2.right * Offset;
    }
}
