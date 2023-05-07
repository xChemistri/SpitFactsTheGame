using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioClip click;

    GameObject scorer;
    GameObject[] eIP;
    GameObject[] eIR;

    [SerializeField] Text text;
    [SerializeField] Text pl;
    private int index;

    void Start()
    {
        eIP = GameObject.FindGameObjectsWithTag("EnabledInPause");
        eIR = GameObject.FindGameObjectsWithTag("EnabledInResume");

        scorer = GameObject.FindWithTag("Scorekeeper");

        foreach (GameObject obj in eIR)
            obj.SetActive(true);
        foreach (GameObject obj in eIP)
            obj.SetActive(false);

        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 4 || index == 5 || index == 6)
            AudioSource.PlayClipAtPoint
            (
                audioClip,
                new Vector3(0f,0f,-10f),
                PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
            );

        if (index == 3)
        {
            Slider slideee= GameObject.FindWithTag("Slider").GetComponent<Slider>();
            slideee.value = PlayerPrefs.GetFloat("GAME_VOLUME",0.5f);

            pl.text = scorer.GetComponent<Scoring>().playerName;
        }
    }

    void Update()
    {
        
    }

    private void Click ()
    {
        AudioSource.PlayClipAtPoint
        (
            click,
            new Vector3(0f,0f,-10f),
            PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
        );
    }

    public void StartGame ()
    {
        SceneManager.LoadScene("Level1");
        scorer.GetComponent<Scoring>().reset();
    }

    public void Instructions ()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void BackHome ()
    {
        SceneManager.LoadScene ("Menu");

        Time.timeScale = 1.0f;
    }

    public void Scores ()
    {
        SceneManager.LoadScene("Scores");
    }

    public void Settings ()
    {
        SceneManager.LoadScene("Settings");
    }

    public void VolumeCtrl ()
    {
        float value = GameObject.FindWithTag("Slider").GetComponent<Slider>().value;

        PlayerPrefs.SetFloat("GAME_VOLUME", value);
    }

    public void ChangeName ()
    {
        scorer.GetComponent<Scoring>().playerName = text.text;
    }

    public void TestSound ()
    {
        AudioSource.PlayClipAtPoint
        (
            audioClip,
            new Vector3(0f,0f,-10f),
            PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
        );
    }

    public void PauseResume (bool isPaused)
    {
        Click();
        
        if (isPaused)
        {
            Time.timeScale = 1.0f;

            foreach (GameObject obj in eIR)
                obj.SetActive(true);
            foreach (GameObject obj in eIP)
                obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in eIP)
                obj.SetActive(true);
            foreach (GameObject obj in eIR)
                obj.SetActive(false);

            Time.timeScale = 0.0f;
        }
    }
}
