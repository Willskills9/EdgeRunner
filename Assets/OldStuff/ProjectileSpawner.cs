using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 50f;
    public float enemyAimHeight;
    public Color projectileColor = Color.red;
    public int projectileDamage = 10;

    public void ShootAtTarget(Transform target)
    {
        // Instantiate the projectile at the spawner's position and rotation
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Get the HomingProjectile script and initialize it
        projectile homing = projectileInstance.GetComponent<projectile>();
        if (homing != null)
        {
            homing.Initialize(target, projectileSpeed, enemyAimHeight, gameObject.transform.root.tag, projectileColor, projectileDamage);
        }
    }
}
