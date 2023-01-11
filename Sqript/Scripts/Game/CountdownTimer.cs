using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text textTimer;

    // public bool timerActive = true;

    void SetText(){
        int menit = Mathf.FloorToInt(PlayerPrefs.GetInt("timer") / 60);
        int detik = Mathf.FloorToInt(PlayerPrefs.GetInt("timer") % 60);
        textTimer.text = menit.ToString("00") + ":" + detik.ToString("00");
    }

    float temp;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("timer", 30);
        PlayerPrefs.SetInt("timerActive", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("timerActive") == 1){
            temp += Time.deltaTime;
            if(temp >= 1){
                PlayerPrefs.SetInt("timer", PlayerPrefs.GetInt("timer")-1);
                temp = 0;
            }
        }
        if(PlayerPrefs.GetInt("timerActive") == 1 && PlayerPrefs.GetInt("timer") <=0){
            Debug.Log("game over");
            PlayerPrefs.SetInt("timerActive", 0);
        }
        SetText();
    }
}
