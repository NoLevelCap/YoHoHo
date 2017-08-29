using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

    private Text InformationText;

    private ItemHolder item;
    public bool Occupied;
    public TypeOfSlot ObjectType;

    public void OnEnable()
    {
        InformationText = transform.parent.Find("SMText").GetComponent<Text>();
        InformationText.text = "Unequipped";
    }

    public Item GetItem()
    {
        return item.holding;
    }

    public bool EquipItem(ItemHolder item)
    {
        if (!isItemValid(item))
            return false;

        this.item = item;
        Occupied = true;
        InformationText.text = item.holding.GetName();
        return true;
    }

    public void UnequipItem()
    {
        item.Unequip();
        Occupied = false;
        InformationText.text = "Unequipped";
    }

    public bool MatchItem(Item i)
    {
        return (object)i == item;
    }

    public bool isItemValid(ItemHolder usedItem)
    {
        
        switch (ObjectType)
        {
            case TypeOfSlot.FireComponent:
                return usedItem.holding.GetType() == typeof(FireComponent);
            case TypeOfSlot.FlankComponent:
                return usedItem.holding.GetType() == typeof(FlankComponent);
            case TypeOfSlot.FleeComponent:
                return usedItem.holding.GetType() == typeof(FleeComponent);
            case TypeOfSlot.GenericItem:
                return usedItem.holding.GetType() == typeof(Item);
            default:
                return false;
        }
    }

    public enum TypeOfSlot{
        FireComponent, FlankComponent, FleeComponent, GenericItem
    }
}
