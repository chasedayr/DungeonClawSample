public class MonsterStats
{
    public int level { get; }
    private int _safeLevel => UnityEngine.Mathf.Clamp(level, 0, 3);

    public MonsterResourceType resourceType { get; }

    // STAT LEVEL MODIFIERS

    private readonly float[] _damageMultipliers;
    private readonly float[] _healthMultipliers;
    private readonly int[] _manaIncrements;
    private readonly int[] _rageIncrements;

    // BASE STATS

    public MonsterBaseStats baseStats { get; }

    public int damage => (int)(baseStats.damage * _damageMultipliers[_safeLevel]);
    public int health => (int)(baseStats.health * _healthMultipliers[_safeLevel]);
    public int resists => baseStats.resists;
    public int attackSpeed => baseStats.attackSpeed;
    public int critical => baseStats.critical;
    public int mana => _manaIncrements[_safeLevel];
    public int rage => _rageIncrements[_safeLevel];

    // TICK STATS

    public MonsterTickStats tickStats { get; }

    public MonsterStats(int level, MonsterBaseStats baseStats, MonsterTickStats tickStats, MonsterResourceType resourceType,
        float[] damageMultipliers, float[] healthMultipliers,
        int[] manaIncrements, int[] rageIncrements)
    {
        this.level = level;
        this.baseStats = baseStats;
        this.tickStats = tickStats;
        this.resourceType = resourceType;
        _damageMultipliers = damageMultipliers;
        _healthMultipliers = healthMultipliers;
        _manaIncrements = manaIncrements;
        _rageIncrements = rageIncrements;
    }
}
