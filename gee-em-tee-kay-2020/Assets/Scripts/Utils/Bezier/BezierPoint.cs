using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/** Bezier point handler class */
[System.Serializable]
public class BezierPoint
{
    public BezierPoint() {}

    public BezierPoint(Vector3 point) 
    {
        this.point = point;
    }

    public BezierPoint(Vector3 point, Vector3 direction)
    {
        this.point = point;
        this.forwardHandle = this.point+direction;
        this.backHandle = this.point-direction;
    }

    public void SetPosition(Vector3 point)
    {
        Vector3 delta = point-this.point;
        this.point = point;
        forwardHandle += delta;
        backHandle += delta;
    }

    public void SetForwardHandle(Vector3 handle)
    {
        forwardHandle = handle;
    }

    public void SetBackHandle(Vector3 handle)
    {
        backHandle = handle;
    }

    public Vector3 GetDir()
    {
        return (point - backHandle).normalized;
    }

    public Vector3 point = Vector3.zero; // our actual main point

    public Vector3 forwardHandle; // the location of the handle

    public Vector3 backHandle; // the location of the other handle
}