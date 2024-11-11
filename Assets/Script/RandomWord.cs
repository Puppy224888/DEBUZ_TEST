using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RandomWord : MonoBehaviour
{
    [SerializeReference] List<GroupShowText> textShow = new List<GroupShowText>();
    [SerializeReference] List<Button> keyCode = new List<Button>();
    [SerializeReference] GameObject playAgainPanel;
    [SerializeReference] TextMeshProUGUI winData;
    [SerializeReference] TextMeshProUGUI answer;
    [SerializeReference] Button playAgian;
    List<string> wordList = new List<string>();
    string win_Word;
    Image btnImage;
    Image textShowImage;

    int totalPlay;
    int totalWin;
    bool played = false;
    bool isEnd;

    int bcrCount = 0;
    int round = 0;
    List<Button> buttonCharInRound = new List<Button>();

    private readonly char[] keyboardKeys = new char[]
    {
        'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
        'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L',
        'Z', 'X', 'C', 'V', 'B', 'N', 'M'
    };

    void LoadWords()
    {
        TextAsset wordData = Resources.Load<TextAsset>("WordList");
        wordList = new List<string>(wordData.text.Split('\n'));
    }

    string GetRandomWord()
    {
        return wordList[Random.Range(0, wordList.Count)].ToUpper();
    }

    private void Start()
    {
        LoadWords();
        win_Word = GetRandomWord();
        var i = 0;
        foreach (var letter in keyboardKeys)
        {
            var btn = keyCode[i];
            btn.name = "" + letter;
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + letter;
            btn.onClick.AddListener(() => AddWord(btn));
            i++;
        }

        /* for (char letter = 'A'; letter <= 'Z'; letter++)
         {
             var btn = keyCode[i];
             btn.name = "" + letter;
             btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + letter;
             btn.onClick.AddListener(() => AddWord(btn));
             i++;
         }
         */

        Debug.Log($"win_Word : {win_Word}");
    }
    public void AddWord(Button btn)
    {
        if(isEnd) return;
        answer.text = "ANSWER";
        answer.color = Color.white;
        if (!played)
        {
            played = true;
            totalPlay = PlayerPrefs.GetInt("Played") + 1;
            Debug.Log(totalPlay);
            PlayerPrefs.SetInt("Played", totalPlay);
        }

        if (buttonCharInRound.Count < 5)
        {
            buttonCharInRound.Add(btn);
            bcrCount = buttonCharInRound.Count;
            textShow[round].textShow[bcrCount - 1].text = buttonCharInRound[bcrCount - 1].name;
        }
    }

    public void OnDelete()
    {
        if(isEnd) return;

        if (buttonCharInRound.Count > 0)
        {
            bcrCount = buttonCharInRound.Count;
            buttonCharInRound.RemoveAt(bcrCount - 1);
            textShow[round].textShow[bcrCount - 1].text = "";
        }
    }

    public void OnEnter()
    {
        var winYet = 0;
        if (buttonCharInRound.Count < 4 || isEnd) return;

        var b = textShow[round];
        var trustWord = false;
        foreach (var item in wordList)
        {
            var truet = 0;
            for (int i = 0; i < 5; i++)
            {
               if(item.ToUpper()[i] == b.textShow[i].text[0]) truet += 1;
            }

            if(truet == 5)
            {
                Debug.Log("item : " + item);
                trustWord = true;
            }
        }

        if (!trustWord)
        {
            Debug.Log("Wrong");
            answer.text = "Not Have This Word";
            answer.color = Color.yellow;
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            btnImage = buttonCharInRound[i].GetComponent<Image>();
            textShowImage = textShow[round].textShow[i].transform.parent.GetComponent<Image>();
            textShowImage.color = Color.gray;
            btnImage.color = Color.gray;


            if (buttonCharInRound[i].name[0] == win_Word[i])
            {
                textShowImage.color = Color.green;
                btnImage.color = Color.green;
                winYet += 1;
            }
            else
            {
                foreach (var item in win_Word)
                {
                    if (buttonCharInRound[i].name[0] == item)
                    {
                        textShowImage.color = Color.yellow;
                        btnImage.color = Color.yellow;
                    }
                }
            }
        }

        buttonCharInRound.Clear();

        if (winYet == 5)
        {
            Debug.Log(">>>> WIN <<<<");
            isEnd = true;
            playAgainPanel.SetActive(true);
            totalWin = PlayerPrefs.GetInt("Win") + 1;
            PlayerPrefs.SetInt("Win", totalWin);
            winData.text = $"TotalWin:{totalWin}/TotalPlay:{totalPlay}";
            answer.text = win_Word;
            answer.color = Color.green;
            playAgian.image.color = Color.green;
            winData.color = Color.green;
            return;
        }

        if (round < 5)
        {
            round += 1;
        }
        else
        {
            Debug.Log(">>>> Fail <<<<");
            isEnd = true;
            playAgainPanel.SetActive(true);
            answer.text = win_Word;
            answer.color = Color.yellow;
            playAgian.image.color = Color.yellow;
            totalWin = PlayerPrefs.GetInt("Win");
            winData.text = $"TotalWin:{totalWin}/TotalPlay:{totalPlay}";
            winData.color = Color.yellow;
        }
    }

    public void PlayerAgain()
    {
        played = false;
        isEnd = false;
        win_Word = GetRandomWord();
        Debug.Log($"win_Word : {win_Word}");
        answer.text = "ANSWER";
        answer.color = Color.white;
        playAgainPanel.SetActive(false);
        buttonCharInRound.Clear();
        round = 0;

        foreach (var item in textShow)
        {
            for (int i = 0; i < 5; i++)
            {
                item.textShow[i].text = "";
                item.textShow[i].transform.parent.GetComponent<Image>().color = Color.white;
            }
        }
        var n = 0;
        foreach (var letter in keyboardKeys)
        {
            var btn = keyCode[n];
            btn.GetComponent<Image>().color = Color.white;
            n++;
        }
    }

    public void ResetData()
    {
            PlayerPrefs.DeleteAll();
            Debug.Log("DeletePPAll");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) ResetData();

    }

}
