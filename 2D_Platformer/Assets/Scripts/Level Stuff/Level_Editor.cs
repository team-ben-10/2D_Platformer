﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class Level_Editor : MonoBehaviour
{
    public static Level_Editor instance;

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
        //        txt.Add(item.obj.name + ": (R=" + item.color.r * 255f + ",G=" + item.color.g * 255f + ",B=" + item.color.b * 255f + (item.hasAlphaNBT ? "" : ",A=" + item.color.a * 255f) + ")\n");
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

    public bool CheckSurroundingPixels(int x, int y)
    {
        List<ColorObj> entityObjects = objs.FindAll(a => a.isEntityObject);

        for (int x1 = -1; x1 <= 1; x1++)
        {
            for (int y1 = -1; y1 <= 1; y1++)
            {
                if (y1 == 0 && x1 == 0)
                    continue;
                if (x + x1 < 0 || x + x1 > sprite.width || y + y1 < 0 || y + y1 > sprite.height)
                    continue;
                var color = sprite.GetPixel(x + x1, y + y1);
                if (entityObjects.Exists(a => a.color.r == color.r && a.color.g == color.g && a.color.b == color.b))
                {
                    return true;
                }
                if (color.a == 0)
                    return true;
            }
        }
        return false;
    }
    public void LoadLevel()
    {
        //objs.ForEach((x) => Debug.Log(x.obj.name + " " + x.hasAlphaNBT));
        /*if (GameObject.FindGameObjectWithTag("Player") != null)
             GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().lastCheckPoint = Vector3.zero;*/

        entityObjects.Clear();
        OBJPos.Clear();
        if (loadedOBJS.Count > 0)
        {
            for (int i = 0; i < loadedOBJS.Count; i++)
            {
                Destroy(loadedOBJS[i]);
            }
        }
        loadedOBJS = new List<GameObject>();
        loadedOBJS.Clear();
        ColorObj lastObject = null;
        for (int i = 0; i < sprite.width * sprite.height; i++)
        {
            int x = i % sprite.width;
            int y = i / sprite.width;
            Color c = sprite.GetPixel(x, y);
            if (c.a > 0)
            {
                if(lastObject != null)
                {
                    var item = lastObject;
                    if (!item.hasAlphaNBT)
                    {
                        if (c.Equals(item.color))
                        {
                            if (CreatorManager.instance != null)
                            {
                                if (item.obj.name == "Player")
                                {
                                    GameObject gb = Instantiate(CreatorManager.instance.playerSpawn, new Vector2(x, y) - offset + new Vector2(0, 0.365f), Quaternion.identity);
                                    gb.transform.localScale = gb.transform.localScale * (1 + (1 - GameManager.instance.levelLoader.SpaceBetweenOBJS));
                                    gb.transform.SetParent(transform);
                                }
                                else
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
                                }
                            }
                            else
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
                                else
                                {
                                    var needsCol = CheckSurroundingPixels(x, y);
                                    if (!needsCol)
                                        Destroy(gb.GetComponent<Collider2D>());
                                }
                                loadedOBJS.Add(gb);
                            }
                        }
                    }
                    else
                    {
                        if (c.r == item.color.r && c.b == item.color.b && c.g == item.color.g)
                        {
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
                            continue;
                        }

                    }
                }

                foreach (var item in objs)
                {
                    if (!item.hasAlphaNBT)
                    {
                        if (c.Equals(item.color))
                        {
                            if (CreatorManager.instance != null)
                            {
                                if (item.obj.name == "Player")
                                {
                                    GameObject gb = Instantiate(CreatorManager.instance.playerSpawn, new Vector2(x, y) - offset + new Vector2(0, 0.365f), Quaternion.identity);
                                    gb.transform.localScale = gb.transform.localScale * (1 + (1 - GameManager.instance.levelLoader.SpaceBetweenOBJS));
                                    gb.transform.SetParent(transform);
                                }
                                else
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
                                    
                                }
                            }
                            else
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
                                else
                                {
                                    var needsCol = CheckSurroundingPixels(x, y);
                                    if (!needsCol)
                                        Destroy(gb.GetComponent<Collider2D>());
                                }
                                loadedOBJS.Add(gb);
                            }
                        }
                    }
                    else
                    {
                        if (c.r == item.color.r && c.b == item.color.b && c.g == item.color.g)
                        {
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
                        }

                    }
                    lastObject = item;
                }
            }
        }
        StartCoroutine(waitForPlayerSkin());
    }

    public IEnumerator waitForPlayerSkin()
    {
        yield return null;
        var animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        var c = Array.Find(GameManager.instance.characters.ToArray(), (item => item.controller == animator.runtimeAnimatorController));
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
            if (item.Value.hasAlphaNBT)
                gb.AddComponent<AlphaNBTTag>().setNBT((int)(item.Value.color.a * 255));
        }
        OBJPos = dic;
    }
}
