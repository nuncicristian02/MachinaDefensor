using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTowerSpriteButton : MonoBehaviour
{
    TowerBase towerBase;

    private void Start()
    {
        var priceText = transform.GetChild(0).GetComponent<TextMeshPro>();
        towerBase = transform.parent.GetComponent<TowerBase>();

        priceText.text = Convert.ToString(towerBase.UpdatePrice);
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
