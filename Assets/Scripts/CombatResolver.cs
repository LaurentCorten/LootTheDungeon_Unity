using Random = UnityEngine.Random;


public class CombatResolver
{

    /// <summary>
    /// Résout une attaque complète : jet de touché, dégâts si touché, état de la cible.
    /// </summary>
    public AttackResultDto ResolveAttack(Fighter attacker, Fighter target)
    {
        if (!RollTouch(attacker.MainStat, target.AC))
        {
            return new AttackResultDto(false, 0, false);
        }

        int dmg = RollDmg(attacker.Damages);
        target.AdaptLife(-dmg);

        return new AttackResultDto(true,dmg,!target.IsAlive);
    }

    /// <summary>
    /// Vérifie qui a l'initiative en fonction des stats de Dexterité respectivement du Hero et du Mob, altérée par un jet de D20.
    /// </summary>
    /// <param name="heroDEX">Valeur de la stat de Dexterité du Hero</param>
    /// <param name="mobDEX">Valeur de la stat de Dexterité du Hero</param>
    /// <returns>Retourne vrai si le Hero à l'initiative, faux si c'est le Mob.</returns>
    public bool RollInit(int heroDEX, int mobDEX)
    {
        int dHero = Random.Range(1, 21);
        int dMob = Random.Range(1, 21);
        return (dHero + (heroDEX / 2)) >= (dMob + (mobDEX / 2));
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
        return (dAtt + (attMainStat / 2)) > defAC;
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