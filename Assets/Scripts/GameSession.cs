using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    int score = 0;
    int lifes;
    [SerializeField] Player player;
    [SerializeField] TextMeshProUGUI life;
    LevelManager levelManager;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (player != null && life != null)
        {
            lifes = player.GetLifes();
            life.text = "X " + lifes;
        }
    }

    private void SetSingleton()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int points)
    {
        score += points;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void ReduceLifes()
    {
        lifes--;
        life.text = "X " + lifes;
        if (lifes == 0)
        {
            levelManager.LoadGameOver();
        }
        else
        {
            StartCoroutine(RespawnDelayer());
        }
    }

    IEnumerator RespawnDelayer()
    {
        Debug.Log("Died");
        yield return new WaitForSeconds(3);
        player.Respawn();
        Debug.Log("live");
    }
}
