using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private TextMeshProUGUI loadingText;
    private string baseLoadingText = "Loading";
    [SerializeField]
    private float dotSpeed = 0.5f;


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
        StartCoroutine(AnimatedLoadingText());
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods


    #endregion

    #region Coroutine

    IEnumerator LoadSceneAsync()
    {
        FindAnyObjectByType<EventSystem>()?.gameObject.SetActive(false);
        AsyncOperation op = SceneManager.LoadSceneAsync(Game.Instance.SceneToLoad, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }


        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(Game.Instance.LoadingScene);
        while (!unloadOp.isDone)
        {
            yield return null;
        }

    }

    IEnumerator AnimatedLoadingText()
    {
        int dotCount = 0;

        while (true)
        {
            loadingText.text = baseLoadingText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(dotSpeed);
        }
    }


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}