using UnityEngine;

public class HandManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private IndexedItemTypeList items = new();

    private GameObject currentShowedItem = null;

    #endregion

    #region Accessors

    //public  Items


    #endregion


    #region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        HideAllItem();
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
    /// <summary>
    /// show the hand with the <paramref name="item"/>
    /// </summary>
    /// <param name="item"></param>
    public void ShowItem(ItemData.ITEM_TYPE item)
    {
        currentShowedItem = items.list[(int)item];
    }

    public void ShowCurrentItem()
    {
        currentShowedItem?.SetActive(true);
    }

    /// <summary>
    /// Hide the hand
    /// </summary>
    public void HideCurrentItem()
    {
        currentShowedItem?.SetActive(false);
    }

    public void HideAllItem()
    {
        foreach (GameObject item in items.list)
        {
            if (item == null) { continue; }
            item.SetActive(false);
        }
    }

    public void UseItem(PlayerAnimatorManager animatorManager)
    {
        currentShowedItem?.GetComponent<IUsableItem>()?.UseItem(animatorManager);
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}