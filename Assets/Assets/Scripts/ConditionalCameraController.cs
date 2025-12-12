using UnityEngine;
using UnityEngine.InputSystem;
using HeroCharacter;

public class ConditionalCameraController : MonoBehaviour
{
    private HeroCharacterController heroController;
    private bool rightMouseButtonHeld = false;

    void Awake()
    {
        heroController = GetComponent<HeroCharacterController>();
    }

    void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            var rotateAction = playerInput.actions.FindAction("RotateCamera");
            if (rotateAction != null)
            {
                rotateAction.performed += OnRotateCameraPerformed;
                rotateAction.canceled += OnRotateCameraCanceled;
                rotateAction.Enable();
            }
        }
    }

    void OnDisable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            var rotateAction = playerInput.actions.FindAction("RotateCamera");
            if (rotateAction != null)
            {
                rotateAction.performed -= OnRotateCameraPerformed;
                rotateAction.canceled -= OnRotateCameraCanceled;
            }
        }
    }

    private void OnRotateCameraPerformed(InputAction.CallbackContext context)
    {
        rightMouseButtonHeld = true;
    }

    private void OnRotateCameraCanceled(InputAction.CallbackContext context)
    {
        rightMouseButtonHeld = false;
    }

    public bool ShouldRotateCharacter()
    {
        return rightMouseButtonHeld;
    }
}
