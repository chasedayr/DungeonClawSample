public class SlimeMonsterInfo : MonsterInfo
{
    public override MonsterType monsterType => MonsterType.SLIME;
    public override string monsterId => "M_SLIME";
    public override string displayName => "Slime";
    protected override MonsterResourceType _resourceType => MonsterResourceType.MANA;

    // STAT LEVEL MODIFIERS

    protected override float[] _damageLevelMultipliers => new float[4] { 0.5f, 1.0f, 1.5f, 2.25f };
    protected override float[] _healthLevelMultipliers => new float[4] { 0.5f, 1.0f, 1.75f, 2.5f };
    protected override int[] _manaLevelIncrements => new int[4] { 90, 90, 100, 110 };
    protected override int[] _rageLevelIncrements => new int[4] { 0, 0, 0, 0 };

    // BASE STATS

    protected override int _baseDamage => 48;
    protected override int _baseHealth => 450;
    protected override int _baseResists => 12;
    protected override int _baseAttackSpeed => 10;
    protected override int _baseCritical => 25;

    // TICK STATS

    protected override int _abilityEndTicks => 12;
    protected override int _abilityExecuteTicks => 8;
    protected override int _attackEndTicks => 10;
    protected override int _attackExecuteTicks => 5;
    protected override int _deathEndTicks => 6;

    public override MonsterStats GetMonsterStats(int level = 1)
    {
        return new MonsterStats(level, baseStats, tickStats, _resourceType,
            _damageLevelMultipliers, _healthLevelMultipliers,
            _manaLevelIncrements, _rageLevelIncrements);
    }

    public override Sim CreateSim(SimInput input)
    {
        return new SlimeSim(input); 
    }

    public override string GetDescription()
    {
        var infoString = "Increase Stats by 30 %/ 40 %/ 50 % of Enemy's Base Stats";
        return infoString;
    }
}
