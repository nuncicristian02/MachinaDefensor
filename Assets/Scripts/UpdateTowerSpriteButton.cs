using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTowerSpriteButton : MonoBehaviour
{
    TowerBase towerBase;

    private TextMeshPro priceText;

    private void Start()
    {
        priceText = transform.GetComponentInChildren<TextMeshPro>();

        towerBase = transform.parent.GetComponent<TowerBase>();

        UpdatePriceText(towerBase.UpdatePrice);

        priceText.gameObject.SetActive(false);
    }

    internal void UpdatePriceText(int price)
    {
        if (!priceText)
            return;

        priceText.text = Convert.ToString(price);
    }

    internal void TogglePriceVisualization()
    {
        if (priceText == null)
            return;

        priceText.gameObject.SetActive(!priceText.gameObject.activeSelf);
    }

    private void OnMouseDown()
    {
        if (transform.parent != null)
        {
            if (towerBase == null)
            {
                return;
            }

            towerBase.OnUpgradeTower();
        }
    }
}
