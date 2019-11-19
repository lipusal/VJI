using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TenisSet
{
//    private const int MAX_GAMES_PER_SET = 7;
    private const int MAX_GAMES_PER_SET = 1;
    private readonly TenisGame[] _games;
    private readonly int[] _results;
    private TenisGame _currentGame;
    private int _gameNumber;
    private int _servingTeam; // 1 for south, 2 for north

    // 0 if no one has won the game yet, 1 if player one won and 2 if player two won.
    private int _winner;

    public TenisSet()
    {
        _games = new TenisGame[MAX_GAMES_PER_SET + MAX_GAMES_PER_SET - 1];
        _results = new int[2];
        _winner = 0;
        _currentGame = new TenisGame();
        _gameNumber = 0;
        _games[_gameNumber] = _currentGame;
        _servingTeam = 1;

    }

    public int[] GetCurrentResult()
    {
        return _results;
    }

    public int GetWinner()
    {
        return _winner;
    }

    // returns true if game make set end and false if set continues
    private bool AddGame(int playerId)
    {
        if (_winner != 0)
        {
            Debug.Log("El set ya termino");//TODO make exception
   
        }
        else if (playerId != 1 && playerId != 2)
        {
            Debug.Log("No existe el id del jugador para agregar game");//TODO make exception
        }
        else
        {
            _results[playerId - 1]++;
            if (HasWon(playerId))
            {
                _winner = playerId;
                return true;
            }
            
            _servingTeam = (_servingTeam  % 2) + 1;
        }

        return false;
    }

    private bool HasWon(int playerId)
    {
        int otherPlayerId = (playerId % 2) + 1;
        int playerGames = _results[playerId - 1];
        int otherPlayerGames = _results[otherPlayerId - 1];

        if (playerGames == MAX_GAMES_PER_SET - 1 && (playerGames - otherPlayerGames >= 2) || playerGames == MAX_GAMES_PER_SET)
        {
            return true;
        }
        
        return false;
    }

    public bool AddPoint(int playerId, Referee referee)
    {
        ScoreManager.GetInstance().GetReferee().SetServing(true);
        
        if (_currentGame.AddPoint(playerId))
        {
            referee.MakeCelebrateAndAngry(playerId, true);
            int opponentId = (playerId % 2) + 1;
            referee.MakeCelebrateAndAngry(opponentId, false);
            if (AddGame(playerId))
            {
                return true;
            }
            _currentGame = new TenisGame();
            _gameNumber++;
            _games[_gameNumber] = _currentGame;
        }

        return false;
    }

    public int[] GetResults()
    {
        return _results;
    }
    
    public int[] GetCurrentGameResults()
    {
        return _currentGame.GetResults();
    }

    public string[] GetCurrentGameStringResults()
    {
        string[] results = new string[2];
        results[0] = _currentGame.GetTeam1Points();
        results[1] = _currentGame.GetTeam2Points();
        return results;
    }

    public int GetServingTeam()
    {
        return _servingTeam;
    }
}
