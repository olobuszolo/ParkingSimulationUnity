using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float fastMoveMultiplier = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 3f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 200f;

    [Header("Default View")]
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private Vector3 defaultRotation;

    private float rotationX;
    private float rotationY;

    private void Start()
    {
        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    private void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleZoom();

        HandleReset();
    }

    private void HandleMovement()
    {
        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= fastMoveMultiplier;
        }

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        if (Input.GetKey(KeyCode.E))
        {
            moveDirection += Vector3.up;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection += Vector3.down;
        }

        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        transform.position += transform.forward * scroll * zoomSpeed * Time.deltaTime;
    }

    private void HandleReset()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = defaultPosition;

            transform.rotation = Quaternion.Euler(defaultRotation);
        }
    }
}