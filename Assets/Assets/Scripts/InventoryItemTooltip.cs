using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Settings")]
    public Vector2 offset = new Vector2(20, 20);

    private ItemStack _itemStack;
    private bool _isHovering;

    private void Start()
    {
        _itemStack = GetComponent<ItemStack>();
    }

    private void Update()
    {
        if (_isHovering && RecipeTooltip.IsTooltipActive)
        {
            UpdateTooltipContent();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_itemStack != null && !string.IsNullOrEmpty(_itemStack.itemName))
        {
            string tooltipText = FormatTooltip();
            RecipeTooltip.ShowTooltip(tooltipText);
            _isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        RecipeTooltip.HideTooltip();
    }

    private void OnDestroy()
    {
        if (_isHovering)
        {
            RecipeTooltip.HideTooltip();
        }
    }

    private void UpdateTooltipContent()
    {
        if (_itemStack != null)
        {
            string tooltipText = FormatTooltip();
            RecipeTooltip.UpdateTooltipText(tooltipText);
        }
    }

    private string FormatTooltip()
    {
        string tooltip = $"<b>{_itemStack.itemName}</b>";

        if (_itemStack.quantity > 1)
        {
            tooltip += $"\n\nQuantity: {_itemStack.quantity}";
        }

        tooltip += GetItemDescription(_itemStack.itemName);

        return tooltip;
    }

    private string GetItemDescription(string itemName)
    {
        switch (itemName.ToLower())
        {
            case "wood":
                return "\n\nBasic crafting material.\nUsed in many recipes.";
            case "stone":
                return "\n\nHard and durable.\nUsed for tools and weapons.";
            case "iron":
                return "\n\nStrong metal ore.\nUsed for advanced crafting.";
            case "axe":
                return "\n\nA tool for chopping wood.";
            case "sword":
                return "\n\nA weapon for combat.";
            case "lockpick":
                return "\n\nUsed to open locked chests.";
            default:
                return "\n\nA useful item.";
        }
    }
}
