using System;
using System.Collections.Generic;

public class SimStatModifiers
{
    public List<MonsterStatModifier> modifiers { get; private set; }
    public List<MonsterStatModifierVisual> visuals { get; private set; }

    private Action _UpdateStunStatus;
    private Action _UpdateSilenceStatus;
    private Action _UpdatePoisonedStatus;

    public SimStatModifiers(Action UpdateStunStatus, Action UpdateSilenceStatus, Action UpdatePoisonedStatus)
    {
        modifiers = new List<MonsterStatModifier>();
        visuals = new List<MonsterStatModifierVisual>();
        _UpdateStunStatus = UpdateStunStatus;
        _UpdateSilenceStatus = UpdateSilenceStatus;
        _UpdatePoisonedStatus = UpdatePoisonedStatus;
    }

    public void RemoveExpiredModifiers(int tick)
    {
        if (modifiers.Count > 0)
        {
            foreach (MonsterStatModifier modifier in new List<MonsterStatModifier>(modifiers))
            {
                if (tick == modifier.endTick)
                {
                    RemoveModifier(modifier);
                }
            }
        }
    }

    public void AddModifier(MonsterStatModifier addModifier, SimStats simStats)
    {
        addModifier.AdjustForCCModifier(simStats.ccModifier);

        if (addModifier.visual != null)
        {
            AddVisual(addModifier.visual);
        }
        modifiers.Add(addModifier);
        UpdateStatEffects(addModifier);
    }

    public void RemoveModifier(MonsterStatModifier removeModifier)
    {
        if (removeModifier.visual != null)
        {
            RemoveVisual(removeModifier.visual);
        }
        modifiers.Remove(removeModifier);
        UpdateStatEffects(removeModifier);
    }

    public void AddVisual(MonsterStatModifierVisual addVisual)
    {
        var shouldAddVisual = true;
        foreach (MonsterStatModifierVisual visual in visuals)
        {
            if (visual.icon == addVisual.icon)
            {
                visual.stacks += addVisual.stacks;
                shouldAddVisual = false;
                break;
            }
        }
        RemoveExpiredVisuals();
        if (shouldAddVisual)
        {
            visuals.Add(addVisual);
        }
    }

    public void RemoveVisual(MonsterStatModifierVisual removeVisual)
    {
        foreach (MonsterStatModifierVisual visual in visuals)
        {
            if (visual.icon == removeVisual.icon)
            {
                visual.stacks -= removeVisual.stacks;
                break;
            }
        }
        RemoveExpiredVisuals();
    }

    private void RemoveExpiredVisuals()
    {
        foreach (MonsterStatModifierVisual displayStatModifier in new List<MonsterStatModifierVisual>(visuals))
        {
            if (displayStatModifier.stacks == 0)
            {
                visuals.Remove(displayStatModifier);
            }
        }
    }

    private void UpdateStatEffects(MonsterStatModifier modifier)
    {
        if (modifier.statEffects.stun != 0)
        {
            _UpdateStunStatus();
        }
        if (modifier.statEffects.silence != 0)
        {
            _UpdateSilenceStatus();
        }
        if (modifier.statEffects.poisoned != 0)
        {
            _UpdatePoisonedStatus();
        }
    }
}
