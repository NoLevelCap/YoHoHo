using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    public Item holding;

    private Image ItemImage;
    private Transform sTransform;
    private RectTransform rt, ot;
    private Camera cam;

    public ItemSlot hoveredItemSlot, equippedItemSlot;

    private bool Dragging;

    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        cam = FindObjectOfType<Camera>();
        ot = GameObject.Find("BackCanvas").GetComponent<RectTransform>();
        ItemImage = GetComponentsInChildren<Image>()[1];

        rt.sizeDelta = new Vector2(64, 64);
    }

    private void Update()
    {
        if (Dragging) {
            Vector2 output;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ot, Input.mousePosition, cam, out output);
            rt.anchoredPosition = output;
        }
    }

    public bool TryEquip(ItemSlot slot)
    {
        Debug.Log(slot);
        if (slot.Occupied)
            return false;

        if (!slot.EquipItem(this))
            return false;

        
        transform.SetParent(slot.transform, false);
        rt.anchorMin = Vector2.one * 0.5f;
        rt.anchorMax = Vector2.one * 0.5f;
        equippedItemSlot = slot;
        return true;
    }

    public void Unequip()
    {
        transform.SetParent(sTransform, false);
        equippedItemSlot = null;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(sTransform, false);

        Dragging = false;

        if (hoveredItemSlot != null)
        {
            if (!hoveredItemSlot.Occupied)
                TryEquip(hoveredItemSlot);
        }
        else if (equippedItemSlot)
            equippedItemSlot.UnequipItem();

            

        rt.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(sTransform == null)
            sTransform = GameObject.Find("Inventory").transform;
        transform.SetParent(ot.transform, false);

        rt.anchorMin = Vector2.one * 0.5f;
        rt.anchorMax = Vector2.one * 0.5f;

        Dragging = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            ItemSlot slot = collision.GetComponent<ItemSlot>();
            if (!slot.Occupied && slot.isItemValid(this))
            {
                hoveredItemSlot = collision.GetComponent<ItemSlot>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            ItemSlot slot = collision.GetComponent<ItemSlot>();
            if (slot == hoveredItemSlot)
            {
                hoveredItemSlot.Occupied = false;
                hoveredItemSlot = null;
            }
        }
    }

    public void SetItem(Item holding)
    {
        Debug.Log(holding.GetIcon());
        ItemImage.sprite = holding.GetIcon();
        this.holding = holding;
    }
}
