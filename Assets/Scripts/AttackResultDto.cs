



public readonly struct AttackResultDto
{
    public bool DidHit { get; }
    public int Damage { get; }
    public bool TargetDied { get; }

    public AttackResultDto(bool didHit, int damage, bool targetDied)
    {
        DidHit = didHit;
        Damage = damage;
        TargetDied = targetDied;
    }
}
