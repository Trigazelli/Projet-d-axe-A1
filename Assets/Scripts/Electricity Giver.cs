using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityGiver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.layer = 9;
        }
    }
}
