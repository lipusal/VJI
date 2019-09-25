using System;

public class ScoreManager
{
    private const int ADVANTAGE = Int32.MaxValue;
    private const int NUM_SETS = 3;
    private static String[] scoreStrings = {"0", "15", "30", "45", "Ad"};
    
    private int[] wonPoints = { 0, 0 };    // One per team
    private int[] wonGames = { 0, 0 };     // One per team
    private int[] wonSets = { 0, 0 };      // Best of NUM_SETS sets

    private static ScoreManager _instance;

    private ScoreManager() {}

    public static ScoreManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ScoreManager();
        }
        return _instance;
    }
    
    public bool OnPoint(int teamNumber)
    {
        var newWonPoints = ++wonPoints[teamNumber];
        var opponentWonPoints = wonPoints[opponentTeamNumber(teamNumber)];
        if (s(newWonPoints) > 45)
        {
            if (s(opponentWonPoints) == 45)
            {
                // Advantage
            } else if (s(opponentWonPoints) == ADVANTAGE)
            {
                // Break opponent's advantage
                wonPoints[opponentTeamNumber(teamNumber)]--;
            }
            else
            {
                // Won game
                return true;
            }
        }
        return false;
    }

    public bool OnGame(int teamNumber)
    {
        var newWonGames = ++wonGames[teamNumber];
        var opponentWonGames = wonGames[opponentTeamNumber(teamNumber)];
        
        // Reset game points
        wonPoints = new[] { 0, 0 };
        
        if (newWonGames >= 4 && newWonGames - opponentWonGames >= 2)
        {
            // Won a set
            return true;
        }

        return false;
    }

    public bool OnSet(int teamNumber)
    {
        return ++wonSets[teamNumber] > NUM_SETS / 2;
    }

    public void onWin(int teamNumber)
    {
        // TODO
    }

    private int opponentTeamNumber(int teamNumber)
    {
        return (teamNumber + 1) % wonPoints.Length;
    }

    private int s(int scoreIndex)
    {
        if (scoreIndex == 4)
        {
            return ADVANTAGE;
        }
        else
        {
            return int.Parse(scoreStrings[scoreIndex]);
        }
    }
    
    private int u(int score)
    {
        switch (score)
        {
            case ADVANTAGE:
                return 4;
            case 45:
                return 3;
            case 30:
                return 2;
            case 15:
                return 1;
            case 0:
                return 0;
            default:
                throw new Exception("Unknown score " + score);
        }
    }
}
