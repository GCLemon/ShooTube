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
{
    public string comment;
    public object icon;
    public string time;
}


public class GetCommentFromFlask : MonoBehaviour
{
    [SerializeField] private InputField inputCommentField;
    [SerializeField] private GetYutubeComment getYutubeComment;
    [SerializeField] private bool isGetComment=false;

    string queryString="test";
    DateTime lastCommentTime = DateTime.Parse("2023 - 03 - 17T04:16:22.484251+00:00");

    void Start()
    {
        inputCommentField = inputCommentField.GetComponent<InputField>();
        if (isGetComment == true)
        {
            StartCoroutine(GetCommentsFromFlask());
            Debug.Log("FlaskAPIŽg—p’†‚Å‚·");
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
            for (int i =0; i< usersData.items.Length; i++)
            {
                if (lastCommentTime < DateTime.Parse(usersData.items[i].time))
                {
                    getYutubeComment.liveChatMassegeQueue.Enqueue(usersData.items[i].comment);
                }

                if (i == usersData.items.Length - 1)
                {
                    lastCommentTime = DateTime.Parse(usersData.items[i].time);
                }

            }

            yield return new WaitForSeconds(10.0f);
            yield return GetCommentsFromFlask();
        }
    }

    IEnumerator PushCommentToServer()
    {
        queryString = inputCommentField.text;
        string url = "https://comment-api-from-shootube-to-flask.onrender.com/commentstore?comment=" + queryString;

        UnityWebRequest pushCommentRequest = UnityWebRequest.Get(url);
        yield return pushCommentRequest.SendWebRequest();
    }

    public void commentPushButtonOnCkick()
    {
        StartCoroutine(PushCommentToServer());
    }

}
