using System.Collections;
using UnityEngine;

public class CombatSequencer : MonoBehaviour
{
    [SerializeField] CombatVisual heroVisual;
    [SerializeField] CombatVisual enemyVisual;

    // --- API publique appelee par CombatManager ---

    /// <summary>
    /// Joue l'animation d'attaque du combattant désigné.
    /// isHeroisHeroAttacking = true => le hero attaque. false => l'ennemi attaque.
    /// Se termine au moment de l'impact (via Animation Event ou fallback transform),
    /// ce qui permet a CombatManager d'enchainer immediatement sur PlayReaction.
    /// </summary>
    public IEnumerator PlayAttack(bool isHero)
    {
        CombatVisual attacker = isHero ? heroVisual : enemyVisual;
        yield return StartCoroutine(attacker.PlayAttack());
    }

    /// <summary>
    /// Joue la réaction (hit ou dodge) de la cible du combattant désigné.
    /// isHeroAttacking = true => le héro a attaque, donc la cible est l'ennemi, et vice-versa.
    /// didHit = true => PlayHit, false => PlayDodge.
    /// </summary>
    public IEnumerator PlayReaction(bool isHeroAttacking, bool didHit)
    {
        CombatVisual target = isHeroAttacking ? enemyVisual : heroVisual;

        if (didHit)
        {
            yield return StartCoroutine(target.PlayHit());
        }
        else
        {
            yield return StartCoroutine(target.PlayDodge());
        }
    }

    /// <summary>
    /// Joue l'animation de mort du combattant désigné.
    /// isHeroTarget = true => le héro meurt. false => l'ennemi meurt.
    /// </summary>
    public IEnumerator PlayDeath(bool isHeroTarget)
    {
        CombatVisual dying = isHeroTarget ? heroVisual : enemyVisual;
        yield return StartCoroutine(dying.PlayDeath());
    }

    /// <summary>
    /// Remet les deux visuels dans leur état initial. À appeler en debut de chaque run de combat.
    /// </summary>
    public void ResetVisuals()
    {
        heroVisual.Reset();
        enemyVisual.Reset();
    }
}