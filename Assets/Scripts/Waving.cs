using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Waving : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float speed = 1f;
    private Vector3 _scale;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _scale = _transform.localScale;
    }
    void Update()
    {
        float scale = Mathf.PingPong(Time.time * speed, 1);
        _transform.localScale = new Vector3 (_scale.x + scale, _scale.y + scale, _scale.z);
    }
}
