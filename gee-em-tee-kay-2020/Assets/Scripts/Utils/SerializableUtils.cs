using UnityEngine;

[System.Serializable]
public class Point
{
    public Point() {}

    public Point(Point other)
    {
        this.position = other.position;
        this.eulerAngles = other.eulerAngles;
    }

    public Vector3 position = Vector3.zero;

    public Vector3 eulerAngles = Vector3.zero;
}

[System.Serializable]
public class NamedPoint
{
    public string name;

    public Vector3 position;

    public Vector3 euler;
}

[System.Serializable]
public class NamedTransform
{
    public string name;

    public Transform transform;
}

[System.Serializable]
public class NamedGameObject
{
    public string name;

    public GameObject go;
}