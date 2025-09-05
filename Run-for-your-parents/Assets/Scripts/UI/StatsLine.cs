using TMPro;
using UnityEngine;

public class StatsLine : MonoBehaviour
{
    #region Variables
    [Tooltip("The text object that will display the value")]
    [SerializeField]
    private TextMeshProUGUI valueObject;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods
    public void UpdateValue(string value)
    {
        valueObject.text = value;
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}