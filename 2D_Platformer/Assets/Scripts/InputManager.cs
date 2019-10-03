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
        public string key;
        
        public KeyPair(string name, string key)
        {
            this.name = name;
            this.key = key;
        }
    }

    

    [System.Serializable]
    public class Axis
    {
        public string name;
        public string negKey;
        public string posKey;

        public Axis(string name,string negKey, string posKey)
        {
            this.negKey = negKey;
            this.posKey = posKey;
            this.name = name;
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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    public bool GetButtonDown(string name, KeyPreset preset)
    {
        return Input.GetKeyDown((preset.joystickName != "" ? preset.joystickName + " " : "") + preset.Find(name).key);
    }

    public bool GetButton(string name, KeyPreset preset)
    {
        return Input.GetKey((preset.joystickName != "" ? preset.joystickName + " " : "") + preset.Find(name).key);
    }

    public bool GetButtonUp(string name, KeyPreset preset)
    {
        return Input.GetKeyUp((preset.joystickName != "" ? preset.joystickName + " " : "") + preset.Find(name).key);
    }

    public int GetAxisRaw(string name, KeyPreset preset)
    {
        if (Input.GetKey((preset.joystickName != "" ? preset.joystickName + " " : "") + preset.FindAxis(name).posKey))
        {
            return 1;
        }
        if (Input.GetKey((preset.joystickName != "" ? preset.joystickName + " " : "") + preset.FindAxis(name).negKey))
        {
            return -1;
        }
        return 0;
    }

    public KeyPreset GetPreset(string name)
    {
        return presets.Find(x => x.name == name);
    }


}
