using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EffectController : MonoBehaviour {

    public Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Debug.Log("ANC");
        Graphics.Blit(source, destination, mat);
        GL.Clear(false, false, Color.red);
    }
}
