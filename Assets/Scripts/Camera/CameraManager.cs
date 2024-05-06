using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance {  get; private set; }

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Profile System")]
    [SerializeField] private CameraProfile _defaultCameraProfile;
    private CameraProfile _currentCameraProfile;
    // Transition
    private float _profileTransitionTimer = 0f;
    private float _profileTransitionDuration = 0f;
    private Vector3 _profileTransitionStartPosition;
    private float _profileTransitionStartSize;
    // Follow
    private Vector3 _profileLastFollowDestination;
    // Damping
    private Vector3 _dampedPosition;

    // Autoscroll
    private Vector3 _autoscrollPosition;
    private float _autoscrollTimer;

    // Bounds
    Rect boundsRect;
    Vector3 worldBottomLeft;
    Vector3 worldTopRight;
    Vector2 worldScreenSize;
    Vector2 worldHalfScreenSize;


    public void EnterProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        if (_currentCameraProfile.ProfilType == CameraProfileType.AutoScroll) return;
        _currentCameraProfile = cameraProfile;
        if (transition != null )
        {
            _PlayProfileTransition(transition);
        }
        _setCameraDampedPosition(_FindCameraNextPosition());
    }

    public void ExitProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        /*On met en argument la camera que l'on veut quitter, si elle est déjà différente
        de la caméra actuelle, on ne fait rien, sinon on revient à la caméra de base.*/
        if (_currentCameraProfile != cameraProfile) return;
        _currentCameraProfile = _defaultCameraProfile;
        if (transition != null )
        {
            _PlayProfileTransition(transition);
        }
        _setCameraDampedPosition(_FindCameraNextPosition());
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitToDefaultProfile();
    }

    private void Update()
    {
        Vector3 nextPosition = _FindCameraNextPosition();
        nextPosition = _ClampPositionIntoBounds(nextPosition);
        nextPosition = _ApplyDamping(nextPosition);

        if (_IsPlayingProfileTransition())
        {
            _profileTransitionTimer += Time.deltaTime;
            Vector3 transitionPosition = _CalculateProfileTransitionPosition(nextPosition);
            _SetCameraPosition(transitionPosition);
            float transitionSize = _CalculateProfilTransitionCameraSize(_currentCameraProfile.CameraSize);
            _SetCameraSize(transitionSize);
        } else
        {
            _SetCameraPosition(nextPosition);
            _SetCameraSize(_currentCameraProfile.CameraSize);
        }
    }

    private void _SetCameraPosition(Vector3 position)
    {
        Vector3 newCameraPosition = _camera.transform.position;
        newCameraPosition.x = position.x;
        newCameraPosition.y = position.y;
        _camera.transform.position = newCameraPosition;
    }

    private void _SetCameraSize(float size)
    {
        _camera.orthographicSize = size;
    }

    private void InitToDefaultProfile()
    {
        _currentCameraProfile = _defaultCameraProfile;
        _SetCameraPosition(_currentCameraProfile.transform.position);
        _SetCameraSize(_currentCameraProfile.CameraSize);
        _setCameraDampedPosition(_ClampPositionIntoBounds(_FindCameraNextPosition()));
    }

    private void _PlayProfileTransition(CameraProfileTransition transition)
    {
        _profileTransitionStartPosition = _camera.transform.position;

        _profileTransitionStartSize = _camera.orthographicSize;

        _profileTransitionTimer = 0f;
        _profileTransitionDuration = transition.duration;
    }

    private bool _IsPlayingProfileTransition()
    {
        return _profileTransitionTimer < _profileTransitionDuration;
    }

    private float _CalculateProfilTransitionCameraSize(float endSize)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        float startSize = _profileTransitionStartSize;
        return Mathf.Lerp(startSize, endSize, percent);
    }

    private Vector3 _CalculateProfileTransitionPosition(Vector3 destination)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        Vector3 origin = _profileTransitionStartPosition;
        return Vector3.Lerp(origin, destination, percent);
    }

    private Vector3 _FindCameraNextPosition()
    {
        if (_currentCameraProfile.ProfilType == CameraProfileType.FollowTarget)
        {
            if (_currentCameraProfile.TargetToFollow != null)
            {
                CameraFollowable targetToFollow = _currentCameraProfile.TargetToFollow;
                _profileLastFollowDestination.x = targetToFollow.FollowPositionX + _currentCameraProfile._followOffsetX;
                _profileLastFollowDestination.y = targetToFollow.FollowPositionY;
                return _profileLastFollowDestination;
            }
        } else if (_currentCameraProfile.ProfilType == CameraProfileType.AutoScroll)
        {
            _getBounds();
            float _minPosX = boundsRect.xMin + worldHalfScreenSize.x;
            float _maxPosX = boundsRect.xMax - worldHalfScreenSize.x;
            _autoscrollPosition.x = Mathf.Lerp(
                _minPosX,
                _maxPosX,
                _autoscrollTimer
                );
            _autoscrollPosition.y = _currentCameraProfile.Position.y;
            _autoscrollTimer += _currentCameraProfile.AutoScrollHorizontal * Time.fixedDeltaTime;
            return _autoscrollPosition;
        }
        return _currentCameraProfile.Position;
    }

    private Vector3 _ApplyDamping(Vector3 position)
    {
        if (_currentCameraProfile.UseDampingHorizontally)
        {
            _dampedPosition.x = Mathf.Lerp(
                _dampedPosition.x,
                position.x,
                _currentCameraProfile.HorizontalDampingFactor * Time.deltaTime
            );
        } else
        {
            _dampedPosition.x = position.x;
        }

        if (_currentCameraProfile.UseDampingVertically)
        {
            _dampedPosition.y = Mathf.Lerp(
                _dampedPosition.y,
                position.y,
                _currentCameraProfile.VerticalDampingFactor * Time.deltaTime
            );
        } else
        {
            _dampedPosition.y = position.y;
        }

        return _dampedPosition;
    }

    private void _setCameraDampedPosition(Vector3 position)
    {
        _dampedPosition.x = position.x;
        _dampedPosition.y = position.y;
    }

    private Vector3 _ClampPositionIntoBounds(Vector3 position)
    {
        if (!_currentCameraProfile.HasBounds) return position;

        _getBounds();

        if (position.x > boundsRect.xMax - worldHalfScreenSize.x)
        {
            position.x = boundsRect.xMax - worldHalfScreenSize.x;
        }

        if (position.x < boundsRect.xMin + worldHalfScreenSize.x)
        {
            position.x = boundsRect.xMin + worldHalfScreenSize.x;
        }

        if (position.y > boundsRect.yMax - worldHalfScreenSize.y)
        {
            position.y = boundsRect.yMax - worldHalfScreenSize.y;
        }

        if (position.y < boundsRect.yMin + worldHalfScreenSize.y)
        {
            position.y = boundsRect.yMin + worldHalfScreenSize.y;
        }

        return position;
    }

    private void _getBounds()
    {
        boundsRect = _currentCameraProfile.BoundsRect;
        worldBottomLeft = _camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        worldTopRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));
        worldScreenSize = new Vector2(worldTopRight.x - worldBottomLeft.x, worldTopRight.y - worldBottomLeft.y);
        worldHalfScreenSize = worldScreenSize / 2f;
    }
}