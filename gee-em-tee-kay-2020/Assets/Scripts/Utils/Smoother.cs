using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Smoother
{
    [SerializeField] private float smoothTime = 0f; // The (approximate) time it takes to smooth the value
    [SerializeField] private float initialDesired = 0f; // The initial desired value
    [SerializeField] public bool isAngular = false; // Is the smoother using angular values

    private float currentValue; // The current value (recieved by smooth)
    private float desiredValue; // The value we're moving towards
    private float smoothVelocity; // The velocity that we're moving at
    
    private float defaultSmoothTime = 0f;

    public Smoother() 
    {
        defaultSmoothTime = smoothTime;
    }

    public Smoother(float smoothTime, float initialDesired, bool isAngular = false)
    {
        this.smoothTime = smoothTime;
        this.initialDesired = initialDesired;
        this.isAngular = isAngular;
        defaultSmoothTime = smoothTime;
    }

    public Smoother(Smoother other)
    {
        smoothTime = other.GetSmoothTime();
        initialDesired = other.GetInitialDesired();
        isAngular = other.isAngular;
        defaultSmoothTime = smoothTime;
    }

    public void InitValues()
    {
        currentValue = initialDesired;
        desiredValue = initialDesired;
        defaultSmoothTime = smoothTime;
    }

    public float GetCurrentValue()
    {
        return this.currentValue;
    }

    public float GetDesiredValue()
    {
        return this.desiredValue;
    }

    public float GetVelocity()
    {
        return smoothVelocity;
    }

    public float GetSmoothTime()
    {
        return smoothTime;
    }

    public void ResetSmoothTime()
    {
        smoothTime = defaultSmoothTime;
    }

    public float GetInitialDesired()
    {
        return initialDesired;
    }
    
    public void ResetDesiredValue()
    {
        desiredValue = initialDesired;
    }

    public void SetValue(float newValue)
    {
        this.currentValue = newValue;
        this.desiredValue = newValue;
    }

    public void SetCurrentValue(float current)
    {
        this.currentValue = current;
    }

    public void SetDesiredValue(float desired)
    {
        this.desiredValue = desired;
    }

    public void SetDesiredValue(float desired, float delay)
    {
        SetDesiredWithDelay(desired, delay);
    }

    public void SetSmoothTime(float newSmoothTime)
    {
        smoothTime = newSmoothTime;
    }

    private IEnumerator SetDesiredWithDelay(float desired, float delay)
    {
        yield return new WaitForSeconds(delay);

        SetDesiredValue(desired);
    }

    public float Smooth()
    {
        if (NeedsSmoothing())
        {
            float newVelocity = smoothVelocity;

            if (!isAngular)
                currentValue = Mathf.SmoothDamp(currentValue, desiredValue, ref newVelocity, smoothTime);
            else
                currentValue = Mathf.SmoothDampAngle(currentValue, desiredValue, ref newVelocity, smoothTime);
                
            smoothVelocity = newVelocity;
        }

        return currentValue;
    }

    public bool NeedsSmoothing()
    {
        return true;
    }
}

[System.Serializable]
public class VectorSmoother
{
    [SerializeField] private float smoothTime = 0f; // The (approximate) time it takes to smooth the value
    [SerializeField] private Vector3 initialDesired = Vector3.zero; // The initial desired value

    private Vector3 currentValue; // The current value (recieved by smooth)
    private Vector3 desiredValue; // The value we're moving towards
    private Vector3 smoothVelocity; // The velocity that we're moving at

    private float defaultSmoothTime = 0f;

    public VectorSmoother() 
    {
        defaultSmoothTime = smoothTime;
    }

    public VectorSmoother(float smoothTime, Vector3 initialDesired)
    {
        this.smoothTime = smoothTime;
        this.initialDesired = initialDesired;

        defaultSmoothTime = smoothTime;
    }

    public VectorSmoother(VectorSmoother other)
    {
        smoothTime = other.GetSmoothTime();
        initialDesired = other.GetInitialDesired();

        defaultSmoothTime = smoothTime;
    }

    public void InitValues()
    {
        defaultSmoothTime = smoothTime;
        SetCurrentValue(initialDesired);
        SetDesiredValue(initialDesired);
    }

    public Vector3 GetCurrentValue()
    {
        return currentValue;
    }

    public void SetDesiredValue(Vector3 newDesired)
    {
        desiredValue = newDesired;
    }

    public void SetCurrentValue(Vector3 newDesired)
    {
        currentValue = newDesired;
    }

    public float GetSmoothTime()
    {
        return smoothTime;
    }

    public void ResetSmoothTime()
    {
        smoothTime = defaultSmoothTime;
    }

    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }

    public Vector3 GetInitialDesired()
    {
        return initialDesired;
    }

    public void SetValue(Vector3 newVal)
    {
        currentValue = newVal;
        desiredValue = newVal;
    }

    public Vector3 Smooth()
    {
        Vector3 newVelocity = smoothVelocity;

        currentValue = Vector3.SmoothDamp(currentValue, desiredValue, ref newVelocity, smoothTime);

        smoothVelocity = newVelocity;

        return currentValue;
    }

    public bool NeedsSmoothing()
    {
        return !(Mathf.Approximately(currentValue.x, desiredValue.x) 
                && Mathf.Approximately(currentValue.y, desiredValue.y) 
                && Mathf.Approximately(currentValue.z, desiredValue.z));
    }
}
