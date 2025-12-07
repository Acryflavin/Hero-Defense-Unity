using UnityEngine;
using UnityEngine.UI;

public class ItemStack : MonoBehaviour
{
    public string itemName;
    public int quantity = 1;
    public int maxStackSize = 99;

    [Header("UI References")]
    public Text quantityText;

    void Start()
    {
        UpdateQuantityDisplay();
    }

    public bool CanAddToStack()
    {
        return quantity < maxStackSize;
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        if (quantity > maxStackSize)
        {
            quantity = maxStackSize;
        }
        UpdateQuantityDisplay();
    }

    public void RemoveFromStack(int amount = 1)
    {
        quantity -= amount;
        if (quantity <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            UpdateQuantityDisplay();
        }
    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            if (quantity > 1)
            {
                quantityText.text = quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
    }
}
