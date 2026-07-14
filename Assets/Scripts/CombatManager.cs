using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class CombatManager : MonoBehaviour
{
    [Header("References flux de jeu")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Combat")]
    [SerializeField] CombatSequencer sequencer;

    [Header("Cameras")]
    [SerializeField] CinemachineCamera cmExploration;
    [SerializeField] CinemachineCamera cmCombat;

    [Header("HUD")]
    [SerializeField] HealthBarUI heroHealthBar;
    [SerializeField] HealthBarUI enemyHealthBar;
    [SerializeField] GameObject enemyHudPanel;

    [Header("Timing")]
    [SerializeField] float delay = 0.8f;

    private CombatResolver _resolver = new CombatResolver();


    // --- Point d'entrée appelé par PlayerState ---
    public IEnumerator StartCombat(Hero playerUnit, Enemy enemyUnit)
    {
        sequencer.ResetVisuals();
        SwitchToCombatCamera();

        // Init HUD combat
        heroHealthBar.SetHealth(playerUnit.CurrentHp, playerUnit.HpMax);
        enemyHealthBar.SetHealth(enemyUnit.CurrentHp, enemyUnit.HpMax);
        enemyHudPanel.SetActive(true);

        Debug.LogWarning($"Un {enemyUnit.Archetype} sauvage est apparut. Préparez vous aux combat !");
        yield return new WaitForSeconds(delay);

        yield return RunCombat(playerUnit, enemyUnit);

        enemyHudPanel.SetActive(false);
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

    // --- Boucle de combat ---
    private IEnumerator RunCombat(Hero playerUnit, Enemy enemyUnit)
    {
        Debug.LogWarning($"Stats de départ : Hero currentPV = {playerUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
        yield return new WaitForSeconds(delay);

        bool isHeroFirst = _resolver.RollInit(playerUnit.DEX, enemyUnit.DEX);
        Fighter firstToPlay = isHeroFirst ? playerUnit : enemyUnit;
        Fighter secondToPlay = isHeroFirst ? enemyUnit : playerUnit;


        Debug.LogWarning($"{firstToPlay.Name} est à l'initiative");
        yield return new WaitForSeconds(delay);

        do
        {
            yield return StartCoroutine(Attack(firstToPlay, secondToPlay, isHeroFirst));
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(Attack(secondToPlay, firstToPlay, !isHeroFirst));
            yield return new WaitForSeconds(delay);

            Debug.LogWarning($"Stats MaJ : Hero currentPV = {playerUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
            yield return new WaitForSeconds(delay);

        } while (playerUnit.IsAlive && enemyUnit.IsAlive);

        // PlayDeath se joue apres la boucle complete, sur l'etat final IsAlive.
        // Mort simultanee possible (cf GDD) : on joue les deux deaths.
        if (!playerUnit.IsAlive)
        {
            yield return StartCoroutine(sequencer.PlayDeath(true));
            Debug.LogWarning($"Vous avez fait ce que vous avez pu mais {enemyUnit.Name} a eu raison de vous. C'était le combat de trop. Puissiez-vous reposez en paix");
        } else
        {
            Debug.LogWarning($"Vous vous êtes vaillamment battu et {enemyUnit.Name} gît davant vous dans une mare de sang. En espérant que ça continue ainsi. Il vous reste {playerUnit.CurrentHp} PV !");
        }

        if (!enemyUnit.IsAlive)
            yield return StartCoroutine(sequencer.PlayDeath(false));


        yield return null;
    }


    // --- Attaque unitaire : resolution + sequencement visuel ---

    /// <summary>
    /// Orchestre une attaque : visuel d'attaque, résolution des calculs, visuel de réaction.
    /// isHeroAttacking identifie qui attaque pour que le Sequencer sache quel visuel animer.
    /// </summary>
    private IEnumerator Attack(Fighter attacker, Fighter target, bool isHeroAttacking)
    {
        // 1. Animation d'attaque — se termine a l'impact
        yield return StartCoroutine(sequencer.PlayAttack(isHeroAttacking));

        // 2. Resolution des calculs (CombatManager garde le Resolver)
        AttackResultDto result = _resolver.ResolveAttack(attacker, target);

        // 3. MaJ HUD de la cible (au moment de l'impact, si touche)
        if (result.DidHit)
        {
            if (isHeroAttacking)
                enemyHealthBar.SetHealth(target.CurrentHp, target.HpMax);
            else
                heroHealthBar.SetHealth(target.CurrentHp, target.HpMax);
        }

        // 4. Reaction de la cible
        yield return StartCoroutine(sequencer.PlayReaction(isHeroAttacking, result.DidHit));

        Debug.LogWarning(result.DidHit
            ? $"{attacker.Name} a fait {result.Damage} dégâts à {target.Name}"
            : $"{attacker.Name} s'est lamentablement foiré et à complètement raté {target.Name}");
    }


    // --- Caméras (candidats futur SceneFlowManager ou CameraManager) ---
    private void SwitchToCombatCamera()
    {
        cmCombat.Priority = 20;
    }

    private void SwitchToExplorationCamera()
    {
        cmCombat.Priority = 0;
    }
}
