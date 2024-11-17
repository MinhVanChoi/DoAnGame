using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 movedir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack)
        {
            return;
        }
        rb.MovePosition(rb.position + movedir * (moveSpeed * Time.fixedDeltaTime));

        if (movedir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public void MoveTo(Vector2 targetPosition)
    {
        movedir = targetPosition;
    }
    public void StopMoving()
    {
        movedir = Vector3.zero;
    }
}
