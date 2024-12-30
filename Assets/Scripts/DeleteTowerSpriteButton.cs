using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTowerSpriteButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (transform.parent != null)
        {
            var towerBase = transform.parent.GetComponent<TowerBase>();

            if (towerBase == null)
            {
                return;
            }

            towerBase.OnDeleteTower();
            Destroy(transform.parent.gameObject);
        }
    }
}
