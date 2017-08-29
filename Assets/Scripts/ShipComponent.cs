using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ShipComponent : ScriptableObject, Item {
    [SerializeField]
    public Sprite Image, SmallImage;
    public string ComponentName, ComponentDesc;

    private bool equipped;


    public virtual void Fire(ShipManager PlayerShip, ShipManager EnemyShip) { }

    public void Equip()
    {
        throw new NotImplementedException();
    }

    public bool GetEquipped()
    {
        return equipped;
    }

    public string GetName()
    {
        return ComponentName;
    }

    public Sprite GetIcon()
    {
        return SmallImage;
    }
}

public interface Item
{
    bool GetEquipped();
    string GetName();
    Sprite GetIcon();

    void Equip();
}
