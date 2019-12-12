using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField]
    List<KeyPreset> presets = new List<KeyPreset>();

    [System.Serializable]
    public class KeyPair
    {
        public string name;
        public KeyCode key;
        
        public KeyPair(string name, KeyCode key)
        {
            this.name = name;
            this.key = key;
        }
    }

    

    [System.Serializable]
    public class Axis
    {
        public string name;
        public KeyCode negKey;
        public KeyCode posKey;
        public bool isController;
        public string ControllerAxis;

        public Axis(string name, KeyCode negKey, KeyCode posKey, bool isController, string ControllerAxis)
        {
            this.negKey = negKey;
            this.posKey = posKey;
            this.name = name;
            this.isController = isController;
            this.ControllerAxis = ControllerAxis;
        }
    }

    [System.Serializable]
    public class KeyPreset
    {
        public string name;
        public string joystickName;
        public List<KeyPair> keyPairs = new List<KeyPair>();
        public List<Axis> axes = new List<Axis>();
        public KeyPair Find(string name)
        {
            return keyPairs.Find(x => x.name == name);
        }
        public Axis FindAxis(string name)
        {
            return axes.Find(x => x.name == name);
        }
    }

    public KeyCode GetPressedKey()
    {
        foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(item))
            {
                return item;
            }
        }
        return KeyCode.None;
    }

    private void Awake()
    {
        //Debug.Log(string.Join(", ", Input.GetJoystickNames()));
        DontDestroyOnLoad(gameObject);
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    public bool GetButtonDown(string name, KeyPreset preset)
    {
        return Input.GetKeyDown(preset.Find(name).key);
    }

    public bool GetButton(string name, KeyPreset preset)
    {
        return Input.GetKey(preset.Find(name).key);
    }

    public bool GetButtonUp(string name, KeyPreset preset)
    {
        return Input.GetKeyUp(preset.Find(name).key);
    }

    public int GetAxisRaw(string name, KeyPreset preset)
    {
        if (preset.FindAxis(name).isController)
        {
            return (int)Input.GetAxisRaw(preset.FindAxis(name).ControllerAxis);
        }
        else
        {
            if (Input.GetKey(preset.FindAxis(name).posKey))
            {
                return 1;
            }
            if (Input.GetKey(preset.FindAxis(name).negKey))
            {
                return -1;
            }
            return 0;
        }
    }

    public KeyPreset GetPreset(string name)
    {
        return presets.Find(x => x.name == name);
    }


}
