using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class StartPromptController : MonoBehaviour
{
    [Header("Input")]
    // Drag an InputActionReference that points to your trigger action 
    [SerializeField] private InputActionReference startHoldAction;
    // Optional skip action 
    [SerializeField] private InputActionReference skipAction;

    [Header("Hold Settings")]
    [SerializeField] private float holdDurationSeconds = 0.6f; // how long to hold
    [SerializeField] private float decayPerSecond = 2.0f;       // progress falloff when released
    [SerializeField, Range(0.1f, 1f)] private float pressThreshold = 0.75f; // analog threshold

    [Header("UI (optional)")]
    [SerializeField] private GameObject promptRoot; // “Hold trigger to start”
    [SerializeField] private Slider progressSlider; // set maxValue=1, wholeNumbers=false
    [SerializeField] private Image progressFill;    // alternative to Slider; uses fillAmount

    [Header("Events")]
    public UnityEvent onCompleted;

    private float _accum;
    private bool _done;

#if ENABLE_INPUT_SYSTEM
    private InputAction _hold;
    private InputAction _skip;
#endif

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        if (startHoldAction != null)
        {
            _hold = startHoldAction.action;
            if (!_hold.enabled) _hold.Enable();
        }
        if (skipAction != null)
        {
            _skip = skipAction.action;
            if (!_skip.enabled) _skip.Enable();
        }
#endif
        _accum = 0f;
        _done = false;
        SetProgress(0f);
        if (promptRoot) promptRoot.SetActive(true);
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        _hold?.Disable();
        _skip?.Disable();
#endif
    }

    private void Update()
    {
        if (_done) return;

        bool pressed = IsStartPressed();
        if (pressed) _accum += Time.unscaledDeltaTime;
        else _accum -= decayPerSecond * Time.unscaledDeltaTime;

        _accum = Mathf.Clamp(_accum, 0f, holdDurationSeconds);
        float t = (_accum / holdDurationSeconds);
        SetProgress(t);

        if (t >= 1f)
        {
            _done = true;
            if (promptRoot) promptRoot.SetActive(false);
            onCompleted?.Invoke();
        }

        if (IsSkipPressed())
        {
            // Optional: if you want a skip, mark complete immediately
            _done = true;
            if (promptRoot) promptRoot.SetActive(false);
            onCompleted?.Invoke();
        }
    }

    private void SetProgress(float normalized)
    {
        if (progressSlider != null) progressSlider.normalizedValue = normalized;
        if (progressFill != null) progressFill.fillAmount = normalized;
    }

    private bool IsStartPressed()
    {
#if ENABLE_INPUT_SYSTEM
        if (_hold == null) return false;

        // Prefer analog value for better feel across devices (XR triggers, gamepad triggers, etc.)
        float v = _hold.ReadValue<float>();
        // If the action is a Button type with a Press/Hold interaction, ReadValue might be 0/1 — still fine.
        return v >= pressThreshold || _hold.IsPressed();
#else
        // Legacy fallback: hold Space/Enter to start
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return);
#endif
    }

    private bool IsSkipPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return _skip != null && _skip.IsPressed();
#else
        return false;
#endif
    }
}
