using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    private bool isShooting = false;
    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootAroundRoutine());
        }
    }
    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        for (int i = 0; i < burstCount; i++)
        {
            Vector2 target = PlayerController.Instance.transform.position - transform.position;

            GameObject newbullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newbullet.transform.right = target;

            if (newbullet.TryGetComponent(out Projectile projectile))
            {
                projectile.UpdateMoveSpeed(bulletMoveSpeed);
            }
            yield return new WaitForSeconds(timeBetweenBurst);
        }
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }
    private IEnumerator ShootAroundRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
        if (stagger) { timeBetweenProjectiles = timeBetweenBurst / projectilesPerBurst; }
        for (int i = 0; i < burstCount; i++)
        {
            if(!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate) 
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);
                GameObject newbullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newbullet.transform.right = newbullet.transform.position - transform.position;

                if (newbullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
                if(stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }
            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBurst); }

        }
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }
    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 target = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }
    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}