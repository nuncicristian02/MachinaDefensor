using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class PositionableTower : MonoBehaviour
{
    internal bool IsMenuShown = false;

    public float gridSize = 1f;

    private TilemapRenderer positionsTilemapRenderer;

    public bool isPositioned = false;

    public float TowerRadius = 5;
    protected GameObject TowerRadiusObject;

    internal bool isTouchingTowers;

    protected virtual void Awake()
    {
        TowerRadiusObject = transform.GetChild(2).gameObject;

        var newRadiusObjLocalScale = new Vector3(TowerRadius, TowerRadius, TowerRadiusObject.transform.localScale.z);

        TowerRadiusObject.transform.localScale = newRadiusObjLocalScale;
    }

    public void OnStart()
     {
        var positionsTilemap = MapManager.Instance.PositionsTilemap;
        if (positionsTilemap != null)
        {
            positionsTilemapRenderer = positionsTilemap.GetComponent<TilemapRenderer>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            isTouchingTowers = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            isTouchingTowers = false;
        }
    }

    public void ToggleTowerRadiusVisualization()
    {
        TowerRadiusObject.GetComponent<SpriteRenderer>().enabled = !TowerRadiusObject.GetComponent<SpriteRenderer>().enabled;
    }

    private void OnMouseDown()
    {
        VisualizeTowerOptionsAndRadius();
    }

    protected void VisualizeTowerOptionsAndRadius()
    {
        if (!IsMenuShown)
        {
            HideOtherTowersOptions();
        }

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = !IsMenuShown;
        gameObject.transform.GetChild(0).GetComponent<Collider2D>().enabled = !IsMenuShown;
        gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = !IsMenuShown;
        gameObject.transform.GetChild(1).GetComponent<Collider2D>().enabled = !IsMenuShown;
        gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(!IsMenuShown);
        TowerRadiusObject.GetComponent<SpriteRenderer>().enabled = !IsMenuShown;

        IsMenuShown = !IsMenuShown;        
    }

    private void HideOtherTowersOptions()
    {
        var towers = GameObject.FindObjectsOfType<PositionableTower>().Where(x => x.IsMenuShown);

        towers.ToList().ForEach(x => { x.VisualizeTowerOptionsAndRadius(); });
    }

    public float overlapRadius = 2f;

    public bool IsOverDropZone()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius);

        Bounds objectBounds = GetComponent<Collider2D>().bounds;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("DropZone") && collider is TilemapCollider2D tilemapCollider)
            {
                Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();

                Vector3[] corners = new Vector3[]
                {
                objectBounds.min, 
                new Vector3(objectBounds.min.x, objectBounds.max.y, 0), 
                new Vector3(objectBounds.max.x, objectBounds.min.y, 0),
                objectBounds.max 
                };

                bool allCornersInside = true;

                foreach (Vector3 corner in corners)
                {
                    Vector3Int tilePosition = tilemap.WorldToCell(corner);

                    if (!tilemap.HasTile(tilePosition))
                    {
                        allCornersInside = false;
                        break;
                    }
                }

                if (allCornersInside)
                {
                    return true; 
                }
            }
        }

        return false; 
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(x, y, position.z);
    }    

    public abstract bool VerifyDraggable();

    public void OnDropPositionableTower()
    {
        isPositioned = true;
        OnDropTower();
    }

    protected abstract void OnDropTower();

}
