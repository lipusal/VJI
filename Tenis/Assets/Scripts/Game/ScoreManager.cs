using System;

public class ScoreManager
{
    private const int ADVANTAGE = Int32.MaxValue;
    private const int NUM_SETS = 3;
    private static readonly string[] ScoreStrings = {"0", "15", "30", "45", "Ad"};
    
    private int[] _wonPoints = { 0, 0 };        // One per team, for current game
    private int[] _wonGames = { 0, 0 };         // One per team, for current set
    private int[] _wonSets = { 0, 0 };          // Best of NUM_SETS sets wins
    private int[,] gameHistory = new int[NUM_SETS, 2]; // One per set, per team (NUM_SETS x 2)
    public int currentSet = 1;

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
    
    /**
     * When a point is scored. Updates scores and returns true if the given team won a game.
     * teamNumber 0 => player 1 team
     * teamNumber 1 => CPU/player 2 team
     */
    public bool OnPoint(int teamNumber)
    {
        var newWonPoints = ++_wonPoints[teamNumber];
        var opponentWonPoints = _wonPoints[OpponentTeamNumber(teamNumber)];
        if (s(newWonPoints) > 45)
        {
            if (s(opponentWonPoints) == 45)
            {
                // Advantage
            } else if (s(opponentWonPoints) == ADVANTAGE)
            {
                // Break opponent's advantage
                _wonPoints[OpponentTeamNumber(teamNumber)]--;
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
        var newWonGames = ++_wonGames[teamNumber];
        var opponentWonGames = _wonGames[OpponentTeamNumber(teamNumber)];

        // Record won game
        gameHistory[currentSet - 1, teamNumber]++;
        // Reset game points
        _wonPoints = new[] { 0, 0 };
        
        return newWonGames >= 4 && newWonGames - opponentWonGames >= 2;
    }

    public bool OnSet(int teamNumber)
    {
        currentSet++;
        return ++_wonSets[teamNumber] > NUM_SETS / 2;
    }

    private int OpponentTeamNumber(int teamNumber)
    {
        return (teamNumber + 1) % _wonPoints.Length;
    }

    /**
     * Returns a numerical value for a score in the ScoreStrings array. Inverse of u.
     */
    private static int s(int scoreIndex)
    {
        return scoreIndex == 4 ? ADVANTAGE : int.Parse(ScoreStrings[scoreIndex]);
    }

    /**
     * Returns an index in the ScoreStrings array from a numerical value. Inverse of s.
     */
    private static int u(int score)
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
