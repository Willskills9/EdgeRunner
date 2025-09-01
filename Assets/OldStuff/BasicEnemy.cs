using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public string playerTag = "Player";
    public ProjectileSpawner projectileSpawner;
    public GameObject player;
    public float rateOfFire = 0.2f;
    public bool isShooting;
    private Coroutine shootingCoroutine;
    public Health hpManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        shootingCoroutine = StartCoroutine(ShootingCoroutine());
        hpManager = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Dash") && (hpManager.shieldBroken))
        {
            hpManager.TakeDamage(100, "Player");
        }

    }
    

    public IEnumerator ShootingCoroutine()
    {
        if (player != null)
        {
            while (isShooting)
            {
                projectileSpawner.ShootAtTarget(player.transform);
                yield return new WaitForSeconds(rateOfFire); // wait before shooting again
            }
        }
    }
}
