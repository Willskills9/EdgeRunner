using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadScroll : MonoBehaviour
{
    public Transform objectToMove;
    public float duration;

    void Start()
    {
        StartCoroutine(Move());
        Destroy(gameObject, 5f); 
    }

    private IEnumerator Move()
    {
        

        // Move backward indefinitely at the same speed
        float speed = 294f / duration;

        while (true)
        {
            objectToMove.position -= objectToMove.forward * speed * Time.deltaTime;
            yield return null;
        }
    }
}
