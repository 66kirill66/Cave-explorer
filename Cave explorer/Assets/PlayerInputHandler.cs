using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;

    public float moveSpeed = 3f;
    public float sensitivity = 2f;
    public float smoothTime = 0.1f; // Время сглаживания

    private CharacterController controller;
    private float verticalLookRotation = 0f; // Угол наклона камеры
    private Vector2 currentVelocity = Vector2.zero; // Для сглаживания
    private Vector2 smoothLookInput; // Плавный ввод камеры

    public Transform cameraTransform; // Ссылка на камеру для наклона

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Плавное движение камеры
        smoothLookInput = Vector2.SmoothDamp(smoothLookInput, lookInput, ref currentVelocity, smoothTime);

        // Передвижение
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Поворот камеры влево/вправо
        transform.Rotate(Vector3.up * smoothLookInput.x * sensitivity);

        // Поворот камеры вверх/вниз
        verticalLookRotation -= smoothLookInput.y * sensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); // Ограничение угла
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void OnEnable() { controls.Enable(); }
    void OnDisable() { controls.Disable(); }
}
