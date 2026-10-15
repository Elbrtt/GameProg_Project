using System.Collections;
using TMPro;
using UnityEngine;
public class Player : MonoBehaviour
{
    private InputSystem_Actions playerInput;

    [SerializeField] private float rotateSpeed = 4f;
    [SerializeField] private TextMeshProUGUI detectText;

    private MovemenComponent movementComponent;

    public bool isDetected;

    private void Awake()
    {
        movementComponent = GetComponent<MovemenComponent>();
    }

    void Start()
    {
        playerInput = new InputSystem_Actions();

        playerInput.Enable();
    }

    void Update()
    {
        if(isDetected)
        {
            detectText.text = "detected";
            detectText.color = Color.red;
            StartCoroutine(ResetDetect());
        }
        else
        {
            detectText.text = "not detected";
            detectText.color = Color.white;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>().normalized;

        float moveDistance = movementComponent.GetSpeed() * Time.deltaTime;
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        movementComponent.Move(moveDir, moveDistance);
    }

    private IEnumerator ResetDetect()
    {
        yield return new WaitForSeconds(2f);

        isDetected = false;
    }
}
