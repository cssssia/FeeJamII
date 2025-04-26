using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
     [SerializeField] private InputActionReference onClickActionReference;
     private InputAction onClickAction;
     public Vector2 MousePos
    {
        get
        {
            return Mouse.current.position.ReadValue();
        }
    }
  
    [field:SerializeField, ReadOnly]   public bool ClickPressed { get; private set; }
 
    [field:SerializeField, ReadOnly]    public bool ClickPressedThisFrame { get; private set; }
   
    [field:SerializeField, ReadOnly]  public bool ClickReleasedThisFrame { get; private set; }

   // [field:SerializeField, ReadOnly]  public bool DoubleClickPerformedThisAction { get; private set; }

    private void OnEnable()
    {
        onClickActionReference.action.Enable();
        // onClickActionReference.action.performed += OnDoubleClick;
        StartCoroutine(IEUpdate());
    }

    [SerializeField] private float m_doubleClickBufferTime;
   public  float t = 0f;
   public bool clickedOnce=false;
    private IEnumerator IEUpdate()
    {
        onClickAction = onClickActionReference.ToInputAction();
        while (true)
        {
            ClickPressed = onClickAction.IsPressed();
            ClickPressedThisFrame = onClickAction.WasPressedThisFrame();
            ClickReleasedThisFrame = onClickAction.WasReleasedThisFrame();
            if (ClickPressedThisFrame && t <= m_doubleClickBufferTime && clickedOnce)
            {
                Debug.Log("double click");
                OnDoubleClickAction?.Invoke();
                clickedOnce = false;
            } else if (ClickPressedThisFrame)
            {
                if (!clickedOnce)
                {
                    clickedOnce = true;
                    Debug.Log("single click");
                }
                t = 0f;
            } 
            t+= Time.deltaTime;
            yield return null;
        }
    }

    private void OnDisable()
    {
        onClickActionReference.action.Disable();
        // onClickActionReference.action.performed -= OnDoubleClick;
        StopAllCoroutines();
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public Action OnDoubleClickAction;
    public void OnDoubleClick(InputAction.CallbackContext context)
    {
    //    DoubleClickPerformedThisAction = true;
        OnDoubleClickAction?.Invoke();
    }
}
