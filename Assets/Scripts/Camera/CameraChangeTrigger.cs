using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangeTrigger : MonoBehaviour
{

    [SerializeField] private UnityEvent _event;
    [SerializeField] private CinemachineBrain _brain;

    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_brain.IsBlending)
        {
            _boxCollider.isTrigger = false;
            return;
        }
        if (collision.CompareTag("CameraTriggerTarget"))
        {
            Debug.Log("yippe kai hey");
            _event.Invoke();
        }
    }

    private void Update()
    {
        if (!_brain.IsBlending)
        {
            StartCoroutine(WaitAndTrigger());
        } else
        {
            StopAllCoroutines();
        }
        // Debug.Log("is Trigger" + _boxCollider.isTrigger);
    }

    private IEnumerator WaitAndTrigger()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _boxCollider.isTrigger = true;
    }
}
