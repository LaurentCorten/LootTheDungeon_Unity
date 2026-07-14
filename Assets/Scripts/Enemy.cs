public class Enemy: Fighter
{

    // Propriétés
    public int BonusAtt { get; }
    public int Score { get; }
    public Size EnemySize{ get; }


    // Ctor
    public Enemy(string archetype, string name, int hpMax, int con, int str, int dex, int intel, int mainStat, string damages, int ac, int bonusAtt, int score, Size size) : base(archetype, name, hpMax, con, str, dex, intel, mainStat, damages, ac)
    {
        BonusAtt = bonusAtt;
        Score = score;
        EnemySize = size;
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
