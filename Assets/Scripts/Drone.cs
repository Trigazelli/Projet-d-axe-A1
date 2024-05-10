using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RaycastHit2D hitresult = Physics2D.Raycast(
            gameObject.transform.position,
            Vector3.forward,
            Mathf.Infinity,
            9);

        if (hitresult.collider != null)
        {

        }
    }
}
