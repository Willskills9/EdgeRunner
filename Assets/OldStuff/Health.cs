using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool hasShield;
    public int shieldHealth = 3;
    public bool shieldBroken = false;
    public float SpotLightBuildUpPerTick = 1f;
    public float SpotLightMeter = 0f;
    public bool inSpotLight;
    public Slider uiSlider;    // assign in Inspector
    public int maxValue = 100;

    [Tooltip("List of owner tags that can damage this object")]
    public List<string> damageableByTags = new List<string> { "Player", "Enemy" };

    private Targeting targeting;

    void Start()
    {
        currentHealth = maxHealth;
        targeting = GetComponentInChildren<Targeting>();
        if (uiSlider != null)
        {
            uiSlider.maxValue = maxValue;
            uiSlider.value = SpotLightMeter;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SpotLight")
        {
            // A collider has entered the trigger
            inSpotLight = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "SpotLight")
        {
            // A collider has entered the trigger
            inSpotLight = false;
        }
    }

    void FixedUpdate()
    {
        if(inSpotLight)
        {
            SpotLightMeter += SpotLightBuildUpPerTick;
            if(SpotLightMeter >= 100f)
            {
                //Die
            }
        }else if (SpotLightMeter >= 0f)
        {
            SpotLightMeter -= (SpotLightBuildUpPerTick/2f);
        }

        if (uiSlider != null)
        {
            uiSlider.value = SpotLightMeter;
        }
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

        if (targeting != null && targeting.sprintShield)
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

    public void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject); // Or trigger animation, disable AI etc.
    }
}
