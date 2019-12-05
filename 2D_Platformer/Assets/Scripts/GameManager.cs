using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

public class GameManager : MonoBehaviour
{
    public delegate void onRestartDelegate();
    public onRestartDelegate onLevelCleared;
    public onRestartDelegate onEntityLoad;
    public TextAsset textFile;
    public static GameManager instance;
    public Level_Editor levelLoader;
    public int time;
    float currentTime = float.MaxValue;
    public TextMeshProUGUI text;
    public TextMeshProUGUI levelName;
    public Image TimeBarFill;
    public Image MushroomFillTime;
    public GameObject[] backgrounds;
    public float bestTime = -1;
    public int starAmount = 0;
    [HideInInspector]
    public int retries = 0;
    [HideInInspector]
    public int deaths = 0;
    public bool AutoStartLevelGeneration;
    public bool isNotNormalLevel;
    public bool useCancelToExit = true;
    public bool onlyUnlocables;


    public GameObject textPlayer1;
    public GameObject textPlayer2;
    public List<CharacterSelection> characters;
    [SerializeField]
    private List<Unlocable> unlocables;
    public List<Quest> allQuests;
    [HideInInspector] public List<Quest> currentQuests;
    [SerializeField] List<AudioClip> clips;

    [System.Serializable]
    public class CharacterSelection
    {
        public string name;
        public RuntimeAnimatorController controller;
        public List<Behaviour> addBehaviours;
        [Header("Sprite Controlled")]
        public Sprite activationSprite;
        public List<GameObject> objsToSpawn;
    }

    [System.Serializable]
    public class AudioClip
    {
        public string name;
        public UnityEngine.AudioClip clip;
    }

    public UnityEngine.AudioClip GetClip(string name)
    {
        return clips.Find(x => x.name == name)?.clip;
    }

    [System.Serializable]
    public class Unlocable
    {
        public string name;
    }

    public void SetCurrentPlayer(CharacterSelection character, int index)
    {
        string path = Application.persistentDataPath + "/Character.dat";
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            lines[index] = character.name;
            File.WriteAllLines(path, lines);
        }
    }

    public void AddQuest(Quest name)
    {
        foreach (var item in instance.allQuests)
        {
            if (item.name == name.name)
            {
                Debug.Log("Quest found!");
                if (!instance.currentQuests.Contains(item))
                {
                    Debug.Log(name + " added");
                    string path = Application.persistentDataPath + "/Quests/" + item.name + ".quest";
                    instance.currentQuests.Add(item);
                    File.Create(path).Close();
                    File.WriteAllLines(path, item.GetSaveString());
                    return;
                }
            }
        }
    }

    public void AddQuest(string name)
    {
        foreach (var item in instance.allQuests)
        {
            if (item.name == name)
            {
                Debug.Log("Quest found!");
                if (!instance.currentQuests.Contains(item))
                {
                    Debug.Log(name + " added");
                    string path = Application.persistentDataPath + "/Quests/" + item.name + ".quest";
                    instance.currentQuests.Add(item);
                    File.Create(path).Close();
                    File.WriteAllLines(path, item.GetSaveString());
                    return;
                }
            }
        }
    }

    public CharacterSelection GetCharacter(string name)
    {
        foreach (var item in characters)
        {
            if (name == item.name)
                return item;
        }
        return null;
    }

    public Dictionary<string, bool> GetUnlocablesWithStringInName(string namePart)
    {
        var bools = new Dictionary<string, bool>();
        string path = Application.persistentDataPath + "/Unlocables.dat";
        if (File.Exists(path))
        {
            foreach (var item in File.ReadAllLines(path))
            {
                if (item.Split(':')[0].Contains(namePart))
                {
                    bools.Add(item.Split(':')[0], bool.Parse(item.Split(':')[1]));
                }
            }
        }
        return bools;
    }

    public string[] GetUnlocdsWithStringInName(string namePart)
    {
        var bools = new List<string>();
        string path = Application.persistentDataPath + "/Unlocables.dat";
        if (File.Exists(path))
        {
            foreach (var item in File.ReadAllLines(path))
            {
                if (item.Split(':')[0].Contains(namePart))
                {
                    if (bool.Parse(item.Split(':')[1]) == true)
                    {
                        bools.Add(item.Split(':')[0]);
                    }
                }
            }
        }
        return bools.ToArray();
    }

    public bool IsUnloced(string name)
    {
        string path = Application.persistentDataPath + "/Unlocables.dat";
        if (File.Exists(path))
        {
            foreach (var item in File.ReadAllLines(path))
            {
                if (item.StartsWith(name))
                {
                    return bool.Parse(item.Split(':')[1]);
                }
            }
        }
        return false;
    }

    public void CheckQuests(string name)
    {
        Debug.Log("Checking Quest");
        for (int i = 0; i < currentQuests.Count; i++)
        {
            var item = currentQuests[i];
            if (item.name == name)
            {
                if (!item.Check())
                {
                    string path = Application.persistentDataPath + "/Quests/" + item.name + ".quest";
                    File.WriteAllLines(path, item.GetSaveString());
                }
            }
        }
    }

    public void ResetUnlocables()
    {
        string path = Application.persistentDataPath + "/Unlocables.dat";
        for (int i = 0; i < 4; i++)
        {
            SetCurrentPlayer(GetCharacter("Standard"), i);
        }
        File.Delete(path);
        foreach (var item in Directory.GetFiles(Application.persistentDataPath + "/Quests"))
        {
            File.Delete(item);
        }
        SetupUnlocables();
    }

    public void SetupUnlocables()
    {
        string path = Application.persistentDataPath + "/Unlocables.dat";
        if (!File.Exists(path))
        {
            foreach (var item in unlocables)
            {
                SetUnlocable(item.name, false);
            }
        }
    }

    public void SetUnlocable(string name, bool value)
    {
        string path = Application.persistentDataPath + "/Unlocables.dat";
        if (File.Exists(path))
        {
            var lines = File.ReadAllLines(path);
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(name))
                {
                    lines[i] = name + ":" + value;
                    found = true;
                }
            }
            if (found)
                File.WriteAllLines(path, lines);
            else
                File.AppendAllLines(path, new string[] { name + ":" + value });
        }
        else
        {
            File.Create(path).Close();
            File.AppendAllLines(path, new string[] { name + ":" + value });
        }
    }

    public void UpdatePlayer()
    {
        Debug.Log("Load Player!");
        string path = Application.persistentDataPath + "/Character.dat";
        if (File.Exists(path))
        {
            var playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            for (int i = 1; i <= playerMov.MaxPlayerAtATime; i++)
            {
                var lines = File.ReadAllLines(path);
                foreach (var character in characters)
                {
                    if (character.name == lines[i - 1])
                    {
                        var player = GameObject.FindGameObjectWithTag("Player" + ((i != 1) ? ("_" + i) : ""));
                        if (player != null)
                        {

                            foreach (var item in player.GetComponents<Ability>())
                            {
                                Destroy(item);
                            }
                            player.GetComponent<Animator>().runtimeAnimatorController = character.controller;
                            foreach (var behaviour in character.addBehaviours)
                            {
                                if (behaviour is Ability)
                                {
                                    Ability a = (Ability)player.AddComponent(behaviour.GetType());
                                    a.sprite = character.activationSprite;
                                    a.objsToSpawn = character.objsToSpawn;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            /*foreach (var character in characters)
            {
                if (character.name == lines[1])
                {
                    var player = GameObject.FindGameObjectWithTag("Player_2");
                    foreach (var item in player.GetComponents<Ability>())
                    {
                        Destroy(item);
                    }
                    player.GetComponent<Animator>().runtimeAnimatorController = character.controller;
                    foreach (var behaviour in character.addBehaviours)
                    {
                        if (behaviour is Ability)
                        {
                            Ability a = (Ability)player.AddComponent(behaviour.GetType());
                            a.sprite = character.activationSprite;
                            a.objsToSpawn = character.objsToSpawn;
                        }
                    }

                    break;
                }
            }*/
        }
        else
        {
            File.Create(path).Close();
            List<string> strings = new List<string>();
            var playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            for (int i = 0; i < playerMov.MaxPlayerAtATime; i++)
            {
                strings.Add("Standard");
            }
            File.WriteAllLines(path, strings.ToArray());
        }

    }



    private void Awake()
    {
        string path = Application.persistentDataPath + "/Quests";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path).Create();
            currentQuests = new List<Quest>();
        }
        else
        {
            currentQuests = new List<Quest>();
            foreach (var item in Directory.GetFiles(path))
            {
                var splits = item.Split('\\');
                var part = splits[splits.Length - 1].Replace(".quest", "");
                Debug.Log(part);
                foreach (var quest in allQuests)
                {
                    if (quest.name == part)
                    {
                        Quest q = quest.Copy();
                        q.Setup(File.ReadAllLines(item));
                        /*q.AmountForUnlocable = int.Parse(File.ReadAllLines(item)[0].Replace("unlocables:", ""));
                        q.isCompleted = bool.Parse(File.ReadAllLines(item)[1].Replace("completed:", ""));*/
                        currentQuests.Add(q);
                        Debug.Log(quest.name + " added");
                    }
                }
            }
        }
        instance = this;
        currentTime = time;
        /*Application.targetFrameRate = 60;*/
        SetupUnlocables();
        for (int i = 0; i < currentQuests.Count; i++)
        {
            currentQuests[i].isFinished();
        }
        if (AutoStartLevelGeneration)
        {
            if(!AudioManager.Instance.isPlaying)
                AudioManager.Instance.PlayMusic(GetClip("Grassland"));
            levelLoader.LoadLevel();
        }
    }

    private void Update()
    {
        /*if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            if(textPlayer1 != null)
                textPlayer1.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.up;
        }
        if(GameObject.FindGameObjectWithTag("Player_2") != null)
        {
            if (textPlayer2 != null)
                textPlayer2.transform.position = GameObject.FindGameObjectWithTag("Player_2").transform.position + Vector3.up;
        }*/
        TestForHiddenCombination();

        if (onlyUnlocables)
            return;
        if (Input.GetButtonDown("Cancel") && useCancelToExit)
        {
            LoadManager.Instance.LoadScene("Level_Selection");
        }
        if (currentTime != -1)
        {
            text.text = Mathf.Round(currentTime) + "s" + ((bestTime != -1) ? (bestTime < currentTime) ? "/" + Mathf.Round(bestTime) + "s" : "" : "");
            currentTime -= Time.deltaTime;
            TimeBarFill.fillAmount = currentTime / time;
            if (bestTime != -1)
            {
                if (currentTime > bestTime)
                {
                    TimeBarFill.color = new Color(0, 0.75f, 0.75f);
                }
                else
                {
                    TimeBarFill.color = new Color(0, 1, 44 / 255f);
                }
            }
            if (currentTime <= 0)
            {
                Restart(true);
            }
        }
        else
        {
            if (text != null)
                text.gameObject.SetActive(false);
            if (TimeBarFill != null)
                TimeBarFill.gameObject.SetActive(false);
        }
    }

    public void Restart(bool withTimeReset)
    {
        if (withTimeReset)
        {
            currentTime = time;
            retries++;
        }
        levelLoader.LoadLevel();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ResetMushroomEffect();
    }

    string lastScene;

    public void LoadMap(Texture2D texture, TextAsset textfile, Sprite background, string lastScene, string clipName)
    {
        this.lastScene = lastScene;
        foreach (var item in backgrounds)
        {
            item.GetComponent<SpriteRenderer>().sprite = background;
        }
        levelLoader.sprite = texture;
        if (AudioManager.Instance.isPlaying)
            AudioManager.Instance.PlayMusicWithCrossFade(GetClip(clipName),0.25f);
        else
            AudioManager.Instance.PlayMusic(GetClip(clipName));
        levelLoader.LoadLevel();
        var player = GameObject.FindGameObjectWithTag("Player");
        GameObject gb = new GameObject("Player_Spawn");
        gb.transform.position = player.transform.position;
        UpdatePlayer();
        if (textfile != null)
        {
            textFile = textfile;
            var lines = textFile.text.Split('\n');
            foreach (var item in lines)
            {
                if (item.StartsWith("time:"))
                {
                    time = int.Parse(item.Replace("time:", "").Trim());
                }
            }
            currentTime = time;
            string path = Application.persistentDataPath + "/" + textFile.name + ".txt";
            if (File.Exists(path))
            {
                float t;
                if (float.TryParse(File.ReadAllText(path).Split('\n')[0].Replace("bestTime: ", ""), out t))
                {
                    bestTime = t;
                }
            }
        }
        else
        {
            currentTime = -1;
            bestTime = -1;
        }
    }

    public void Win()
    {
        starAmount++;
        if (onLevelCleared != null)
            onLevelCleared.Invoke();
        if (deaths == 0)
            starAmount++;
        if (retries == 0)
            starAmount++;
        if (!isNotNormalLevel)
        {
            if (textFile != null)
            {
                string path = Application.persistentDataPath + "/" + textFile.name + ".txt";
                Debug.Log(path);
                if (File.Exists(path))
                {
                    string[] text = File.ReadAllLines(path);
                    float value = 0;
                    int star = 0;
                    List<string> lines = new List<string>();
                    foreach (var line in text)
                    {
                        if (line.StartsWith("bestTime:"))
                        {
                            if (float.TryParse(line.Replace("bestTime:", ""), out value))
                            {
                                if (value < currentTime)
                                {
                                    lines.Add("bestTime: " + currentTime);
                                }
                                else
                                {
                                    lines.Add("bestTime: " + value);
                                }
                            }
                        }
                        if (line.StartsWith("starAmount:"))
                        {
                            if (int.TryParse(line.Replace("starAmount:", ""), out star))
                            {
                                if (starAmount > star)
                                {
                                    lines.Add("starAmount: " + starAmount);
                                }
                                else
                                {
                                    lines.Add("starAmount: " + star);
                                }
                            }
                        }
                    }
                    File.WriteAllLines(path, lines.ToArray());
                }
                else
                {
                    File.Create(path).Close();
                    File.WriteAllLines(path, new string[] { "bestTime: " + currentTime, "starAmount: " + starAmount });
                }
            }
            Debug.Log("Won The Game");
            LoadManager.Instance.LoadScene(lastScene);
        }
        else
        {
            Debug.Log("Won");
            CreatorManager.instance.SetTo(false);
        }
    }


    public void GoToScene(string name)
    {
        if (name == "END")
        {
            Application.Quit();
        }
        else
        {
            LoadManager.Instance.LoadScene(name);
        }
    }

    [System.Serializable]
    private class HiddenCombis
    {
        public KeyCode[] hiddenKeyCode;
        public string functionName;
    }

    int hiddenKeyNum = 0;
    [SerializeField] private List<HiddenCombis> hiddenCombis;
    HiddenCombis combi;

    private void TestForHiddenCombination()
    {
        if (Input.anyKeyDown)
        {
            if (combi == null)
                foreach (var item in hiddenCombis)
                {
                    if (Input.GetKeyDown(item.hiddenKeyCode[0]))
                    {
                        combi = item;
                        hiddenKeyNum++;
                    }
                }
            else
            {
                if (Input.GetKeyDown(combi.hiddenKeyCode[hiddenKeyNum]))
                {
                    if (combi.hiddenKeyCode[hiddenKeyNum] == KeyCode.Return)
                    {
                        Debug.Log("Cheat activated!");
                        Invoke(combi.functionName, 0f);
                        hiddenKeyNum = 0;
                        combi = null;
                    }
                    else
                    {
                        hiddenKeyNum++;
                    }
                }
                else
                {
                    hiddenKeyNum = 0;
                    combi = null;
                }
            }
        }
    }

    void KillPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().Die();
        }
    }

    void UnlocAll()
    {
        if (FindObjectOfType<Level_Selector>() != null)
        {
            FindObjectOfType<Level_Selector>().UnlocAll();
        }
    }
}
