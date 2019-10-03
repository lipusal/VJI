using System;
using Game.Score;
using UnityEngine;

public class ScoreManager
{
    private const int ADVANTAGE = Int32.MaxValue;
    private const int NUM_SETS = 2;
    private static readonly string[] ScoreStrings = {"0", "15", "30", "40", "Ad"};

    private Set[] _sets;
    private Set _currentSet;
    private int _setNumber;
    private int[] _results;
    private Referee _referee;

    
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

    public void loadReferee(Vector3 eastCourtSide, Vector3 westCourtSide,
        Vector3 southCourtSide, Vector3 northCourtSide, Vector3 southServiceWall,
        Vector3 southEastServiceWall, Vector3 southWestServiceWall,
        Vector3 southMiddleServiceWall, Vector3 northServiceWall,
        Vector3 northEastServiceWall, Vector3 northWestServiceWall, 
        Vector3 northMiddleServiceWall,  Vector3 southServiceDelimiter,
        Vector3 eastServiceDelimiter, Vector3 westServiceDelimiter,
        Vector3 northServiceDelimiter, PlayerLogic player1, AIPlayer player2)
    {
        _referee = new Referee(eastCourtSide, westCourtSide, southCourtSide, northCourtSide,
                                southServiceWall, southEastServiceWall, southWestServiceWall,
                                southMiddleServiceWall, northServiceWall, northEastServiceWall,
                                northWestServiceWall, northMiddleServiceWall, southServiceDelimiter,
                                eastServiceDelimiter, westServiceDelimiter, northServiceDelimiter,
                                player1, player2);
    }
    
    /**
     * When a point is scored. Updates scores.
     * returns true if team won the game
     */
    public bool OnPoint(int teamNumber)
    {
        if (_currentSet.AddPoint(teamNumber))
        {
            _results[teamNumber - 1]++;
            if (_results[teamNumber - 1] == NUM_SETS)
            {
                return true;
            }
            _currentSet = new Set();
            _setNumber++;
            _sets[_setNumber] = _currentSet;
        }

        return false;
    }

    public void manageBounce(Vector3 bouncePosition, int hitterId)
    {

        int result = _referee.isPoint(bouncePosition, hitterId);
        if (result > 0)
        {
            if (OnPoint(hitterId))
            {
                //TODO match finished
            }
        }
        else if (result < 0)
        {
            int opponentId = (hitterId % 2) + 1;
            if (OnPoint(opponentId))
            {
                //TODO match finished

            }
        }

        if (result != 0)
        {
            ShowPartialResults();
            _referee.SetServing(true);
            BallLogic.Instance.ResetConfig();
            _referee.MakePlayerServe(hitterId); //TODO change to opponent when game

        }
        else if(hitterId != 0)
        {
            _referee.SetServing(false);
        }
    }

    private void ShowPartialResults()
    {
        string points1 = _currentSet.GetCurrentGameStringResults()[0];
        string points2 = _currentSet.GetCurrentGameStringResults()[1];
        int games1 = _currentSet.GetCurrentSetResults()[0];
        int games2 = _currentSet.GetCurrentSetResults()[1];
        int sets1 = GetSetsResults()[0];
        int sets2 = GetSetsResults()[1];
        

        Debug.Log("player1: " + sets1 + " " + games1 + " " + points1 + "\n" + "player2 " + sets2 + " " + games2 + " " + points2 + "\n" );
    }

    private int[] GetSetsResults()
    {
        int[] result = new int[2];
        for (int i = 0; i < _setNumber; i++)
        {
            if (_sets[i].GetWinner() == 1)
            {
                result[0]++;
            }
            else
            {
                result[1]++;
            }
        }

        return result;
    }

    public int GetServingTeam()
    {
        return _currentSet.GetServingTeam();
    }

    // returns 0 if serving side is right and 1 if serving side is left
    public Side GetServingSide()
    {
        int servingTeam = GetServingTeam(); // 1 for south,2 for north
        int[] results = _currentSet.GetCurrentGameResults();
        Side resultSide;
        
        if ((results[0] + results[1]) % 2 == 0)
        {
            resultSide = servingTeam == 1 ? Side.RIGHT : Side.LEFT;
        }
        else
        {
            resultSide = servingTeam == 1 ? Side.LEFT : Side.RIGHT;
        }

        return resultSide;
    }

    public void UpdateLastHitter(int hitter)
    {
        _referee.UpdateLastHitter(hitter);
    }

    public Referee GetReferee()
    {
        return _referee;
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

    public void ActivateServingWalls(int id)
    {
        _referee.ActivateServingWalls(id);
    }
}
