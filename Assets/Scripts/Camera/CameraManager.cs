using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private int _vertical;
    [SerializeField] private int _horizontal;

    // [SerializeField] private Camera _camera;
    [SerializeField] private _virtualCameras[] _allVirtualCameras;
    private CinemachineVirtualCamera _currentCamera;
    
    private CinemachineBrain _brain;
    private HeroController _controller;


    _virtualCameras _currentstruct;

    [Serializable]
    struct _virtualCameras
    {
        [SerializeField] public CinemachineVirtualCamera[] virtualCamera;
    }

    private void Awake()
    {
        _brain = GetComponent<CinemachineBrain>();
        _controller = FindObjectOfType<HeroController>();

        _horizontal -= 1;
        _vertical -= 1;
        _SwitchToCamera();
        _currentstruct = _allVirtualCameras[_horizontal];
    }

    public void SwitchCameraLeft()
    {
        if (_brain.IsBlending) return;
        Debug.Log("Left");
        _horizontal = Mathf.Max(0, _horizontal - 1);
        _SwitchToCamera();
    }

    public void SwitchCameraUp()
    {
        if (_brain.IsBlending) return;
        Debug.Log("Up");
        _vertical = Mathf.Max(0, _vertical - 1);
        _SwitchToCamera();
    }

    public void SwitchCameraRight()
    {
        if (_brain.IsBlending) return;
        GetArrayOfCamera();
        Debug.Log("Right");
        _horizontal = Mathf.Min(_horizontal + 1, _currentstruct.virtualCamera.Length - 1);
        _SwitchToCamera();
    }

    public void SwitchCameraDown()
    {
        if (_brain.IsBlending) return;
        Debug.Log("Down");
        _vertical = Mathf.Min(_vertical + 1, _allVirtualCameras.Length - 1);
        _SwitchToCamera();
    }

    private void _SwitchToCamera()
    {
        _currentCamera = _allVirtualCameras[_vertical].virtualCamera[_horizontal];
        Debug.Log(_allVirtualCameras[1].virtualCamera[0].name);
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
        Debug.Log(ControllerDisabler.Instance);
        // ControllerDisabler.Instance.InBlending = _brain.IsBlending;
    }

    private void GetArrayOfCamera()
    {
        foreach (_virtualCameras cameras in _allVirtualCameras)
        {
            _currentstruct = cameras;
            foreach (CinemachineVirtualCamera camera in cameras.virtualCamera)
            {
                if (_currentCamera == camera)
                {
                    return;
                }
            }
        }
    }
}
