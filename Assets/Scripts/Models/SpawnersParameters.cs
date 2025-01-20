using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnersParameters
{
    public float BaseEnemiesCount { get; set; }
    public float IntermediateEnemiesCount { get; set; }
    public float BossEnemiesCount { get; set; }
    public float SpawnCoolDown { get; set; }

    public float TotEnemiesCount => Mathf.RoundToInt(BaseEnemiesCount + IntermediateEnemiesCount + BossEnemiesCount);
}
