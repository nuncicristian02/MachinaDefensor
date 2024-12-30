using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    internal Tilemap PositionsTilemap;
    internal Tilemap PathTilemap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FindReferences();
            PositionsTilemap.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void FindReferences()
    {
        PositionsTilemap = GameObject.Find("PositionsTilemap").GetComponent<Tilemap>();
        PathTilemap = GameObject.Find("PathTilemap")?.GetComponent<Tilemap>();
    }
}
