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
    private int maxSets = 3;

    private TenisSet[] _sets;
    private TenisSet _currentSet;
    private int _setNumber;
    private int[] _results;
    private Referee _referee;
    
    private static ScoreManager _instance;
    private GameManagerLogic _gameManagerLogic;

    private int _winnerId;

    private ScoreManager()
    {
        _results = new int[2];
        _sets = new TenisSet[maxSets];
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
        Vector3 northServiceDelimiter, PlayerLogic player1, AIPlayer player2,
        GameManagerLogic gameManagerLogic)
    {
        _gameManagerLogic = gameManagerLogic;
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
            // Won set
            _results[teamNumber - 1]++;
            if (_results[teamNumber - 1] > maxSets/2)
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
            int[] points = _currentSet.GetCurrentGameResults();
            SayScore(points[0], points[1]);
            AudioManager.Instance.PlaySound((int) SoundId.SOUND_CLAP);
        }
        
        // Reset ball and server
        BallLogic.Instance.ResetConfig();
        _referee.MakePlayerServe(1); //TODO change to opponent when game
        TriggerCallout();

        return false;
    }

    /// <summary>
    ///  Triggers the callout UI panel with the new score, or special game conditions (ie. match point).
    /// </summary>
    private void TriggerCallout()
    {
        // TODO NOW check if in special condition. If so, call TriggerCallout with the correct message. If not, don't do anything.
        
        throw new NotImplementedException();
    }
    
    private void TriggerCallout(string message)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if the current match score is in game point condition.
    /// </summary>
    /// <param name="currentGamePoints">Current game points</param>
    /// <returns>1 if team 1 is on game point, 2 if team 2 is on game point, 0 otherwise.</returns>
    private int IsGamePoint(int[] currentGamePoints)
    {
        int team1Score = currentGamePoints[0], team2Score = currentGamePoints[1];
        int difference = Math.Abs(team1Score - team2Score);
        if (difference >= 1)
        {
            if (team1Score >= 3 && team1Score > team2Score)
            {
                return 1;
            }
            else if (team2Score >= 3 && team2Score > team1Score)
            {
                return 2;
            }
        }

        return 0;
    }
    
    /// <summary>
    /// Checks if the current score is in set point condition.
    /// </summary>
    /// <param name="currentGamePoints">Current game points</param>
    /// <param name="games">Current set games per team</param>
    /// <returns>1 if team 1 is on set point, 2 if team 2 is on set point, 0 otherwise.</returns>
    private int IsSetPoint(int[] currentGamePoints, int[] games)
    {
        int gamePointTeam = IsGamePoint(currentGamePoints);
        if (gamePointTeam == 0)
        {
            return 0;
        }

        int possibleWinnerGames = 0, otherPlayerGames = 0;

        if (gamePointTeam == 1)
        {
            possibleWinnerGames = games[0] + 1;
            otherPlayerGames = games[1];
        }
        else
        {
            possibleWinnerGames = games[1] + 1;
            otherPlayerGames = games[0];
        }
        if (possibleWinnerGames == TenisSet.MAX_GAMES_PER_SET - 1 && (possibleWinnerGames - otherPlayerGames >= 2) ||
            possibleWinnerGames == TenisSet.MAX_GAMES_PER_SET)
        {
            return gamePointTeam;
        }
        
        return 0;
    }

    private int IsMatchPoint(int[] currentGamePoints, int[] games)
    {
        int possibleWinnerTeam = IsSetPoint(currentGamePoints, games);
        int possibleWinnerSets = 0;
        if (possibleWinnerTeam == 0)
        {
            return 0;
        }
        else if (possibleWinnerTeam == 1)
        {
            possibleWinnerSets = _currentSet.GetResults()[0];
        }
        else
        {
            possibleWinnerSets = _currentSet.GetResults()[1];

        }

        if (possibleWinnerSets > maxSets / 2)
        {
            return possibleWinnerTeam;
        }

        return 0;
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
            AudioManager.Instance.PlaySound(TenisGameSoundList.GetPointSoundId(team1Points, team2Points));
        }
    }

    public void ManageBounce(Vector3 bouncePosition, int hitterId)
    {
        int result = _referee.IsPoint(bouncePosition, hitterId);
        if (result > 0)
        {
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

    public string[] GetPartialResults()
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
    
    public string[] GetFullResults()
    {
        var player1Result = new int[maxSets];
        var player2Result = new int[maxSets];
        for (int i = 0; i < maxSets; i++)
        {
            var set = _sets[i];
            player1Result[i] = set?.GetResult()[0] ?? 0;
            player2Result[i] = set?.GetResult()[1] ?? 0;
        }

        return new[] {
            string.Join(" ", player1Result),
            string.Join(" ", player2Result)
        };
    }

    public string[] GetCurrentGameResults()
    {
        return _currentSet.GetCurrentGameStringResults();
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
    public int MaxSets
    {
        get => maxSets;
        set => maxSets = value;
    }

    public static void Reset()
    {
        _instance = new ScoreManager();
    }
}
