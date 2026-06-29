using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] HeroData data;
    [SerializeField] GameManager gameManager;

    public Hero hero;
    

    private void OnEnable()
    {
        gameManager.OnStartNewLevel += HandleStartNewLevel;        
    }

    private void OnDisable()
    {
        gameManager.OnStartNewLevel -= HandleStartNewLevel;
    }

    private void HandleStartNewLevel()
    {
        hero = new Hero(

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
}
