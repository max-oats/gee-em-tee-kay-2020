using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reasonator
{
    public Dictionary<System.Guid, string> reasons = new Dictionary<System.Guid, string>();

    private const string genericReason = "generic";

    public System.Guid AddReason(string newReason)
    {
        System.Guid guid = System.Guid.NewGuid();
        reasons.Add(guid, newReason);

        return guid;
    }

    public void RemoveReason(System.Guid reasonID)
    {
        reasons.Remove(reasonID);
    }

    public void Increment()
    {
        reasons.Add(System.Guid.NewGuid(), genericReason);
    }

    public void Decrement()
    {
        foreach (System.Guid guid in reasons.Keys)
        {
            if (reasons[guid] == genericReason)
            {
                reasons.Remove(guid);
                break;
            }
        }
    }

    public bool HasReasons()
    {
        return reasons.Count > 0;
    }
}