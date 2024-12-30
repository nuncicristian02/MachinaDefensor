using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    private Tilemap pathTilemap;
    public float Speed = 1f;
    private const float _tileProximityThreshold = 0.1f;

    private GameObject EnemyLifeGameObject;
    private SpriteRenderer lifeBarSpriteRenderer;

    public float _life;

    internal float Damage = 10;
    public float DeathMoney = 10;

    public float Life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
            ManageDeath();
            ManageLifeBar();
        }
    }

    private Vector3Int _currentTargetTile;
    private Vector3Int _lastTile;
    private bool _hasTarget = false;

    private void Start()
    {
        RoundsManager.Instance.SpawnedEnemies += 1;
        pathTilemap = MapManager.Instance.PathTilemap;
        _currentTargetTile = pathTilemap.WorldToCell(transform.position);
        _lastTile = _currentTargetTile;
        EnemyLifeGameObject = transform.GetChild(1).gameObject;
        lifeBarSpriteRenderer = EnemyLifeGameObject?.transform.GetChild(0).GetComponent<SpriteRenderer>();

        Speed = RoundsManager.Instance.DifficultyParameters.EnemiesSpeed;
        Life = RoundsManager.Instance.DifficultyParameters.EnemiesLife;
        Damage = RoundsManager.Instance.DifficultyParameters.EnemiesDamage;
    }

    void Update()
    {
        ManageTarget();
    }

    internal void TakeDamage(float damage)
    {
        Life -= damage;
    }

    private void ManageTarget()
    {
        if (!_hasTarget || Vector2.Distance(transform.position, pathTilemap.GetCellCenterWorld(_currentTargetTile)) < _tileProximityThreshold)
        {
            _hasTarget = FindNextTile();
        }

        if (_hasTarget)
        {
            Vector3 targetPosition = pathTilemap.GetCellCenterWorld(_currentTargetTile);
            Vector2 direction = (targetPosition - transform.position).normalized;

            transform.Translate(direction * Speed * Time.deltaTime);
        }
    }

    private bool FindNextTile()
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        foreach (var direction in directions)
        {
            Vector3Int nextTile = _currentTargetTile + direction;

            if (pathTilemap.HasTile(nextTile) && nextTile != _lastTile)
            {
                _lastTile = _currentTargetTile; 
                _currentTargetTile = nextTile;
                return true;
            }
        }

        return false;
    }

    private void ManageLifeBar()
    {
        if (!EnemyLifeGameObject.activeSelf && _life < RoundsManager.Instance.DifficultyParameters.EnemiesLife)
        {
            EnemyLifeGameObject.SetActive(true);
        }

        if (_life > 0)
        {
            var newXLocalScale = _life/ RoundsManager.Instance.DifficultyParameters.EnemiesLife;

            var lifeBarLocalScale = lifeBarSpriteRenderer.gameObject.transform.localScale;

            var newLocalScale = new Vector3(newXLocalScale, lifeBarLocalScale.y, lifeBarLocalScale.z);

            lifeBarSpriteRenderer.gameObject.transform.localScale = newLocalScale;
        }
    }

    private void ManageDeath()
    {
        if (_life > 0) return;

        GameManager.Instance.Player.Money += DeathMoney;
        RoundsManager.Instance.RoundKilledEnemies += 1;
        GameObject.Destroy(gameObject);
    }
}