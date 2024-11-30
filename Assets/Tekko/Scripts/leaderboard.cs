
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    public GameObject slotUi;
    public GameObject slotPlace;

    private string publicKey = "2af0978cbf4be17c6e16335bf90779232197be046741da3f3184bfb941272907";

    private void Start()
    {
        GetLeaderBoard();
    }

    public void GetLeaderBoard()
    {
        Leaderboards.SpeedJam.GetEntries(msg => {

             foreach (Transform child in slotPlace.transform)
             {
                 Destroy(child.gameObject);
             }

             print(msg.Length + " msg lenght");
             int loopLenght = (msg.Length < 20) ? msg.Length : 20;
             print(loopLenght + " loop lenght");
             for (int i = 0; i < loopLenght; i++)
             {
                 
                 GameObject newSlot = (GameObject)Instantiate(slotUi);
                 newSlot.transform.SetParent(slotPlace.transform);

                 newSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
                 newSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = msg[i].Username;
                 newSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = msg[i].Score.ToString();
             }
             
        });
    }


    public void SetLeaderboardEntry(string username, string score)
    {
        print(score + score.GetType());
        Leaderboards.SpeedJam.UploadNewEntry(username, int.Parse(score) , isSuccessful => {
            if (isSuccessful)
            {
                GetLeaderBoard();
            }
            else
            {
                print("error");
            }
        });
    }

}
