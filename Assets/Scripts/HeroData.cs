using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    public string archetype;
    public string heroName;
    public int hpMax;
    public int CON;
    public int STR;
    public int DEX;
    public int INT;
    public int mainStat;
    public string damages;
    public int AC;
}
