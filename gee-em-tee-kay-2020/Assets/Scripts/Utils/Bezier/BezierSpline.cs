using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BezierSpline
{
    /** Root point */
    public Point root = new Point();

    /** Points on the spline */
    public List<BezierPoint> points = new List<BezierPoint>();

    /** The length of each curve */
    public List<float> curveLengths = new List<float>();     
    
    public int PointCount { get { return points.Count; } } /** Number of control points (used for GUI stuff i think) */
    public int CurveCount { get { return (points.Count - 1); } } /** Number of curves */
    
    [SerializeField]
    private float totalSplineLength = 0f;

    public void Clear()
    {
        root = new Point();
        points.Clear();
        curveLengths.Clear();
        totalSplineLength = 0f;

        // set default values
        BezierPoint point = new BezierPoint(root.position);
        point.backHandle = root.position + Vector3.back;
        point.forwardHandle = root.position + Vector3.forward;
        points.Add(point);
    }

    public int AddCurve(int index)
    {
        int i = 0;
        if (index == -1 || index == PointCount-1)
        {
            i = AddCurveToEnd();
        }
        else
        {
            i = InsertCurve(index);
        }

        // add new curve!
        int curveIndex = GetCurveFromPointNumber(index);
        curveLengths.Insert(curveIndex, 0f);

        /** Update the curve lengths */
        UpdateCurveLengths();

        return i;
    }
    
    public int AddCurveToEnd()
    {
        int index = PointCount-1;

        // Grab direction
        Vector3 directionOfCurve = points[index].GetDir();
        
        Vector3 positionOfNewPoint = points[index].point + directionOfCurve;
        points[index].forwardHandle = points[index].point + directionOfCurve;

        // create our new point and update its stuff too!
        BezierPoint point = new BezierPoint();
        point.backHandle = positionOfNewPoint - (directionOfCurve*2f);
        point.point = positionOfNewPoint;
        point.forwardHandle = positionOfNewPoint + (directionOfCurve*2f);

        points.Add(point);

        return index+1;
    }

    public int InsertCurve(int index)
    {
        // Set direction to the direction half way along the curve
        Vector3 directionOfCurve = GetDirectionFromTAlongCurve(0.5f, index);
        
        Vector3 positionOfNewPoint = GetPointFromTAlongCurveRelative(0.5f, index);

        points[index].forwardHandle = points[index].point + directionOfCurve;
        
        // create our new point and update its stuff too!
        BezierPoint point = new BezierPoint();
        point.backHandle = positionOfNewPoint - (directionOfCurve*2f);
        point.point = positionOfNewPoint;
        point.forwardHandle = positionOfNewPoint + (directionOfCurve*2f);
        
        // insert point
        points.Insert(index+1, point);

        return index+1;
    }

    public void RemovePoint(int index)
    {
        if (PointCount > 1)
        {
            points.RemoveAt(index);

            if (index == 0)
            {
                curveLengths.RemoveAt(index);
            }
            else
            {
                curveLengths.RemoveAt(index-1);
            }

            int newIndex = GetCurveFromPointNumber(index);

            UpdateCurveLengths();
        }
    }

    public void SetPoints(List<BezierPoint> other)
    {
        points = new List<BezierPoint>(other);

        UpdateCurveLengths();
    }

    public void SetPointPosition(int index, Vector3 newPos)
    {
        points[index].SetPosition(newPos);

        UpdateCurveLengths();
    }

    /** Grabs the index of the curve with a given distance */
    public int GetCurveFromDistanceAlongSpline(float d)
    {
        float totalLength = 0f;
        for (int i = 0; i < curveLengths.Count; ++i)
        {
            totalLength += curveLengths[i];

            if (d < totalLength)
            {
                return i;
            }
        }

        return (curveLengths.Count - 1);
    }

    /** Grabs the remainder of the distance into the curve that you're in */
    public float GetRemainderFromDistanceAlongSpline(float d, int curve)
    {
        float totalLength = 0f;

        for (int i = 0; i < curve; ++i)
        {
            totalLength += curveLengths[i];
        }

        return d - totalLength;
    }
   
    /** Get T on total spline */
    public float GetTAlongSplineFromTAlongCurve(float t, int curve)
    {
        return (t/CurveCount + ((1.0f/CurveCount)*curve));
    }

    /**
    * GetTFromDistanceAlongCurve
    * -> d: the distance along the specified curve
    * -> curveIndex: the index of the specified curve
    * <- t, the value that the distance corresponds to along the curve
    */
    public (float, int) GetTFromDistanceAlongCurve(float d)
    {
        if (CurveCount == 0)
        {
            Debug.LogError("Attempting to get T from a spline with only one point");
            return (-1f, -1);
        }

        // Grab curve index
        int curveIndex = GetCurveFromDistanceAlongSpline(d);

        // Get the remainder of the distance from along the spine
        float remainder = GetRemainderFromDistanceAlongSpline(d, curveIndex);

        // Find the t
        float t = BezierUtils.FindTValue(remainder, curveLengths[curveIndex], GetCurveVector3ArrayFromPoints(points[curveIndex], points[curveIndex+1]));

        return (t, curveIndex);
    }

    public Vector3 GetDirectionFromTAlongSpline(float t)
    {
        if (Mathf.Approximately(t, 0f))
        {
            return GetPoint(0).GetDir();
        }
        else if (Mathf.Approximately(t, 1f))
        {
            return GetPoint(PointCount-1).GetDir();
        }

        int curveIndex = GetCurveIndexFromTAlongSpline(t);
        Vector3[] pointsArray = (GetCurveVector3ArrayFromPoints(points[curveIndex], points[curveIndex+1]));
        Vector3 direction = BezierUtils.GetFirstDerivative(pointsArray, (t/(float)CurveCount)*(curveIndex+1));

        return (Socks.Utils.TransformPoint(direction,
                root.position, Quaternion.Euler(root.eulerAngles)) - root.position).normalized;
    }

    public Vector3[] GetCurveVector3ArrayFromPoints(BezierPoint one, BezierPoint two)
    {
        return new Vector3[]{one.point, one.forwardHandle, two.backHandle, two.point};
    }

    public int GetCurveIndexFromTAlongSpline(float t)
    {
        if (t == 0f)
        {
            return 0;
        }
        else if (t == 1f)
        {
            return CurveCount-1;
        }

        float amountOfTPerPoint = 1.0f / CurveCount;
        for (int i = 0; i < PointCount; ++i)
        {
            float lowerBound = (float)i * amountOfTPerPoint;
            float upperBound = (float)(i+1) * amountOfTPerPoint;
            if (t >= lowerBound && t <= upperBound)
            {
                return i;
            }
        }

        Debug.LogErrorFormat("Reached end of BezierSpline without finding a 't'- this shouldn't happen?");
        return -1;
    }

    /**
    * GetPointFromDistance
    * -> d: the distance along the entire spline
    * <- the actual position along the spline
    */
    public Vector3 GetPointFromDistanceAlongSpline(float d)
    {
        int curve = 0;
        float t = 0f;

        (t,curve) = GetTFromDistanceAlongCurve(d);

        return GetPointFromTAlongCurve(t, curve);
    }

    /**
    * GetPointFromTAlongCurve
    * -> t: the value corresponding to the amount along the curve
    * -> curveIndex: the index of the specified curve
    * <- the transform position at the given t-value along spline
    */
    public Vector3 GetPointFromTAlongCurve(float t, int curveIndex) 
    {
        if (t >= 1f) 
        {
            t = 1f;
        }

        //Get the coordinate on the Bezier curve at this t value
        return Socks.Utils.TransformPoint(GetPointFromTAlongCurveRelative(t, curveIndex), root.position, Quaternion.Euler(root.eulerAngles));
    }

    public Vector3 GetPointFromTAlongCurveRelative(float t, int curveIndex)
    {
        return BezierUtils.DeCasteljausAlgorithm(t, 
            GetCurveVector3ArrayFromPoints(points[curveIndex], points[curveIndex+1]));
    }

    public Vector3 GetPointFromTAlongSpline(float t) 
    {
        int i;
        if (t >= 1f) 
        {
            t = 1f;
            i = PointCount - 2;
        }
        else 
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
        }

        return Socks.Utils.TransformPoint(BezierUtils.DeCasteljausAlgorithm(t, 
            GetCurveVector3ArrayFromPoints(points[i], points[i+1])), root.position, Quaternion.Euler(root.eulerAngles));
    }

    /**
    * GetDirectionFromTAlongCurve
    * -> t: the value corresponding to the amount along the curve
    * -> curveIndex: the index of the specified curve
    * <- the direction at the given t-value along spline
    */
    public Vector3 GetDirectionFromTAlongCurve(float t, int curveIndex)
    {
        return (Socks.Utils.TransformPoint(BezierUtils.GetFirstDerivative(GetCurveVector3ArrayFromPoints(points[curveIndex], points[curveIndex+1]), t), 
                root.position, Quaternion.Euler(root.eulerAngles)) - root.position).normalized;
    }
            
    /** Updates the lengths of the curves */
    public void UpdateCurveLengths()
    {
        // Resize array to fit lengths
        if (curveLengths.Count != CurveCount)
        {
            curveLengths.Clear();
            for (int i = 0; i < CurveCount; ++i)
            {
                curveLengths.Add(0);
            }
        }

        totalSplineLength = 0f;

        // Get lengths
        for (int i = 0; i < curveLengths.Count; ++i)
        {
            curveLengths[i] = BezierUtils.GetLengthSimpsons(0f, 1f, 
                GetCurveVector3ArrayFromPoints(points[i], points[i+1]));

            totalSplineLength += curveLengths[i];
        }
    }

    public int GetCurveFromPointNumber(int no)
    {
        return no;
    }

    public float GetTFromPointNumber(int no)
    {
        return (no+1)/PointCount;
    }

    /** Getters */
    public float GetLength()
    {
        return totalSplineLength;
    }

    public BezierPoint GetPoint(int index) 
    {
        return points[index];
    }
}