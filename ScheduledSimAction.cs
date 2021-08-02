using System;

public class ScheduledSimAction
{
    public int tick { get; }
    public bool canOverlap { get; }
    private Action _action;

    public ScheduledSimAction(int tick, Action action, bool canOverlap = false)
    {
        this.tick = tick;
        this.canOverlap = canOverlap;
        _action = action;
    }

    public bool Compare(ScheduledSimAction scheduledAction)
    {
        return scheduledAction._action == _action;
    }

    public void DoAction()
    {
        _action();
    }
}
