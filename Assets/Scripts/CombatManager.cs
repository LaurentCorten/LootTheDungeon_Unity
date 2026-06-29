using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class CombatManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameManager gameManager;

    Hero heroUnit;
    Enemy enemyUnit;

    float delay = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement.OnPlayerMoved += HandlePlayerMoved;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandlePlayerMoved(Vector2Int playerPosition)
    {
        if (gridManager.Grid[playerPosition].TileState == TileState.Encounter)
        {                
            enemyUnit = gridManager.Grid[playerPosition].Enemy;
            StartCoroutine(StartCombat());
        }
    }

    //TODO check comment faire le pendant visuel
    IEnumerator StartCombat()
    {
        playerMovement.SetCanMove(false);
        Debug.LogWarning($"Un {enemyUnit.Archetype} sauvage est apparut. Préparez vous aux combat !");
        yield return new WaitForSeconds(delay);
        yield return RunCombat();
        if (heroUnit.IsAlive)
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

    private IEnumerator RunCombat()
    {
        heroUnit = player.GetComponent<PlayerState>().hero;
        Debug.LogWarning($"Stats de départ : Hero currentPV = {heroUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
        yield return new WaitForSeconds(delay);

        bool isHeroFirst = RollInit(heroUnit.DEX, enemyUnit.DEX);
        Fighter firstToPlay = isHeroFirst ? heroUnit : enemyUnit;
        Fighter secondToPlay = isHeroFirst ? enemyUnit : heroUnit;
        Debug.LogWarning($"{firstToPlay.Name} est à l'initiative");
        yield return new WaitForSeconds(delay);

        do
        {
            Attack(firstToPlay, secondToPlay);
            yield return new WaitForSeconds(delay);
            Attack(secondToPlay, firstToPlay);
            yield return new WaitForSeconds(delay);
            Debug.LogWarning($"Stats MaJ : Hero currentPV = {heroUnit.CurrentHp} - Enemy currentPV = {enemyUnit.CurrentHp}");
            yield return new WaitForSeconds(delay);
        } while (heroUnit.IsAlive && enemyUnit.IsAlive);
        Debug.LogWarning($"{(heroUnit.IsAlive ? $"Vous vous êtes vaillamment battu et {enemyUnit.Name} gît davant vous dans une mare de sang. En espérant que ça continue ainsi. Il vous reste {heroUnit.CurrentHp} PV !" : $"Vous avez fait ce que vous avez pu mais {enemyUnit.Name} a eu raison de vous. C'était le combat de trop. Puissiez-vous reposez en paix")}");

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

    private void Attack(Fighter attacker, Fighter target)
    {
        if (!RollTouch(attacker.MainStat, target.AC))
        {
            Debug.LogWarning($"{attacker.Name} s'est lamentablement foiré et à complètement raté {target.Name}");
            return;
        }
        int dmg = RollDmg(attacker.Damages);
        target.AdaptLife(-dmg);
        Debug.LogWarning($"{attacker.Name} a fait {dmg} dégât à {target.Name}");
    }

}
