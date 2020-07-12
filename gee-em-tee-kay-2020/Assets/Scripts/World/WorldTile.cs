using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldTile : MonoBehaviour
{
    public int x = 0;
    public int z = 0;
    
    public Vector3 rgb;
    public Vector3 colourOffsets;

    public float bushChance;

    public GameObject bush;


    private Color color;

    private List<BaseEntity> inhabitants = new List<BaseEntity>();

    void Awake()
    {
        SetColour();

        if (Random.value <= bushChance)
        {
            GameObject go = Instantiate(bush, 
                transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0f, Random.Range(0.3f, 0.4f)), 
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)), transform);

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", color);
            MeshRenderer meshRenderer = go.GetComponentInChildren<MeshRenderer>();
            meshRenderer.SetPropertyBlock(block);
        }
    }

    public void SetMaterial(Material material)
    {
        MeshRenderer debugRenderer = GetComponentInChildren<MeshRenderer>();
        debugRenderer.material = material;
    }

    public void SetHeight(float height)
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    void SetColour()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        color = new Color(rgb.x + Random.Range(-colourOffsets.x, colourOffsets.x), 
                                            rgb.y + Random.Range(-colourOffsets.y, colourOffsets.y), 
                                            rgb.z + Random.Range(-colourOffsets.z, colourOffsets.z), 1f);
        block.SetColor("_Color", color);
                                            
        MeshRenderer[] debugRenderer = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in debugRenderer)
        {
            if (renderer.gameObject.name == "Grass")
                renderer.SetPropertyBlock(block);
        }
    }
    
    public void TriggerInteract(BaseInteractParams interactParams)
    {
        List<BaseEntity> inhabitantsCopy = new List<BaseEntity>(inhabitants);
        foreach (BaseEntity inhabitant in inhabitantsCopy)
        {
            inhabitant.TriggerInteract(interactParams);
        }
    }

    public bool HasObstacle(List<EntityType> obstacleTypes)
    {
        foreach (BaseEntity inhabitant in inhabitants)
        {
            if (obstacleTypes.Contains(inhabitant.GetEntityType()))
            {
                return true;
            }
        }
        return false;
    }

    public bool HasInhabitantOfType(EntityType type)
    {
        foreach (BaseEntity inhabitant in inhabitants)
        {
            if (inhabitant.GetEntityType() == type)
            {
                return true;
            }
        }

        return false;
    }

    public void AddInhabitant(BaseEntity newInhabitant)
    {
        inhabitants.Add(newInhabitant);
    }

    public void RemoveInhabitant(BaseEntity inhabitant)
    {
        inhabitants.Remove(inhabitant);
    }

    void Update()
    {
        if (inhabitants.Count > 0)
        {
            SetDebugColour(inhabitants[0].debugColor);
        }
        else
        {
            SetDebugColour(Color.white);
        }
    }

    public void SetDebugColour(Color inDebugColour)
    {
        float alpha = 0.5f;
        if (inDebugColour == Color.white)
        {
            alpha = 0f;
        }

        SpriteRenderer debugRenderer = GetComponentInChildren<SpriteRenderer>();
        debugRenderer.color = new Color(inDebugColour.r, inDebugColour.g, inDebugColour.b, alpha);
    }
}
