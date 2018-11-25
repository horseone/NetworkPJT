using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class lightMapManager : MonoBehaviour
{
    public bool night;

    public float time = 60;
    public float changeTime = 60;
    public float voteTime = 15;

    private void Awake()
    {
        SceneManager.LoadScene("DayScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("NightScene", LoadSceneMode.Additive);
    }

    private void Start()
    {
        SceneManager.UnloadSceneAsync("NightScene");
        night = false;
    }

    void Update()
    {
        DayNight();
        //execution();
    }

    void DayNight()
    {
        time -= Time.deltaTime;
        //Debug.Log(time);
        if (!night)
        {
            if (time <= 0) //if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("밤 씬 이동");
                night = !night;
                SceneManager.LoadScene("NightScene", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("DayScene");
                time = changeTime;
            }
        }
        else if (night)
        {
            if (time <= 0) //if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("낮 씬 이동");
                night = !night;
                SceneManager.LoadScene("DayScene", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("NightScene");
                time = changeTime;
            }
        }
    }

    /*void execution()
    {
        if (!night)
        {
            if (Input.GetButtonDown("Fire2"))
            {

            }
        }
    }*/
}
