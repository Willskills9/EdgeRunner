using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool hasShield;
    public int shieldHealth = 3;
    public bool shieldBroken = false;

    [Tooltip("List of owner tags that can damage this object")]
    public List<string> damageableByTags = new List<string> { "Player", "Enemy" };

    private vThirdPersonController controller;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<vThirdPersonController>();
    }

    // Call this method to apply damage
    public void TakeDamage(int amount, string attackerTag)
    {
        if (!damageableByTags.Contains(attackerTag))
        {
            // Not allowed to damage us
            //gameObject ignored damage from attackerTag
            return;
        }

        if (controller != null && controller.isSprinting)
        {
            //blocked damage while sprinting.
            return;
        }
        if (hasShield)
        {
            DamageShield();
        }
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage from {attackerTag}. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void DamageShield()
    {
        shieldHealth -= 1;

        if (shieldHealth <= 0)
        {
            shieldHealth = 0;
            shieldBroken = true;
            Debug.Log("Shield Broken");
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject); // Or trigger animation, disable AI etc.
    }
}
