using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private HeroEntity _entity;
    private bool _entityWastouchingGround = false;

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;
    private float _cooldowntime = 0f;

    [Header("Jump Buffer")]
    [SerializeField] private float _jumpBufferDuration = 0.2f;
    private float _jumpBufferTimer = 0f;

    [Header("Coyote Time")]
    [SerializeField] private float _coyoteTimeDuration = 0.1f;
    private float _coyoteTimeCountDown = -1f;

    private void Start()
    {
        _CancelJumpBuffer();
    }

    private void Update()
    {

        _UpdateJumpBuffer();

        _entity.SetMoveDirX(GetInputMoveX());
        _entity.SetMoveDirY(GetInputMoveY());

        GetInputdash();

        if (_EntityHasExitGround())
        {
            _ResetCoyoteTime();
        } else
        {
            _UpdateCoyoteTime();
        }

        if (_GetInputDownJump())
        {
            if ((_entity.IsTouchingGround || _IsCoyoteTimeActive()) || !_entity._CheckIfMaxJumpReached())
            {
                Debug.Log("ici");
                _entity.JumpStart();
            } else
            {
                _ResetJumpBuffer();
            }
        }

        if (IsJumpBufferActive())
        {
            if ((_entity.IsTouchingGround || _IsCoyoteTimeActive()) && !_entity.isJumping)
            {
                _entity.JumpStart();
            }
        }

        if (_entity.IsJumpImpulsing)
        {
            if (!_GetInputJump() && _entity.isJumpMinDurationReached)
            {
                _entity.StopJumpimpulsion();
            }
        }

        _entityWastouchingGround = _entity.IsTouchingGround;
    }

    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"Timer du Jump buffer = {_jumpBufferTimer}");
        GUILayout.Label($"Coyote Time CountDown = {_coyoteTimeCountDown}");
        GUILayout.EndVertical();
    }

    private float GetInputMoveX()
    {
        float inputMoveX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            // Negative Means : To the left <=
            inputMoveX = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // Positive means : To the right =>
            inputMoveX = 1f;
        }

        return inputMoveX;
    }

    private float GetInputMoveY()
    {
        float inputMoveY = 0f;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
        {
            inputMoveY = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveY= -1f;
        }

        return inputMoveY;
    }

    private bool _GetInputDownJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool _GetInputJump()
    {
        return Input.GetKey(KeyCode.Space);
    }

    private void _ResetJumpBuffer()
    {
        _jumpBufferTimer = 0f;
    }

    private void _UpdateJumpBuffer()
    {
        if (!IsJumpBufferActive()) return;
        _jumpBufferTimer += Time.deltaTime;
    }

    private bool IsJumpBufferActive()
    {
        return _jumpBufferTimer < _jumpBufferDuration;
    }

    private void _CancelJumpBuffer()
    {
        _jumpBufferTimer = _jumpBufferDuration;
    }

    private void _UpdateCoyoteTime()
    {
        if (!_IsCoyoteTimeActive()) return;
        _coyoteTimeCountDown -= Time.deltaTime;
    }

    private bool _IsCoyoteTimeActive()
    {
        return _coyoteTimeCountDown > 0f;
    }

    private void _ResetCoyoteTime()
    {
        _coyoteTimeCountDown = _coyoteTimeDuration;
    }

    private bool _EntityHasExitGround()
    {
        return _entityWastouchingGround && !_entity.IsTouchingGround;
    }

    private void GetInputdash()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _entity.Dash();
        }
    }
}