using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public class MonsterInfoProcessor
{
    private static Dictionary<MonsterType, MonsterInfo> _monstersTypeDict = new Dictionary<MonsterType, MonsterInfo>();
    private static bool _initialized;
    private static void Initialize()
    {
        _monstersTypeDict.Clear();
        var assembly = Assembly.GetAssembly(typeof(MonsterInfo));
        var allMonsterInfoTypes = assembly.GetTypes()
            .Where(t => typeof(MonsterInfo).IsAssignableFrom(t) && t.IsAbstract == false);
        foreach (var monsterInfoType in allMonsterInfoTypes)
        {
            MonsterInfo monsterInfo = Activator.CreateInstance(monsterInfoType) as MonsterInfo;
            _monstersTypeDict.Add(monsterInfo.monsterType, monsterInfo);
        }
        _initialized = true;
    }

    private static MonsterInfo GetMonsterInfo(MonsterType monsterType)
    {
        if (!_initialized)
        {
            Initialize();
        }
        return _monstersTypeDict[monsterType];
    }

    public static Sim CreateSim(MonsterType monsterType, SimInput input)
    {
        return GetMonsterInfo(monsterType).CreateSim(input);
    }

    public static MonsterBaseStats GetMonsterBaseStats(MonsterType monsterType)
    {
        return GetMonsterInfo(monsterType).baseStats;
    }

    public static MonsterTickStats GetMonsterTickStats(MonsterType monsterType)
    {
        return GetMonsterInfo(monsterType).tickStats;
    }

    public static MonsterStats GetMonsterStats(MonsterType monsterType, int level = 1)
    {
        return GetMonsterInfo(monsterType).GetMonsterStats(level);
    }

    public static string GetDisplayName(MonsterType monsterType)
    {
        return GetMonsterInfo(monsterType).displayName;
    }

    public static string GetDescription(MonsterType monsterType)
    {
        return GetMonsterInfo(monsterType).GetDescription();
    }
}