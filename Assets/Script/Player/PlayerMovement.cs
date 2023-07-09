using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInfo = gameObject.GetComponent<PlayerInfomation>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        GetInteractionInput();
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void SetMoveSpeedMultiplier(bool isPlayerSlow)
    {
        isMoveSpeedSlow = isPlayerSlow;
    }
    private void GetInteractionInput()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up), Color.red);
        if (Input.GetKeyDown(KeyCode.J))
        {
            //Place Item
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), interactableDistance, layerMaskIgnore);
            if (hit.collider == null) return;
            //Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Interactable"))
            {
                if (hit.collider.GetComponent<PictureMachine>() == null) return;
                hit.collider.GetComponent<PictureMachine>().PlaceDropItem(playerInfo);
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            //Use Machine
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), interactableDistance, layerMaskIgnore);
            if (hit.collider == null) return;
            //Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Interactable"))
            {
                if (hit.collider.GetComponent<PictureMachine>() == null) return;
                hit.collider.GetComponent<PictureMachine>().UseMachine(playerInfo);
            }
        }
    }
    private void GetInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    private void Move()
    {
        if (!isMoveSpeedSlow)
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        else
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * moveSpeedReduction, moveDirection.y * moveSpeed * moveSpeedReduction);
        //Debug.Log(rb.velocity);
        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
