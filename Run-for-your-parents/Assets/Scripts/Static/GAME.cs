using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    #region Variables
    [Tooltip("Game object that will be used to manage the game")]
    public static Game Instance;

    private string savePath => Application.persistentDataPath + "/save.json";

    private Transform north;

    private Dictionary<string, string> pathDict = new()
    {
        ["MainMenu"] = "Assets/Scenes/MainMenu.unity",
        ["Level"] = "Assets/Scenes/Level.unity",
        ["Landfill"] = "Assets/Scenes/Landfill.unity",
        ["DemoObject"] = "Assets/Scenes/DemoObjects.unity",
        ["DemoEntity"] = "Assets/Scenes/DemoEntity.unity",
    };

    public GameData data;

    private int sceneToLoad = 0;

    private bool dataRecovered = false;

    #endregion

    #region Accessors

    public Transform North
    {
        get => north;
        set
        {
            if (value == north) { return; }
            north = value;
        }
    }

    public int SceneToLoad
    {
        get => sceneToLoad;
        set
        {
            if (sceneToLoad == value) { return; }
            if (value < 0 || value >= SceneManager.sceneCountInBuildSettings) { sceneToLoad = 0; }
            else { sceneToLoad = value; }

        }
    }

    public string LoadingScene { get; } = "LoadingScreen";

    public bool DataRecovered { get => dataRecovered; }


    #endregion

    #region Built-In
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.data = new() { masterVolume = .5f, musicVolume = .5f, sfxVolume = .5f };
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitEventsListening();
        StartGame();
    }

    #endregion

    #region Methods
    private void InitEventsListening()
    {
        Events.Instance.SceneChanged.AddListener(OnEventsSceneChanged);
        Events.Instance.GenerationFinished.AddListener(OnEventsGenerationFinished);
    }

    private void StartGame()
    {
        RecoverSavedGameData();
        ProceduralGenerationManager proceduralGenerationManager = FindAnyObjectByType<ProceduralGenerationManager>();
        if (proceduralGenerationManager == null) { return; }
        //StopTime();
    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newScene"></param>
    public void ChangeScene(string sceneName)
    {
        ChangeScene(SceneUtility.GetBuildIndexByScenePath(pathDict.GetValueOrDefault(sceneName, "")));
    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void ChangeScene(int sceneIndex)
    {
        RestartTime();
        SceneToLoad = sceneIndex;
        SceneManager.LoadSceneAsync(LoadingScene);
    }

    public void RestartScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }


    //TODO
    /// <summary>
    /// 
    /// </summary>
    public void StopTime()
    {
        Time.timeScale = 0;

    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    public void RestartTime()
    {
        Time.timeScale = 1;
    }

    private void RecoverSavedGameData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);
        }

        SoundMixerManager.Instance.UpdateMixer(data);
        dataRecovered = true;
    }

    public void SaveGameData()
    {
        SoundMixerManager.Instance.RecoverAllVolume(ref data);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    #endregion

    #region Events

    private void OnEventsSceneChanged(string newScene)
    {
        ChangeScene(newScene);
    }

    private void OnEventsGenerationFinished()
    {
        RestartTime();
    }

    private void OnDestroy()
    {
        if(data == null) { return; }
        SaveGameData();
    }

    #endregion
}

[Serializable]
public class GameData
{
    [Header("Audio-Mixer")]
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}