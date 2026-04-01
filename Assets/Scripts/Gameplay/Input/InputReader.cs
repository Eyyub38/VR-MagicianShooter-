using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu( fileName = "New Input Reader", menuName = "Game/Input Reader" )]
public class InputReader : ScriptableObject, GameInputs.IPlayerActions, GameInputs.ISpellActions, GameInputs.IUIActions {
    GameInputs inputActions;

    public event System.Action<Vector2> OnLookInitiated;
    public event System.Action OnStartCast;
    public event System.Action OnReleaseCast;

    void OnEnable() {
        if(inputActions == null) {
            inputActions = new GameInputs();
            inputActions.Player.AddCallbacks( this );
            inputActions.Spell.AddCallbacks( this );
        }

        inputActions.Enable();
    }

    void OnDisable() {
        inputActions.Disable();
    }

    public void OnLook(InputAction.CallbackContext callbackContext) {
        OnLookInitiated?.Invoke( callbackContext.ReadValue<Vector2>() );
    }
    
    public void OnCast(InputAction.CallbackContext callbackContext) {
        if(callbackContext.started) OnStartCast?.Invoke();
        else if(callbackContext.canceled) OnReleaseCast?.Invoke();
    }

    public void OnNavigate(InputAction.CallbackContext context) {
    }

    public void OnSubmit(InputAction.CallbackContext context) {
    }
}
