using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HeroData data;
    [SerializeField] GridManager gridManager;
    [SerializeField] CombatManager combatManager;

    [Header("Suscriptions")]
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerMovement playerMovement;
    
    //public Hero PlayerUnit => _playerUnit;

    private Hero _playerUnit;
    private Tile _currentTile;
        
    private void OnEnable()
    {
        gameManager.OnStartNewLevel += HandleStartNewLevel;
        playerMovement.OnPlayerMoved += HandlePlayerMoved;
    }

    private void OnDisable()
    {
        gameManager.OnStartNewLevel -= HandleStartNewLevel;
        playerMovement.OnPlayerMoved -= HandlePlayerMoved;
    }

    private void HandlePlayerMoved(Vector2Int playerPosition)
    {
        _currentTile = gridManager.Grid[playerPosition];
        switch (_currentTile.TileState)
        {
            case TileState.Exit:
                playerMovement.SetCanMove(false);
                gameManager.EndLevel(_playerUnit.IsAlive);
                break;

            case TileState.Encounter:
                playerMovement.SetCanMove(false);
                StartCoroutine(combatManager.StartCombat(_playerUnit, _currentTile.Enemy));
                break;
        }
    }

    private void HandleStartNewLevel()
    {
        _playerUnit = new Hero(
            data.archetype,
            data.heroName,
            data.hpMax,
            data.CON,
            data.STR,
            data.DEX,
            data.INT,
            data.mainStat,
            data.damages,
            data.AC
            );
    }
}
