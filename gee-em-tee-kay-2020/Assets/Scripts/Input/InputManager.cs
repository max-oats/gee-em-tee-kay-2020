using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum InputType
{
    Default = 0,
    Movement = 1,
    Actions = 2,
    Camera = 3,
    UI = 5,
}

public class InputManager : MonoBehaviour
{
    public List<bool> enabledByDefault = new List<bool>();

    private Dictionary<InputType, Reasonator> enableReasons = new Dictionary<InputType, Reasonator>();
    private Dictionary<InputType, Reasonator> disableReasons = new Dictionary<InputType, Reasonator>();

    private Rewired.Player rewired = null;

    private bool CanUseInput
    {
        get
        {
            return inputDisableTime == 0f;
        }
    }

    private float inputDisableTime = 0f;

    void Awake()
    {
        rewired = Rewired.ReInput.players.GetPlayer("Player0");

        InputType[] types = (InputType[])System.Enum.GetValues(typeof(InputType));
        for (int i = 0; i < types.Length; ++i)
        {
            enableReasons.Add(types[i], new Reasonator());
            disableReasons.Add(types[i], new Reasonator());

            if (enabledByDefault[i])
            {
                AddEnableReason(types[i], "Enabled by default");
            }
        }
    }

    void Update()
    {
        inputDisableTime -= Time.unscaledDeltaTime;

        if (inputDisableTime < 0f)
        {
            inputDisableTime = 0f;
        }
    }

    public void DisableInputTemporarily()
    {
        inputDisableTime = 0.1f;
    }

    private void UpdateInputActivity(InputType type)
    {
        bool enabled = false;
        if (!disableReasons[type].HasReasons())
        {
            enabled = enableReasons[type].HasReasons();
        }
        
        rewired.controllers.maps.SetMapsEnabled(enabled, (int)type);
    }

    public Guid AddDisableReason(InputType type, string reason)
    {
        if (disableReasons.TryGetValue(type, out Reasonator reasonator))
        {
            Guid id = reasonator.AddReason(reason);

            UpdateInputActivity(type);

            return id;
        }

        return Guid.Empty;
    }

    public void RemoveDisableReason(InputType type, Guid id)
    {
        if (disableReasons.TryGetValue(type, out Reasonator reasonator))
        {
            reasonator.RemoveReason(id);

            UpdateInputActivity(type);
        }
    }

    public Guid AddEnableReason(InputType type, string reason)
    {
        if (enableReasons.TryGetValue(type, out Reasonator reasonator))
        {
            Guid id = reasonator.AddReason(reason);
            
            UpdateInputActivity(type);

            return id;
        }

        return Guid.Empty;
    }

    public void RemoveEnableReason(InputType type, Guid id)
    {
        if (enableReasons.TryGetValue(type, out Reasonator reasonator))
        {
            reasonator.RemoveReason(id);
            
            UpdateInputActivity(type);
        }
    }

    public void RemoveDisableReason(params InputType[] types)
    {
        foreach (InputType type in types)
        {
            if (disableReasons.TryGetValue(type, out Reasonator reasonator))
            {
                reasonator.Decrement();
                UpdateInputActivity(type);
            }
        }
    }

    public void AddDisableReason(params InputType[] types)
    {
        foreach (InputType type in types)
        {
            if (disableReasons.TryGetValue(type, out Reasonator reasonator))
            {
                reasonator.Increment();
                UpdateInputActivity(type);
            }
        }
    }

    public void ResetAll()
    {
        foreach (InputType type in System.Enum.GetValues(typeof(InputType)))
        {
            UpdateInputActivity(type);
        }
    }

    public void OverrideDisableAll()
    {
        Game.input.rewired.controllers.maps.SetAllMapsEnabled(false);
    }

    public bool GetButtonDown(int buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButtonDown(buttonName);
    }

    public bool GetButtonDown(string buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButtonDown(buttonName);
    }

    public bool GetButton(int buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButton(buttonName);
    }

    public bool GetButton(string buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButton(buttonName);
    }

    public float GetAxis(int axisID)
    {
        if (!CanUseInput)
        {
            return 0f;
        }

        return rewired.GetAxis(axisID);
    }

    public float GetAxis(string axisName)
    {
        if (!CanUseInput)
        {
            return 0f;
        }

        return rewired.GetAxis(axisName);
    }

    public bool GetButtonUp(int buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButtonUp(buttonName);
    }

    public bool GetButtonUp(string buttonName)
    {
        if (!CanUseInput)
        {
            return false;
        }

        return rewired.GetButtonUp(buttonName);
    }

}