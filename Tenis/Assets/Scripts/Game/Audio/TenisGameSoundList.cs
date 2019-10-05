// FrameLord

using System;
using System.Collections.Generic;
using FrameLord;

public enum SoundId : int
{
    SOUND_HIT = 0,
    SOUND_SERVE = 1,
    SOUND_STEPS = 2,
    SOUND_BOUNCE = 3,
    SOUND_CLAP = 4,
    SOUND_OUT = 5,
    SOUND_WALL = 6,
    SOUND_NET = 7,
    SOUND_ADVANTAGE = 7,
    SOUND_WIN = 8,
    SOUND_NET_1 = 9,  // Net on first service
    SOUND_NET_2 = 10, // Net on second service
    SOUND_WOW_CLAP = 11,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class TenisGameSoundList : SoundList
{
    private static readonly SoundProp[] Sounds =
    {
        new SoundProp((int) SoundId.SOUND_HIT, "HitSound", 2, 100),
        new SoundProp((int) SoundId.SOUND_SERVE, "ServeSound", 2, 100),
        new SoundProp((int) SoundId.SOUND_STEPS, "StepsSound", 1, 50),
        new SoundProp((int) SoundId.SOUND_BOUNCE, "BallBounceSound", 1, 100),
        new SoundProp((int) SoundId.SOUND_CLAP, "Clap", 1, 50),
        new SoundProp((int) SoundId.SOUND_WOW_CLAP, "WowClap", 1, 50),
        new SoundProp((int) SoundId.SOUND_OUT, "Out", 1, 100),
        new SoundProp((int) SoundId.SOUND_WALL, "WallSound", 1, 100),
        new SoundProp((int) SoundId.SOUND_NET, "NetSound", 1, 100),
        new SoundProp((int) SoundId.SOUND_ADVANTAGE, "Advantage", 1, 100),
        new SoundProp((int) SoundId.SOUND_WIN, "Game Set and Match", 1, 100),
        new SoundProp((int) SoundId.SOUND_NET_1, "Net-1", 1, 100),
        new SoundProp((int) SoundId.SOUND_NET_2, "Net-2", 1, 100),
        // Point sounds in GetSoundProps
    };

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Get the sound ID corresponding to the given score.
    /// </summary>
    public static int GetPointSoundId(int team1Score, int team2Score)
    {
        if (team1Score < 0 || team1Score > 3 || team2Score < 0 || team2Score > 3 || (team1Score == 0 && team2Score == 0))
        {
            throw new ArgumentException($"Invalid scores, should be between 0 and 3 and both scores can NOT be 0. Team 1 = {team1Score}, team 2 = {team2Score}");
        }
        return Sounds.Length - 1 // Point sounds start after other sounds
               + (team1Score*4)  // Between each change of team 1 points there are 3 or 4 points for team 2 (3 for team1 = 0, 4 otherwise. Since 3*0 == 4*0 == 0 we don't differentiate that case)
               + team2Score;
    }

    protected override SoundProp[] GetSoundProps()
    {
        var result = new List<SoundProp>();
        result.AddRange(Sounds);
        
        // Add all point sounds, excluding advantage
        for (int team1Score = 0; team1Score < TenisGame.AdvantageIndex; team1Score++)
        {
            for (int team2Score = 0; team2Score < TenisGame.AdvantageIndex; team2Score++)
            {
                if (team1Score == 0 && team2Score == 0)
                {
                    continue;
                }
                // 0-padded scores with 2 digits always
                String team1PrettyScore = TenisGame.PointStrings[team1Score].PadLeft(2, '0'),
                       team2PrettyScore = TenisGame.PointStrings[team2Score].PadLeft(2, '0'),
                       scoreName = $"{team1PrettyScore}-{team2PrettyScore}";
                result.Add(new SoundProp(scoreName.GetHashCode(), scoreName, 1, 100));
            }            
        }
        
        return result.ToArray();
    }
    
}