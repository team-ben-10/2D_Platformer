using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class CoinManager : MonoBehaviour
{
    public int coins;
    public int allCoins;

    public static CoinManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coins++;
    }

    public void SaveCoins()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/coins.kys";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, coins);
        stream.Close();
    }

    public int LoadCoins()
    {
        string path = Application.persistentDataPath + "/coins.kys";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int coins = (int)formatter.Deserialize(stream);
            stream.Close();

            return coins;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return -1;
        }
    }
    private void Start()
    {
        allCoins = LoadCoins();
    }
}
