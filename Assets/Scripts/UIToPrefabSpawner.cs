using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToPrefabSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject towerPrefab;
    private GameObject ghostTower;
    private Canvas canvas;

    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private void Start()
    {
        FindReferences();
    }

    private void FindReferences()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var towerPrefabScript = towerPrefab.GetComponent<PositionableTower>();
        if (!towerPrefabScript.VerifyDraggable())
        {
            return;
        }
        ghostTower = Instantiate(towerPrefab, canvas.transform);
        ghostTower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        MapManager.Instance.PositionsTilemap.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostTower != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            ghostTower.transform.position = worldPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (ghostTower != null)
        {
            PositionableTower dragTower = ghostTower.GetComponent<PositionableTower>();
            if (dragTower != null && dragTower.IsOverDropZone())
            {
                Vector3 snappedPosition = dragTower.SnapToGrid(ghostTower.transform.position);
                var newTower = Instantiate(towerPrefab, snappedPosition, Quaternion.identity);
                newTower.GetComponent<PositionableTower>().OnDropPositionableTower();
            }

            Destroy(ghostTower);

            MapManager.Instance.PositionsTilemap.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (ghostTower != null)
        {
            PositionableTower dragTower = ghostTower.GetComponent<PositionableTower>();
            UpdateColor(dragTower.IsOverDropZone());
        }
    }
    private void UpdateColor(bool isValid = false)
    {
        ghostTower.GetComponent<SpriteRenderer>().color = isValid ? validColor : invalidColor;
    }
}
