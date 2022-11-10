using UnityEngine;
using UnityEngine.InputSystem;


// For this class to function, you will need to create an inputactions asset called FPSControllerActions, and include Move, Look, Jump, Sprint actions.

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Constants
    private const float Gravity = -9.8f;

    // Components
    private CharacterController _charController;
    private Camera _camera;

    // Configuration Data
    [Header("Movement")] [SerializeField] private Vector2 walkSpeed = Vector2.one;
    [SerializeField] private Vector2 sprintSpeed = Vector2.one * 3f;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private float gravityMultiplier = 2f;

    [Header("Camera")] [SerializeField] private bool IsCameraInverted = false;
    [SerializeField] private Vector2 cameraRotationSpeedMultiplier = Vector2.one;
    [SerializeField] private float MinVerticalCameraAngle = -75;
    [SerializeField] private float MaxVerticalCameraAngle = 75;

    // Input
    private FPSControllerActions _inputActions;

    // Data members
    private Vector2 _walkDirection = Vector2.zero;
    private Vector3 _worldMovement = Vector3.zero;
    private float _cameraVerticalAngle = 0f;

    private void Awake()
    {
        _inputActions = new FPSControllerActions();
        _camera = GetComponentInChildren<Camera>();
        _charController = GetComponent<CharacterController>();
        
        // subscribe to the events that change the input values of the movement axis
        _inputActions.FindAction("Move").performed += Move;
        _inputActions.FindAction("Move").canceled += Move;
    }

    private void OnEnable()
    {
        // enable the input actions and set the cursor settings for an FPS camera
        _inputActions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        // Disable the input actions and set the cursor settings back to default for the app
        _inputActions.Disable();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Move(InputAction.CallbackContext context)
    {
        _walkDirection = context.ReadValue<Vector2>();
    }


    void Update()
    {
        UpdateCameraMovement();

        if (_charController.isGrounded)
        {
            UpdateGroundMovement();

            if (_inputActions.FindAction("Jump").WasPressedThisFrame())
            {
                _worldMovement.y += jumpSpeed;
            }
        }

        _worldMovement.y += Gravity * gravityMultiplier * Time.deltaTime;
        _charController.Move(_worldMovement * Time.deltaTime);
    }

    /// <summary>
    /// Updates the rotation of the camera, rotation around the Y-axis rotates the character (left/right), and
    /// rotation around the X-axis rotates the camera (up/down) 
    /// </summary>
    private void UpdateCameraMovement()
    {
        var cameraMovement = _inputActions.FindAction("Look").ReadValue<Vector2>();
        cameraMovement.Scale(cameraRotationSpeedMultiplier);
        transform.Rotate(Vector3.up, cameraMovement.x, Space.Self);

        // A configuration that allows inverting the camera control axis
        if (IsCameraInverted)
        {
            _cameraVerticalAngle = _cameraVerticalAngle + cameraMovement.y;
        }
        else
        {
            _cameraVerticalAngle = _cameraVerticalAngle - cameraMovement.y;
        }

        _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, MinVerticalCameraAngle, MaxVerticalCameraAngle);
        _camera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0f, 0f);
    }

    /// <summary>
    /// Updates the movement this frame based on the input directions pressed for moving
    /// </summary>
    private void UpdateGroundMovement()
    {
        Vector2 moveAtSpeed;

        if (_inputActions.FindAction("Sprint").IsPressed())
        {
            moveAtSpeed = Vector2.Scale(_walkDirection, sprintSpeed);
        }
        else
        {
            moveAtSpeed = Vector2.Scale(_walkDirection, walkSpeed);
        }

        var localMovement = new Vector3(moveAtSpeed.x, 0, moveAtSpeed.y);
        _worldMovement = transform.TransformDirection(localMovement);
    }
}