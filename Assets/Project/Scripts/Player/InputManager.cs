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
    [SerializeField] private InputActionReference onAimClickActionReference;
    private InputAction m_onRightClickAction;

    #endregion

    #region Parameters

    [Header("Parameters"), SerializeField] private float m_doubleClickBufferTime;

    #endregion

    #region Outputs

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickPressedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickPerformedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickReleasedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool RightClickPerformedThisFrame { get; private set; }

    [field: SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool ClickHeld { get; private set; }

    public Action OnPerformHold;

    public Action OnReleaseHold;
    // public Action OnPerformAimClick;
    // public Action OnReleaseAimClick;

    public Vector2 MousePos
    {
        get { return Mouse.current.position.ReadValue(); }
    }

    #endregion

    private void OnEnable()
    {
        m_onClickAction = onClickActionReference.ToInputAction();
        m_onRightClickAction = onAimClickActionReference.ToInputAction();

        m_onClickAction.Enable();
        m_onRightClickAction.Enable();

        // m_onAimClickAction.performed += HoldAimClickPerformed;
        // m_onAimClickAction.canceled += HoldAimClickCanceled;

        StartCoroutine(IEUpdate());
    }

    private void OnDisable()
    {
        m_onClickAction.Disable();

        // m_onAimClickAction.performed -= HoldAimClickPerformed;
        // m_onAimClickAction.canceled -= HoldAimClickCanceled;

        StopAllCoroutines();
    }

    // private void HoldAimClickPerformed(InputAction.CallbackContext context)
    private void HoldClickPerformed()
    {
        OnPerformHold?.Invoke();
        ClickHeld = true;
        Debug.Log("start hold click");
    }

    // private void HoldAimClickCanceled(InputAction.CallbackContext context)
    private void HoldClickCanceled()
    {
        if (ClickHeld) OnReleaseHold?.Invoke();
        Debug.Log("stop hold aim click");

        ClickHeld = false;
    }

    [SerializeField, ReadOnly, ShowIf("m_debug")]
    public float t = 0f;

    [SerializeField, ReadOnly, ShowIf("m_debug")]
    public bool clickedOnce = false;

    public bool ShouldNormalShootThisFrame { get; private set; }

    private IEnumerator IEUpdate()
    {
        // release do click normal sem ter performado = tiro normal
        // hold do click normal = laser
        // click direito = antigo double click

        bool l_startedClickInCurrentAction = false;
        bool l_performedClickInCurrentAction = false;
        bool l_releasedClickInCurrentAction = false;

        while (true)
        {
            ShouldNormalShootThisFrame = false;
            ClickPressedThisFrame = m_onClickAction.WasPressedThisFrame();
            ClickPerformedThisFrame = m_onClickAction.WasPerformedThisFrame();
            ClickReleasedThisFrame = m_onClickAction.WasReleasedThisFrame();

            RightClickPerformedThisFrame = m_onRightClickAction.WasPerformedThisFrame();

            if (ClickPressedThisFrame) l_startedClickInCurrentAction = true;
            if (ClickPerformedThisFrame) l_performedClickInCurrentAction = true;
            if (ClickReleasedThisFrame) l_releasedClickInCurrentAction = true;

            if (l_startedClickInCurrentAction && !l_performedClickInCurrentAction && l_releasedClickInCurrentAction)
            {
                ShouldNormalShootThisFrame = true;
                l_startedClickInCurrentAction = false;
                l_releasedClickInCurrentAction = false;
            }
            else if (l_startedClickInCurrentAction && l_performedClickInCurrentAction && ClickPerformedThisFrame &&
                     !l_releasedClickInCurrentAction)
            {
                HoldClickPerformed();
            }
            else if (l_startedClickInCurrentAction && l_performedClickInCurrentAction &&
                     l_releasedClickInCurrentAction && ClickReleasedThisFrame)
            {
                HoldClickCanceled();
                l_startedClickInCurrentAction = false;
                l_performedClickInCurrentAction = false;
                l_releasedClickInCurrentAction = false;
            }

            // DoubleClickPressedThisFrame = false;

            // if (ClickPressedThisFrame && t < m_doubleClickBufferTime && clickedOnce)
            // {
            //     ClickPressedThisFrame = false;
            //     DoubleClickPressedThisFrame = true;
            //     Debug.Log("double clicked");
            //     clickedOnce = false;
            // }
            // else if (ClickPressedThisFrame)
            // {
            //     Debug.Log("normal click");
            //     if (!clickedOnce)
            //     {
            //         clickedOnce = true;
            //     }
            //
            //     t = 0f;
            // }
            //
            // t += Time.deltaTime;
            yield return null;
        }
    }

    // private void OnAimClick(InputAction.CallbackContext context)
    // {
    //     OnPerformAimClick?.Invoke();
    // }
    //
    // private void ReleaseAimClick(InputAction.CallbackContext context)
    // {
    //     OnReleaseAimClick?.Invoke();
    // }

    [Header("Debug"), SerializeField] private bool m_debug;
}