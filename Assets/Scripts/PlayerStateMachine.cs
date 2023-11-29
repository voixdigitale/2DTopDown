using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _rollDuration = .3f;
    [SerializeField] private float _runSpeed = 1f;
    [SerializeField] private float _sprintSpeed = 2f;
    [SerializeField] private float _rollSpeed = 3f;
    
    private PlayerState _currentState;
    private Animator _animator;
    private Rigidbody2D _rb2d;
    private float _currentSpeed = 0;
    private Vector2 _direction = new Vector2();

    private int H_MoveSpeedX = Animator.StringToHash("MoveSpeedX");
    private int H_MoveSpeedY = Animator.StringToHash("MoveSpeedY");
    private int H_IsRolling = Animator.StringToHash("isRolling");
    private int H_IsSprinting = Animator.StringToHash("isSprinting");

    private float _endRollTime;

    void Awake()
    {
        _animator = _playerModel.GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        TransitionToState(PlayerState.Run);
    }

    void Update()
    {
        OnStateUpdate();
    }

    private void FixedUpdate() {
        _rb2d.velocity = _direction * _currentSpeed;
    }

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.Run:
                _currentSpeed = _runSpeed;
                break;
            case PlayerState.Sprint:
                _animator.SetBool(H_IsSprinting, true);
                _currentSpeed = _sprintSpeed;
                break;
            case PlayerState.Roll:
                _animator.SetBool(H_IsRolling, true);
                _currentSpeed = _rollSpeed;
                _endRollTime = Time.time + _rollDuration;
                break;
        }
    }

    private void OnStateUpdate() {

        switch (_currentState) {
            case PlayerState.Run:
                SetDirection();

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(PlayerState.Roll);
                }
                break;
            case PlayerState.Sprint:
                SetDirection();

                if (Input.GetButtonUp("Fire3"))
                {
                    TransitionToState(PlayerState.Run);
                }
                break;
            case PlayerState.Roll:
                if (Time.time > _endRollTime)
                {
                    if (Input.GetButton("Fire3"))
                    {
                        TransitionToState(PlayerState.Sprint);
                    }
                    else
                    {
                        TransitionToState(PlayerState.Run);
                    }
                }
                break;
        }
    }

    private void OnStateExit() {
        switch (_currentState) {
            case PlayerState.Run:
                break;
            case PlayerState.Sprint:
                _animator.SetBool(H_IsSprinting, false);
                break;
            case PlayerState.Roll:
                _animator.SetBool(H_IsRolling, false);
                break;
        }
    }

    private void TransitionToState(PlayerState state)
    {
        OnStateExit();
        _currentState = state;
        OnStateEnter();
    }

    private void SetDirection() {
        if (!GameManager.instance.IsGameOver) {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");
        } else {
            _direction = Vector2.zero;
        }

        _animator.SetFloat(H_MoveSpeedX, _direction.x);
        _animator.SetFloat(H_MoveSpeedY, _direction.y);
    }
}


public enum PlayerState {
    Run,
    Sprint,
    Roll
}