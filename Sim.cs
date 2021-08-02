using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Sim
{
    public Sim(SimInput input)
    {
        Initialize(input);
    }

    public int monsterId { get; private set; }
    public PlayerSide playerSide { get; private set; }
    public MonsterStats stats { get; private set; }
    public SimStats simStats { get; private set; }

    public bool isDead { get; protected set; }
    public bool isRemoved { get; protected set; }
    public int currentHealth { get; protected set; }
    public int currentMana { get; protected set; }
    public int currentRage { get; protected set; }
    public Sim currentTarget { get; protected set; }

    public MonsterStatEffects statEffects { get; protected set; }
    public SimStatModifiers statModifiers { get; protected set; }

    protected MatchSim matchSim;
    protected int randomSeed;
    protected SimActionLoop actionLoop;
    protected List<ScheduledSimAction> scheduledActions = new List<ScheduledSimAction>();

    public delegate void MonsterDeath(Sim monster);
    public static event MonsterDeath MonsterDeathEvent;

    protected void Initialize(SimInput input)
    {
        matchSim = input.matchSim;
        stats = input.collectedMonster.stats;
        simStats = new SimStats(this);
        playerSide = input.playerSide;
        randomSeed = input.randomSeed;
        monsterId = CreateMonsterId(input.positionIndex, input.playerSide);
        statModifiers = new SimStatModifiers(UpdateStunStatus, UpdateSilenceStatus, UpdatePoisonedStatus);
        actionLoop = new SimActionLoop(this, matchSim, InitiateDeath, ExecuteDeath, InitiateAttack, ExecuteAttack, InitiateAbility, ExecuteAbility);
        MatchSim.StartMatchEvent += MatchStart;
        MatchSim.EndMatchEvent += MatchEnd;
    }

    protected virtual void MatchStart()
    {
        MatchSim.StartMatchEvent -= MatchStart;
        MatchSim.MatchTickEvent += ExecuteActionLoop;
        currentHealth = stats.health;
    }

    protected virtual void MatchEnd()
    {
        MatchSim.EndMatchEvent -= MatchEnd;
        MatchSim.MatchTickEvent -= ExecuteActionLoop;
    }

    private void ExecuteActionLoop()
    {
        statModifiers.RemoveExpiredModifiers(matchSim.currentTick);
        ExecuteScheduledActions();
        actionLoop.PerformActions();
    }

    protected virtual void InitiateAbility()
    {
    }

    protected virtual void ExecuteAbility()
    {
    }

    protected virtual void InitiateAttack()
    {
    }

    protected virtual void ExecuteAttack()
    {
        if (simStats.confused)
        {
            currentTarget = this;
        }
        if (SimRandom(0, 100) < simStats.critical)
        {
            var actualDamage = currentTarget.CalculateAttackDamage(simStats.damage * 2.0f * simStats.damageBoost);
            currentTarget.TakeDamage(actualDamage, DamageType.ATTACK, true, this);
            TriggerOnHit(actualDamage);
        }
        else
        {
            var actualDamage = currentTarget.CalculateAttackDamage(simStats.damage * simStats.damageBoost);
            currentTarget.TakeDamage(actualDamage, DamageType.ATTACK, false, this);
            TriggerOnHit(actualDamage);
        }
        var manaGain = SimRandom(8, 19);
        ChangeCurrentMana(manaGain);
    }

    protected virtual void InitiateDeath()
    {
    }

    protected virtual void ExecuteDeath()
    {
        MonsterDeathEvent?.Invoke(this);
    }

    protected virtual void TriggerOnTakingDamage(int damageAmount, DamageType damageType, Sim source)
    {
    }

    protected virtual void TriggerOnHit(int damageAmount)
    {
    }

    private void ExecuteScheduledActions()
    {
        if (scheduledActions.Count > 0)
        {
            foreach (ScheduledSimAction scheduledAction in new List<ScheduledSimAction>(scheduledActions))
            {
                if (matchSim.currentTick == scheduledAction.tick)
                {
                    scheduledAction.DoAction();
                    scheduledActions.Remove(scheduledAction);
                }
            }
        }
    }

    protected virtual void ChangeCurrentMana(int amount, bool showText = false)
    {
        if (simStats.silence) return;

        if (amount > 0)
        {
            amount = (int)(amount * simStats.manaGainBoost);
        }
        currentMana += amount;
        if (currentMana < 0) currentMana = 0;
    }

    protected virtual void ResetCurrentMana()
    {
        currentMana = currentMana - simStats.manaPool;
    }

    protected int SimRandom(int min, int max)
    {
        UnityEngine.Random.InitState(randomSeed);
        randomSeed++;
        var randomNum = UnityEngine.Random.Range(min, max);
        return randomNum;
    }

    public void AcquireTarget()
    {
        if (isDead) return;

        currentTarget = null;
        var tauntTarget = simStats.tauntTarget;
        if (tauntTarget != null)
        {
            if (Mathf.Abs(GetPositionIndex() - tauntTarget.GetPositionIndex()) < 2)
            {
                currentTarget = tauntTarget;
                return;
            }
        }
        currentTarget = GetAttackableEnemy();
    }

    public void ApplyManaBurn(int amount)
    {
        if (stats.resourceType != MonsterResourceType.MANA) return;
        currentMana -= amount;
        if (currentMana < 0) currentMana = 0;
    }

    public void ApplyLifeSteal(int damageDealt, DamageType damageType)
    {
        if (simStats.lifeSteal == 0 || damageType == DamageType.TRUE) return;

        var healAmount = (int)(damageDealt * simStats.lifeSteal);
        Heal(healAmount);
    }

    public void ApplyThorns(int amount, Sim source)
    {
        var thornsAmount = simStats.thorns;
        if (thornsAmount > 0)
        {
            var reflectedAmount = (int)(amount * thornsAmount);
            source.TakeDamage(reflectedAmount, DamageType.TRUE, false, null);
        }
    }

    public void TakeDamage(int amount, DamageType damageType, bool isCritical, Sim source)
    {
        if (isRemoved) return;

        TriggerOnTakingDamage(amount, damageType, source);
        if (source != null)
        {
            ApplyThorns(amount, source);
            source.ApplyLifeSteal(amount, damageType);
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            actionLoop.AssignDeathTicks();
            return;

        }
        var percentDamageTaken = (float)amount / (float)simStats.healthPool;
        var randomManaPercent = SimRandom(25, 50) * 0.01f;
        var manaGain = (int)((simStats.manaPool * randomManaPercent) * percentDamageTaken);
        ChangeCurrentMana(manaGain);
    }

    public int CalculateAttackDamage(float amount)
    {
        if (simStats.invulnerable) return 0;
        return (int)(amount - (amount * (simStats.resists / 100.0f)));
    }

    public int CalculateAbilityDamage(float amount)
    {
        if (simStats.invulnerable) return 0;
        return (int)(amount - (amount * (simStats.resists / 100.0f)));
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        var adjustedAmount = (int)(amount * simStats.healingReduction);
        currentHealth += adjustedAmount;
        if (currentHealth > simStats.healthPool)
        {
            currentHealth = simStats.healthPool;
        }
    }

    public int GetPositionIndex()
    {
        if (isDead)
        {
            Debug.LogWarning("GetPositionIndex() called on Sim.isDead == true");
            return -10;
        }

        var monsterList = isPlayerSideA() ? matchSim.playerAMonsterSims : matchSim.playerBMonsterSims;
        var cleanMonsterList = monsterList.Where(x => x.isDead == false).ToList();
        return cleanMonsterList.IndexOf(this);
    }

    protected Sim GetAttackableEnemy()
    {
        var myPositionIndex = GetPositionIndex();
        var targetableEnemies = GetTargetableEnemies();
        if(targetableEnemies.Count >= myPositionIndex + 1)
        {
            return targetableEnemies[myPositionIndex];
        }
        else if(targetableEnemies.Count >= myPositionIndex)
        {
            return targetableEnemies[myPositionIndex - 1];
        }
        else
        {
            return null;
        }
    }

    protected List<Sim> GetTargetableEnemies()
    {
        var enemyMonsterList = isPlayerSideA() ? matchSim.playerBMonsterSims : matchSim.playerAMonsterSims;
        var targetableMonsterList = new List<Sim>();
        foreach (Sim monster in enemyMonsterList)
        {
            if (!monster.isDead && !monster.isRemoved)
            {
                targetableMonsterList.Add(monster);
            }
        }
        return targetableMonsterList;
    }

    public bool isPlayerSideA()
    {
        return playerSide == PlayerSide.A;
    }

    private int CreateMonsterId(int positionIndex, PlayerSide playerSide)
    {
        var a = playerSide == PlayerSide.A ? -1 : 1;
        return (positionIndex + 1) * a;
    }

    public static Sim GetSimByMonsterId(List<Sim> monsterList, int targetId)
    {
        foreach (Sim monster in monsterList)
        {
            if (monster.monsterId == targetId)
            {
                return monster;
            }
        }
        Debug.Log("GetSimByMonsterId() monster doesnt exist in list");
        return null;
    }

    private void UpdateStunStatus()
    {
        if (simStats.stun)
        {
            if (isRemoved) return;
            ShowStun();
        }
        else
        {
            if (isRemoved) return;
            HideStun();
        }
    }

    private void UpdateSilenceStatus()
    {
        if (simStats.silence)
        {
            ShowSilence();
        }
        else
        {
            HideSilence();
        }
    }

    private void UpdatePoisonedStatus()
    {
        if (simStats.poisoned)
        {
            ShowPoison();
        }
        else
        {
            HidePoison();
        }
    }

    protected void ShowPoison()
    {
    }

    protected void HidePoison()
    {
    }

    protected void ShowSilence()
    {
    }

    protected void HideSilence()
    {
    }

    protected void ShowStun()
    {
    }

    protected void HideStun()
    {
    }
}


public enum ActionType
{
    NONE, ATTACK, ABILITY, DEATH
}

public enum DamageType
{
    ATTACK, ABILITY, TRUE
}

public enum StatModifierIcon
{
    SLIME_ABILITY
}

public enum StatModifierType
{
    NEUTRAL,POSITIVE,NEGATIVE
}
