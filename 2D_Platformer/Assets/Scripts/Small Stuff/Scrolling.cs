using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    static public float backgroundSize;
    public float offsetSize;
    public float paralaxSpeed;

    private GameObject player;
    private Transform cameraTransform;
    private Transform[] layers;
    private float viewzone;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.localScale = new Vector3(Camera.main.transform.localScale.x * ((Camera.main.orthographicSize / 10) + 1), Camera.main.transform.localScale.y * ((Camera.main.orthographicSize / 10) + 1));

        viewzone = Camera.main.pixelWidth/100;
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
        
        leftIndex = 0;
        rightIndex = layers.Length-1;
        ScrollRight();
        ScrollRight();
        //ScrollLeft();
    }

    private void Update()
    {
        transform.localScale = new Vector3(Camera.main.transform.localScale.x*((Camera.main.orthographicSize/10)+1), Camera.main.transform.localScale.y * ((Camera.main.orthographicSize / 10) + 1));
        backgroundSize = offsetSize*transform.localScale.x;
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        //Debug.Log(backgroundSize);
        float deltax = cameraTransform.position.x - lastCameraX;
        transform.position += Vector3.right * (deltax * paralaxSpeed);
        lastCameraX = cameraTransform.position.x;
        float leftCheck = (layers[leftIndex].transform.position.x + viewzone);
        float rightCheck = (layers[rightIndex].transform.position.x - viewzone);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        if (player.transform.position.x < leftCheck)
        {
            ScrollLeft();
            //Debug.Log("Left "+ player.transform.position.x+" "+ (layers[leftIndex].transform.position.x + viewzone) + " " + viewzone + "(" + leftCheck + ")");
        }
        if (player.transform.position.x > rightCheck)
        {
            ScrollRight();
            //Debug.Log("Right "+ player.transform.position.x + " "+ (layers[rightIndex].transform.position.x - viewzone)+" "+viewzone +"("+rightCheck+")");
        }
    }
    private void ScrollLeft()
    {
        int lastright = rightIndex;
        if (layers != null)
            if (layers[leftIndex] != null && layers[rightIndex] != null && Camera.main != null)
            {
                layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize) + Vector3.forward + new Vector3(0, Camera.main.transform.position.y, 0);
                leftIndex = rightIndex;
                rightIndex--;
                if (rightIndex < 1)
                {
                    rightIndex = layers.Length - 1;
                }
            }
    }
    public void ScrollRight()
    {
        int lastleft = leftIndex;
        if(layers != null)
            if (layers[leftIndex] != null && layers[rightIndex] != null && Camera.main != null)
            {
                layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize) + Vector3.forward + new Vector3(0, Camera.main.transform.position.y, 0);
                rightIndex = leftIndex;
                leftIndex++;
                if (leftIndex == layers.Length)
                {
                    leftIndex = 0;
                }
            }
    }
}
