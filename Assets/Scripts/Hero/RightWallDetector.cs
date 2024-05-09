using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWallDetector : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private HeroEntity _entity;
    [SerializeField] private Transform[] _detectionspoints;
    [SerializeField] private float _detectionLength = 0.1f;
    [SerializeField] private LayerMask _rightLayerMask;

    public bool DetectWallNearBy()
    {
        foreach (Transform detectionPoint in _detectionspoints)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(
                detectionPoint.position,
                Vector2.right,
                _detectionLength,
                _rightLayerMask);

            if (hitResult.collider != null)
            {
                if (_entity != null) _entity.touchedLayer = hitResult.collider.gameObject.layer;
                Debug.Log(hitResult.transform.gameObject.layer);
                return true;
            }
        }

        return false;
    }
}
