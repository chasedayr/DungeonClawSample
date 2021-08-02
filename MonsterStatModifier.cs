public class MonsterStatModifier
{
    private static int idCounter;

    public MonsterStatModifierVisual visual { get; }
    public StatModifierType modifierType { get; }

    public int startTick { get; }
    public int endTick { get; private set; }
    public int id { get; }

    public MonsterBaseStats stats { get; }
    public MonsterStatEffects statEffects { get; }

    public MonsterStatModifier(int startTick, int endTick, MonsterBaseStats stats,
        MonsterStatEffects statEffects, StatModifierType modifierType)
    {
        this.startTick = startTick;
        this.endTick = endTick;
        this.stats = stats;
        this.statEffects = statEffects;
        this.modifierType = modifierType;
        visual = null;
        id = idCounter;
        idCounter++;
    }
    public MonsterStatModifier(int startTick, int endTick, MonsterBaseStats stats,
        MonsterStatEffects statEffects, StatModifierType modifierType, StatModifierIcon icon)
    {
        this.startTick = startTick;
        this.endTick = endTick;
        this.stats = stats;
        this.statEffects = statEffects;
        this.modifierType = modifierType;
        visual = new MonsterStatModifierVisual(icon, 1);
        id = idCounter;
        idCounter++;
    }

    public MonsterStatModifier(int startTick, int endTick, MonsterBaseStats stats,
        MonsterStatEffects statEffects, StatModifierType modifierType, StatModifierIcon icon, int stacks)
    {
        this.startTick = startTick;
        this.endTick = endTick;
        this.stats = stats;
        this.statEffects = statEffects;
        this.modifierType = modifierType;
        visual = new MonsterStatModifierVisual(icon, stacks);
        id = idCounter;
        idCounter++;
    }

    public bool isCrowdControl()
    {
        return (statEffects.silence > 0 || statEffects.stun > 0 || statEffects.confused > 0 || statEffects.tauntTarget != null);
    }

    public void AdjustForCCModifier(float modifier)
    {
        if (endTick != -1 && modifier != 1.0f)
        {
            if (isCrowdControl())
            {
                var effectDuration = endTick - startTick;
                var newEffectDuration = (int)(effectDuration * modifier);
                endTick = startTick + newEffectDuration;
            }
        }
    }
}
