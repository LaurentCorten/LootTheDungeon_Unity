using System.Collections;
using UnityEngine;

public class CombatVisual : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Vector3 attackDirection;
    [SerializeField] float attackDistance = 0.5f;
    [SerializeField] float attackSpeed = 8f;

    [Header("Hit")]
    [SerializeField] Vector3 hitDirection;
    [SerializeField] float hitDistance = 0.3f;
    [SerializeField] float hitSpeed = 10f;
    [SerializeField] Color hitColor = Color.red;
    [SerializeField] float hitFlashDuration = 0.15f;

    [Header("Death")]
    [SerializeField] float deathFadeDuration = 1f;

    private Vector3 _originPosition;
    private Material _material;
    private Color _originalColor;

    private void Start()
    {
        _originPosition = transform.position;
        _material = GetComponentInChildren<MeshRenderer>().material;
        _originalColor = _material.color;
    }

    public IEnumerator PlayAttack()
    {
        Vector3 target = _originPosition + attackDirection * attackDistance;
        yield return MoveToPosition(target, attackSpeed);
        yield return MoveToPosition(_originPosition, attackSpeed);
    }

    public IEnumerator PlayHit()
    {
        Vector3 target = _originPosition + hitDirection * hitDistance;
        yield return MoveToPosition(target, hitSpeed);

        _material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        _material.color = _originalColor;

        yield return MoveToPosition(_originPosition, hitSpeed);
    }

    public IEnumerator PlayDodge()
    {
        Vector3 target = _originPosition + hitDirection * hitDistance;
        yield return MoveToPosition(target, hitSpeed);

        yield return new WaitForSeconds(hitFlashDuration);

        yield return MoveToPosition(_originPosition, hitSpeed);
    }

    public IEnumerator PlayDeath()
    {
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

    public void Reset()
    {
        transform.position = _originPosition;
        _material.color = _originalColor;
        gameObject.SetActive(true);
    }
}