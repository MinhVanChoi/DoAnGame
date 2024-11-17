using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        MoveProjectile();
    }
    public void UpdateProjectileRange(float range)
    {
        this.projectileRange = range;
    }
    public void UpdateMoveSpeed(float speed)
    {
        this.moveSpeed = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();
        PlayerHeath playerHeath = collision.gameObject.GetComponent<PlayerHeath>();

        if (!collision.isTrigger && (enemyHealth || indestructible || playerHeath))
        {
            if ((playerHeath && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                playerHeath?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if(!collision.isTrigger && indestructible)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
