using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierUtils
{
	public static Vector3 GetFirstDerivative(Vector3[] points, float t) 
    {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (points[1] - points[0]) +
			6f * oneMinusT * t * (points[2] - points[1]) +
			3f * t * t * (points[3] - points[2]);
	}

	//Use Newton–Raphsons method to find the t value at the end of this distance d
    public static float FindTValue(float d, float totalLength, Vector3[] points)
    {
        //Need a start value to make the method start
        float t = d / totalLength;

        //Need an error so we know when to stop the iteration
        float error = 0.001f;

        //We also need to avoid infinite loops
        int iterations = 0;

        while (true)
        {
            //Newton's method
            float tNext = t - ((GetLengthSimpsons(0f, t, points) - d) / GetArcLengthIntegrand(t, points));

            //Have we reached the desired accuracy?
            if (Mathf.Abs(tNext - t) < error)
            {
                break;
            }

            t = tNext;

            iterations += 1;

            if (iterations > 1000)
            {
                break;
            }
        }

        return t;
    }

    //Get the length of the curve between two t values with Simpson's rule
    public static float GetLengthSimpsons(float tStart, float tEnd, Vector3[] points)
    {
        //This is the resolution and has to be even
        int n = 20;

        //Now we need to divide the curve into sections
        float delta = (tEnd - tStart) / (float)n;

        //The main loop to calculate the length
        
        //Everything multiplied by 1
        float endPoints = GetArcLengthIntegrand(tStart, points) + GetArcLengthIntegrand(tEnd, points);

        //Everything multiplied by 4
        float x4 = 0f;
        for (int i = 1; i < n; i += 2)
        {
            float t = tStart + delta * i;

            x4 += GetArcLengthIntegrand(t, points);
        }

        //Everything multiplied by 2
        float x2 = 0f;
        for (int i = 2; i < n; i += 2)
        {
            float t = tStart + delta * i;

            x2 += GetArcLengthIntegrand(t, points);
        }

        //The final length
        float length = (delta / 3f) * (endPoints + 4f* x4 + 2f * x2);

        return length;
    }

    //The De Casteljau's Algorithm
    public static Vector3 DeCasteljausAlgorithm(float t, Vector3[] points)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;
        
        //Layer 1
        Vector3 Q = oneMinusT * points[0] + t * points[1];
        Vector3 R = oneMinusT * points[1] + t * points[2];
        Vector3 S = oneMinusT * points[2] + t * points[3];

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }

    //The derivative of cubic De Casteljau's Algorithm
    public static Vector3 DeCasteljausAlgorithmDerivative(float t, Vector3[] points)
    {
        Vector3 dU = t * t * (-3f * (points[0] - 3f * (points[1] - points[2]) - points[3]));

        dU += t * (6f * (points[0] - 2f * points[1] + points[2]));

        dU += -3f * (points[0] - points[1]); 

        return dU;
    }

    //Get and infinite small length from the derivative of the curve at position t
    public static float GetArcLengthIntegrand(float t, Vector3[] points)
    {
        //The derivative at this point (the velocity vector)
        Vector3 dPos = DeCasteljausAlgorithmDerivative(t, points);

        //Same as above
        float integrand = dPos.magnitude;

        return integrand;
    }
}
