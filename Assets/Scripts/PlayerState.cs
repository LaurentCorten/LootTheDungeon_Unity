using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] HeroData data;
    public Hero Hero { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hero = new Hero(
            data.archetype,
            data.heroName,
            data.hpMax,
            data.CON,
            data.STR,
            data.DEX,
            data.INT,
            data.mainStat,
            data.damages,
            data.AC
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
