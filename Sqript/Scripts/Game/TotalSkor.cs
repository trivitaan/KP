using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TotalSkor : MonoBehaviour
{
    // public static int totalSkor;
    // public Text teksTotalSkor;
    // Start is called before the first frame update
    private int jumlah;
    void Start()
    {
        if(!PlayerPrefs.HasKey("totalSkor")){       	
            PlayerPrefs.SetInt("totalSkor", 0);
            // Debug.Log(PlayerPrefs.GetInt("totalSkor"));
        }else{
        	Load();
            // Debug.Log(PlayerPrefs.GetInt("totalSkor"));
        }
        GetComponent<Text>().text = PlayerPrefs.GetInt("totalSkor").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<Text>().text = PlayerPrefs.GetInt("totalSkor").ToString();
    }

    public void Save(){
        jumlah = PlayerPrefs.GetInt("totalSkor") + PlayerPrefs.GetInt("skor");
        PlayerPrefs.SetInt("totalSkor", jumlah);
    }

    void Load(){
        PlayerPrefs.GetInt("totalSkor");
    }

    public void TambahSkor(){

    }
    void OnMouseDown(){
    	Save();
    }
}
