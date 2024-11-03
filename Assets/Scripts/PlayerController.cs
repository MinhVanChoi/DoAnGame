using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myanimator;
    private SpriteRenderer mySpriteRenderer;

     
    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void Update()
    {
        PlayerInPut();
    }
    private void FixedUpdate()
    {
        AdjustPlayerFacingDiretion();
        Move();
        
    }
    private void PlayerInPut()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myanimator.SetFloat("moveX", movement.x);
        myanimator.SetFloat("moveY",movement.y);

    }

    private void AdjustPlayerFacingDiretion()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;

        }
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

    }
}
