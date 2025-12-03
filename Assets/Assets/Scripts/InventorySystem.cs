using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    public List<string> itemList = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddToInventory(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return;

        itemList.Add(itemName);
        Debug.Log($"Inventory: Added {itemName}");
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (string.IsNullOrEmpty(itemName) || amount <= 0)
            return;

        int removed = 0;
        for (int i = itemList.Count - 1; i >= 0 && removed < amount; i--)
        {
            if (itemList[i] == itemName)
            {
                itemList.RemoveAt(i);
                removed++;
            }
        }

        Debug.Log($"Inventory: Removed {removed}/{amount} of {itemName}");
    }

    public void ReCalculateList()
    {
        // Placeholder for more complex systems (stacking, sorting, etc.)
    }

    public int GetItemCount(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return 0;

        int count = 0;
        foreach (var item in itemList)
        {
            if (item == itemName)
                count++;
        }
        return count;
    }
}
