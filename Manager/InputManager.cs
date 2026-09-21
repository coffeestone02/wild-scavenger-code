using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    InputSystem_Actions _inputActions;

    // 폴링용
    public Vector2 MoveAmt { get; private set; }
    public Vector2 LookAmt { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool AimPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    // 퀵슬롯
    public bool FirstQSPressed { get; private set; }
    public bool SecondQSPressed { get; private set; }
    public bool ThirdQSPressed { get; private set; }
    public bool FourQSPressed { get; private set; }
    public bool FifthQSPressed { get; private set; }

    // 이벤트
    public event Action OnInventoryPressed;
    public event Action OnUIClosePressed;

    void OnInventoryPerformed(InputAction.CallbackContext ctx) => OnInventoryPressed?.Invoke();
    void OnUIClosePerformed(InputAction.CallbackContext ctx) => OnUIClosePressed?.Invoke();

    public void Init()
    {
        _inputActions = new InputSystem_Actions();

        // 이벤트 구독
        _inputActions.Player.Inventory.performed += OnInventoryPerformed;
        _inputActions.Player.Escape.performed += OnUIClosePerformed;
        _inputActions.UI.Escape.performed += OnUIClosePerformed;

        EnableGameplay();
    }

    public void OnUpdate()
    {
        MoveAmt = _inputActions.Player.Move.ReadValue<Vector2>();
        LookAmt = _inputActions.Player.Look.ReadValue<Vector2>();
        AttackPressed = _inputActions.Player.Attack.IsPressed();
        AimPressed = _inputActions.Player.Aim.IsPressed();
        InteractPressed = _inputActions.Player.Interact.WasPressedThisFrame();

        FirstQSPressed = _inputActions.Player.FirstQuickslot.WasPressedThisFrame();
        SecondQSPressed = _inputActions.Player.SecondQuickslot.WasPressedThisFrame();
        ThirdQSPressed = _inputActions.Player.ThirdQuickslot.WasPressedThisFrame();
        FourQSPressed = _inputActions.Player.FourthQuickslot.WasPressedThisFrame();
        FifthQSPressed = _inputActions.Player.FifthQuickslot.WasPressedThisFrame();
    }

    /// <summary> 게임 플레이 입력만 활성화 </summary>
    public void EnableGameplay()
    {
        if (_inputActions == null) { return; }

        _inputActions.Player.Enable();
        _inputActions.UI.Disable();
    }

    /// <summary> UI 입력만 활성화 </summary>
    public void EnableUI()
    {
        if (_inputActions == null) { return; }

        _inputActions.Player.Disable();
        _inputActions.UI.Enable();
    }

    public void Clear()
    {
        if (_inputActions == null) { return; }

        _inputActions.Player.Inventory.performed -= OnInventoryPerformed;
        _inputActions.Player.Escape.performed -= OnUIClosePerformed;
        _inputActions.UI.Escape.performed -= OnUIClosePerformed;

        _inputActions.Disable();
        _inputActions.Dispose();
        _inputActions = null;
    }
}
