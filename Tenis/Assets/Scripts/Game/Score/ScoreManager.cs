using System;
using System.Collections.Generic;
using System.Linq;
using FrameLord;
using Game.GameManager;
using Game.Score;
using UnityEngine;

public class ScoreManager
{
    /// <summary>
    /// Maximum number of sets to play. Best of MAX_SETS wins, ie. it is enough to win (MAX_SETS/2) + 1 sets (integer division).
    /// E.g. If MAX_SETS is 3, 2 consecutive sets is enough to win. 3 sets would be played if each player has won 1 set.
    /// </summary>
    private const int MAX_SETS = 3;
//  private const int MAX_SETS = 2;

    private TenisSet[] _sets;
    private TenisSet _currentSet;
    private int _setNumber;
    private int[] _results;
    private Referee _referee;
    private bool _twoPlayers;
    private int _difficulty;
    
    private static ScoreManager _instance;
    private GameManagerLogic _gameManagerLogic;

    private int _winnerId;

    private ScoreManager()
    {
        _results = new int[2];
        _sets = new TenisSet[MAX_SETS];
        _setNumber = 0;
        _currentSet = new TenisSet();
        _sets[_setNumber] = _currentSet;
        _winnerId = 0;
    }

    public static ScoreManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ScoreManager();
        }
        return _instance;
    }

    public void LoadReferee(Vector3 eastCourtSide, Vector3 westCourtSide,
        Vector3 southCourtSide, Vector3 northCourtSide, GameObject southServiceWall,
        GameObject southEastServiceWall, GameObject southWestServiceWall,
        GameObject southMiddleServiceWall, GameObject northServiceWall,
        GameObject northEastServiceWall, GameObject northWestServiceWall, 
        GameObject northMiddleServiceWall,  Vector3 southServiceDelimiter,
        Vector3 eastServiceDelimiter, Vector3 westServiceDelimiter,
        Vector3 northServiceDelimiter, PlayerLogic player1, AIPlayer Aiplayer,
        Player2Logic player2, GameManagerLogic gameManagerLogic)
    {
        _gameManagerLogic = gameManagerLogic;
        _referee = new Referee(eastCourtSide, westCourtSide, southCourtSide, northCourtSide,
                                southServiceWall, southEastServiceWall, southWestServiceWall,
                                southMiddleServiceWall, northServiceWall, northEastServiceWall,
                                northWestServiceWall, northMiddleServiceWall, southServiceDelimiter,
                                eastServiceDelimiter, westServiceDelimiter, northServiceDelimiter,
                                player1, Aiplayer, player2, _twoPlayers, _difficulty);
    }
    
    /**
     * When a point is scored. Updates scores.
     * returns true if team won the game
     */
    public bool OnPoint(int teamNumber)
    {
        if (_referee.GetIsServing() && _referee.GetServiceTimes() == 0)
        {
            _referee.IncreaseServiceTimes();
        }
        else if (_currentSet.AddPoint(teamNumber, _referee))
        {
            // Won set
            _referee.ResetServiceTimes();
            _results[teamNumber - 1]++;
            if (_results[teamNumber - 1] > MAX_SETS / 2)
            {
                // Won match
                AudioManager.Instance.PlaySound((int) SoundId.SOUND_WIN);
                AudioManager.Instance.PlaySound((int) SoundId.SOUND_WOW_CLAP);
                BallLogic.Instance.ResetConfig();
                _winnerId = teamNumber;
                
                return true;
            }
            // Advance to next set
            _currentSet = new TenisSet();
            _setNumber++;
            _sets[_setNumber] = _currentSet;
        }
        else
        {
            // Won point, show score
            _referee.ResetServiceTimes();
            int[] points = _currentSet.GetCurrentGameResults();
            SayScore(points[0], points[1]);
            GetSummarizedScore();
            
            AudioManager.Instance.PlaySound((int) SoundId.SOUND_CLAP);
        }
        
        // Reset ball and server
        BallLogic.Instance.ResetConfig();
       
        _referee.MakePlayerServe(GetServingTeam()); //TODO change to opponent when game

        return false;
    }

    /// <summary>
    /// Play audio of score corresponding to the given points.
    /// </summary>
    private void SayScore(int team1Points, int team2Points)
    {
        if (team1Points == TenisGame.AdvantageIndex || team2Points == TenisGame.AdvantageIndex)
        {
            AudioManager.Instance.PlaySound((int) SoundId.SOUND_ADVANTAGE);
        }
        else if (team1Points != 0 || team2Points != 0) // No callout for 0-0
        {
            if (GetServingTeam() == 1)
            {
                AudioManager.Instance.PlaySound(TenisGameSoundList.GetPointSoundId(team1Points, team2Points));
            }
            else
            {
                AudioManager.Instance.PlaySound(TenisGameSoundList.GetPointSoundId(team2Points, team1Points));

            }
        }
    }

    public void ManageBounce(Vector3 bouncePosition, int hitterId)
    {
        int result = _referee.IsPoint(bouncePosition, hitterId);
        if (result > 0)
        {
            BallLogic.Instance.DeactivateCollisions();
            if (OnPoint(hitterId))
            {
                if (hitterId == 1)
                {
                    Debug.Log("You win");
                    _gameManagerLogic.EndGame(true);
                }
                else
                {
                    Debug.Log("You lose");
                    _gameManagerLogic.EndGame(false);
                }
            }
        }
        else if (result < 0)
        {
            BallLogic.Instance.DeactivateCollisions();
            if (_referee.IsOut(bouncePosition))
            {
                AudioManager.Instance.PlaySound((int) SoundId.SOUND_OUT);
            }
          
            int opponentId = (hitterId % 2) + 1;
            if (OnPoint(opponentId))
            {
                if (opponentId == 1)
                {
                    Debug.Log("You win");
                    _gameManagerLogic.EndGame(true);
                }
                else
                {
                    Debug.Log("You lose");
                    _gameManagerLogic.EndGame(false);
                }
            }
            
        }

        if(result == 0 && hitterId != 0)
        {
            _referee.SetServing(false);
        }
        _referee.ResetHitters();
    }

    public String[] GetSummarizedScore()
    {
        string points1 = _currentSet.GetCurrentGameStringResults()[0];
        string points2 = _currentSet.GetCurrentGameStringResults()[1];
        int games1 = _currentSet.GetResults()[0];
        int games2 = _currentSet.GetResults()[1];
        int sets1 = GetSetsResults()[0];
        int sets2 = GetSetsResults()[1];
        String[] results = new string[2];
        results[0] = $"Player 1: {sets1}  sets {games1} games {points1} points";
        results[1] = $"Player 2: {sets2}  sets {games2} games {points2} points";
      //  Debug.Log($"Player 1: {sets1} sets {games1} games {points1} points" + "\n" + $"Player 2: {sets2} sets {games2} games {points2} points");
        return results;
    }

    public int[] GetCurrentGamescore()
    {
        return _currentSet.GetCurrentGameResults();
    }
    
    public List<string>[] GetScore()
    {
        var currentGamePoints = _currentSet.GetCurrentGameStringResults();
        List<string>[] results = { new List<string>(), new List<string>() };
        results[0].Add(currentGamePoints[0]);
        results[1].Add(currentGamePoints[1]);

        for (int i = 0; i <= _setNumber; i++)
        {
            var set = _sets[i];
            results[0].Add(set.GetResults()[0].ToString());
            results[1].Add(set.GetResults()[1].ToString());
        }
        // Fill remaining sets with zeroes
        for (int i = _setNumber+1; i < MAX_SETS; i++)
        {
            results[0].Add("0");
            results[1].Add("0");
        }
        return results;
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
            resultSide =  Side.RIGHT;
        }
        else
        {
            resultSide = Side.LEFT;
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

    public bool CanPlayerHit(int playerId)
    {
        if (GetServingTeam() == playerId)
        {
            return true;
        }
        return !_referee.GetIsServing();
    }

    public int GetWinnerId()
    {
        return _winnerId;
    }

    public void SetGameMode(bool twoPlayers)
    {
        _twoPlayers = twoPlayers;
    }

    public void SetGameDifficulty(int difficulty)
    {
        _difficulty = difficulty;
    }
}
