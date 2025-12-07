using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<String> itemList = new List<String>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;

    //Pickup Popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;


    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemStack stack = slot.transform.GetChild(0).GetComponent<ItemStack>();
                if (stack != null && stack.itemName == itemName)
                {
                    count += stack.quantity;
                }
            }
        }
        return count;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
        GameObject existingStack = FindStackableSlot(itemName);

        if (existingStack != null)
        {
            ItemStack stack = existingStack.transform.GetChild(0).GetComponent<ItemStack>();
            stack.AddToStack(1);
        }
        else
        {
            whatSlotToEquip = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            ItemStack newStack = itemToAdd.GetComponent<ItemStack>();
            if (newStack == null)
            {
                newStack = itemToAdd.AddComponent<ItemStack>();
            }
            newStack.itemName = itemName;
            newStack.quantity = 1;
            newStack.UpdateQuantityDisplay();

            if (itemToAdd.GetComponent<InventoryItemTooltip>() == null)
            {
                itemToAdd.AddComponent<InventoryItemTooltip>();
            }
        }

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
    }


    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;



    }

    private GameObject FindStackableSlot(string itemName)
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemStack stack = slot.transform.GetChild(0).GetComponent<ItemStack>();
                if (stack != null && stack.itemName == itemName && stack.CanAddToStack())
                {
                    return slot;
                }
            }
        }
        return null;
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == slotList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int remainingToRemove = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                ItemStack stack = slotList[i].transform.GetChild(0).GetComponent<ItemStack>();

                if (stack != null && stack.itemName == nameToRemove && remainingToRemove > 0)
                {
                    if (stack.quantity <= remainingToRemove)
                    {
                        remainingToRemove -= stack.quantity;
                        Destroy(slotList[i].transform.GetChild(0).gameObject);
                    }
                    else
                    {
                        stack.RemoveFromStack(remainingToRemove);
                        remainingToRemove = 0;
                    }
                }
            }
        }

        ReCalculateList();
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemStack stack = slot.transform.GetChild(0).GetComponent<ItemStack>();
                if (stack != null)
                {
                    for (int i = 0; i < stack.quantity; i++)
                    {
                        itemList.Add(stack.itemName);
                    }
                }
            }
        }
    }
}
