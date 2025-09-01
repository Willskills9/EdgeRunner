using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private Transform target;
    private float projectileSpeed;
    private float enemyAimHeight; // new
    private string ownerTag; // new field for owner

    public Renderer projectileRenderer;
    private int damageAmount;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Initialize the projectile with target, speed, and aim height
    public void Initialize(Transform target, float speed, float aimHeight, string ownerTag, Color color, int damageAmount)
    {
        this.target = target;
        projectileSpeed = speed;
        enemyAimHeight = aimHeight;
        this.ownerTag = ownerTag;
        this.damageAmount = damageAmount;

        if (projectileRenderer != null)
        {
            projectileRenderer.material.color = color;
        }

        // Immediately rotate to look at adjusted target position
        Vector3 adjustedTargetPos = target.position + Vector3.up * enemyAimHeight;
        Vector3 direction = (adjustedTargetPos - transform.position).normalized;
        transform.forward = direction;
    }

    void Update()
    {
        if (target == null) return;

        // Always aim at adjusted target position
        Vector3 adjustedTargetPos = target.position + Vector3.up * enemyAimHeight;
        Vector3 direction = (adjustedTargetPos - transform.position).normalized;
        transform.position += direction * projectileSpeed * Time.deltaTime;
        transform.forward = direction;
    }

    void OnTriggerEnter(Collider other)
    {
        // Skip hitting objects with the same tag as owner
        if (other.CompareTag(ownerTag)) return;

        if (other.CompareTag("Dash")) return;

        // Example: also ignore other projectiles
        if (!other.CompareTag("Projectile"))
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Deal damage (for now hardcoded 10, you could add a public damageAmount)
                targetHealth.TakeDamage(damageAmount, ownerTag);
            }
            // Hit something else: destroy self (or apply damage logic etc)
            Destroy(this.gameObject);
            Debug.Log($"Projectile hit: {other.gameObject.name}");
        }
    }
}
