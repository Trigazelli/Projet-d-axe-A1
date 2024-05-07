using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // [SerializeField] private Camera _camera;
    [SerializeField] private _virtualCameras[] _allVirtualCameras;
    private CinemachineVirtualCamera _currentCamera;
    [SerializeField] private GameObject _leftBoxCollider;
    [SerializeField] private GameObject _rightBoxCollider;
    [SerializeField] private GameObject _downBoxCollider;
    [SerializeField] private GameObject _upBoxCollider;

    private int _vertical;
    private int _horizontal;

    [Serializable]
    struct _virtualCameras
    {
        [SerializeField] public CinemachineVirtualCamera[] virtualCamera;
    }

    public void SwitchCameraLeft()
    {
        Debug.Log("Left");
        _horizontal = Mathf.Max(0, _horizontal - 1);
        _SwitchToCamera();
    }

    public void SwitchCameraUp()
    {
        Debug.Log("Up");
        _vertical -= 1;
        _SwitchToCamera();
    }

    public void SwitchCameraRight()
    {
        Debug.Log("Right");
        _horizontal += 1;
        _SwitchToCamera();
    }

    public void SwitchCameraDown()
    {
        Debug.Log("Down");
        _vertical += 1;
        _SwitchToCamera();
    }

    private void Start()
    {
        _vertical = 0;
        _horizontal = 1;
        _currentCamera = _allVirtualCameras[_vertical].virtualCamera[_horizontal];
        _SwitchToCamera();
    }

    private void _SwitchToCamera()
    {
        _currentCamera = _allVirtualCameras[_vertical].virtualCamera[_horizontal];
        foreach (_virtualCameras cameras in _allVirtualCameras)
        {
            foreach (CinemachineVirtualCamera camera in cameras.virtualCamera)
            {
                camera.enabled = camera == _currentCamera;
            }
        }
    }

    private void Update()
    {
        Debug.Log("Horizontal :" + _horizontal + ", Vertical :" + _vertical);
    }
}
