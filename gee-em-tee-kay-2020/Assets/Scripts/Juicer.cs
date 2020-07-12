using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juicer : MonoBehaviour
{
    class JuiceSmoother
    {
        public JuiceSmoother(float newxAxis, float newyAxis, float newzAxis){xAxis = newxAxis; yAxis = newyAxis; zAxis = newzAxis;}

        public float xAxis = 0.0f;
        public float yAxis = 0.0f;
        public float zAxis = 0.0f;

        public float desired = 0.0f;
        public float current = 0.0f;
        public float time = 0.0f;

        private float timeCounter = 0f;

        public float Smooth()
        {
            float timeIn = time*0.3f;
            timeCounter += Time.deltaTime;

            if (timeCounter >= time)
            {
                markForDelete = true;
            }

            float output = 0f;

            if (timeCounter < timeIn)
            {
                output = Mathf.Lerp(0f, desired, timeCounter / timeIn);
            }
            else
            {
                output = Mathf.Lerp(desired, 0f, (timeCounter-timeIn) / (time-timeIn));
            }

            return output;
        }

        public bool markForDelete = false;
    }

    [SerializeField] private float defaultStrength = 0.08f;

    private List<JuiceSmoother> smoothers = new List<JuiceSmoother>();
    private Vector3 initialScale = Vector3.one;
    [SerializeField]  private Transform toModify = null;

    void Awake()
    {
        if (toModify == null)
            toModify = transform.GetComponentInChildren<Animator>().transform;

        // Set initial scale
        initialScale = toModify.localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newScale = initialScale;

        foreach (JuiceSmoother smoother in smoothers)
        {   
            newScale = newScale + (new Vector3(smoother.xAxis, smoother.yAxis, smoother.zAxis) * smoother.Smooth());
        }

        toModify.localScale = newScale;

        for (int i = smoothers.Count-1; i >= 0; i--)
        {
            if (smoothers[i].markForDelete)
            {
                smoothers.RemoveAt(i);
                
                UpdateActivity();
            }
        }
    }

    private void UpdateActivity()
    {
        enabled = smoothers.Count > 0;
    }

    public void Squash(float strength, float time)
    {
        JuiceSmoother squashSmoother = new JuiceSmoother(1.0f, 0.0f, 1.0f);
        squashSmoother.time = time;
        squashSmoother.desired = strength * defaultStrength;

        smoothers.Add(squashSmoother);

        JuiceSmoother stretchSmoother = new JuiceSmoother(0.0f, 1.0f, 0.0f);
        stretchSmoother.time = time;
        stretchSmoother.desired = -strength * defaultStrength;

        smoothers.Add(stretchSmoother);

        UpdateActivity();
    }

    public void Stretch(float strength, float time)
    {
        JuiceSmoother stretchSmoother = new JuiceSmoother(0.0f, 1.0f, 0.0f);
        stretchSmoother.time = time;
        stretchSmoother.desired = strength * defaultStrength;

        smoothers.Add(stretchSmoother);

        JuiceSmoother squashSmoother = new JuiceSmoother(1.0f, 0.0f, 1.0f);
        squashSmoother.time = time;
        squashSmoother.desired = -strength * defaultStrength;

        smoothers.Add(squashSmoother);
        
        UpdateActivity();
    }
}
