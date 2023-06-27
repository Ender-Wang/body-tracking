using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasStateManager : MonoBehaviour
{
    public GameObject canvasPrefab; // The prefab of the Canvas objects

    private static CanvasStateManager instance; // Singleton instance reference
    private GameObject currentCanvasInstance; // Reference to the current Canvas instance

    public static CanvasStateManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InstantiateCanvasOnStart();
    }

    void InstantiateCanvasOnStart()
    {
        InstantiateCanvas();
        // Additional setup or initialization code if needed
    }

    public void InstantiateCanvas()
    {
        // Destroy the current Canvas instance (if any)
        if (currentCanvasInstance != null)
        {
            Destroy(currentCanvasInstance);
        }

        // Instantiate a new Canvas instance from the prefab
        currentCanvasInstance = Instantiate(canvasPrefab, transform);
    }

    public void DestroyCanvas()
    {
        // Destroy the current Canvas instance (if any)
        if (currentCanvasInstance != null)
        {
            Destroy(currentCanvasInstance);
        }
    }
}
