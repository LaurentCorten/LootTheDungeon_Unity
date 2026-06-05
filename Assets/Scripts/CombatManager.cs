using System;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class CombatManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] GameObject player;
    PlayerState playerState;
    EnemyState enemyState;
    Enemy enemyUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.GetComponent<PlayerMovement>().OnPlayerMoved += HandlePlayerMoved;
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
            StartCombat(enemyUnit);
        }
    }

    private void StartCombat(Enemy enemyUnit)
    {
        playerState = player.GetComponent<PlayerState>();
        //TODO boucle de combat !! Avec Blocage du reste !!!
        Debug.Log(RollInit(playerState.Hero.DEX, enemyUnit.DEX));
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

}
