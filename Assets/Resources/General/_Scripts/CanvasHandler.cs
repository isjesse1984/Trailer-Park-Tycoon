using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class CanvasHandler : MonoBehaviour
{
    InputActionMap actionMap;
    InputAction managementCanvas;

    [SerializeField] private GameObject[] canvases;


    private void Awake()
    {
        actionMap = InputHandler.Instance.GetInput().FindActionMap("Default");
        managementCanvas = actionMap.FindAction("GameManagementCanvas");
        managementCanvas.performed += ctx => ToggleCanvas("GameManagementCanvas");

        ToggleCanvas();
    }

    private void ToggleCanvas(string requestedCanvas = null)
    {
        print($"{requestedCanvas} canvas");
        foreach (GameObject canvas in canvases)
        {
            if(requestedCanvas == null) { canvas.SetActive(false);   continue;   }

            if(canvas.name == requestedCanvas)
            {
                canvas.SetActive(!canvas.activeSelf);
            }
        }
    }
}
