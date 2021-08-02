public class MonsterStatModifierVisual
{
    public int stacks;
    public StatModifierIcon icon { get; }

    public MonsterStatModifierVisual(StatModifierIcon icon, int stacks)
    {
        this.stacks = stacks;
        this.icon = icon;
    }
}
