using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public string selectableTag = "Selectable";
    public GameObject selectedObject;
    private GameObject previousSelectedObject;
    public ProjectileSpawner projectileSpawner;
    public float rateOfFire;
    public float dashSpeed = 1000f;
    public float dashDuration = 0.2f;
    public Rigidbody rb;
    public float dashCooldown = 2f;    // Cooldown between dashes

    private bool canDash = true;
    private bool isDashing = false;
    public bool dashPrimed = false;
    public bool gravityOff = false;
    bool inputShooting;
    bool inputDashing;
    private float dashTimer = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = transform.root.GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputShooting = Input.GetAxis("Fire1") == 1f;
        inputDashing = Input.GetAxis("Fire2") == 1f;
        SelectClosestObject();

        // Start shooting on left mouse button press or Fire1
        if (inputShooting)
        {
            StartShooting();
        }
        // Stop shooting on button release
        else if (!inputShooting)
        {
            StopShooting();
        }

        // Dash input
        if (inputDashing && canDash && selectedObject != null)
        {
            dashPrimed = true;
        }

        if (dashPrimed && !inputDashing && canDash && selectedObject != null)
        {
            dashPrimed = false;
            StartCoroutine(DashTowardsTarget());
        }

        if (dashPrimed)
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * Time.deltaTime;
        }

        if(gravityOff)
        {
            rb.useGravity = false;
        }else
        {
            rb.useGravity = true;
        }

        // Cooldown timer update
        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                gravityOff = false;
                canDash = true;
            }
        }
    }

    private IEnumerator DashTowardsTarget()
    {
        canDash = false;
        dashTimer = dashCooldown;
        Debug.Log("Dashed");
        isDashing = true;

        yield return new WaitForSeconds(0.1f);
        

        
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on player root!");
            yield break;
        }

        Vector3 startPosition = rb.position;
        Vector3 targetPosition = selectedObject.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / dashDuration;

            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            rb.MovePosition(newPosition);

            yield return null;
        }

        //rb.MovePosition(targetPosition);
        gravityOff = true;
        isDashing = false;
    }


    void SelectClosestObject()
    {
        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag(selectableTag);
        if (selectableObjects.Length == 0)
        {
            selectedObject = null;
            return;
        }

        Camera cam = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        float minDistance = float.MaxValue;
        GameObject closestObject = null;

        foreach (GameObject obj in selectableObjects)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(obj.transform.position);
            if (screenPos.z < 0) continue;

            float distance = Vector2.Distance(screenCenter, screenPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = obj;
            }
        }

        if (closestObject != selectedObject)
        {
            previousSelectedObject = selectedObject;
            selectedObject = closestObject;

            EnableSelectedCanvas();
            DisablePreviousCanvas();

            Debug.Log("Selected: " + (selectedObject != null ? selectedObject.name : "None"));
        }
    }

    private bool isShooting = false;
    private Coroutine shootingCoroutine;

    // Call this when the shoot button is pressed down
    public void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootingCoroutine());
        }
    }

    // Call this when the shoot button is released
    public void StopShooting()
    {
        if (isShooting)
        {
            isShooting = false;
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
            }
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        if (selectedObject != null)
        {
            while (isShooting)
            {
            
                projectileSpawner.ShootAtTarget(selectedObject.transform);
                yield return new WaitForSeconds(rateOfFire); // wait before shooting again
            }
        }
    }


    void EnableSelectedCanvas()
    {
        if (selectedObject == null) return;

        Canvas canvas = selectedObject.GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvas.enabled = true;
        }
        else
        {
            Debug.LogWarning("No Canvas found on selected object: " + selectedObject.name);
        }
    }

    void DisablePreviousCanvas()
    {
        if (previousSelectedObject == null) return;

        Canvas canvas = previousSelectedObject.GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }
}