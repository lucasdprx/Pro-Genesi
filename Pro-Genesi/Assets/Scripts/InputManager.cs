using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;

    public void Move(InputAction.CallbackContext ctx)
    {
        OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
    }
}
