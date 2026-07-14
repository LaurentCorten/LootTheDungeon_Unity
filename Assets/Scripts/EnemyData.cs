using UnityEngine;
using static Enemy;

[CreateAssetMenu(fileName = "MobData", menuName = "Scriptable Objects/MobData")]
public class EnemyData : ScriptableObject
{
    public string archetype;
    public int hpMax;
    public int CON;
    public int STR;
    public int DEX;
    public int INT;
    public int mainStat;
    public string damages;
    public int AC;
    public int bonusAtt;
    public int Score;
    public Size size;
}
