using UnityEngine;
using UnityEngine.InputSystem;

public class ConditionalCameraInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction lookAction;
    private Vector2 storedLookValue;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null && playerInput.actions != null)
        {
            lookAction = playerInput.actions.FindAction("Look");
        }
    }

    void Update()
    {
        if (lookAction == null)
            return;

        if (Mouse.current.rightButton.isPressed)
        {
            storedLookValue = lookAction.ReadValue<Vector2>();
        }
        else
        {
            storedLookValue = Vector2.zero;
        }
    }

    public Vector2 GetModifiedLookInput()
    {
        return storedLookValue;
    }
}
