using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreHandler : MonoBehaviour
{
    private Scoring keeper;
    [SerializeField] Text text;

    private bool newScore = false;
    private int index = 0;

    void Start()
    {
        keeper = GameObject.FindWithTag("Scorekeeper").GetComponent<Scoring>();

        int CurrentScore = keeper.playerScore;
        string CurrentPlayer = keeper.playerName;

        if (!PlayerPrefs.HasKey("INITIALIZED"))
        {
            for (int i = 1; i <= 5; i++)
            {
                PlayerPrefs.SetString((string)("HS_NAME"+i), "???");
                PlayerPrefs.SetInt((string)("HS_SCORE"+i), 0);
            }

            PlayerPrefs.SetInt("INITIALIZED", 1);

        }
    
        for (int i = 1; i <= 5; i++)
        {
            if (PlayerPrefs.GetInt((string)("HS_SCORE"+i)) < CurrentScore)
            {
                if (index == 0)
                {
                    index = i;
                    newScore = true;

                    for (int j = 4; j >= i; j--)
                    {
                        PlayerPrefs.SetString
                        (
                            (string)("HS_NAME"+(j+1)),
                            PlayerPrefs.GetString((string)("HS_NAME"+j))
                        );

                        PlayerPrefs.SetInt
                        (
                            (string)("HS_SCORE"+(j+1)),
                            PlayerPrefs.GetInt((string)("HS_SCORE"+j))
                        );
                    }
                }
            }
        }

        if (newScore)
        {
            PlayerPrefs.SetString((string)("HS_NAME"+index), CurrentPlayer);
            PlayerPrefs.SetInt((string)("HS_SCORE"+index), CurrentScore);
            PlayerPrefs.Save();
        }

        text.text = "";

        for (int i = 1; i <= 5; i++)
        {
            text.text = text.text + PlayerPrefs.GetString((string)("HS_NAME"+i)) + " - ";
            text.text = text.text + PlayerPrefs.GetInt((string)("HS_SCORE"+i)) + "\n";
        }

        keeper.reset();
    }

    void Update()
    {
    }
}
