public class Enemy
{

    // Propriétés
    public string Archetype { get; } // Probablement aussi à remplacer par Enum
    public int HpMax { get; }
    public int CurrentHp { get; private set; }
    public int CON { get; }
    public int STR { get; }
    public int DEX { get; }
    public int INT { get; }
    public int MainStat { get; } // Pour le moment héberge en dur la valeur de la stat principale => redondance. À remplacer par un Enum probablement.
    public string Damages { get; } //string type "1d6". Remplacer par <int,int> ?
    public int AC { get; }
    public int BonusAtt { get; }
    public int Score { get; }
    public Size EnemySize{ get; }
    public bool IsAlive { get; private set; }

    // Ctor
    public Enemy(string archetype, int hpMax, int con, int str, int dex, int intel, int mainStat, string damages, int ac, int bonusAtt, int score, Size size)
    {
        Archetype = archetype;
        HpMax = hpMax;
        CurrentHp = hpMax;
        CON = con;
        STR = str;
        DEX = dex;
        INT = intel;
        MainStat = mainStat;
        Damages = damages;
        AC = ac;
        BonusAtt = bonusAtt;
        Score = Score;
        EnemySize = size;
        IsAlive = true;
    }

    // Méthodes internes
    /// <summary>
    /// Permet d'interagir avec la vie du fighter
    /// </summary>
    /// <param name="amount">À donner en positif pour soigner et en negatif pour blesser !</param>
    public void AdaptLife(int amount)
    {
        if (CurrentHp + amount > HpMax)
        { CurrentHp = HpMax; return; }
        if (CurrentHp + amount <= 0)
        {
            CurrentHp = 0;
            IsAlive = false;
            return;
        }
        CurrentHp += amount;
        return;

    }

    public enum Size
    {
        Small,
        Medium,
        Large,
        Huge,
        Gargantuan
    }
}
