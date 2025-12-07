using System.Collections;
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
    public Image backgroundImage;
    public Slider craftingProgressBar;

    [Header("Colors")]
    public Color hasRequirementColor = Color.green;
    public Color missingRequirementColor = Color.red;
    public Color defaultTextColor = Color.white;
    public Color craftingColor = new Color(1f, 0.8f, 0.2f);

    private BlueprintSO _blueprint;
    private Text _buttonText;
    private Color _originalBackgroundColor;
    private bool _isCrafting;

    private void Start()
    {
        if (craftButton != null)
        {
            _buttonText = craftButton.GetComponentInChildren<Text>();
        }

        if (backgroundImage != null)
        {
            _originalBackgroundColor = backgroundImage.color;
        }

        if (craftingProgressBar != null)
        {
            craftingProgressBar.gameObject.SetActive(false);
        }
    }

    public void Initialize(BlueprintSO bp, UnityAction onCraftClick)
    {
        _blueprint = bp;

        RecipeTooltip tooltip = GetComponent<RecipeTooltip>();
        if (tooltip != null)
        {
            tooltip.itemName = bp.itemName;
            tooltip.description = $"Craft a {bp.itemName}\nCraft Time: {bp.craftTime}s";
        }

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

    public void Refresh(int req1Count, int req2Count, bool canCraft)
    {
        if (_blueprint == null)
            return;

        if (req1Text != null && !string.IsNullOrEmpty(_blueprint.req1))
        {
            req1Text.text = $"{_blueprint.req1Amount} {_blueprint.req1} [{req1Count}]";
            req1Text.color = req1Count >= _blueprint.req1Amount ? hasRequirementColor : missingRequirementColor;
        }

        if (req2Text != null && !string.IsNullOrEmpty(_blueprint.req2))
        {
            req2Text.text = $"{_blueprint.req2Amount} {_blueprint.req2} [{req2Count}]";
            req2Text.color = req2Count >= _blueprint.req2Amount ? hasRequirementColor : missingRequirementColor;
        }

        if (craftButton != null && !_isCrafting)
        {
            craftButton.interactable = canCraft;
        }
    }

    public void StartCraftingAnimation(float craftTime)
    {
        if (_isCrafting)
            return;

        StartCoroutine(CraftingAnimationCoroutine(craftTime));
    }

    private IEnumerator CraftingAnimationCoroutine(float craftTime)
    {
        _isCrafting = true;

        if (craftButton != null)
            craftButton.interactable = false;

        if (_buttonText != null)
            _buttonText.text = "Crafting...";

        if (craftingProgressBar != null)
        {
            craftingProgressBar.gameObject.SetActive(true);
            craftingProgressBar.value = 0;
        }

        if (backgroundImage != null)
        {
            backgroundImage.color = craftingColor;
        }

        float elapsed = 0f;
        while (elapsed < craftTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / craftTime);

            if (craftingProgressBar != null)
                craftingProgressBar.value = progress;

            if (iconImage != null)
            {
                float scale = 1f + Mathf.Sin(elapsed * 8f) * 0.1f;
                iconImage.transform.localScale = Vector3.one * scale;
            }

            yield return null;
        }

        if (iconImage != null)
            iconImage.transform.localScale = Vector3.one;

        if (craftingProgressBar != null)
        {
            craftingProgressBar.value = 1f;
            yield return new WaitForSeconds(0.2f);
            craftingProgressBar.gameObject.SetActive(false);
        }

        if (backgroundImage != null)
        {
            backgroundImage.color = _originalBackgroundColor;
        }

        if (_buttonText != null)
            _buttonText.text = "Craft";

        _isCrafting = false;
    }

    public bool IsCrafting => _isCrafting;
}
