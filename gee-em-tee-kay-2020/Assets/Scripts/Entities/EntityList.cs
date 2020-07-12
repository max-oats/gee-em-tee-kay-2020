using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="EntityList", order=0)]
public class EntityList : ScriptableObject
{
    public List<EntityType> entities;
}