using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSim : Sim
{
    public SlimeSim(SimInput input) : base(input) { }

    protected override void InitiateAbility()
    {
        base.InitiateAbility();

        AcquireTarget();
        if (currentTarget == null)
        {
            actionLoop.UpdateCurrentAction(ActionType.NONE);
        }
    }

    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();

        if (currentTarget != null)
        {
            ResetCurrentMana();
            var multiplier = GetAbilityMultiplier();
            var healthModifier = (int)(currentTarget.stats.health * multiplier);
            var damageModifier = (int)(currentTarget.stats.damage * multiplier);
            var resistModifier = (int)(currentTarget.stats.resists * multiplier);
            var modifiedStats = new MonsterBaseStats(damageModifier, healthModifier, resistModifier, 0, 0);
            var newStatChange = new MonsterStatModifier(matchSim.currentTick, -1, modifiedStats, new MonsterStatEffects(), StatModifierType.POSITIVE, StatModifierIcon.SLIME_ABILITY);
            statModifiers.AddModifier(newStatChange, simStats);
            currentHealth += healthModifier;
        }
    }

    private float GetAbilityMultiplier()
    {
        var level = Mathf.Clamp(stats.level, 0, 3);
        switch(level)
        {
            case 0:
                // drop through
            case 1:
                return 0.3f;
            case 2:
                return 0.4f;
            case 3:
                // drop through
            default:
                return 0.5f;
        }
    }
}