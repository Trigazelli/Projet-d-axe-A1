using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DOMove(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, 0), 2);
        Debug.Log(collision.gameObject.name);
        // collision.gameObject.transform.SetParent(gameObject.transform, false);
    }
}
