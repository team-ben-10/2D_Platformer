using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransfer.DataTransfer;
using System.Threading;
using System.Runtime.InteropServices;
using Unity;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;

class Client : MonoBehaviour
{
    //public static Client instance;
    static string Name;
    //public static DataTransfer.DataTransfer.Client client;
    public InputField nameInput;
    public InputField pathInput;
    public InputField searchInput;
    public GameObject uploadButton;
    //public TMPro.TextMeshProUGUI errorText;

    /*public void Search()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        client.WriteMessage("SearchLevel:" + searchInput.text);
    }*/

    /*private void Awake()
    {
        instance = this;
    }*/

    bool reqeuestedData;
    WebClient client;

    public void GetAllLevels()
    {
        string data = client.DownloadString("http://localhost:25565/Search/");
        string[] names = data.Split(' ');
        foreach (var name in names)
        {
            StartCoroutine(SetupLevel(name));
        }
    }

    public IEnumerator<object> SetupLevel(string name)
    {
        //string data = client.DownloadString("http://localhost:25565/Maps/" + name);
        //Debug.Log(path);
        /*if (!File.Exists(path))
        {
            File.Create(path).Close();
            File.WriteAllBytes(path, );
        }*/
        
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://localhost:25565/Maps/" + name + ".png",false);
        yield return www.SendWebRequest();
        var text = ((DownloadHandlerTexture)www.downloadHandler).texture;
        text.name = name;
        GameObject gb = Instantiate(prefabLevel, content);
        var lsp = gb.GetComponent<Level_Selection_Prefab>();
        lsp.GetComponent<Image>().sprite = Sprite.Create(text,new Rect(0,0,300,150),new Vector2(0,0));
        lsp.isEnabled = true;
        lsp.texture = lsp.GetComponent<Image>().sprite.texture;
        lsp.background = back;
        lsp.startWithCutscene = false;
        lsp.setDiamonds(3);
        lsp.setName(name);
    }

    private void Start()
    {
        persistantPath = Application.persistentDataPath;
        string ip = "127.0.0.1";//"194.118.240.16";
         client = new WebClient();
        
        GetAllLevels();
        /*client = new DataTransfer.DataTransfer.Client(ip, 255);
        client.onMessageRecieved += onMessageRecieved;
        client.onFailedConnection += onFailedConnection;
        client.Setup();
        bool requestData = true;
        new Thread(() =>
        {
            while (!client.TCPClient.Connected)
            {
                Thread.Sleep(1);
            }
            while (client.TCPClient.Connected)
            {
                if (requestData)
                {
                    client.WriteMessage("!RequestData!");
                    requestData = false;
                    reqeuestedData = true;
                }
                string msg = "";
                if (msg == "!disconnect!")
                {
                    client.WriteMessage("!disconnect!");
                }
            }
        }).Start();
        client.Run();*/
    }

    /*private bool onFailedConnection(object data)
    {
        Debug.Log("OnFailed!");
        e = (Exception)data;
        return false;
    }*/

    string persistantPath;
    public GameObject prefabLevel;
    public Transform content;
    public Sprite back;

    /*private void OnApplicationQuit()
    {
        client.WriteMessage("!disconnect!");
    }

    bool isReadingLevel;
    string textbytes = "";

    private static bool onMessageRecieved(object data)
    {
        string txt = data.ToString().Replace("\0", "").Trim();

        if (instance.isReadingLevel)
        {
            var splits = txt.Split(':');
            if (!txt.Contains("end"))
            {
                instance.textbytes += splits[0].Trim() + "/";
            }
            else
            {
                instance.textbytes += splits[0].Trim() + "/";
                instance.bytes = TurnIntoByteArray(instance.textbytes.Trim());
                instance.isReadingLevel = false;
                instance.textbytes = "";
                Debug.Log("Got Full Level in Steps!");
            }
        }
        if (txt.StartsWith("LoadLevel"))
        {
            Debug.Log(txt);
            string[] splits = txt.Split(':');
            instance.isReadingLevel = true;
            instance.nameLevel = splits[1];
            if (txt.Contains("end"))
            {
                instance.bytes = TurnIntoByteArray(splits[2].Trim());
                instance.isReadingLevel = false;
                instance.textbytes = "";
                Debug.Log("Got Full Level!");
            }
            else
            {
                instance.textbytes += splits[2].Trim() + "/";
            }
            //File.WriteAllText(instance.persistantPath + "/SaveData" + splits[1] +".txt", txt);
            //File.Create(path).Close();
            //File.WriteAllBytes(path, TurnIntoByteArray(splits[2]));
        }
        return true;
    }

    */public void Refresh()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        GetAllLevels();
    }/*

    string nameLevel = "";
    byte[] bytes = null;
    Exception e = null;
    private void Update()
    {
        if (reqeuestedData)
        {
            uploadButton.SetActive(true);
            errorText.gameObject.SetActive(false);
            reqeuestedData = false;
        }
        if (e != null)
        {
            errorText.text = e.Message;
            e = null;
        }

        if (nameLevel != "" && bytes != null)
        {
            var text = new Texture2D(300, 150, TextureFormat.RGBA32, false, false);
            Sprite sprite = Sprite.Create(text, new Rect(Vector2.zero, new Vector2(300, 150)), Vector2.zero);
            string path = Application.persistentDataPath + "/" + nameLevel + ".png";
            //Debug.Log(path);
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllBytes(path, bytes);
            }
            ImageConversion.LoadImage(sprite.texture, bytes, false);
            sprite.texture.filterMode = FilterMode.Point;
            sprite.texture.Apply();
            GameObject gb = Instantiate(prefabLevel, content);
            var lsp = gb.GetComponent<Level_Selection_Prefab>();
            //lsp.GetComponent<Image>().sprite = Sprite.Create(text,new Rect(0,0,300,150),new Vector2(0,0));
            lsp.isEnabled = true;
            lsp.texture = sprite.texture;
            lsp.background = back;
            lsp.startWithCutscene = false;
            lsp.setDiamonds(3);
            lsp.setName(nameLevel);
            nameLevel = "";
            bytes = null;
            client.WriteMessage("!SendNext!");
        }
    }

    public void Upload()
    {
        Debug.Log("Upload ?");
        Debug.Log(pathInput.text);
        Debug.Log(nameInput.text);
        if (nameInput.text != "" && File.Exists(pathInput.text))
        {
            client.WriteMessage("UploadLevel:" + nameInput.text + ":" + TurnIntoString(File.ReadAllBytes(pathInput.text)) + ":end");
        }
    }

    static byte[] TurnIntoByteArray(string txt)
    {
        List<byte> bytes = new List<byte>();
        foreach (var item in txt.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
        {
            bytes.Add((byte)int.Parse(item.ToString()));
        }
        return bytes.ToArray();
    }
    static string TurnIntoString(byte[] bytes)
    {
        string txt = "";
        foreach (var item in bytes)
        {
            txt += item + " ";
        }
        return txt.Trim();
    }*/
}