using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    public bool isPlayMode = false;
    public GameObject[] objsToEnable;
    GameObject cam;
    public ObjClick selectedObject;
    public static CreatorManager instance;
    public GameObject prefabSelection;
    public Transform objectSelectionContent;
    public GameObject objectSelection;
    public GameObject playButton;
    public GameObject inputSave;
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject playerSpawn;
    [HideInInspector] public bool isSelecting;
    public ObjClick[] selectionObjects;
    [HideInInspector] public bool isOnClick = false;
    [HideInInspector] public Vector3 offset;
    public int WIDTH = 300;
    public int HEIGHT = 150;

    [System.Serializable]
    public class ObjClick
    {
        public GameObject gb;
        public bool isOnClick;
        public Sprite showIcon;
        public Vector3 offset;
        public bool isEntityObject;
    }

    private void Start()
    {
        SetTo(false);
        isSelecting = true;
        cam.transform.position = new Vector3(WIDTH / 2, HEIGHT / 2, -10);
    }

    void Awake()
    {
        instance = this;
        cam = Camera.main.gameObject;
        int i = 0;
        foreach (var item in selectionObjects)
        {
            if (i == 0)
                selectedObject = item;
            GameObject gb = Instantiate(prefabSelection, objectSelectionContent);
            gb.GetComponent<Object_Selection_Prefab>().gb = item;
            gb.GetComponent<Object_Selection_Prefab>().OnInstant(item.showIcon != null ? item.showIcon : item.gb.GetComponent<SpriteRenderer>().sprite);
            i++;
            
        }
    }

    public Vector3 getGridPos(Vector3 pos)
    {
        return new Vector3(Mathf.RoundToInt(pos.x / GameManager.instance.levelLoader.SpaceBetweenOBJS) * GameManager.instance.levelLoader.SpaceBetweenOBJS, Mathf.RoundToInt(pos.y / GameManager.instance.levelLoader.SpaceBetweenOBJS) * GameManager.instance.levelLoader.SpaceBetweenOBJS);
    }

    void Update()
    {
        if (!isPlayMode)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetAxisRaw("Horizontal") <= 0.35 && Input.GetAxisRaw("Horizontal") >= -0.35)
            {
                
               horizontal = 0;
            }
            float vertical = Input.GetAxisRaw("Vertical");
            if (!inputSave.GetComponent<InputField>().isFocused)
                cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x + horizontal, 0, WIDTH), Mathf.Clamp(cam.transform.position.y + vertical, 0, HEIGHT), cam.transform.position.z);

            if ((!isOnClick && Input.GetMouseButton(0) && !isSelecting) || (isOnClick && Input.GetMouseButtonDown(0) && !isSelecting) && !inputSave.GetComponent<InputField>().isFocused)
            {

                Vector3 pos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                Vector3 realPos = new Vector3(Mathf.RoundToInt(pos.x/GameManager.instance.levelLoader.SpaceBetweenOBJS)* GameManager.instance.levelLoader.SpaceBetweenOBJS, Mathf.RoundToInt(pos.y/ GameManager.instance.levelLoader.SpaceBetweenOBJS)* GameManager.instance.levelLoader.SpaceBetweenOBJS, 0);
                if (Physics2D.OverlapCircleAll(new Vector2(realPos.x, realPos.y), 0.05f).Length <= 0)
                {
                    if (realPos.x >= 0 && realPos.x <= WIDTH && realPos.y >= 0 && realPos.y <= HEIGHT)
                    {
                        GameObject gb = Instantiate(selectedObject.gb, realPos + selectedObject.offset, selectedObject.gb.transform.rotation, GameManager.instance.levelLoader.transform);
                        //gb.transform.localScale = gb.transform.localScale * (1 + (1 - GameManager.instance.levelLoader.SpaceBetweenOBJS));
                        if (selectedObject.isEntityObject)
                        {
                            GameManager.instance.levelLoader.entityObjects.Add(gb);
                            Level_Editor.ColorObj obj = null;
                            foreach (var item in GameManager.instance.levelLoader.objs)
                            {
                                if (item.obj.name == gb.name.Replace("(Clone)", ""))
                                {
                                    obj = item;
                                    break;
                                }
                            }
                            GameManager.instance.levelLoader.OBJPos.Add(new Vector2(gb.transform.position.x, gb.transform.position.y), obj);
                        }
                    }

                }
            }
            if (Input.GetMouseButton(1) && !isSelecting)
            {
                Vector3 pos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                foreach (var item in Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y), 0.05f))
                {
                    GameManager.instance.levelLoader.entityObjects.Remove(item.gameObject);
                    GameManager.instance.levelLoader.OBJPos.Remove(item.transform.position);
                    Destroy(item.gameObject);
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                isSelecting = !isSelecting;
                objectSelection.SetActive(isSelecting);
                playButton.SetActive(isSelecting);
                saveButton.SetActive(isSelecting);
                inputSave.SetActive(isSelecting);
                loadButton.SetActive(isSelecting);
            }
            if (Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Start_Scene");
            }
        }
    }

    public void LoadMap()
    {
        string path = inputSave.GetComponent<InputField>().text;
        if(path != "")
        {
            if (File.Exists(path))
            {
                Texture2D text = new Texture2D(WIDTH, HEIGHT);
                ImageConversion.LoadImage(text,File.ReadAllBytes(path));
                GameManager.instance.levelLoader.sprite = text;
                GameManager.instance.levelLoader.LoadLevel();
                GameManager.instance.levelLoader.sprite = null;
            }
        }
    }

    public void SaveMap()
    {
        string path = inputSave.GetComponent<InputField>().text;
        if (path != "")
        {
            Texture2D text = new Texture2D(WIDTH, HEIGHT);
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    bool isBlock = false;
                    foreach (var block in Physics2D.OverlapCircleAll(new Vector2(x, y), 0.005f))
                    {
                        if (block.name.Replace("(Clone)", "") == "Player_Spawn")
                        {
                            text.SetPixel(x, y, new Color(10 / 255f, 1, 0, 1));
                            isBlock = true;
                        }
                        else
                        {
                            foreach (var item in GameManager.instance.levelLoader.objs)
                            {
                                if (block.name.Replace("(Clone)", "") == item.obj.name)
                                {
                                    text.SetPixel(x, y, item.color);
                                    isBlock = true;
                                }
                            }
                        }
                    }
                    if (!isBlock)
                        text.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
            text.Apply();
            File.WriteAllBytes(path, text.EncodeToPNG());
        }
    }

    public void SetTo(bool play)
    {
        isPlayMode = play;
        foreach (var item in objsToEnable)
        {
            item.SetActive(play);
        }
        cam.GetComponent<Cinemachine.CinemachineBrain>().enabled = play;
        if (play)
        {
            Time.timeScale = 1;
            if (GameManager.instance.levelLoader.transform.GetComponentInChildren<Player_Spawner>() != null)
            {
                GameManager.instance.levelLoader.transform.GetComponentInChildren<Player_Spawner>().SpawnPlayer();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().lastCheckPoint = GameManager.instance.levelLoader.transform.GetComponentInChildren<Player_Spawner>().transform;
            }
        }
        else
        {
            Time.timeScale = 0;
            for (int i = 0; i < GameManager.instance.levelLoader.transform.childCount; i++)
            {
                Transform child = GameManager.instance.levelLoader.transform.GetChild(i);
                if (child.name.Contains("Checkpoint"))
                {
                    child.GetComponent<CheckPoint>().isTriggered = false;
                    child.GetComponent<SpriteRenderer>().sprite = child.GetComponent<CheckPoint>().uncheckedSprite;
                }
                if (child.name.Contains("Finish_Point"))
                {
                    child.GetComponent<Finish_Point>().isTriggered = false;
                }
            }
            GameManager.instance.levelLoader.LoadEntityObjects();
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    public void SetSelectedObject(ObjClick gb)
    {
        selectedObject = gb;
    }
}
