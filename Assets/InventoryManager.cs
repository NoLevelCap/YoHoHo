using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public GameObject ItemPrefab;
    public ItemSlot FireSlot, FlankSlot, FleeSlot;

    private Transform Inventory;
    private List<ItemHolder> PlaceHolders;

	// Use this for initialization
	void Start () {
        Inventory = transform.Find("Content").Find("Inventory");

        PlaceHolders = new List<ItemHolder>();

        LoadItems();
        EquipBasicItems();
    }

    private void EquipBasicItems()
    {
        foreach (ItemHolder it in PlaceHolders)
        {
            if ((Object)it.holding == GameManager.gm.EquippedFire)
                it.TryEquip(FireSlot);
            else if ((Object)it.holding == GameManager.gm.EquippedFlank)
                it.TryEquip(FlankSlot);
            else if ((Object)it.holding == GameManager.gm.EquippedFlee)
                it.TryEquip(FleeSlot);
        }
    }

    private void LoadItems()
    {
        Debug.Log(GameManager.Items);
        foreach (Transform item in Inventory)
        {
            DestroyImmediate(item.gameObject);
        }
        foreach (Item item in GameManager.Items)
        {
            GameObject gj = Instantiate<GameObject>(ItemPrefab);
            gj.transform.SetParent(Inventory, false);

            ItemHolder it = gj.GetComponent<ItemHolder>();
            it.SetItem(item);
            PlaceHolders.Add(it);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
