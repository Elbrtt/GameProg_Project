using UnityEngine;
public class Player : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float rotateSpeed = 4f;

    void Start()
    {
        playerInput = new InputSystem_Actions();

        playerInput.Enable();
    }

    void Update()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += moveDir * Time.deltaTime * playerSpeed;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
