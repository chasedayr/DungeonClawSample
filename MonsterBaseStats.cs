public class MonsterBaseStats
{
    public int damage { get; }
    public int health { get; }
    public int resists { get; }
    public int attackSpeed { get; }
    public int critical { get; }

    public MonsterBaseStats(int damage, int health, int resists, int attackSpeed, int critical)
    {
        this.damage = damage;
        this.health = health;
        this.resists = resists;
        this.attackSpeed = attackSpeed;
        this.critical = critical;
    }
}
