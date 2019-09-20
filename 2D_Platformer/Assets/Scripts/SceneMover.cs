using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public void MoveTo(string scene)
    {
        /*if (Client.instance != null && Client.client.TCPClient.Connected)
            Client.client.WriteMessage("!disconnect!");*/
        SceneManager.LoadScene(scene);
    }

    public string unlocableName;

    void Start()
    {
        if (unlocableName != "") {
            var b = GameManager.instance.IsUnloced(unlocableName);
            Debug.Log(b);
            if (!b)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
