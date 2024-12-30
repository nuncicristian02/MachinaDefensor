using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class PositionableTower : MonoBehaviour
{
    private bool isMenuShown = false;

    public float gridSize = 1f;

    private TilemapRenderer positionsTilemapRenderer;

    public bool isPositioned = false;

     public void OnStart()
     {
        var positionsTilemap = MapManager.Instance.PositionsTilemap;
        if (positionsTilemap != null)
        {
            positionsTilemapRenderer = positionsTilemap.GetComponent<TilemapRenderer>();
        }
     }


    private void OnMouseDown()
    {
        VisualizeTowerOptions();
    }

    protected void VisualizeTowerOptions()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(!isMenuShown);
        gameObject.transform.GetChild(1).gameObject.SetActive(!isMenuShown);
        isMenuShown = !isMenuShown;
    }

    public float overlapRadius = 0.1f;

    public bool IsOverDropZone()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("DropZone"))
            {
                return true;
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
