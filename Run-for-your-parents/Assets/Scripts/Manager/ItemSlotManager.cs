using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class ItemSlotManager : MonoBehaviour
{
    #region Variables
    [Tooltip("Image of the slot")]
    [SerializeField]
    private Image origin;
    [Tooltip("Image of the slot when not selected")]
    [SerializeField]
    private Sprite unselectedSlot;
    [Tooltip("Image of the slot when selected")]
    [SerializeField]
    private Sprite selectedSlot;
    [Tooltip("Text of the number of item in the slot")]
    [SerializeField]
    private TextMeshProUGUI numberOfItemText;

    [Tooltip("The image of the item in the slot")]
    [SerializeField]
    private Image edgeObject;
    [Tooltip("The image of the mute icon")]
    [SerializeField]
    private Image muteIcone;

    [HideInInspector]
    public ItemData item;

    private Queue itemPool = new();
    private bool sound = true;



    #endregion

    #region Accessors

    public int NbItem { get => itemPool.Count; }
    public bool Sound { get => sound; }


    #endregion


    #region Built-in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        edgeObject.sprite = unselectedSlot;
        UpdateSlot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods
    /// <summary>
    /// Update UI for select feedback in the slot
    /// </summary>
    public void Select()
    {
        edgeObject.sprite = selectedSlot;
    }

    /// <summary>
    /// Update UI for deselect feedback in the slot
    /// </summary>
    public void Deselect()
    {
        edgeObject.sprite = unselectedSlot;
    }

    /// <summary>
    /// Add item <paramref name="newItem"/> in emplacement and update UI
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns>0: add item successfuly<br/>
    /// 1: can't add item
    /// </returns>
    public int AddItem(ItemInteractable newItem)
    {
        if (item == newItem.Item && sound == newItem.Sound)
        {
            if (NbItem < item.MaxStack)
            {
                EnqueueItem(newItem);
            }
            else
            {
                return 1;
            }
        }
        else
        {
            item = newItem.Item;
            sound = newItem.Sound;
            EnqueueItem(newItem);
        }
        UpdateSlot();

        return 0;
    }

    private void EnqueueItem(ItemInteractable newItem)
    {
        itemPool.Enqueue(newItem);
        newItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update UI for removing the item
    /// </summary>
    public void RemoveItem()
    {
        item = null;
        sound = true;
        UpdateSlot();
    }

    /// <summary>
    /// Update UI for slot
    /// </summary>
    private void UpdateSlot()
    {
        if (!item)
        {
            origin.sprite = unselectedSlot;
        }
        else
        {
            origin.sprite = NbItem > 1 ? item.imageForMultipleItem : item.image;
        }

        muteIcone.enabled = !sound;

        if (NbItem <= 1)
        {
            numberOfItemText.text = "";
        }
        else
        {
            numberOfItemText.text = NbItem.ToString();
        }
    }

    /// <summary>
    /// Spawn item prefab and apply <paramref name="throwingForce"/> to throwing it. Update nbitem and UI after.
    /// </summary>
    /// <param name="camTransform">Camera position</param>
    /// <param name="spawnTransform">Position where the item will spawn</param>
    /// <param name="throwingForce"></param>
    /// <returns>0: still have items in the slot<br/>1: haven't item anymore in the slot</returns>
    public int ThrowItem(Transform camTransform, Transform spawnTransform, float throwingForce)
    {
        //instantiate a new prefab of item
        ItemInteractable interactable = (ItemInteractable)itemPool.Dequeue();
        GameObject objectItem = interactable.gameObject;
        objectItem.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        objectItem.SetActive(true);

        //calcultate the direction of the item
        Vector3 throwingDirection = (spawnTransform.position - camTransform.position).normalized;
        //add force rigibody of the item
        objectItem.GetComponent<Rigidbody>().linearVelocity = throwingDirection * throwingForce;

        return UpdateItem();
    }

    private int UpdateItem()
    {
        if (NbItem == 0)
        {
            RemoveItem();
            return 1;
        }

        UpdateSlot();
        return 0;
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
