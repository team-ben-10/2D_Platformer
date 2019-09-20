using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Level_Editor : MonoBehaviour
{
    public static Level_Editor instance;
    public Behaviour render;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector2.zero - offset, 0.1f);
    }

    [System.Serializable]
    public class ColorObj
    {
        public Color color;
        public GameObject obj;
        public bool isChild;
        public bool isEntityObject;
        public bool hasAlphaNBT;
        public Vector2 offset;

        public ColorObj(Color color, GameObject obj, bool isChild, bool isEntityObject, bool hasAlphaNBT, Vector2 offset)
        {
            this.color = color;
            this.obj = obj;
            this.isChild = isChild;
            this.isEntityObject = isEntityObject;
            this.hasAlphaNBT = hasAlphaNBT;
            this.offset = offset;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public float SpaceBetweenOBJS;
    public List<ColorObj> objs = new List<ColorObj>();
    public Texture2D sprite;
    public Vector2 offset;
    public CameraMovement cam;

    private void Start()
    {
        Debug.Log("LOADED!");
        //REMOVE WHEN GAME FINISHED
        //string path = $"C:\\Users\\{Environment.UserName}\\Desktop\\colorOBJS.txt";
        //if (!File.Exists(path))
        //{
        //    File.Create(path).Close();
        //    List<string> txt = new List<string>();
        //    foreach (var item in objs)
        //    {
        //        txt.Add(item.obj.name + ": (R=" + item.color.r*255f + ",G=" + item.color.g*255f + ",B=" + item.color.b*255f + (item.hasAlphaNBT?"":",A=" + item.color.a*255f) + ")\n");
        //    }
        //    File.WriteAllLines(path, txt);
        //}
    }

    [HideInInspector]
    public List<GameObject> loadedOBJS = new List<GameObject>();
    [HideInInspector]
    public bool CamFollowPlayer = true;

    private void Update()
    {
        if (CamFollowPlayer)
            if (GameObject.FindGameObjectWithTag("Player") != null)
                cam.SetFollow(GameObject.FindGameObjectWithTag("Player").transform);
    }

    [HideInInspector]
    public Dictionary<Vector2, ColorObj> OBJPos = new Dictionary<Vector2, ColorObj>();
    [HideInInspector]
    public List<GameObject> entityObjects = new List<GameObject>();

    public void LoadLevel()
    {
        //objs.ForEach((x) => Debug.Log(x.obj.name + " " + x.hasAlphaNBT));
        if (GameObject.FindGameObjectWithTag("Player") != null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().lastCheckPoint = null;

        entityObjects.Clear();
        OBJPos.Clear();
        if (loadedOBJS.Count > 0)
        {
            for (int i = 0; i < loadedOBJS.Count; i++)
            {
                Destroy(loadedOBJS[i]);
            }
        }
        loadedOBJS.Clear();
        bool done = false;
        for (int x = 0; x < sprite.width; x++)
        {
            for (int y = 0; y < sprite.height; y++)
            {
                
                Color c = sprite.GetPixel(x, y);
                foreach (var item in objs)
                {
                    if (!item.hasAlphaNBT)
                    {
                        if (c == item.color)
                        {
                            if (CreatorManager.instance != null)
                            {
                                if (item.obj.name == "Player")
                                {
                                    GameObject gb = Instantiate(CreatorManager.instance.playerSpawn, new Vector2(x, y) - offset + new Vector2(0, 0.365f), Quaternion.identity);
                                    gb.transform.localScale = gb.transform.localScale * (1 + (1 - GameManager.instance.levelLoader.SpaceBetweenOBJS));
                                    gb.transform.SetParent(transform);
                                    gb.AddComponent(render.GetType());
                                }
                                else
                                {
                                    GameObject gb = Instantiate(item.obj, new Vector2(x, y) - offset + item.offset, Quaternion.identity);
                                    gb.transform.localScale = gb.transform.localScale * (1 + (1 - GameManager.instance.levelLoader.SpaceBetweenOBJS));
                                    if (item.isChild)
                                    {
                                        gb.transform.SetParent(transform);
                                    }
                                    if (item.isEntityObject)
                                    {
                                        OBJPos.Add(gb.transform.position, item);
                                        entityObjects.Add(gb);
                                    }
                                    gb.AddComponent(render.GetType());
                                }
                            }
                            else
                            {
                                bool skip = true;
                                if (skip)
                                {
                                    GameObject gb = Instantiate(item.obj, new Vector2(x * SpaceBetweenOBJS, y * SpaceBetweenOBJS) - offset + item.offset, Quaternion.identity);
                                    if (item.isChild)
                                    {
                                        gb.transform.SetParent(transform);
                                    }
                                    if (item.isEntityObject)
                                    {
                                        OBJPos.Add(gb.transform.position, item);
                                        entityObjects.Add(gb);
                                    }
                                    loadedOBJS.Add(gb);
                                    gb.AddComponent(render.GetType());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (c.r == item.color.r && c.b == item.color.b && c.g == item.color.g && c.a > 0)
                        {
                            Debug.Log(item.obj.name);
                            GameObject gb = Instantiate(item.obj, new Vector2(x * SpaceBetweenOBJS, y * SpaceBetweenOBJS) - offset + item.offset, Quaternion.identity);
                            if (item.isChild)
                            {
                                gb.transform.SetParent(transform);
                            }
                            if (item.isEntityObject)
                            {
                                ColorObj cO = new ColorObj(item.color, item.obj, item.isChild, item.isEntityObject, item.hasAlphaNBT, item.offset);
                                cO.color.a = c.a;
                                OBJPos.Add(gb.transform.position, cO);
                                entityObjects.Add(gb);
                            }
                            loadedOBJS.Add(gb);
                            gb.AddComponent<AlphaNBTTag>().setNBT((int)(c.a * 255));
                            gb.AddComponent(render.GetType());
                        }

                    }
                }
                done = true;
            }
        }
        StartCoroutine(waitForPlayerSkin());
    }

    public IEnumerator waitForPlayerSkin()
    {
        yield return null;
        var c = Array.Find(GameManager.instance.characters.ToArray(), (item => item.controller == GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().runtimeAnimatorController));
        if (c != null)
            GameManager.instance.UpdatePlayer();
    }

    public GameObject InstantiateColorOBJ(Color color, Vector2 position)
    {
        foreach (var item in objs)
        {
            if (item.color == color)
            {
                GameObject gb = Instantiate(item.obj, new Vector2(position.x, position.y) - offset + item.offset, Quaternion.identity);
                if (item.isChild)
                {
                    gb.transform.SetParent(transform);
                }
                if (item.isEntityObject)
                {
                    OBJPos.Add(gb.transform.position, item);
                    entityObjects.Add(gb);
                }
                loadedOBJS.Add(gb);
                gb.AddComponent(render.GetType());
                return gb;
            }
        }
        return null;
    }

    public void LoadEntityObjects()
    {
        for (int i = 0; i < entityObjects.Count; i++)
        {
            loadedOBJS.Remove(entityObjects[i]);
            Destroy(entityObjects[i]);
        }
        entityObjects.Clear();
        var dic = new Dictionary<Vector2, ColorObj>();
        foreach (var item in OBJPos)
        {
            GameObject gb = Instantiate(item.Value.obj, item.Key, Quaternion.identity);
            if (item.Value.isChild)
            {
                gb.transform.SetParent(transform);
            }
            dic.Add(item.Key, item.Value);
            entityObjects.Add(gb);
            loadedOBJS.Add(gb);
            if(item.Value.hasAlphaNBT)
                gb.AddComponent<AlphaNBTTag>().setNBT((int)(item.Value.color.a * 255));
            gb.AddComponent(render.GetType());
        }
        OBJPos = dic;
    }
}
