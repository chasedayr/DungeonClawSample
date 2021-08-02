using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimActionLoop
{
    private Sim _sim;
    private MatchSim _matchSim;
    public ActionType currentAction { get; private set; } = ActionType.NONE;
    private int _abilityQueueCount;
    private int _attackTime;
    private int _startActionTick;
    private int _executeActionTick;
    private int _endActionTick;

    private Action _InitiateDeath;
    private Action _ExecuteDeath;
    private Action _InitiateAttack;
    private Action _ExecuteAttack;
    private Action _InitiateAbility;
    private Action _ExecuteAbility;

    public SimActionLoop(Sim sim, MatchSim matchSim, Action InitiateDeath, Action ExecuteDeath,
        Action InitiateAttack, Action ExecuteAttack,
        Action InitiateAbility, Action ExecuteAbility)
    {
        _sim = sim;
        _matchSim = matchSim;
        _InitiateDeath = InitiateDeath;
        _ExecuteDeath = ExecuteDeath;
        _InitiateAttack = InitiateAttack;
        _ExecuteAttack = ExecuteAttack;
        _InitiateAbility = InitiateAbility;
        _ExecuteAbility = ExecuteAbility;
    }

    public void UpdateCurrentAction(ActionType actionType)
    {
        currentAction = actionType;
    }

    public void PerformActions()
    {
        if (currentAction == ActionType.DEATH)
        {
            if (_matchSim.currentTick == _startActionTick)
            {
                _InitiateDeath();
            }
            else if (_matchSim.currentTick == _endActionTick)
            {
                _ExecuteDeath();
            }
            return;
        }

        if (_sim.isRemoved)
        {
            currentAction = ActionType.NONE;
            return;
        }

        if (_sim.simStats.stun)
        {
            currentAction = ActionType.NONE;
            return;
        }

        if (currentAction == ActionType.NONE)
        {
            if (_abilityQueueCount > 0)
            {
                _abilityQueueCount--;
                AssignAbilityTicks();
            }
            else if (IsAbilityReady())
            {
                AssignAbilityTicks();
            }
            else if (_matchSim.currentTick >= _attackTime)
            {
                _sim.AcquireTarget();
                if (_sim.currentTarget != null)
                {
                    AssignAttackTicks();
                }
            }
        }
        if (currentAction == ActionType.ATTACK)
        {

            if (CanAbilityOverrideAttack())
            {
                _abilityQueueCount = 1;
            }

            if (_matchSim.currentTick == _startActionTick)
            {
                _InitiateAttack();
            }
            else if (_matchSim.currentTick == _executeActionTick)
            {
                if (_sim.currentTarget != null)
                {
                    _ExecuteAttack();
                }
                else
                {
                    ExecuteAttackFailed();
                }
            }
            else if (_matchSim.currentTick == _endActionTick)
            {
                _attackTime = _matchSim.currentTick + _sim.simStats.attackSpeed;
                currentAction = ActionType.NONE;
            }
        }
        if (currentAction == ActionType.ABILITY)
        {
            if (_matchSim.currentTick == _startActionTick)
            {
                _InitiateAbility();
            }
            else if (_matchSim.currentTick == _executeActionTick)
            {
                _ExecuteAbility();
            }
            else if (_matchSim.currentTick == _endActionTick)
            {
                currentAction = ActionType.NONE;
            }
        }
    }

    public void AssignDeathTicks()
    {
        currentAction = ActionType.DEATH;
        _startActionTick = _matchSim.currentTick + 1;
        _executeActionTick = 0;
        _endActionTick = _matchSim.currentTick + 1 + _sim.stats.tickStats.deathEndTicks;
    }

    private void AssignAbilityTicks()
    {
        currentAction = ActionType.ABILITY;
        _startActionTick = _matchSim.currentTick;
        _executeActionTick = _matchSim.currentTick + _sim.stats.tickStats.abilityExecuteTicks;
        _endActionTick = _matchSim.currentTick + _sim.stats.tickStats.abilityEndTicks;
    }

    private void AssignAttackTicks()
    {
        currentAction = ActionType.ATTACK;
        _startActionTick = _matchSim.currentTick;
        _executeActionTick = _matchSim.currentTick + _sim.stats.tickStats.attackExecuteTicks;
        _endActionTick = _matchSim.currentTick + _sim.stats.tickStats.attackEndTicks;
    }

    private bool CanAbilityOverrideAttack()
    {
        if (currentAction == ActionType.ATTACK && _matchSim.currentTick >= _executeActionTick)
        {
            if (_abilityQueueCount == 0 && IsAbilityReady())
            {
                return true;
            }
        }
        return false;
    }

    private bool IsAbilityReady()
    {
        if (_sim.simStats.silence) return false;

        if (_sim.stats.resourceType == MonsterResourceType.MANA && _sim.currentMana >= _sim.simStats.manaPool)
        {
            return true;
        }
        if (_sim.stats.resourceType == MonsterResourceType.RAGE && _sim.currentRage >= _sim.simStats.ragePool)
        {
            return true;
        }
        return false;
    }

    private void ExecuteAttackFailed()
    {
        _attackTime = _matchSim.currentTick + 1;
        currentAction = ActionType.NONE;
    }
}
