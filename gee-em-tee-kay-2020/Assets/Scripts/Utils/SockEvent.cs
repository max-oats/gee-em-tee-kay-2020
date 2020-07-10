using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> {}

[System.Serializable]
public class FloatEvent : UnityEvent<float> {}