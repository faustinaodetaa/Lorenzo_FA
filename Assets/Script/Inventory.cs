
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public Sprite sprite;
    }

    public class InventorySlot
    {
        public Item item;
        public int qty;
    }

    [System.Serializable]
    public class InventorySlotDisplay
    {
        public Image img;
        public Text qty;
    }

    public List<Item> itemList; 
    public List<InventorySlotDisplay> inventorySlotsDisplay;
    public Player player;

    private List<InventorySlot> slotList; 
    private int maxSlot = 6;

    public void AddItem(string itemName)
    {
        Item item = new Item();
        foreach (Item i in itemList)
        {
            if (itemName.Contains(i.name))
            {
                item = i;
                Debug.Log("tes" + itemName + i.name);
                break;
            }
        }

        if (item != null)
        {
            for (int i = 0; i < maxSlot; i++)
            {
                if (slotList[i].item == null)
                {
                    slotList[i].item = item;
                }

                if (slotList[i].item == item)
                {
                    slotList[i].qty++;
                    break;
                }
            }
        }
    }

    public void UseItem(int itemSlot)
    {
        itemSlot--;
        InventorySlot use = slotList[itemSlot];
        if (use.item != null)
        {
            if (use.item.name.Equals("ItemAmmo"))
            {
                player.useAmmo();
            }
            else if (use.item.name.Equals("ItemShield"))
            {
                StartCoroutine(player.useShield());
            }
            else if (use.item.name.Equals("ItemHealthPotion"))
            {
                player.useHealthPotion();
            }
            else if (use.item.name.Equals("ItemSkillPotion"))
            {
                player.useSkillPotion();
            }
            else if (use.item.name.Equals("ItemPainKiller"))
            {
                StartCoroutine(player.usePainKiller());
            }
            else if (use.item.name.Equals("ItemDamageMultiplier"))
            {
                StartCoroutine(player.useDamageMultiplier());
            }
            use.qty--;
            inventorySlotsDisplay[itemSlot].qty.text = use.qty.ToString();
            if (use.qty <= 0)
                removeItem(itemSlot);
        }
    }

    private void removeItem(int itemSlot)
    {
        slotList.RemoveAt(itemSlot);

        InventorySlot inventories = new InventorySlot();
        inventories.item = null;
        inventories.qty = 0;

        slotList.Add(inventories);

        updateInventory();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        slotList = new List<InventorySlot>(maxSlot + 1);
        initSlots();
    }

    private void initSlots()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            InventorySlot inventories = new InventorySlot();
            inventories.item = null;
            inventories.qty = 0;

            slotList.Add(inventories);
        }
    }
    void updateInventory()
    {
        for (int i = 0; i < 6; i++)
        {
            inventorySlotsDisplay[i].img.sprite = null;
            inventorySlotsDisplay[i].qty.enabled = false;
        }

        for (int i = 0; i < 6; i++)
        {
            if (slotList[i].item != null)
            {
                inventorySlotsDisplay[i].img.sprite = slotList[i].item.sprite;
                inventorySlotsDisplay[i].qty.enabled = true;
                inventorySlotsDisplay[i].qty.text = slotList[i].qty.ToString();
                //Debug.Log("item ke " + i);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateInventory();
    }
}

