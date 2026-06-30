using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class CombatManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameManager gameManager;
    [SerializeField] CombatVisual heroVisual;
    [SerializeField] CombatVisual enemyVisual;
    [SerializeField] CinemachineCamera cmExploration;
    [SerializeField] CinemachineCamera cmCombat;

    float delay = 0.8f;
    private CombatResolver _resolver = new CombatResolver();

    public IEnumerator StartCombat(Hero playerUnit, Enemy enemyUnit)
    {
        heroVisual.Reset();
        enemyVisual.Reset();

        SwitchToCombatCamera();
        Debug.LogWarning($"Un {enemyUnit.Archetype} sauvage est apparut. Préparez vous aux combat !");
        yield return new WaitForSeconds(delay);
        yield return RunCombat(playerUnit, enemyUnit);
        SwitchToExplorationCamera();

        if (playerUnit.IsAlive) //? À faire gérer par PlayerState ? Ou GameManager via event ?
        {
            Vector2Int gridPosition = gridManager.ConvertPositionMapToGrid(player.transform.position);
            gridManager.ClearOneTile(gridPosition);
            playerMovement.SetCanMove(true);
        } 
        else
        {
            gameManager.EndLevel(false);
        }
    }

    private IEnumerator RunCombat(Hero playerUnit, Enemy enemyUnit)
    {
        Debug.LogWarning($"Stats de départ : Hero currentPV = {playerUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
        yield return new WaitForSeconds(delay);

        bool isHeroFirst = _resolver.RollInit(playerUnit.DEX, enemyUnit.DEX);
        Fighter firstToPlay = isHeroFirst ? playerUnit : enemyUnit;
        Fighter secondToPlay = isHeroFirst ? enemyUnit : playerUnit;
        CombatVisual firstVisual = isHeroFirst ? heroVisual : enemyVisual;
        CombatVisual secondVisual = isHeroFirst ? enemyVisual : heroVisual;

        Debug.LogWarning($"{firstToPlay.Name} est à l'initiative");
        yield return new WaitForSeconds(delay);

        do
        {
            yield return StartCoroutine(Attack(firstToPlay, secondToPlay, firstVisual, secondVisual));
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(Attack(secondToPlay, firstToPlay, secondVisual, firstVisual));
            yield return new WaitForSeconds(delay);
            Debug.LogWarning($"Stats MaJ : Hero currentPV = {playerUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
            yield return new WaitForSeconds(delay);
        } while (playerUnit.IsAlive && enemyUnit.IsAlive);

        if (!playerUnit.IsAlive)
        {
            yield return StartCoroutine(heroVisual.PlayDeath());
            Debug.LogWarning($"Vous avez fait ce que vous avez pu mais {enemyUnit.Name} a eu raison de vous. C'était le combat de trop. Puissiez-vous reposez en paix");
        } else
        {
            yield return StartCoroutine(enemyVisual.PlayDeath());
            Debug.LogWarning($"Vous vous êtes vaillamment battu et {enemyUnit.Name} gît davant vous dans une mare de sang. En espérant que ça continue ainsi. Il vous reste {playerUnit.CurrentHp} PV !");
        }

        yield return null;
    }

    private IEnumerator Attack(Fighter attacker, Fighter target, CombatVisual attackerVisual, CombatVisual targetVisual)
    {
        yield return StartCoroutine(attackerVisual.PlayAttack());

        AttackResultDto result = _resolver.ResolveAttack(attacker, target);

        if (!result.DidHit)
        {
            yield return StartCoroutine(targetVisual.PlayDodge());
            Debug.LogWarning($"{attacker.Name} s'est lamentablement foiré et à complètement raté {target.Name}");
            yield break;
        }
        yield return StartCoroutine(targetVisual.PlayHit());


        Debug.LogWarning($"{attacker.Name} a fait {result.Damage} dégât à {target.Name}");
    }

    private void SwitchToCombatCamera()
    {
        cmCombat.Priority = 20;
    }

    private void SwitchToExplorationCamera()
    {
        cmCombat.Priority = 0;
    }
}
