using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Object : MonoBehaviour
{
    public Path_Object nextObject;
    public Path_Object lastObject;
    public BoxCollider2D coll;
    public bool isSet = false;


    public void Check() {

        checkColls(new Vector3(1, 0, 0));
        checkColls(new Vector3(0, 1, 0));
        checkColls(new Vector3(0, -1, 0));
        checkColls(new Vector3(-1, 0, 0));
    }

    void checkColls(Vector3 pos)
    {
        var colls = Physics2D.OverlapCircleAll(transform.position +pos , 0.1f);
        foreach (var item in colls)
        {
            if(item.tag == "Path")
            {
                if (!item.GetComponent<Path_Object>().isSet)
                {
                    nextObject = item.gameObject.GetComponent<Path_Object>();
                    nextObject.setColliderToOff(this);
                    nextObject.isSet = true;
                    nextObject.Check();
                }
            }
        }
    }

    public void setColliderToOff(Path_Object lastOBJ)
    {
        coll.enabled = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        lastObject = lastOBJ;

        if (lastObject != null)
        {
            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, lastObject.transform.position);
            lr.material = new Material(Shader.Find("Standard"));
            lr.startColor = new Color(0,0,0,0.1f);
            lr.endColor = new Color(0,0,0,0.1f);
        }
    }
}
