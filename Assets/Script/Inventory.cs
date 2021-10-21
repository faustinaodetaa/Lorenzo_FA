
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

    public List<Item> itemsAvailable; //ada item apa aja
    public List<InventorySlotDisplay> inventorySlotsDisplay;
    public Player player;

    private List<InventorySlot> slots; //isi slot
    private int maxSlot = 6;

    public void AddItem(string itemName)
    {
        Item item = new Item();
        foreach (Item i in itemsAvailable)
        {
            if (itemName.Contains(i.name))
            {
                item = i;
                Debug.Log("found " + itemName);
                break;
            }
        }

        //insert
        if (item != null)
        {
            for (int i = 0; i < maxSlot; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].item = item;
                    Debug.Log("masuk");
                }

                if (slots[i].item == item)
                {
                    slots[i].qty++;
                    Debug.Log("tambah");
                    break;
                }
            }
        }
    }

    public void UseItem(int itemSlot)
    {
        itemSlot--;
        InventorySlot use = slots[itemSlot];
        if (use.item != null)
        {
            if (use.item.name.Equals("ItemAmmo"))
            {
                player.useAmmo();
            }
            else if (use.item.name.Equals("ItemShield"))
            {
                player.useShield();
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
                player.usePainKiller();
            }
            else if (use.item.name.Equals("ItemDamageMultiplier"))
            {
                player.useDamageMultiplier();
            }
            use.qty--;
            inventorySlotsDisplay[itemSlot].qty.text = use.qty.ToString();
            if (use.qty <= 0)
                removeItem(itemSlot);
        }
    }

    private void removeItem(int itemSlot)
    {
        slots.RemoveAt(itemSlot);

        InventorySlot s = new InventorySlot();
        s.item = null;
        s.qty = 0;

        slots.Add(s);

        updateInventory();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        slots = new List<InventorySlot>(maxSlot + 1);
        initSlots();
    }

    private void initSlots()
    {
        for (int i = 0; i < maxSlot; i++)
        {
            InventorySlot s = new InventorySlot();
            s.item = null;
            s.qty = 0;

            slots.Add(s);
        }
    }
    void updateInventory()
    {
        for (int i = 0; i < 6; i++)
        {
            inventorySlotsDisplay[i].img.sprite = null;
            inventorySlotsDisplay[i].qty.enabled = false;

            //var color = inventorySlotsDisplay[i].img.color;
            //color.a = 0f;
            //inventorySlotsDisplay[i].img.color = color;
        }

        for (int i = 0; i < 6; i++)
        {
            if (slots[i].item != null)
            {
                inventorySlotsDisplay[i].img.sprite = slots[i].item.sprite;

                //var color = inventorySlotsDisplay[i].img.color;
                //color.a = 1f;
                //inventorySlotsDisplay[i].img.color = color;
                inventorySlotsDisplay[i].qty.enabled = true;
                inventorySlotsDisplay[i].qty.text = slots[i].qty.ToString();
                Debug.Log("item ke " + i);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateInventory();
    }
}

