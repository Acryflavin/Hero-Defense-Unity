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

        if (iconImage != null)
        {
            if (_blueprint.icon != null)
            {
                iconImage.sprite = _blueprint.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.enabled = false;
            }
        }

        if (nameText != null)
            nameText.text = bp.itemName;

        if (req1Text != null)
        {
            if (!string.IsNullOrEmpty(bp.req1))
                req1Text.text = $"{bp.req1Amount} {bp.req1}";
            else
                req1Text.gameObject.SetActive(false);
        }

        if (req2Text != null)
        {
            if (!string.IsNullOrEmpty(bp.req2))
                req2Text.text = $"{bp.req2Amount} {bp.req2}";
            else
                req2Text.gameObject.SetActive(false);
        }

        if (craftButton != null && onCraftClick != null)
        {
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(onCraftClick);
        }
    }
}
