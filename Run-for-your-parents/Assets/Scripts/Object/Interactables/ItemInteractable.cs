using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ChangeChunkTracker))]
public class ItemInteractable : InteractableObjectSO
{
    #region Variables
    private Rigidbody itemRigidbody;
    [Tooltip("The object that will be destroyed when the item is picked up")]
    [SerializeField]
    private GameObject digitalizeAura;
    [Tooltip("The data of the item")]
    [SerializeField]
    private ItemData item;
    private ChangeChunkTracker changeChunkTracker;

    private bool sound = true;
    private float currentSpeed;

    private bool detectCollision = false;

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

            if (digitalizeAura) digitalizeAura.SetActive(!sound);
        }
    }

    public ItemData Item
    {
        get
        {
            return item;
        }
    }

    #endregion


    #region Built-in
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemRigidbody = GetComponent<Rigidbody>();
        changeChunkTracker = GetComponent<ChangeChunkTracker>();

        if (digitalizeAura) digitalizeAura.SetActive(!sound);
        itemRigidbody.mass = item.mass;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = itemRigidbody.linearVelocity.magnitude;
        changeChunkTracker.IsMoving = currentSpeed >= 0.5;
        if (!changeChunkTracker.IsMoving)
        {
            itemRigidbody.linearVelocity = Vector3.zero;
            currentSpeed = 0;

        }
    }

    protected override void Interact(GameObject player, BodyMemberType member, Collider collider)
    {
        if (BodyMemberType.LeftArm == member && BodyMemberType.RightArm == member) return;

        player.GetComponent<PlayerItemManager>().AddItemInInventory(this, member);
    }

    #endregion

    #region Methods


    #endregion


    #region Events
    private void OnCollisionEnter(Collision collision)
    {
        if (!detectCollision) { return; }

        if (sound && item.contactSoundData != null)
        {
            SoundEmitor.EmiteSound(gameObject, item.contactSoundData, false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!detectCollision) { return; }

        if (currentSpeed != 0 && sound && item.staySoundData != null)
        {
            SoundEmitor.EmiteSound(gameObject, item.staySoundData, true);
        }
    }

    protected void OnEnable()
    {
        StartCoroutine(DelayCollisionDetection());
    }


    #endregion

    #region Coroutines

    IEnumerator DelayCollisionDetection()
    {
        detectCollision = false;
        yield return new WaitForSeconds(1f);
        detectCollision = true;
    }

    #endregion

    #region Editor


    #endregion
}
