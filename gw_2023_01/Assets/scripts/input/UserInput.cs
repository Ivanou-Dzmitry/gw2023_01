using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{

    public static UserInput instance;

    public bool Button1Pressed {  get; private set; }
    public bool Button2Pressed { get; private set; }
    public bool Button3Pressed { get; private set; }
    public bool Button4Pressed { get; private set; }
    public bool CancelMenu { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _direction1;
    private InputAction _direction2;
    private InputAction _direction3;
    private InputAction _direction4;
    private InputAction _cancelmenu;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void SetupInputActions()
    {
        _direction1 = _playerInput.actions["Button1"];
        _direction2 = _playerInput.actions["Button2"];
        _direction3 = _playerInput.actions["Button3"];
        _direction4 = _playerInput.actions["Button4"];
        _cancelmenu = _playerInput.actions["CancelMenu"];
    }

    private void UpdateInputs()
    {
        Button1Pressed = _direction1.IsPressed();
        Button2Pressed = _direction2.IsPressed();
        Button3Pressed = _direction3.IsPressed();
        Button4Pressed = _direction4.IsPressed();
        CancelMenu = _cancelmenu.WasPressedThisFrame();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
