
using System.Collections.Generic;
using UnityEngine;

public class MatchSim
{
    public class Settings
    {
        public int overTimeDamageAmount { get; }
        public int overTimeDamageTick { get; }
        public int randomSeed { get; }

        public Settings(int overTimeDamageAmount, int overTimeDamageTick, int randomSeed)
        {
            this.overTimeDamageAmount = overTimeDamageAmount;
            this.overTimeDamageTick = overTimeDamageTick;
            this.randomSeed = randomSeed;
        }
    }

    public List<Sim> playerAMonsterSims { get; } = new List<Sim>();
    public List<Sim> playerBMonsterSims { get; } = new List<Sim>();
    public int currentTick { get; private set; }

    private bool _isInitialized;
    private Settings _settings;
    private int _playerAId;
    private int _playerBId;
    private List<CollectedMonster> _playerACollection = new List<CollectedMonster>();
    private List<CollectedMonster> _playerBCollection = new List<CollectedMonster>();
    private List<Sim> _deadMonsters = new List<Sim>();
    private int _overTimeDamage;
    private int _overTimeTick;

    public delegate void StartMatch();
    public static event StartMatch StartMatchEvent;
    public delegate void EndMatch();
    public static event EndMatch EndMatchEvent;
    public delegate void MatchTick();
    public static event MatchTick MatchTickEvent;
        

    public MatchSim(Settings settings, int playerAId, List<CollectedMonster> playerAMonsters, int playerBId, List<CollectedMonster> playerBMonsters)
    {
        _settings = settings;
        _playerAId = playerAId;
        _playerBId = playerBId;
        _playerACollection = playerAMonsters;
        _playerBCollection = playerBMonsters;
        _overTimeDamage = settings.overTimeDamageAmount;
        _overTimeTick = settings.overTimeDamageTick;
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("MatchSim already intialized");
            return;
        }

        int monsterPositionIndex = 0;
        int simRandomSeed = _settings.randomSeed;
        foreach (CollectedMonster collectedMonster in _playerACollection)
        {
            var newSimInput = new SimInput(simRandomSeed, this, collectedMonster, PlayerSide.A, monsterPositionIndex);
            var newMonsterSim = MonsterInfoProcessor.CreateSim(collectedMonster.monsterType, newSimInput);
            playerAMonsterSims.Add(newMonsterSim);
            simRandomSeed++;
            monsterPositionIndex++;
        }
        monsterPositionIndex = 0;
        foreach (CollectedMonster collectedMonster in _playerBCollection)
        {
            var newSimInput = new SimInput(simRandomSeed, this, collectedMonster, PlayerSide.B, monsterPositionIndex);
            var newMonsterSim = MonsterInfoProcessor.CreateSim(collectedMonster.monsterType, newSimInput);
            playerBMonsterSims.Add(newMonsterSim);
            simRandomSeed++;
            monsterPositionIndex++;
        }
        PreMatchActions();
        Simulate();
    }

    public void Simulate()
    {
        if(!_isInitialized)
        {
            Debug.LogWarning("not initialized");
            return;
        }

        var maximumSimulateTime = 45.0;
        var maximumSimulateTicks = (int)(maximumSimulateTime * 10.0);
        for (currentTick = 0; currentTick < maximumSimulateTicks; currentTick++)
        {
            UpdateDeadMonsters();
            if (_playerACollection.Count == 0 || _playerBCollection.Count == 0)
            {
                break;
            }
            if (currentTick == _settings.overTimeDamageTick)
            {
                OverTimeDamage();
            }
            MatchTickEvent?.Invoke();
        }
        PostMatchActions();
    }

    private void PreMatchActions()
    {
        Sim.MonsterDeathEvent += AddDeadMonster;
        StartMatchEvent?.Invoke();
    }

    private void PostMatchActions()
    {
        EndMatchEvent?.Invoke();
        Sim.MonsterDeathEvent -= AddDeadMonster;
    }

    private void OverTimeDamage()
    {
        foreach (Sim monster in playerAMonsterSims)
        {
            monster.TakeDamage(_settings.overTimeDamageAmount, DamageType.TRUE, false, null);
        }
        foreach (Sim monster in playerBMonsterSims)
        {
            monster.TakeDamage(_settings.overTimeDamageAmount, DamageType.TRUE, false, null);
        }
        _overTimeDamage += 50;
        _overTimeTick += 10;
    }

    private void AddDeadMonster(Sim deadMonster)
    {
        _deadMonsters.Add(deadMonster);
    }

    private void UpdateDeadMonsters()
    {
        if (_deadMonsters.Count > 0)
        {
            foreach (Sim monster in playerAMonsterSims)
            {
                monster.AcquireTarget();
            }
            foreach (Sim monster in playerBMonsterSims)
            {
                monster.AcquireTarget();
            }
            foreach (Sim deadMonster in _deadMonsters)
            {
                if (deadMonster.isPlayerSideA())
                {
                    playerAMonsterSims.Remove(deadMonster);
                }
                else
                {
                    playerBMonsterSims.Remove(deadMonster);
                }
            }
            _deadMonsters = new List<Sim>();
        }
    }

}

public enum PlayerSide { A, B };
