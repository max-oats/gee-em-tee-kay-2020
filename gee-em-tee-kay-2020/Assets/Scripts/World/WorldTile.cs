using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public int x = 0;
    public int z = 0;
    
    public Vector3 rgb;
    public Vector3 colourOffsets;

    public float bushChance;

    public GameObject bush;


    private Color color;

    private BaseEntity inhabitant = null;

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
            MeshRenderer debugRenderer = go.GetComponentInChildren<MeshRenderer>();
            debugRenderer.SetPropertyBlock(block);
        }
    }

    void SetColour()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        color = new Color(rgb.x + Random.Range(-colourOffsets.x, colourOffsets.x), 
                                            rgb.y + Random.Range(-colourOffsets.y, colourOffsets.y), 
                                            rgb.z + Random.Range(-colourOffsets.z, colourOffsets.z), 1f);
        block.SetColor("_Color", color);
                                            
        MeshRenderer debugRenderer = GetComponentInChildren<MeshRenderer>();
        debugRenderer.SetPropertyBlock(block);
    }
    
    public bool TriggerInteract(InteractParams interactParams)
    {
        if (inhabitant)
        {
            return inhabitant.TriggerInteract(interactParams);
        }
        return false;
    }

    public bool HasInhabitant()
    {
        return inhabitant != null;
    }

    public void SetInhabitant(BaseEntity newInhabitant)
    {
        inhabitant = newInhabitant;
    }

    void Update()
    {
        if (inhabitant)
        {
            SetDebugColour(inhabitant.debugColor);
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
