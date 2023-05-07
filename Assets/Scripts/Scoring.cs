using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scoring : MonoBehaviour
{
    [SerializeField] public int playerScore = 0;
    [SerializeField] public string playerName = "";

    [SerializeField] private Text textbox;

    void Start ()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Menu");
    }

    void Update ()
    {
        if (!(SceneManager.GetActiveScene().buildIndex <= 3 || SceneManager.GetActiveScene().buildIndex == 7))
        {
            textbox = GameObject.FindWithTag("Text").GetComponent<Text>();

            textbox.text =
            (
                playerName + "'s Game\nLevel " +
                ((SceneManager.GetActiveScene().buildIndex)-3) +
                "\nScore: " + playerScore
            );
        }
    }

    public void increment (int value)
    {
        playerScore += value; 
    }

    public void reset ()
    {
        playerScore = 0;
    }
}
