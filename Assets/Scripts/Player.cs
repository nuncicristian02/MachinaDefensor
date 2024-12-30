using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float StartingLife = 50;

    public float StartingMoney = 100;

    private float _life;

    private void Start()
    {
        _life = StartingLife;
    }

    public float Life
    {
        get 
        {
            return _life;
        }
        set 
        {
            if (value > 0)
            {
                UIManager.Instance.SetLifeBar(value);
            }
            _life = value;
            ManageDeath(_life);
        }
    }

    private float _money = 100;
    public float Money
    {
        get
        {
            return _money;
        }
        set 
        { 
            UIManager.Instance.SetMoneyText(value);
            _money = value;
        }
    }

    private void ManageDeath(float life)
    {
        if (life <= 0)
            GameManager.Instance.Death();
    }
}
