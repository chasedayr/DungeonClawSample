using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<CollectedMonster> playerAMonsters;
    private List<CollectedMonster> playerBMonsters;
    private MatchSim match;

    public void CreateMatch()
    {
        var matchSettings = new MatchSim.Settings(50, 30 * 10, 1337);
        match = new MatchSim(matchSettings, 0, playerAMonsters, 1, playerBMonsters);
        match.Initialize();
        match.Simulate();
    }

    void Update()
    {
    }
}
