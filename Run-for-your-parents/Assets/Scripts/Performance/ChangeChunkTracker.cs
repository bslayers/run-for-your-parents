using UnityEngine;

public class ChangeChunkTracker : MonoBehaviour
{
    #region Variables

    private Vector2Int currentChunkCoord = Vector2Int.zero;
    private ChunkManager chunkManager;
    private bool stopping = false;
    private bool isMoving = false;

    #endregion

    #region Accessors

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            if (value == isMoving) { return; }
            isMoving = value;
            stopping = !isMoving;
            NoticeChunkManager();
        }
    }


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        chunkManager = FindAnyObjectByType<ChunkManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckChunkManager();
        chunkManager?.UpdateStoppingEntityInChunk(transform, ref currentChunkCoord);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods

    private void CheckChunkManager()
    {
        if (chunkManager == null) { chunkManager = FindAnyObjectByType<ChunkManager>(); if (chunkManager = null) { return; } }
    }

    private void NoticeChunkManager()
    {
        CheckChunkManager();
        chunkManager?.SurveyEntity(transform, isMoving, ref currentChunkCoord);
    }




    #endregion

    #region Coroutine


    #endregion

    #region Events

    void OnDestroy()
    {
        CheckChunkManager();
        chunkManager?.RemoveEntityFromSurveyList(transform);
    }

    #endregion

    #region Editor


    #endregion

}