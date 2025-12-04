using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;

    [Header("Cursor Settings")]
    public Texture2D defaultCursor;
    public Texture2D interactCursor;
    public Vector2 cursorHotspot = Vector2.zero;

    Text interaction_text;

    private void Start()
    {
        Cursor.visible = true;
        //Cursor.lockState = CursorLockState.None;

                onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();

        SetDefaultCursor();
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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject Interactable = selectionTransform.GetComponent<InteractableObject>();

            if (Interactable)
            {
                onTarget = true;
                selectedObject = Interactable.gameObject;
                interaction_text.text = Interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (Interactable.playerInRange)
                {
                    SetInteractCursor();
                }
                else
                {
                    SetDefaultCursor();
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                SetDefaultCursor();
            }
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            SetDefaultCursor();
        }
    }

    private void SetDefaultCursor()
    {
        if (defaultCursor != null)
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void SetInteractCursor()
    {
        if (interactCursor != null)
        {
            Cursor.SetCursor(interactCursor, cursorHotspot, CursorMode.Auto);
        }
    }
}
