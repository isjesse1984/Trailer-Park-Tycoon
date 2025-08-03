using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public InputActionAsset GetInput()
    {
        return playerInput.actions;
    }
    public InputActionMap ChangeActionMap(string actionName)
    {
        //playerInput.actions.FindActionMap(actionName)?.Enable();
        playerInput.SwitchCurrentActionMap(actionName);
        print(actionName + " action map enabled.");
        return playerInput.currentActionMap;
    }
}
