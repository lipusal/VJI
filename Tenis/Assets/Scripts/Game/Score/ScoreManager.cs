using System;

public class ScoreManager
{
    private const int ADVANTAGE = Int32.MaxValue;
    private const int NUM_SETS = 2;
    private static readonly string[] ScoreStrings = {"0", "15", "30", "40", "Ad"};

    private Set[] _sets;
    private Set _currentSet;
    private int _setNumber;
    private int[] _results;

    
//    private int[] _wonPoints = { 0, 0 };        // One per team, for current game
//    private int[] _wonGames = { 0, 0 };         // One per team, for current set
//    private int[] _wonSets = { 0, 0 };          // Best of NUM_SETS sets wins
//    private int[,] gameHistory = new int[NUM_SETS, 2]; // One per set, per team (NUM_SETS x 2)

    private static ScoreManager _instance;

    private ScoreManager()
    {
        _results = new int[2];
        _sets = new Set[NUM_SETS + NUM_SETS - 1];
        _setNumber = 0;
        _currentSet = new Set();
        _sets[_setNumber] = _currentSet;
    }

    public static ScoreManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ScoreManager();
        }
        return _instance;
    }
    
    /**
     * When a point is scored. Updates scores.
     * returns true if team won the game
     */
    public bool OnPoint(int teamNumber)
    {
        if (_currentSet.AddPoint(teamNumber))
        {
            _results[teamNumber]++;
            if (_results[teamNumber] == NUM_SETS)
            {
                return true;
            }
            _currentSet = new Set();
            _setNumber++;
            _sets[_setNumber] = _currentSet;
        }

        return false;
    }

//    /**
//     * Returns a numerical value for a score in the ScoreStrings array. Inverse of u.
//     */
//    private static int s(int scoreIndex)
//    {
//        return scoreIndex == 4 ? ADVANTAGE : int.Parse(ScoreStrings[scoreIndex]);
//    }
//
    /**
     * Returns an index in the ScoreStrings array from a numerical value. Inverse of s.
     */
//    private static int u(int score)
//    {
//        switch (score)
//        {
//            case ADVANTAGE:
//                return 4;
//            case 45:
//                return 3;
//            case 30:
//                return 2;
//            case 15:
//                return 1;
//            case 0:
//                return 0;
//            default:
//                throw new Exception("Unknown score " + score);
//        }
//    }
    
}
