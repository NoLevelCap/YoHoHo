using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerBarButton : MonoBehaviour, IPointerClickHandler {

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(state);
    }

    private bool state = false;

    void SwitchState(bool on)
    {
        if (state != on)
        {
            state = on;
            transform.GetChild(0).gameObject.SetActive(state);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<PowerBarManager>().OnButtonPress(transform.GetSiblingIndex());
    }
}
