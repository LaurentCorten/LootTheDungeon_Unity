public abstract class Fighter
{

    // Propriétés
    public string Archetype { get; } // Probablement aussi à remplacer par Enum
    public string Name { get; } // Pour le moment le héro sélectionné à un nom, plus tard ça pourra plutôt être le joueur qui à un nom même s'il joue différents archétypes
    public int HpMax { get; }
    public int CurrentHp { get; private set; }
    public int CON { get; }
    public int STR { get; }
    public int DEX { get; }
    public int INT { get; }
    public int MainStat { get; } // Pour le moment héberge en dur la valeur de la stat principale => redondance. À remplacer par un Enum probablement.
    public string Damages { get; } //string type "1d6". Remplacer par <int,int> ?
    public int AC { get; }
    public bool IsAlive { get; private set; }


    // Ctor
    public Fighter(string archetype, string name, int hpMax, int con, int str, int dex, int intel, int mainStat, string damages, int ac)
    {
        Archetype = archetype;
        Name = name;
        HpMax = hpMax;
        CurrentHp = hpMax;
        CON = con;
        STR = str;
        DEX = dex;
        INT = intel;
        MainStat = mainStat;
        Damages = damages;
        AC = ac;
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
        {
            CurrentHp = HpMax;
            return;
        }
        if (CurrentHp + amount <= 0)
        {
            CurrentHp = 0;
            IsAlive = false;
            return;
        }
        CurrentHp += amount;
        return;

    }

}
