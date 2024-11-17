using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : MonoBehaviour
{
    [SerializeField] private int maxHeath = 3;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHeath;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }
    private void Start()
    {
        currentHeath = maxHeath;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }
    public void TakeDamage(int damage, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }
        Debug.Log(currentHeath);
        knockback.GetKnockedBack(hitTransform, knockbackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHeath -= damage;
        StartCoroutine(DamageRecoveryRoutine());
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
