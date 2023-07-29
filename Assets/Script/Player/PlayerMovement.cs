using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed = 5;
    private float moveSpeedReduction = .5f;
    private bool isMoveSpeedSlow = false;
    private float rotationSpeed = 720;
    private Rigidbody2D rb;
    private float interactableDistance = 1.5f;
    private int layerMaskIgnore = (1 << 7);
    private PlayerInfomation playerInfo;
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction useMachine;
    private InputAction pickUpItem;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInfo = gameObject.GetComponent<PlayerInfomation>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void OnEnable()
    {
        playerControls.Enable();
        move = playerControls.Player.Move;
        move.Enable();

        pickUpItem = playerControls.Player.PickUpDropItems;
        pickUpItem.Enable();
        pickUpItem.performed += PickUpDropItems;

        useMachine = playerControls.Player.UseMachine;
        useMachine.Enable();
        useMachine.performed += UseMachine;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        move.Disable();
        pickUpItem.Disable();
        useMachine.Disable();
    }
    private void PickUpDropItems(InputAction.CallbackContext context)
    {
        //Place Item
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), interactableDistance, layerMaskIgnore);
        if (hit.collider == null) return;
        //Debug.Log(hit.collider.name);
        if (hit.collider.CompareTag("Interactable"))
        {
            if (hit.collider.GetComponent<Machine>() == null) return;
            hit.collider.GetComponent<Machine>().PlaceDropItem(playerInfo);
        }
    }
    private void UseMachine(InputAction.CallbackContext context)
    {
        //Use Machine
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), interactableDistance, layerMaskIgnore);
        if (hit.collider == null) return;
        //Debug.Log(hit.collider.name);
        if (hit.collider.CompareTag("Interactable"))
        {
            if (hit.collider.GetComponent<Machine>() == null) { Debug.Log(hit.collider.name); return; }
            hit.collider.GetComponent<Machine>().UseMachine(playerInfo);
        }
    }
    public void SetMoveSpeedMultiplier(bool isPlayerSlow)
    {
        isMoveSpeedSlow = isPlayerSlow;
    }
    private void GetInput()
    {
        /*    float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;*/

        moveDirection = move.ReadValue<Vector2>();
    }
    private void Move()
    {
        if (!isMoveSpeedSlow)
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        else
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * moveSpeedReduction, moveDirection.y * moveSpeed * moveSpeedReduction);
        Debug.Log(rb.velocity);
        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
