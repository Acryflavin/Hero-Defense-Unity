using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    public Image backgroundImage;
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color hoverColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public float scaleFactor = 1.05f;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;

        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        if (backgroundImage != null)
            backgroundImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (backgroundImage != null)
            backgroundImage.color = hoverColor;

        transform.localScale = _originalScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (backgroundImage != null)
            backgroundImage.color = normalColor;

        transform.localScale = _originalScale;
    }
}
