using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    #region Input System References

    [Header("References"), SerializeField] private InputActionReference onClickActionReference;
    private InputAction m_onClickAction;

    #endregion

    #region Parameters

    [Header("Parameters"), SerializeField] private float m_doubleClickBufferTime;

    #endregion

    #region Outputs

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickPressedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickReleasedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool DoubleClickPressedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickHeld { get; private set; }

    public Vector2 MousePos
    {
        get { return Mouse.current.position.ReadValue(); }
    }

    #endregion

    private void OnEnable()
    {
        m_onClickAction = onClickActionReference.ToInputAction();

        m_onClickAction.Enable();

        m_onClickAction.performed += HoldClickPerformed;
        m_onClickAction.canceled += HoldClickCanceled;

        StartCoroutine(IEUpdate());
    }

    private void OnDisable()
    {
        m_onClickAction.Disable();

        m_onClickAction.performed -= HoldClickPerformed;
        m_onClickAction.canceled -= HoldClickCanceled;

        StopAllCoroutines();
    }

    private void HoldClickPerformed(InputAction.CallbackContext context)
    {
        ClickHeld = true;
    }

    private void HoldClickCanceled(InputAction.CallbackContext context)
    {
        ClickHeld = false;
    }

    [SerializeField, ReadOnly, ShowIf("m_debug")]
    public float t = 0f;

    [SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool clickedOnce = false;

    private IEnumerator IEUpdate()
    {
        while (true)
        {
            ClickPressedThisFrame = m_onClickAction.WasPressedThisFrame();
            ClickReleasedThisFrame = m_onClickAction.WasReleasedThisFrame();
            DoubleClickPressedThisFrame = false;

            if (ClickPressedThisFrame && t < m_doubleClickBufferTime && clickedOnce)
            {
                DoubleClickPressedThisFrame = true;
                clickedOnce = false;
            }
            else if (ClickPressedThisFrame)
            {
                if (!clickedOnce)
                {
                    clickedOnce = true;
                }

                t = 0f;
            }

            t += Time.deltaTime;
            yield return null;
        }
    }

    [Header("Debug"), SerializeField] private bool m_debug;
}