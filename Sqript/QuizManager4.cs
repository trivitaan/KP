using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager4 : MonoBehaviour
{
    public static QuizManager4 instance; //Instance to make is available in other scripts without reference
    public GameObject feed_benar, feed_salah;
    [SerializeField] private GameObject gameComplete;
    [SerializeField] private GameObject gameFailed;
    //Scriptable data which store our questions data
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private Image questionImage;           //image element to show the image
    [SerializeField] private WordData[] answerWordList;     //list of answers word in the game
    [SerializeField] private WordData[] optionsWordList;    //list of options word in the game


    private GameStatus gameStatus = GameStatus.Playing;     //to keep track of game status
    private char[] wordsArray = new char[3];               //array which store char of each options

    private List<int> selectedWordsIndex;                   //list which keep track of option word index w.r.t answer word index
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;   //index to keep track of current answer and current question
    private bool correctAnswer = true;                      //bool to decide if answer is correct or not
    private string answerWord;     

    //banyaknya soal
    private int[] rand = new int[3];
    private int skor;

    //string to store answer of current question
    private void Awake()
    {
        Debug.Log(questionDataScriptable.questions.Count);
        for(int i = 0; i<questionDataScriptable.questions.Count; i++){
            rand[i] = i;
        }

        System.Random random = new System.Random();
        rand = rand.OrderBy(x => random.Next()).ToArray();
        
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedWordsIndex = new List<int>();           //create a new list at start
        SetQuestion();                                  //set question
    }

    void SetQuestion()
    {
        gameStatus = GameStatus.Playing;                //set GameStatus to playing 

        //set the answerWord string variable
        answerWord = questionDataScriptable.questions[rand[currentQuestionIndex]].answer;
        //set the image of question
        questionImage.sprite = questionDataScriptable.questions[rand[currentQuestionIndex]].questionImage;
            
        ResetQuestion();                               //reset the answers and options value to orignal     

        selectedWordsIndex.Clear();                     //clear the list for new question
        Array.Clear(wordsArray, 0, wordsArray.Length);  //clear the array

        //add the correct char to the wordsArray
        for (int i = 0; i < answerWord.Length; i++)
        {
            wordsArray[i] = char.ToUpper(answerWord[i]);
        }

        //add the dummy char to wordsArray
        for (int j = answerWord.Length; j < wordsArray.Length; j++)
        {
            wordsArray[j] = (char)UnityEngine.Random.Range(48, 57);
        }

        wordsArray = ShuffleList.ShuffleListItems<char>(wordsArray.ToList()).ToArray(); //Randomly Shuffle the words array

        //set the options words Text value
        for (int k = 0; k < optionsWordList.Length; k++)
        {
            optionsWordList[k].SetWord(wordsArray[k]);
        }

        Debug.Log(questionDataScriptable.questions[rand[currentQuestionIndex]].answer);
    }

    //Method called on Reset Button click and on new question
    public void ResetQuestion()
    {
        //activate all the answerWordList gameobject and set their word to "_"
        for (int i = 0; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(true);
            answerWordList[i].SetWord('_');
        }

        //Now deactivate the unwanted answerWordList gameobject (object more than answer string length)
        for (int i = answerWord.Length; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(false);
        }

        //activate all the optionsWordList objects
        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    /// <summary>
    /// When we click on any options button this method is called
    /// </summary>
    /// <param name="value"></param>
    public void SelectedOption(WordData value)
    {
        //if gameStatus is next or currentAnswerIndex is more or equal to answerWord length
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex()); //add the child index to selectedWordsIndex list
        value.gameObject.SetActive(false); //deactivate options object
        answerWordList[currentAnswerIndex].SetWord(value.wordValue); //set the answer word list
        GetComponent<AudioSource>().Play();
        currentAnswerIndex++;   //increase currentAnswerIndex

        //if currentAnswerIndex is equal to answerWord length
        if (currentAnswerIndex == answerWord.Length)
        {
            correctAnswer = true;   //default value
            //loop through answerWordList
            for (int i = 0; i < answerWord.Length; i++)
            {
                //if answerWord[i] is not same as answerWordList[i].wordValue
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordList[i].wordValue))
                {
                    correctAnswer = false; //set it false
                    break; //and break from the loop
                }
            }

            //if correctAnswer is true
            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                

                if(PlayerPrefs.GetInt("timer") >=24){
                    skor = PlayerPrefs.GetInt("skor") + 100;
                }
                else if(PlayerPrefs.GetInt("timer") >=14){
                    skor = PlayerPrefs.GetInt("skor") + 50;
                }
                else if(PlayerPrefs.GetInt("timer") >=1){
                    skor = PlayerPrefs.GetInt("skor") + 10;
                }
                
                PlayerPrefs.SetInt("skor", skor);
                PlayerPrefs.SetInt("timer", 30);

                gameStatus = GameStatus.Next; //set the game status
                currentQuestionIndex++; //increase currentQuestionIndex

                //if currentQuestionIndex is less that total available questions
                if (currentQuestionIndex < questionDataScriptable.questions.Count)
                {
                    feed_benar.SetActive(false);
    		        feed_benar.SetActive(true);
                    Invoke("SetQuestion", 0.5f); //go to next question
                }
                else
                {
                    Debug.Log("Game Complete"); //else game is complete
                    gameComplete.SetActive(true);
                }
            }else{
                
                PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") - 1);

                PlayerPrefs.SetInt("timer", 30);
                
                
                if(PlayerPrefs.GetInt("hp") == 0){
                    gameFailed.SetActive(true);
                }else{
                feed_salah.SetActive(false);
    		    feed_salah.SetActive(true);
                ResetQuestion();
                }
            }
        }
    }

    void Update()
    {
		// jika waktu habis
        if(PlayerPrefs.GetInt("timerActive") == 0){
			
			
			
			// Debug.Log(transform.parent.childCount - 1);
			PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") - 1);

			if(PlayerPrefs.GetInt("hp") == 0){
				gameFailed.SetActive(true);
			}else{
                feed_salah.SetActive(false);
    		    feed_salah.SetActive(true);
                ResetQuestion();
				PlayerPrefs.SetInt("timer", 30);
				PlayerPrefs.SetInt("timerActive",1);
			}
		}
    }


}

[System.Serializable]
public class QuestionData5
{
    public Sprite questionImage;
    public string answer;
}

public enum GameStatus4
{
   Next,
   Playing
}
