# DungeonClawSample
Sample code from my mobile game Dungeon Claw

To see the full game you can find it here:

[Google Play Link](https://play.google.com/store/apps/details?id=com.DontGoChase.DungeonClaw)

[App Store Link](https://apps.apple.com/us/app/dungeon-claw-auto-battler/id1503115278)

# Gameplay Summary
### Dungeon Claw is an 8 player pvp auto battler game.  The game consists of two alternating stages of gameplay: 

1. A timed claw machine game mode that allows the player to pick up and collect monsters and items from a claw machine.  The player can strategically organize their collected objects before they move to the next stage.

2. A 1v1 match against one of the 8 other players in the form of an "auto battle".  The player's selected monsters will fight and use their special abilities on their own.  The player can influence the outcome of the auto battle through the use of items.


# Code Summary
### The code sampled here highlights the monster classes and the auto battle classes.  Some code has been removed or simplified for the sample.

**MonsterInfo.cs**

The base abstract monster class.  This class declares abstract values such as: Stats, Name, MonsterType


**SlimeMonsterInfo.cs**

An example of how a monster inherits from MonsterInfo.cs


**MonsterInfoProcessor.cs**

A very useful class that accesses classes that inherit from MonsterInfo through the use of reflection and enum MonsterType.  
Example usage:
```
string slimeDisplayName = MonsterInfoProcessor.GetDisplayName(MonsterType.Slime);
```


**CollectedMonster.cs SlimeMonsterInfo.cs MonsterBaseStats.cs MonsterTickStats.cs MonsterStats.cs SimStats.cs**

In order to give myself flexibility with monster stat interactions I separated the monster's stats into several classes.  For example, the Slime monster's ability is based on the enemy monster's base stats before any modifications.

**CollectedMonster.cs**

A representation of a player's selected monster that can be used in a match.

**SlimeMonsterInfo.cs**

Initializes the values for MonsterBaseStats.cs, MonsterTickStats.cs, and MonsterStats.cs for a Slime monster.

**MonsterBaseStats.cs**

Monster base stats before any modifiers: health, damage, attackspeed, resists, critical

**MonsterTickStats.cs**

Tick stats are values used to represent the duration of actions such as: using an ability, using an attack, dying.  These values are derived from each monster's animation lengths.

**MonsterStats.cs**

Monster stats modified by monster level.

**MonsterStatEffects.cs**

Monster stats modified by various effects throughout a match.

**SimStats.cs**

Final monster stats used in a match by combining all of the above.

**GameManager.cs**

Very simple implementation of creating a match.

**MatchSim.cs**

Takes two player's "loadouts" and allows them to fight.  Combat in this game is made deterministic through the use of a random seed and "ticks" instead of time.  This gives me the ability to: rewind, fast forward, reconnect disconnected players, instantly simulate match outcomes.  In this code sample the match is executed instantly in a for loop.

*sidenote*: Before I would add new content to the live version of the game I was able to simulate thousands of matches to determine if my changes would be overpowered or underpowered.

**Sim.cs**

The base abstract class for a monster that will battle in a match.  Contains all of the base logic for a monster's actions in a match.  

**SlimeSim.cs**

Example usage of inheritance of Sim.cs.  The Slime overrides ExecuteAbility() and InitiateAbility() in order to implement the Slime's ability logic.
