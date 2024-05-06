using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform[] _cameraLocations;
    [SerializeField] private Camera _camera;

    // Transition
    private float _profileTransitionTimer = 1.1f;
    private float _profileTransitionDuration = 1f;
    private Vector3 _profileTransitionStartPosition;
    private Vector3 _target;
    private bool _active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!_isPlayingProfileTransition())
        {
            Debug.Log("chose");
            _PlayProfileTransition();
            _active = !_active;
        }
    }

    private void _PlayProfileTransition()
    {
        Debug.Log(_active);
        _profileTransitionStartPosition = _camera.transform.position;
        if (_camera.transform.position == _cameraLocations[1].position)
        {
            _target = _cameraLocations[0].position;
        } else
        {
            _target = _cameraLocations[1].position;
        }

        _profileTransitionTimer = 0f;
    }

    private bool _isPlayingProfileTransition()
    {
        return _profileTransitionTimer <= _profileTransitionDuration;
    }

    private Vector3 _CalculateProfileTransitionPosition(Vector3 destination)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        if (percent > 0.9)
        {
            percent = 1;
        }
        Vector3 origin = _profileTransitionStartPosition;
        return Vector3.Lerp(origin, destination, percent);
    }

    private void Update()
    {
        if (_isPlayingProfileTransition())
        {
            Debug.Log(_profileTransitionTimer);
            Debug.Log(_target);
            _camera.transform.position = _CalculateProfileTransitionPosition(_target);
            _profileTransitionTimer += Time.deltaTime;
        }
    }
}
