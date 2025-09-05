using UnityEngine;

public class PlayerItemManager : PlayerComponentSO
{
    #region Variables

    private PlayerBodyManager bodyManager;
    private PlayerAnimatorManager animatorManager;
    private Camera cam;
    [Tooltip("The spawn point of the item")]
    [SerializeField] private GameObject spawnPoint;
    [Tooltip("The list of the item slot in the inventory")]
    [SerializeField] private GameObject[] slotList;

    private int rightHandItemSelected = -1;
    private int leftHandItemSelected = -1;

    private bool noHand = false;

    [Header("Hands")]
    private IndexedHandManagerList handManagers;


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Update is called once per frame
    void Update()
    {

    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        bodyManager = GetComponent<PlayerBodyManager>();
        animatorManager = player.animatorManager;
        cam = GetComponent<PlayerLook>().cam;
    }

    protected override void StartPlayer()
    {
        base.StartPlayer();
        handManagers = player.handManagers;
    }

    #endregion

    #region Methods

    public void LoseHand(BodyMemberType member)
    {
        switch (member)
        {
            case BodyMemberType.RightHand:
                LoseHand(Hand.Right);
                break;
            case BodyMemberType.LeftHand:
                LoseHand(Hand.Left);
                break;
        }
    }

    public void LoseHand(Hand hand)
    {
        int index = hand == Hand.Left ? leftHandItemSelected : rightHandItemSelected;

        if (index != -1)
        {
            ItemSlotManager slot = slotList[index].GetComponent<ItemSlotManager>();
            if (slot.item != null)
            {
                UpdateItemInHand(hand, false);
                animatorManager.TriggerLoseItem(hand);
            }

            slot.Deselect();
        }

        bool otherHandExisting = bodyManager.GetExisting(hand == Hand.Left ? BodyMemberType.RightHand : BodyMemberType.LeftHand);
        if (!otherHandExisting) { noHand = true; }
    }

    private Hand GetHandFromRef(ref int hand)
    {
        return hand == leftHandItemSelected ? Hand.Left : Hand.Right;
    }

    /// <summary>
    /// Select the item at <paramref name="emplacementIndex"/> and put it in <paramref name="hand"/>.
    /// </summary>
    /// <param name="emplacementIndex">a integer between [0,5] represent the index of item emplacement in the inventory</param>
    /// <param name="hand">reference to a integer representing the right or the left hand of player</param>
    private void SelectedItem(int emplacementIndex, ref int hand)
    {
        ItemSlotManager slot = null;
        Hand member = GetHandFromRef(ref hand);
        //deselect previous item
        if (hand != -1)
        {
            slot = slotList[hand].GetComponent<ItemSlotManager>();
            //update animation
            if (slot.item != null) { animatorManager.TriggerLoseItem(member); }
            animatorManager.SetShowItem(GetHandFromRef(ref hand), false);

            animatorManager.ObjectRotationInHandFollowCamera[member] = slot.item?.objectRotationFollowingCamera ?? false;

            //update ui
            slot.Deselect();
        }

        //select new item
        hand = emplacementIndex;

        slot = slotList[hand].GetComponent<ItemSlotManager>();
        //Update the UI
        slot.Select();
        //update animation
        if (slot.item) { UpdateItemInHand(GetHandFromRef(ref hand), slot.item.type, true); }

    }

    /// <summary>
    /// show or hide an unknown item in <paramref name="hand"/>
    /// prefer use this methode for hiding the item in the hand
    /// </summary>
    /// <param name="hand">the hand where perform the updating</param>
    /// <param name="showing">a boolean that indicate the action to perform : true = show ; false = hide</param>
    private void UpdateItemInHand(Hand hand, bool showing)
    {
        UpdateItemInHand(hand, ItemData.ITEM_TYPE.UNKNOWN, showing);
    }

    /// <summary>
    /// Show or Hide the item in <paramref name="hand"/>
    /// </summary>
    /// <param name="hand">the hand where perform the updating</param>
    /// <param name="itemType">the item to show (not necessary for hiding)</param>
    /// <param name="showing">a boolean that indicate the action to perform : true = show ; false = hide</param>
    private void UpdateItemInHand(Hand hand, ItemData.ITEM_TYPE itemType, bool showing)
    {
        animatorManager.SetShowItem(hand, showing);

        if (showing) { handManagers[hand].ShowItem(itemType); }
    }

    /// <summary>
    /// Select the item at emplacement <paramref name="slotIndex"/> in the inventory. Choose automaticaly the hand which select the item
    /// </summary>
    /// <param name="slotIndex">a integer between [0,5] represent the index of item emplacement in the inventory</param>
    public void SelectedItem(int slotIndex)
    {
        if (!IsValideEmplacement(slotIndex)) return;

        if (noHand) return;

        if (!bodyManager.GetExisting(BodyMemberType.RightHand))
        {
            SelectedItem(slotIndex, ref leftHandItemSelected);
        }
        else if (!bodyManager.GetExisting(BodyMemberType.LeftHand))
        {
            SelectedItem(slotIndex, ref rightHandItemSelected);
        }
        else
        {
            //check if the emplacement is for the right hand of the left hand
            if (slotIndex <= 2)
            {
                SelectedItem(slotIndex, ref leftHandItemSelected);
            }
            else
            {
                SelectedItem(slotIndex, ref rightHandItemSelected);
            }
        }

    }

    /// <summary>
    /// Use item in slot refered by parameter <paramref name="hand"/>
    /// </summary>
    /// <param name="hand">reference to a integer representing the right or the left hand of player</param>
    public void ThrowItemInInventory(ref int hand)
    {
        //check if hand select emplacement
        if (hand == -1) return;

        ItemSlotManager item = slotList[hand].GetComponent<ItemSlotManager>();

        //check if item at emplemcentIndex exist
        if (!(item && item.item)) return;

        int status = -1;
        ItemData.ITEM_TYPE tmpItemType = item.item.type;
        status = item.ThrowItem(cam.transform, spawnPoint.transform, 20f);
        AnimateThrowing(GetHandFromRef(ref hand));

        //update animation
        if (status == 1)
        {
            UpdateItemInHand(GetHandFromRef(ref hand), tmpItemType, false);
        }

        if (status != -1) { animatorManager.TriggerLoseItem(GetHandFromRef(ref hand)); }
    }

    public void UseItemInInventory(ref int hand)
    {
        if (hand == -1) return;
        ItemSlotManager item = slotList[hand].GetComponent<ItemSlotManager>();

        //check if item at emplemcentIndex exist
        if (item?.item?.usingType != ItemData.USING_TYPE.UsingItem) return;
        handManagers[GetHandFromRef(ref hand)].UseItem(animatorManager);
    }

    private void AnimateThrowing(Hand hand)
    {
        animatorManager.SetShowItem(hand, false);
        animatorManager.TriggerLoseItem(hand);

    }

    /// <summary>
    /// Use item in the <paramref name="hand"/>
    /// </summary>
    /// <param name="hand">represent which hand of player want to use item</param>
    public void ActionItemInInventory(BodyMemberType hand, ItemData.USING_TYPE actionType)
    {
        if (!IsHand(hand)) return;

        if (noHand) return;

        int tmpHand = hand == BodyMemberType.LeftHand ? leftHandItemSelected : rightHandItemSelected;

        switch (actionType)
        {
            case ItemData.USING_TYPE.ThrowingItem:
                ThrowItemInInventory(ref tmpHand);
                break;
            case ItemData.USING_TYPE.UsingItem:
                UseItemInInventory(ref tmpHand);
                break;
        }

    }

    /// <summary>
    /// Check if <paramref name="slotIndex"/> is a existing slot in inventory
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns>true if value is a valid slot, false otherwise</returns>
    public bool IsValideEmplacement(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > 5)
        {
            Debug.Log("SelectedItem() error: Invalid index: " + slotIndex);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Add <paramref name="newItemInteractable"/> in inventory and update the item in hand if necessary
    /// </summary>
    /// <param name="newItemInteractable">a ScriptableObect Item representing the data of the item to add in inventory</param>
    /// <param name="hand">represent which member of player want to add item</param>
    public void AddItemInInventory(ItemInteractable newItemInteractable, BodyMemberType hand)
    {
        if (isArm(hand)) return;
        if (!IsHand(hand)) return;

        int begin, end, firstVerification;

        if (BodyMemberType.LeftHand == hand)
        {
            begin = 0;
            if (bodyManager.GetExisting(BodyMemberType.RightHand)) end = 2;
            else end = 5;

            if (leftHandItemSelected == -1) firstVerification = begin;
            else firstVerification = leftHandItemSelected;
        }
        else
        {
            end = 5;
            if (bodyManager.GetExisting(BodyMemberType.LeftHand)) begin = 3;
            else begin = 0;

            if (rightHandItemSelected == -1) firstVerification = begin;
            else firstVerification = rightHandItemSelected;
        }

        int index = GetIndexToPlaceItem(newItemInteractable, begin, end, firstVerification);
        if (index == -1) return;

        if (index == leftHandItemSelected)
        {
            UpdateItemInHand(Hand.Left, newItemInteractable.Item.type, true);
        }
        else if (index == rightHandItemSelected)
        {
            UpdateItemInHand(Hand.Right, newItemInteractable.Item.type, true);
        }

        slotList[index].GetComponent<ItemSlotManager>().AddItem(newItemInteractable);

        return;
    }

    /// <summary>
    /// Check if <paramref name="member"/> pass into parameter is a hand
    /// </summary>
    /// <param name="member">represent a member of player</param>
    /// <returns>true if the member is a hand, false otherwise</returns>
    private bool IsHand(BodyMemberType member)
    {
        if (BodyMemberType.LeftHand != member && BodyMemberType.RightHand != member)
        {
            Debug.Log("Unknown member: " + member, this);
            return false;
        }
        return true;
    }

    private bool isArm(BodyMemberType member)
    {
        return BodyMemberType.RightArm == member || BodyMemberType.LeftArm == member;
    }

    /// <summary>
    /// Search an emplacement to put <paramref name="newItem"/> in inventory
    /// </summary>
    /// <param name="newItem">a ItemData representing the informations of the item</param>
    /// <param name="begin">an integer which is the start index where we check</param>
    /// <param name="end">an integer which is the last index where we check</param>
    /// <param name="firstVerification">the first index to check if is only a void emplacement</param>
    /// <returns>the index in the inventory where put item, -1 if is impossible</returns>
    private int GetIndexToPlaceItem(ItemInteractable newItem, int begin, int end, int firstVerification)
    {
        int index = -1;

        if (slotList[firstVerification].GetComponent<ItemSlotManager>().item == null) index = firstVerification;

        for (int i = begin; i <= end; i++)
        {
            ItemSlotManager slot = slotList[i].GetComponent<ItemSlotManager>();
            //find a void emplacement
            if (slot.item == null && index == -1)
            {
                index = i;
            }

            //find an emplacement with the same item
            if (slot.item == newItem.Item && slot.NbItem < newItem.Item.MaxStack && slot.Sound == newItem.Sound)
            {
                index = i;
                break;
            }

        }

        return index;
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}

public enum Hand { Left, Right };
