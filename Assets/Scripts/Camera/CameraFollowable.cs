using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowable : MonoBehaviour
{
    private HeroEntity _entity;

    public float FollowPositionX { get; set; }

    public float FollowPositionY { get; set; }

    private void Start()
    {
        _entity = GetComponent<HeroEntity>();
    }

    public float _getOrientXFromEntity()
    {
        return _entity.OrientX;
    }
}
