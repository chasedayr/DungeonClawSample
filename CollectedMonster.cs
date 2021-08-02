public class CollectedMonster
{
    public int level { get; }
    public MonsterStats stats { get; }
    public MonsterType monsterType { get; }

    public CollectedMonster(int level, MonsterType monsterType)
    {
        this.level = level;
        this.monsterType = monsterType;
        stats = MonsterInfoProcessor.GetMonsterStats(monsterType, level);
    }
}
