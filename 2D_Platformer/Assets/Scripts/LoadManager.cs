using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    [SerializeField]
    Image loadingBar_Fill;
    [SerializeField]
    GameObject loadingScrene;

    public TMPro.TextMeshProUGUI levelText;
    public Image background;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
    }

    AsyncOperation operation;

    public void LoadScene(string i)
    {
        loadingScrene.SetActive(true);
        operation = SceneManager.LoadSceneAsync(i);
    }

    private void Update()
    {
        if (operation != null)
        {
            if (!operation.isDone)
            {
                loadingBar_Fill.fillAmount = operation.progress;
            }
            else
            {
                loadingScrene.SetActive(false);
            }
        }
    }
}
