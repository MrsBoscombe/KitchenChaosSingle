using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding{
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause
    }

    public static GameInput Instance {get; private set;}

    private PlayerInputActions playerInputActions;
    private void Awake(){
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)){
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }        
        
        playerInputActions.Player.Enable();     // was playerInputActions.Enable(); 03/27/2024

        playerInputActions.Player.Interact.performed += InteractPerformed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
        playerInputActions.Player.Pause.performed += PausePerformed;

        
    }

    private void OnDestroy(){
        // Needed to remove the static PlayerInputActions and the listeners for when we restart the
        // game (ie, Pause->MainMenu->Play->Pause)
        playerInputActions.Player.Interact.performed -= InteractPerformed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
        playerInputActions.Player.Pause.performed -= PausePerformed;

        playerInputActions.Dispose();

    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        // check for listeners
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        /*
        if (OnInteractAction != null){
            OnInteractAction.Invoke(this, EventArgs.Empty);
        }
        else{
            Debug.Log("OnInteractAction is null");
        }

        Debug.Log(obj);
        */
    }
    
    private void InteractAlternatePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        // check for listeners
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void PausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized(){

        // This uses the new input system
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        /* old input system
        if (Input.GetKey(KeyCode.W)){
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A)){
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S)){
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D)){
            inputVector.x = 1;
        }
        end of old input system */

        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding){
        switch (binding){
            default:
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamepadInteract:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamepadInteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamepadPause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();

            // The following all add " " around the value they return - UGH!
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();


        }

    }

    public void RebindBinding(Binding binding, Action onActionRebound){
        // First - disable the player input actions
        playerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;

        switch (binding){
            default:
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamepadInteract:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamepadInteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamepadPause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).
                OnComplete(callback => {
                callback.Dispose();     
                playerInputActions.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            }).Start();
        


    }
}
