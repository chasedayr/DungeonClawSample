public class MonsterTickStats
{
    public int abilityEndTicks { get; }
    public int abilityExecuteTicks { get; }
    public int attackEndTicks { get; }
    public int attackExecuteTicks { get; }
    public int deathEndTicks { get; }

    public MonsterTickStats(int abilityEndTicks, int abilityExecuteTicks, int attackEndTicks, int attackExecuteTicks, int deathEndTicks)
    {
        this.abilityEndTicks = abilityEndTicks;
        this.abilityExecuteTicks = abilityExecuteTicks;
        this.attackEndTicks = attackEndTicks;
        this.attackExecuteTicks = attackExecuteTicks;
        this.deathEndTicks = deathEndTicks;
    }
}
