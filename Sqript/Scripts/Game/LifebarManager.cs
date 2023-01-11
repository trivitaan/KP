using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifebarManager : MonoBehaviour
{
    public GameObject heart1, heart2, heart3;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("hp", 3);
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        int getHP = PlayerPrefs.GetInt("hp");
        switch (getHP)
        {
            case 3:   
                    heart1.gameObject.SetActive(true);
                    heart2.gameObject.SetActive(true);
                    heart3.gameObject.SetActive(true);
                    break; 
            case 2:   
                    heart1.gameObject.SetActive(true);
                    heart2.gameObject.SetActive(true);
                    heart3.gameObject.SetActive(false);
                    break; 
            case 1:   
                    heart1.gameObject.SetActive(true);
                    heart2.gameObject.SetActive(false);
                    heart3.gameObject.SetActive(false);
                    break; 
           
        }
    }
}
