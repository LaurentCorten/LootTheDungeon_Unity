using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] EnemyData data;
    public Enemy Enemy { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy = new Enemy(
            data.archetype,
            data.hpMax,
            data.CON,
            data.STR,
            data.DEX,
            data.INT,
            data.mainStat,
            data.damages,
            data.AC,
            data.bonusAtt,
            data.Score,
            data.size
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
