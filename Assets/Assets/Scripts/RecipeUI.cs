using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public Text nameText;
    public Text req1Text;
    public Text req2Text;
    public Button craftButton;

    private BlueprintSO _blueprint;

    public void Initialize(BlueprintSO bp, UnityAction onCraftClick)
    {
        _blueprint = bp;

        if (iconImage != null && _blueprint.icon != null)
            iconImage.sprite = _blueprint.icon;

        if (nameText != null)
            nameText.text = bp.itemName;

        if (craftButton != null && onCraftClick != null)
        {
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(onCraftClick);
        }
    }
}
