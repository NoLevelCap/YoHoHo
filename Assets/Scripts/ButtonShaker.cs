using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShaker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool active;
    private Vector3 sPos;
    private float sTime;

    
    public float range, speed;

    private void Update()
    {
        if (active)
        {
            transform.localPosition = sPos + (Vector3.up * Mathf.Sin((Time.time - sTime) * speed) * range);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        sPos = transform.localPosition;
        active = true;
        sTime = Time.time;

        transform.localPosition = sPos + (Vector3.up * Mathf.Sin(0) * range);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = sPos;
        active = false;
    }
}
