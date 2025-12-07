using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Settings")]
    public string itemName;
    public string description;
    public bool useFixedPosition = false;
    public Vector2 fixedScreenPosition = new Vector2(100, 100);
    public Vector2 mouseOffset = new Vector2(20, 20);

    private static GameObject _tooltipPanel;
    private static Text _tooltipText;
    private static CanvasGroup _tooltipCanvasGroup;
    private static RectTransform _tooltipRect;
    private static int _activeTooltips = 0;
    private bool _isHovering;
    private static bool _useFixedPos;
    private static Vector2 _fixedPos;
    private static Vector2 _offset;

    public static bool IsTooltipActive => _tooltipPanel != null && _tooltipPanel.activeSelf;

    private void Start()
    {
        if (_tooltipPanel == null)
        {
            CreateTooltipUI();
        }
        _useFixedPos = useFixedPosition;
        _fixedPos = fixedScreenPosition;
        _offset = mouseOffset;
    }

    private void CreateTooltipUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
            return;

        _tooltipPanel = new GameObject("TooltipPanel");
        _tooltipPanel.transform.SetParent(canvas.transform, false);

        _tooltipRect = _tooltipPanel.AddComponent<RectTransform>();
        _tooltipRect.sizeDelta = new Vector2(300, 100);
        _tooltipRect.pivot = new Vector2(0, 1);

        _tooltipCanvasGroup = _tooltipPanel.AddComponent<CanvasGroup>();
        _tooltipCanvasGroup.blocksRaycasts = false;
        _tooltipCanvasGroup.interactable = false;

        Image bg = _tooltipPanel.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        bg.raycastTarget = false;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(_tooltipPanel.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);

        _tooltipText = textObj.AddComponent<Text>();
        _tooltipText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        _tooltipText.fontSize = 14;
        _tooltipText.color = Color.white;
        _tooltipText.alignment = TextAnchor.UpperLeft;
        _tooltipText.raycastTarget = false;

        _tooltipPanel.transform.SetAsLastSibling();
        _tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (_isHovering && _tooltipPanel != null && _tooltipPanel.activeSelf)
        {
            UpdateTooltipPosition();
        }
    }

    private void UpdateTooltipPosition()
    {
        Vector2 position;

        if (_useFixedPos)
        {
            position = _fixedPos;
        }
        else
        {
            position = Input.mousePosition + new Vector3(_offset.x, _offset.y, 0);
        }

        Canvas canvas = _tooltipPanel.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            _tooltipRect.position = position;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                position,
                canvas.worldCamera,
                out Vector2 localPoint);
            _tooltipRect.localPosition = localPoint;
        }

        if (!_useFixedPos)
        {
            ClampToScreen();
        }
    }

    private void ClampToScreen()
    {
        Vector3[] corners = new Vector3[4];
        _tooltipRect.GetWorldCorners(corners);

        float overflowX = corners[2].x - Screen.width;
        float overflowY = corners[1].y - Screen.height;

        if (overflowX > 0)
            _tooltipRect.position -= new Vector3(overflowX, 0, 0);

        if (overflowY > 0)
            _tooltipRect.position -= new Vector3(0, overflowY, 0);

        if (corners[0].x < 0)
            _tooltipRect.position += new Vector3(-corners[0].x, 0, 0);

        if (corners[0].y < 0)
            _tooltipRect.position += new Vector3(0, -corners[0].y, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            ShowTooltip($"<b>{itemName}</b>\n\n{description}");
            _isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        HideTooltip();
    }

    private void OnDestroy()
    {
        if (_isHovering)
        {
            _activeTooltips--;
            if (_activeTooltips <= 0)
            {
                _activeTooltips = 0;
                if (_tooltipPanel != null)
                    _tooltipPanel.SetActive(false);
            }
        }
    }

    public static void ShowTooltip(string text)
    {
        if (_tooltipPanel == null)
        {
            RecipeTooltip tooltip = FindFirstObjectByType<RecipeTooltip>();
            if (tooltip != null)
                tooltip.CreateTooltipUI();
        }

        if (_tooltipPanel != null && _tooltipText != null)
        {
            _tooltipText.text = text;
            _tooltipPanel.SetActive(true);
            _tooltipPanel.transform.SetAsLastSibling();
            _activeTooltips++;
        }
    }

    public static void HideTooltip()
    {
        _activeTooltips--;
        if (_activeTooltips <= 0)
        {
            _activeTooltips = 0;
            if (_tooltipPanel != null)
            {
                _tooltipPanel.SetActive(false);
            }
        }
    }

    public static void UpdateTooltipText(string text)
    {
        if (_tooltipText != null)
        {
            _tooltipText.text = text;
        }
    }
}
