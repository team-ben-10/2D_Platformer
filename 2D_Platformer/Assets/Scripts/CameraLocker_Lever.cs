using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocker_Lever : LevelInteracte
{

    bool isLocked = false;
    public override void InteractOff()
    {
        isLocked = false;
        Debug.Log("DeLock Camera");
        GameObject virtualCam = GameObject.FindGameObjectWithTag("VirtualCam");
        virtualCam.GetComponent<CameraMovement>().enabled = true;
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
        Camera.main.orthographicSize = 8;
    }

    public override void InteractOn()
    {
        if (!isLocked)
        {
            isLocked = true;
            Debug.Log("Lock Camera");
            GameObject virtualCam = GameObject.FindGameObjectWithTag("VirtualCam");
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            virtualCam.GetComponent<CameraMovement>().enabled = false;
            Camera.main.orthographicSize = 20;
            Camera.main.transform.position = transform.position + new Vector3(0, 0, -10);
        }
    }
}
