using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
      
    Cinemachine.CinemachineVirtualCamera virtualCamera;
    public List<Transform> targets = new List<Transform>(); 

    public void Start()
    {
        virtualCamera=GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            targets.Add(GameObject.FindGameObjectWithTag("Player").transform);
            SetFollow(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    void Update()
    {

        if(targets.Count > 1)
        {
            virtualCamera.enabled = false;
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            foreach (var item in targets)
            {
                bounds.Encapsulate(item.position);
            }
            Camera.main.orthographicSize = Mathf.Lerp(8, 15, Vector2.Distance(targets[0].transform.position, targets[1].transform.position) / 15f);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(bounds.center.x, bounds.center.y,Mathf.Clamp(bounds.center.z, -50,-10)), 0.5f);
        }
        else
        {
            virtualCamera.enabled = true;
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
        }
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y, 8, 100000), Camera.main.transform.position.z);
    }

    public void SetFollow(Transform transform)
    {
        virtualCamera.Follow = transform;
    }
}
