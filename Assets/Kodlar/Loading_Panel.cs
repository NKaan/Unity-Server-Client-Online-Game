using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_Panel : MonoBehaviour
{

    public static Loading_Panel loading_Panel = null;
    public GameObject login_screen;
    public Slider slider;
    public Text progressText;

    private void Awake()
    {
        loading_Panel = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this); // Sahneler Arası Kaybolmamasını Sağlıyor
        login_screen.SetActive(false);
    }

    public void LoadLevel(int sahneNumaram)
    {
        login_screen.SetActive(true);
        StartCoroutine(LoadAsynchronously(sahneNumaram));


    }

    IEnumerator LoadAsynchronously(int sahneNumarasi)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sahneNumarasi);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100 + "%";

            yield return null;

        }

        login_screen.SetActive(false);
        DataSender.SendMerhabaServer();

    }


}
