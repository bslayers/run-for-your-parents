using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    #region Variables
    protected bool sound = true;

    #endregion

    #region Accessors
    public bool Sound
    {
        get
        {
            return sound;
        }
        set
        {
            if (value == sound) return;

            sound = value;

        }
    }

    #endregion


    #region Built-in
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
    private void DigitalizeFeedback()
    {
        return;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Digitalize()
    {
        DigitalizeFeedback();
        Destroy(gameObject);
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
