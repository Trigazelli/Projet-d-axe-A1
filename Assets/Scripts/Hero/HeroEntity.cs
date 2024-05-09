using UnityEngine;
using UnityEngine.Serialization;

public class HeroEntity : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Horizontal Movements")]
    [FormerlySerializedAs("_movementSettings")]
    [SerializeField] private HeroHorizontalMovementSettings _groundHorizontalMovementSettings;
    [SerializeField] private HeroHorizontalMovementSettings _airHorizontalMovementSettings;
    private float _horizontalSpeed = 0f;
    private float _moveDirX = 0f;

    [Header("Vertical Movements")]
    private float _verticalSpeed = 0f;

    [Header("Jump")]
    [SerializeField] private HeroJumpSettings[] _allJumpsSettings;
    [SerializeField] private HeroJumpSettings _wallJumpSettings;
    [SerializeField] private HeroFallSettings _jumpFallSettings;
    [SerializeField] private HeroHorizontalMovementSettings _jumpHorizontalMovementSettings;
    private HeroJumpSettings _currentJumpSettings;
    private int _jumpIndex = 0;

    // Camera Follow
    private CameraFollowable _cameraFollowable;

    enum JumpState
    {
        Notjumping,
        JumpImpulsion,
        Falling,
    }

    private JumpState _jumpState = JumpState.Notjumping;
    private float _jumpTimer = 0f;

    [Header("Ground")]
    [SerializeField] private GroundDetector _groundDetector;
    public bool IsTouchingGround { get; private set; }

    [Header("LeftWall")]
    [SerializeField] private LeftWallDetector _leftWallDetector;

    private bool _isTouchingLeftWall;

    [Header("RightWall")]
    [SerializeField] private RightWallDetector _rightWallDetector;

    private bool _isTouchingRightWall;

    private int _touchedLayer;
    public int touchedLayer {  get { return _touchedLayer; } set { _touchedLayer = value; } }

    [Header("Fall")]
    [SerializeField] private HeroFallSettings _fallSettings;

    [Header("WallSlide and WallJump")]
    // Je voulais mettre cette valeur dans HeroHorizontalMovementSettings,
    // mais elle ne sert que pour ce cas particulier, donc ça n'a pas beaucoup d'intérêt.
    [SerializeField] private float _maxWallSlideSpeed;

    [Header("Dash")]
    [SerializeField] private HeroDashSettings _dashSettings;
    public HeroDashSettings DashSettings
    {
        get { return _dashSettings; }
    }
    private float _dashTime = 0f;
    private bool _isDashing = false;
    public bool isDashing
    {
        get { return _isDashing; }
    }


    [Header("Orientation")]
    [SerializeField] private Transform _orientVisualRoot;
    private float _orientX = 1f;
    public float OrientX
    {
        get { return _orientX; }
    }

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;

    private void Awake()
    {
        _cameraFollowable = GetComponent<CameraFollowable>();
        _cameraFollowable.FollowPositionX = _rigidbody.position.x;
        _cameraFollowable.FollowPositionY = _rigidbody.position.y;
        Debug.Log(_allJumpsSettings.Length);
    }

    #region Functions Move Dir

    /*    public float GetMoveDirX()
        {
            return _moveDirX;
        }*/
    public void SetMoveDirX(float dirX)
    {
        _moveDirX = dirX;
    }

    #endregion

    public void Dash()
    {
        Debug.Log(_jumpState);
        _isDashing = true;
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = _dashSettings.speed * _orientX;
        velocity.y = 0;
        _rigidbody.velocity = velocity;
    }

    public void StopJumpimpulsion()
    {
        _jumpState = JumpState.Falling;
    }

    public bool IsJumpImpulsing => _jumpState == JumpState.JumpImpulsion;

    public bool isJumpMinDurationReached => _jumpTimer >= _currentJumpSettings.jumpMinDuration;

    public void JumpStart()
    {
        if (!IsTouchingGround)
        {
            if (_isTouchingLeftWall)
            {
                if (touchedLayer != 3) return;
                _orientX = 1;
                _wallJump();
                return;
            }
            else if (_isTouchingRightWall)
            {
                if (touchedLayer != 3) return;
                _orientX = -1;
                _wallJump();
                return;
            }
        }
        _GetCurrentJumpSettings();
        _jumpState = JumpState.JumpImpulsion;
        _jumpTimer = 0f;
    }

    public bool isJumping => _jumpState != JumpState.Notjumping;

    public bool _CheckIfMaxJumpReached()
    {
        if (_isTouchingLeftWall || _isTouchingRightWall) return false;
        return _jumpIndex >= _allJumpsSettings.Length;
    }

    private void _UpdateJumpStateImpulsion(HeroJumpSettings settings)
    {
        _jumpTimer += Time.fixedDeltaTime;
        if (_jumpTimer < settings.jumpMaxDuration)
        {
            _verticalSpeed = settings.jumpSpeed;
        }
        else
        {
            _jumpState = JumpState.Falling;
        }
    }

    private void _UpdateJumpStateFalling()
    {
        if (!IsTouchingGround)
        {
            _ApplyFallGravity(_jumpFallSettings);
        } else
        {
            _jumpIndex = 0;
            _ResetVerticalSpeed();
            _jumpState = JumpState.Notjumping;
        }
    }

    private void _Updatejump()
    {
        switch (_jumpState)
        {
            case JumpState.JumpImpulsion:
                _UpdateJumpStateImpulsion(_currentJumpSettings);
                break;

            case JumpState.Falling:
                _UpdateJumpStateFalling();
                break;
        }
    }

    private void _GetCurrentJumpSettings()
    {
        if (_CheckIfMaxJumpReached()) return;
        _currentJumpSettings = _allJumpsSettings[_jumpIndex];
        _jumpIndex++;
    }

    private void FixedUpdate()
    {
        _ApplyLeftWallDetection();
        _ApplyRightWallDetection();
        _ApplyGroundDetection();
        _UpdateCameraFollowPosition();

        HeroHorizontalMovementSettings horizontalMovementSettings = _GetCurrentHorizontalMovementSettings();

        if (_isDashing)
        {
            if (_dashTime > _dashSettings.duration || _isTouchingLeftWall || _isTouchingRightWall)
            {
                _isDashing = false;
                _dashTime = 0f;
                _horizontalSpeed = horizontalMovementSettings.speedMax;
                _ResetVerticalSpeed();
                _jumpState = JumpState.Falling;
            }
            else
            {
                Dash();
                _dashTime += Time.fixedDeltaTime;
                return;
            }
        }

        if ((_isTouchingLeftWall && _orientX == -1) || (_isTouchingRightWall && _orientX == 1))
        {
            _horizontalSpeed = 0;
        }

        if (_AreOrientAndMovementOpposite())
        {
            _TurnBack(horizontalMovementSettings);
        } else
        {
            _UpdateHorizontalSpeed(horizontalMovementSettings);
            _ChangeOrientFromHorizontalMovement();
        }

        if (isJumping)
        {
            if ((_isTouchingLeftWall || _isTouchingRightWall) && _verticalSpeed < 0f && touchedLayer == 3)
            {
                _WallSlide(horizontalMovementSettings);
            }
            else
            {
                _Updatejump();
            }
        } else
        {
            if (!IsTouchingGround)
            {
                _ApplyFallGravity(_fallSettings);
            } else
            {
                _jumpIndex = 0;
                _ResetVerticalSpeed();
            }
        }

        _ApplyHorizontalSpeed();
        _ApplyVerticalSpeed();
    }

    private void _ChangeOrientFromHorizontalMovement()
    {
        if (_moveDirX == 0f) return;
        _orientX = Mathf.Sign(_moveDirX);
    }

    private bool _AreOrientAndMovementOpposite()
    {
        return _moveDirX * _orientX < 0f;
    }

    private void _ApplyHorizontalSpeed()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = _horizontalSpeed * _orientX;
        _rigidbody.velocity = velocity;
    }

    private void _ApplyVerticalSpeed()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.y = _verticalSpeed;
        _rigidbody.velocity = velocity;
    }

    private void _ApplyFallGravity(HeroFallSettings settings)
    {
        _verticalSpeed -= settings.fallGravity * Time.fixedDeltaTime;
        if (_verticalSpeed < - settings.fallSpeedMax)
        {
            _verticalSpeed = - settings.fallSpeedMax;
        }
    }

    private void _ApplyGroundDetection()
    {
        IsTouchingGround = _groundDetector.DetectGroundNearBy();
    }

    private void _ApplyLeftWallDetection()
    {
        _isTouchingLeftWall = _leftWallDetector.DetectWallNearBy();
    }

    private void _ApplyRightWallDetection()
    {
        _isTouchingRightWall = _rightWallDetector.DetectWallNearBy();
    }

    private void _ResetVerticalSpeed()
    {
        _verticalSpeed = 0f;
    }

    private void _Accelerate(HeroHorizontalMovementSettings settings)
    {
        _horizontalSpeed += settings.acceleration * Time.fixedDeltaTime;
        if (_horizontalSpeed > settings.speedMax)
        {
            _horizontalSpeed = settings.speedMax;
        }
    }

    private void _Decelerate(HeroHorizontalMovementSettings settings)
    {   
        _horizontalSpeed -= settings.deceleration * Time.fixedDeltaTime;
        if (_horizontalSpeed < 0f)
        {
            _horizontalSpeed = 0f;
        }
    }
    
    private void _UpdateHorizontalSpeed(HeroHorizontalMovementSettings settings)
    {
        if (_moveDirX != 0f)
        {
            _Accelerate(settings);
        } else
        {
            _Decelerate(settings);
        }
    }

    private void Update()
    {
        _UpdateOrientVisual();
    }

    private void _TurnBack(HeroHorizontalMovementSettings settings)
    {
        _horizontalSpeed -= settings.turnBackFrictions * Time.fixedDeltaTime;
        if (_horizontalSpeed < 0f)
        {
            _horizontalSpeed = 0f;
            _ChangeOrientFromHorizontalMovement();
        }
    }

    private HeroHorizontalMovementSettings _GetCurrentHorizontalMovementSettings()
    {
        return IsTouchingGround ? _groundHorizontalMovementSettings : _airHorizontalMovementSettings;
    }

    private void _UpdateOrientVisual()
    {
        Vector3 newScale = _orientVisualRoot.localScale;
        newScale.x = _orientX;
        _orientVisualRoot.localScale = newScale;
    }

    private void _UpdateCameraFollowPosition()
    {
        _cameraFollowable.FollowPositionX = _rigidbody.position.x;
        if (IsTouchingGround && !isJumping)
        {
            _cameraFollowable.FollowPositionY = _rigidbody.position.y;
        }
    }

    private void _WallSlide(HeroHorizontalMovementSettings settings)
    {
        _verticalSpeed = _maxWallSlideSpeed;
    }

    private void _wallJump()
    {
        _horizontalSpeed = 8f;
        _verticalSpeed = 8f;
        _jumpState = JumpState.JumpImpulsion;
    }

    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"MoveDirX = {_moveDirX}");
        GUILayout.Label($"OrientX = {_orientX}");
        if (IsTouchingGround)
        {
            GUILayout.Label("OnGround");
        } else
        {
            GUILayout.Label("InAir");
        }
        GUILayout.Label($"JumpState = {_jumpState}");
        GUILayout.Label($"Horizontal Speed = {_horizontalSpeed}");
        GUILayout.Label($"Vertical Speed = { _verticalSpeed}");
        GUILayout.Label($"_isTouchingLeftWall = {_isTouchingLeftWall}");
        GUILayout.Label($"_isTouchingRightWall = {_isTouchingRightWall}");
        GUILayout.Label($"_jumpIndex = {_jumpIndex}");
        GUILayout.EndVertical();
    }
}