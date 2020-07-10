using UnityEngine;
using System.Collections;
 
public class TempOut : MonoBehaviour 
{
    public Shader outlineShader;
    public Shader drawShader;
    public Material postOutlineMaterial;
    public RenderTexture TempRT;
    public string layerMaskString;
    
    Camera attachedCamera;
    Camera temporaryCamera;

    int layerMask;
 
    void Awake() 
    {
        attachedCamera = GetComponent<Camera>();
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        temporaryCamera = go.AddComponent<Camera>();
        temporaryCamera.enabled = false;    
        layerMask = LayerMask.NameToLayer(layerMaskString);  
    }
 
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //set up a temporary camera
        temporaryCamera.CopyFrom(attachedCamera);
        temporaryCamera.clearFlags = CameraClearFlags.Color;
        temporaryCamera.backgroundColor = Color.clear;
 
        //cull any layer that isn't the outline
        temporaryCamera.cullingMask = 1 << layerMask;
 
        //set the camera's target texture when rendering
        temporaryCamera.targetTexture = TempRT;
 
        //render all objects this camera can render, but with our custom shader.
        temporaryCamera.RenderWithShader(drawShader,"");

        RenderTexture rt = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = destination;
        GL.Clear(true, true, Color.clear);
        UnityEngine.RenderTexture.active = rt; 

        //copy the temporary RT to the final image
        Graphics.Blit(TempRT, destination, postOutlineMaterial);
 
        //release the temporary RT
        TempRT.Release();
    }
 
}