using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimStats
{
    private Sim sim;

    public SimStats(Sim sim)
    {
        this.sim = sim;
    }

    public int healthPool
    {
        get
        {
            var totalHealth = sim.stats.health;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalHealth += statModifier.stats.health;
            }
            return totalHealth;
        }
    }

    public int manaPool
    {
        get
        {
            return sim.stats.mana;
        }
    }

    public int ragePool
    {
        get
        {
            return sim.stats.rage;
        }
    }

    public int damage
    {
        get
        {
            var totalDamage = sim.stats.damage;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalDamage += statModifier.stats.damage;
            }
            return totalDamage;
        }
    }
    public int critical
    {
        get
        {
            var totalCritical = sim.stats.critical;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalCritical += statModifier.stats.critical;
            }
            return totalCritical;
        }
    }
    public int attackSpeed
    {
        get
        {
            var totalAttackSpeed = sim.stats.attackSpeed;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalAttackSpeed += statModifier.stats.attackSpeed;
            }
            if (totalAttackSpeed < 0) totalAttackSpeed = 0;
            return totalAttackSpeed;
        }
    }
    public int resists
    {
        get
        {
            var totalResist = sim.stats.resists;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalResist += statModifier.stats.resists;
            }
            return totalResist;
        }
    }

    public bool silence
    {
        get
        {
            var totalSilence = sim.statEffects.silence;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalSilence += statModifier.statEffects.silence;
            }
            if (totalSilence > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool invulnerable
    {
        get
        {
            var totalInvulnerable = sim.statEffects.invulnerable;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalInvulnerable += statModifier.statEffects.invulnerable;
            }
            if (totalInvulnerable > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public int undying
    {
        get
        {
            var totalUndying = sim.statEffects.undying;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalUndying += statModifier.statEffects.undying;
            }
            return totalUndying;
        }
    }

    public bool stun
    {
        get
        {
            var totalStun = sim.statEffects.stun;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalStun += statModifier.statEffects.stun;
            }
            if (totalStun > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool confused
    {
        get
        {
            var totalConfused = sim.statEffects.confused;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalConfused += statModifier.statEffects.confused;
            }
            if (totalConfused > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool poisoned
    {
        get
        {
            var totalPoison = sim.statEffects.poisoned;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalPoison += statModifier.statEffects.poisoned;
            }
            if (totalPoison > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //thorns is percent damage reflected as int, returned as easy multiply value
    //int 20 thorns returns 0.2f;
    public float thorns
    {
        get
        {
            var totalThorns = sim.statEffects.thorns;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalThorns += statModifier.statEffects.thorns;
            }
            var percentThorns = totalThorns / 100.0f;
            return percentThorns;
        }
    }

    //healing reduction is percent healing reduced as int, returned as easy multiply value
    //int 80 healingReduction returns 0.2f;
    public float healingReduction
    {
        get
        {
            var totalHealingReduction = sim.statEffects.healingReduction;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalHealingReduction += statModifier.statEffects.healingReduction;
            }
            if (totalHealingReduction >= 100)
            {
                return 0.0f;
            }
            else
            {
                var percentHealingReduction = (float)(100 - totalHealingReduction) / 100.0f;
                return percentHealingReduction;
            }
        }
    }

    //mana boost is percent manaBoost as int, returned as easy multiply value
    //int 20 manaBoost returns 1.2f
    public float manaGainBoost
    {
        get
        {
            var totalManaGainBoost = sim.statEffects.manaGainBoost;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalManaGainBoost += statModifier.statEffects.manaGainBoost;
            }
            var percentManaGainBoost = (float)(100 + totalManaGainBoost) / 100.0f;
            return percentManaGainBoost;
        }
    }

    //ccmodifier is percent change as int, returned as easy multiply value
    //int 20 ccmodifier returns 1.2f, int -20 return 0.8f
    public float ccModifier
    {
        get
        {
            var totalCCModifier = sim.statEffects.ccModifier;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalCCModifier += statModifier.statEffects.ccModifier;
            }
            var percentCCModifier = (float)(100 + totalCCModifier) / 100.0f;
            return percentCCModifier;
        }
    }

    //damage boost is percent dmg increase as int, returned as easy multiply value
    //int 20 damageBoost returns 1.2f
    public float damageBoost
    {
        get
        {
            var totalDamageBoost = sim.statEffects.damageBoost;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalDamageBoost += statModifier.statEffects.damageBoost;
            }
            var percentDamageBoost = (float)(100 + totalDamageBoost) / 100.0f;
            return percentDamageBoost;
        }
    }

    //lifeSteal is percent lifeSteal increase as int, returned as easy multiply value
    //int 20 lifeSteal returns 0.2f
    public float lifeSteal
    {
        get
        {
            var totalLifeSteal = sim.statEffects.lifeSteal;
            foreach (MonsterStatModifier statModifier in sim.statModifiers.modifiers)
            {
                totalLifeSteal += statModifier.statEffects.lifeSteal;
            }
            var percentLifeSteal = totalLifeSteal / 100.0f;
            return percentLifeSteal;
        }
    }

    public Sim tauntTarget
    {
        get
        {
            var tauntTargetList = new List<Sim>();
            foreach (MonsterStatModifier statModifier in new List<MonsterStatModifier>(sim.statModifiers.modifiers))
            {
                if (statModifier.statEffects.tauntTarget != null)
                {
                    if (statModifier.statEffects.tauntTarget.isDead)
                    {
                        sim.statModifiers.RemoveModifier(statModifier);
                    }
                    else if(!statModifier.statEffects.tauntTarget.isRemoved)
                    {
                        tauntTargetList.Add(statModifier.statEffects.tauntTarget);
                    }
                }
            }
            if (tauntTargetList.Count > 0)
            {
                return tauntTargetList[tauntTargetList.Count - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
