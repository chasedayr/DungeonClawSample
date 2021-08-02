using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterInfo
{
    public abstract MonsterType monsterType { get; }
    public abstract string monsterId { get; }
    public abstract string displayName { get; }
    public abstract MonsterStats GetMonsterStats(int level);
    protected abstract MonsterResourceType _resourceType { get; }

    // STAT LEVEL MODIFIERS

    protected abstract float[] _damageLevelMultipliers { get; }
    protected abstract float[] _healthLevelMultipliers { get; }
    protected abstract int[] _manaLevelIncrements { get; }
    protected abstract int[] _rageLevelIncrements { get; }

    // BASE STATS

    public MonsterBaseStats baseStats
    {
        get
        {
            if (_baseStats == null)
            {
                _baseStats = new MonsterBaseStats(_baseDamage, _baseHealth, _baseResists, _baseAttackSpeed, _baseCritical);
            }
            return _baseStats;
        }
    }
    protected MonsterBaseStats _baseStats;
    protected abstract int _baseDamage { get; }
    protected abstract int _baseHealth { get; }
    protected abstract int _baseResists { get; }
    protected abstract int _baseAttackSpeed { get; }
    protected abstract int _baseCritical { get; }

    //TICK STATS

    public MonsterTickStats tickStats
    {
        get
        {
            if (_tickStats == null)
            {
                _tickStats = new MonsterTickStats(_abilityEndTicks, _abilityExecuteTicks, _attackEndTicks, _attackExecuteTicks, _deathEndTicks);
            }
            return _tickStats;
        }
    }
    protected MonsterTickStats _tickStats;
    protected abstract int _abilityEndTicks { get; }
    protected abstract int _abilityExecuteTicks { get; }
    protected abstract int _attackEndTicks { get; }
    protected abstract int _attackExecuteTicks { get; }
    protected abstract int _deathEndTicks { get; }

    public abstract Sim CreateSim(SimInput input);
    public abstract string GetDescription();
}

public enum MonsterResourceType { MANA, RAGE, NONE };

public enum MonsterType
{
    BAT,
    CHEST,
    DRAGON,
    FLAME,
    KNIGHT,
    OWLBEAR,
    RABBIT,
    SKELETON,
    SLIME,
    TOAD,
    GOLEM,
    SCARECROW,
    MUSHROOM,
    NONE
};
