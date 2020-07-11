using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public new static CameraController camera = null;
    public static InputManager input = null;
    public static EntityManager entities = null;
    public static WorldMap worldMap = null;

    public static Game instance = null;
    public static float gravity = -80f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            input = GetComponent<InputManager>();
            entities = GetComponent<EntityManager>();
            worldMap = GetComponent<WorldMap>();
        }
        else
        {
            // Destroy object
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUpGlobalObject();
    }

    public static void SetUpGlobalObject()
    {
        camera = GameObject.FindObjectOfType<CameraController>();
    }

    public static void Quit()
    {
        Application.Quit();
    }
}
