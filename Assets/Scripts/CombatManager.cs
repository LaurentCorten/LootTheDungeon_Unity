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

    float delay = 0.5f;

    //TODO check comment faire le pendant visuel
    public IEnumerator StartCombat(Hero playerUnit, Enemy enemyUnit)
    {
        enemyVisual.Reset();
        SwitchToCombatCamera();
        Debug.LogWarning($"Un {enemyUnit.Archetype} sauvage est apparut. Préparez vous aux combat !");
        yield return new WaitForSeconds(delay);
        yield return RunCombat(playerUnit, enemyUnit);
        if (playerUnit.IsAlive) //? À faire gérer par PlayerState ? Ou GameManager via event ?
        {
            Vector2Int gridPosition = gridManager.ConvertPositionMapToGrid(player.transform.position);
            gridManager.ClearOneTile(gridPosition);
            playerMovement.SetCanMove(true);
            SwitchToExplorationCamera();
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

        bool isHeroFirst = RollInit(playerUnit.DEX, enemyUnit.DEX);
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

    /// <summary>
    /// Vérifie qui a l'initiative en fonction des stats de Dexterité respectivement du Hero et du Mob, altéré par un jet de D20.
    /// </summary>
    /// <param name="heroDEX">Valeur de la stat de Dexterité du Hero</param>
    /// <param name="mobDEX">Valeur de la stat de Dexterité du Hero</param>
    /// <returns>Retourne vrai si le Hero à l'initiative, faux si c'est le Mob.</returns>
    private bool RollInit(int heroDEX, int mobDEX)
    {
        int dHero = Random.Range(1, 21);
        int dMob = Random.Range(1, 21);
        bool heroFirst = ((dHero + (heroDEX / 2)) >= (dMob + (mobDEX / 2))) ? true : false;
        return (heroFirst);
    }

    /// <summary>
    /// Vérifie si l'attaque tentée atteind la cible.
    /// </summary>
    /// <param name="attMainStat">Valeur de la stat principale de l'attaquant</param>
    /// <param name="defAC">Valeur de la classe d'armure du défenseur</param>
    /// <returns>Retourne vrai si l'attaque touche la cible, faux si c'est un échec</returns>
    private bool RollTouch(int attMainStat, int defAC)
    {
        int dAtt = Random.Range(1, 21);
        bool touche = ((dAtt + (attMainStat / 2)) > defAC) ? true : false;
        return (touche);
    }

    /// <summary>
    /// Permet de connaitre la quantité de dégâts à appliquer (si le RollTouch est revenu positif).
    /// </summary>
    /// <param name="attDmg">Nombre x de dés à y faces au format "xdy"</param>
    /// <returns>Retourne donc le nombre de dégâts subit par le défenseur</returns>
    private int RollDmg(string attDmg) //TODO : Sortir les "+x" des dmg mobs in DB
    {
        int dmg = 0;
        string[] subs = attDmg.Split('d');
        for (int i = 0; i < int.Parse(subs[0]); i++)
        {
            dmg += Random.Range(1, int.Parse(subs[1]) + 1);
        }
        return dmg;
    }

    private IEnumerator Attack(Fighter attacker, Fighter target, CombatVisual attackerVisual, CombatVisual targetVisual)
    {
        yield return StartCoroutine(attackerVisual.PlayAttack());

        if (!RollTouch(attacker.MainStat, target.AC))
        {
            yield return StartCoroutine(targetVisual.PlayDodge());
            Debug.LogWarning($"{attacker.Name} s'est lamentablement foiré et à complètement raté {target.Name}");
            yield break;
        }
        yield return StartCoroutine(targetVisual.PlayHit());

        int dmg = RollDmg(attacker.Damages);
        target.AdaptLife(-dmg);
        Debug.LogWarning($"{attacker.Name} a fait {dmg} dégât à {target.Name}");
        yield return null;
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
