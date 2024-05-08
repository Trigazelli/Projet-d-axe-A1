using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangeTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraTriggerTarget"))
        {
            Debug.Log("yippe kai hey");
            _event.Invoke();
        }
    }
}
