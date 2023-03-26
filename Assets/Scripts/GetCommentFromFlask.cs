using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

[System.Serializable]
public class UsersData
{
    public Items[] items;
}

[System.Serializable]
public class Items
{   public string userName;
    public string comment;
    public string icon;
    public string time;
}


public class GetCommentFromFlask : MonoBehaviour
{
    [SerializeField] private GetYutubeCommentR getYutubeComment;
    [SerializeField] private bool isGetComment=false;

    TimeZoneInfo timeZoneJst = TimeZoneInfo.Local;
    DateTime lastCommentTime;


    void Start()
    {
        lastCommentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneJst);
        if (isGetComment == true)
        {
            StartCoroutine(GetCommentsFromFlask());
            Debug.Log("useFlaskAPI");
        }
    }

    IEnumerator GetCommentsFromFlask()
    {
        string url = "https://comment-api-from-shootube-to-flask.onrender.com/commentretrieve";

        UnityWebRequest getCommentRequest = UnityWebRequest.Get(url);
        yield return getCommentRequest.SendWebRequest();

        if (getCommentRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + getCommentRequest.error);
        }
        else
        {
            UsersData usersData = JsonUtility.FromJson<UsersData>(getCommentRequest.downloadHandler.text);
            if (usersData.items != null)
            {
                for (int i = 0; i < usersData.items.Length; i++)
                {
                    if (lastCommentTime < DateTime.Parse(usersData.items[i].time))
                    {
                        getYutubeComment.liveChatMassegeQueue.Enqueue(usersData.items[i].comment);
                        getYutubeComment.userIconUrlQueue.Enqueue(usersData.items[i].icon);
                        getYutubeComment.userNameQueue.Enqueue(usersData.items[i].userName);
                        if(getYutubeComment.userNameQueue.Peek()==null){
                            Debug.Log("userNameNull");
                        }
                    }

                    if (i == usersData.items.Length - 1)
                    {
                        lastCommentTime = DateTime.Parse(usersData.items[i].time);
                    }
                }
            }

            yield return new WaitForSeconds(10.0f);
            yield return GetCommentsFromFlask();
        }
    }

}
