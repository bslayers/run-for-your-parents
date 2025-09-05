using System;
using UnityEngine;

public class StatisticManager : MonoBehaviour
{
    #region Variables
    [Tooltip("The GameObject that will be used to get the player body manager")]
    [SerializeField] private GameObject player;
    private PlayerBodyManager bodyManager;
    [Tooltip("The GameObject that will be used to get the distance from the landfill")]
    public StatsLine distanceFromLandfill;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        //bodyManager = player.GetComponent<PlayerBodyManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods
    public void UpdateStatistics()
    {
        distanceFromLandfill.UpdateValue(Math.Round(Vector3.Distance(Vector3.zero, player.transform.position), 2).ToString());
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}