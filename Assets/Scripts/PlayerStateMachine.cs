using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;


public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float _rollDuration = .3f;
    
    private PlayerState _currentState;
    private Animator _animator;

    private int H_MoveSpeedX = Animator.StringToHash("MoveSpeedX");
    private int H_MoveSpeedY = Animator.StringToHash("MoveSpeedY");
    private int H_IsRolling = Animator.StringToHash("isRolling");
    private int H_IsSprinting = Animator.StringToHash("isSprinting");

    private float _endRollTime;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        TransitionToState(PlayerState.Locomotion);
    }

    void Update()
    {
        OnStateUpdate();
    }

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.Locomotion:
                break;
            case PlayerState.Sprint:
                _animator.SetBool(H_IsSprinting, true);
                break;
            case PlayerState.Roll:
                _animator.SetBool(H_IsRolling, true);
                _endRollTime = Time.time + _rollDuration;
                break;
        }
    }

    private void OnStateUpdate() {
        switch (_currentState) {
            case PlayerState.Locomotion:
                _animator.SetFloat(H_MoveSpeedX, Input.GetAxis("Horizontal"));
                _animator.SetFloat(H_MoveSpeedY, Input.GetAxis("Vertical"));

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(PlayerState.Roll);
                }
                break;
            case PlayerState.Sprint:
                _animator.SetFloat(H_MoveSpeedX, Input.GetAxis("Horizontal"));
                _animator.SetFloat(H_MoveSpeedY, Input.GetAxis("Vertical"));
                if (Input.GetButtonUp("Fire3"))
                {
                    TransitionToState(PlayerState.Locomotion);
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
                        TransitionToState(PlayerState.Locomotion);
                    }
                }
                break;
        }
    }

    private void OnStateExit() {
        switch (_currentState) {
            case PlayerState.Locomotion:
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
}


public enum PlayerState {
    Locomotion,
    Sprint,
    Roll
}