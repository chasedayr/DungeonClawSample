public class SimInput
{
    public int randomSeed { get; }
    public MatchSim matchSim { get; }
    public CollectedMonster collectedMonster { get; }
    public PlayerSide playerSide { get; }
    public int positionIndex { get; }

    public SimInput(int randomSeed, MatchSim matchSim, CollectedMonster collectedMonster, PlayerSide playerSide, int positionIndex)
    {
        this.randomSeed = randomSeed;
        this.matchSim = matchSim;
        this.collectedMonster = collectedMonster;
        this.playerSide = playerSide;
        this.positionIndex = positionIndex;
    }
}
