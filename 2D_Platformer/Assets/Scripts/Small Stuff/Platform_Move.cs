using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Move : MonoBehaviour
{

    public float MaximumMove = 5;
    public Vector3 dir;
    Vector3 positionStart;
    public float time = 0.1f;
    [Header("Platform Path")]
    public bool moveAfterPath;
    public GameObject pathPrefab;
    public Path_Object pathOBJ;
    public float platformThreshold = 0.01f;
    public bool startFromBeginning = true;
    bool isStarted;
    public bool RepeatPath = true;
    public bool isCompleteCircle = true;

    [Header("Player Movement")]
    public bool movePlayerWithPlatform = true;

    GameObject spawn;

    private void OnDestroy()
    {
        Destroy(spawn);
    }

    void Start()
    {
        //GameManager.instance.onEntityLoad += OnEntityLoad;
        isStarted = startFromBeginning;
        positionStart = transform.position;
        if(moveAfterPath)
        {
            if (pathOBJ == null)
            {
                var gb = Instantiate(pathPrefab, transform.position, Quaternion.identity);//GameManager.instance.levelLoader.InstantiateColorOBJ(new Color(50 / 255f, 10 / 255f, 50 / 255f), new Vector2(transform.position.x, transform.position.y - 0.01f));
                spawn = gb;
                pathOBJ = gb.GetComponent<Path_Object>();
                if (!isCompleteCircle)
                {
                    pathOBJ.setColliderToOff(null);
                    pathOBJ.isSet = true;
                }
                pathOBJ.Check();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveAfterPath)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + dir, time);
            if (transform.position.x >= positionStart.x + MaximumMove)
            {
                dir.x *= -1;
            }
            if (transform.position.x <= positionStart.x - MaximumMove)
            {
                dir.x *= -1;
            }
            if (transform.position.y >= positionStart.y + MaximumMove)
            {
                dir.y *= -1;
            }
            if (transform.position.y <= positionStart.y - MaximumMove)
            {
                dir.y *= -1;
            }
        }
        else
        {
            if (isStarted)
            {
                if ((pathOBJ.nextObject == null && !RepeatPath && !isBackwards)|| (pathOBJ.lastObject == null && !RepeatPath && isBackwards))
                    return;
                if ((pathOBJ.nextObject == null && RepeatPath && !isBackwards) || (pathOBJ.lastObject == null && RepeatPath && isBackwards))
                    isBackwards = !isBackwards;
                if (transform.position.x >= pathOBJ.transform.position.x - platformThreshold && transform.position.x <= pathOBJ.transform.position.x + platformThreshold &&
                    transform.position.y >= pathOBJ.transform.position.y - platformThreshold && transform.position.y <= pathOBJ.transform.position.y + platformThreshold)
                {
                    pathOBJ = isBackwards ? pathOBJ.lastObject:pathOBJ.nextObject;
                }
                transform.position = Vector3.MoveTowards(transform.position, pathOBJ.transform.position, time);
            }
        }
    }

    bool isBackwards = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (movePlayerWithPlatform)
        {
            if (collision.transform.tag.StartsWith("Player"))
            {
                collision.transform.SetParent(transform);
                isStarted = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (movePlayerWithPlatform)
        {
            if (collision.transform.tag.StartsWith("Player"))
            {
                collision.transform.SetParent(null);
            }
        }
    }
}
