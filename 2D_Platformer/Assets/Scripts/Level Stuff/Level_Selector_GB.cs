using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Selector_GB : MonoBehaviour
{

    public static Level_Selector_GB instance;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public string lastScene;

    [HideInInspector]
    public bool isCutscene;

    private void OnLevelWasLoaded(int level)
    {
        if(level == SceneManager.GetSceneByName("SampleScene").buildIndex)
        {
            GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
            if (manager != null)
            {
                GameManager gameManager = manager.GetComponent<GameManager>();
                if (texture != null && background != null)
                {
                    gameManager.LoadMap(texture, textFile, background, lastScene);
                }
                Destroy(gameObject);
            }
        }
        if(level == SceneManager.GetSceneByName("Level_Creator").buildIndex)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isCutscene)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                LoadManager.Instance.LoadScene("SampleScene");
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                LoadManager.Instance.LoadScene("Start_Scene");
                /*if (Client.instance != null && Client.client.TCPClient.Connected)
                    Client.client.WriteMessage("!disconnect!");*/
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    public Texture2D texture;
    public Sprite background;
    public TextAsset textFile;
}
