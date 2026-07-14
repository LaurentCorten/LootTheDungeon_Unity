using System;
using System.Collections;
using UnityEngine;

public class CombatVisual : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator animator;

    [Header("Sound")]
    [SerializeField] AudioClip audioDodge;
    [SerializeField] AudioClip audioGetHit;
    [SerializeField] AudioClip audioDies;


    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int DeathTrigger = Animator.StringToHash("Death");

    [Header("Attack")]
    [SerializeField] Vector3 attackDirection;
    [SerializeField] float attackDistance = 0.5f;
    [SerializeField] float attackSpeed = 8f;

    [Header("Hit")]
    [SerializeField] Vector3 hitDirection;
    [SerializeField] float hitDistance = 0.3f;
    [SerializeField] float dodgeDistance = 0.6f;
    [SerializeField] float hitSpeed = 10f;
    [SerializeField] Color hitColor = Color.red;
    [SerializeField] float hitFlashDuration = 0.15f;

    [Header("Death")]
    [SerializeField] float deathFadeDuration = 1f;

    private Vector3 _originPosition;
    private Material _material;
    private Color _originalColor;
    private bool _impactReached; // Mis a true par l'Animation Event place sur le clip d'attaque, au moment de l'impact visuel
    private AudioSource _audioSource;

    private void Start()
    {
        _originPosition = transform.position;
        _material = GetComponentInChildren<MeshRenderer>().material;
        _originalColor = _material.color;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator PlayAttack()
    {
        _impactReached = false;
        TriggerAnimator(AttackTrigger);

        Vector3 target = _originPosition + attackDirection * attackDistance;
        yield return MoveToPosition(target, attackSpeed);

        if (animator != null)
        {
            // On attend le vrai event d'impact pose sur le clip d'attaque
            yield return new WaitUntil(() => _impactReached);
        }

        // Le retour a la position d'origine n'est pas attendu : il se joue en parallele
        // de la reaction de la cible, ce qui est voulu (cf notes de design).
        yield return MoveToPosition(_originPosition, attackSpeed);
    }

    /// <summary>
    /// À appeler depuis un Animation Event placé sur le clip d'attaque, au moment de l'impact.
    /// </summary>
    public void ReceiveAttackImpact()
    {
        _impactReached = true;
    }

    public IEnumerator PlayHit()
    {
        TriggerAnimator(HitTrigger);

        Vector3 target = _originPosition + hitDirection * hitDistance;
        yield return MoveToPosition(target, hitSpeed);
        _audioSource.PlayOneShot(audioGetHit);
        yield return MoveToPosition(_originPosition, hitSpeed);

    }

    public IEnumerator PlayDodge()
    {
        Vector3 target = _originPosition + hitDirection * dodgeDistance;
        yield return MoveToPosition(target, hitSpeed);

        _audioSource.PlayOneShot(audioDodge);
        yield return new WaitForSeconds(hitFlashDuration);

        yield return MoveToPosition(_originPosition, hitSpeed);
    }

    public IEnumerator PlayDeath()
    {
        TriggerAnimator(DeathTrigger);
        _audioSource.PlayOneShot(audioDies);

        float t = 0;
        Color start = _material.color;
        Color transparent = new Color(start.r, start.g, start.b, 0f);

        while (t < deathFadeDuration)
        {
            t += Time.deltaTime;
            _material.color = Color.Lerp(start, transparent, t / deathFadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    private void TriggerAnimator(int triggerHash)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerHash);
        }
    }

    public void Reset()
    {
        transform.position = _originPosition;
        _material.color = _originalColor;
        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.ResetTrigger(AttackTrigger);
            animator.ResetTrigger(HitTrigger);
            animator.ResetTrigger(DeathTrigger);
            // Suppose un state nomme "Idle" dans le Controller, a ajuster a l'etape 2
            animator.Play("Idle", 0, 0f);
        }
    }

}