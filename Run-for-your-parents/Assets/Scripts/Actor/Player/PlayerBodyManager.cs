using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBodyManager : PlayerComponentSO
{
    #region Variables

    private PlayerMotor motor;
    private PlayerItemManager itemManager;
    private MenusManager menusManager;


    [SerializeField]
    private BodyMemberList members = new();

    [Header("Sprite for voice")]
    [SerializeField, Tooltip("Sprite for the voice when unmuted")]
    private Sprite voiceUnMuted;
    [SerializeField, Tooltip("Sprite for the voice when muted")]
    private Sprite voiceMuted;


    private Color MemberExistingColor = Color.white;
    private Color MemberDigitalizedColor = Color.black;


    #endregion

    #region Accessors

    #endregion


    #region Built-in

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        motor = GetComponent<PlayerMotor>();
        itemManager = GetComponent<PlayerItemManager>();

        menusManager = player.menusManager;
    }

    #endregion

    #region Methods

    public bool GetExisting(BodyMemberType memberType)
    {
        return members[memberType]?.existing ?? false;
    }

    public void SetExisting(BodyMemberType memberType, bool value)
    {
        BodyMember member = members[memberType];

        if (member.existing == value) { return; }

        if (memberType == BodyMemberType.Voice)
        {
            member.ChangeExistingWithUpdate(value, voiceUnMuted, voiceMuted);
        }
        else
        {
            member.ChangeExistingWithUpdate(value, MemberExistingColor, MemberDigitalizedColor);
        }

        UpdateMember(ref member, memberType);
    }

    private void UpdateMember(ref BodyMember member, BodyMemberType memberType)
    {
        switch (memberType)
        {
            case BodyMemberType.RightHand:
            case BodyMemberType.LeftHand:
                Debug.Log("lose hand");
                itemManager.LoseHand(memberType);
                member.Disable();
                break;
            default:
                DigitalizeMember(member);
                break;
        }
    }


    private void DigitalizeConsequences()
    {
        if (!GetExisting(BodyMemberType.LeftFoot) || !GetExisting(BodyMemberType.RightFoot)) { motor.Crawl(); }
        if (!GetExisting(BodyMemberType.LeftLeg) || !GetExisting(BodyMemberType.RightLeg)) { motor.Slither(); }
        if (!GetExisting(BodyMemberType.LeftHand) && !GetExisting(BodyMemberType.RightHand) && motor.IsCrawling) { motor.Slither(); }
    }

    private void DigitalizeMemberFeedBack(BodyMember member)
    {

    }

    private void DigitalizeMember(BodyMemberType member)
    {
        DigitalizeMember(members[member]);
    }

    private void DigitalizeMember(BodyMember member)
    {
        DigitalizeMemberFeedBack(member);
        member.Disable();

        DigitalizeConsequences();
    }

    private void DigitalizeFeedback()
    {
        return;
    }

    public void Digitalize()
    {
        DigitalizeFeedback();
        menusManager.OpenMenu(MenuData.MenuType.GameOver);
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}

[Serializable]
public class BodyMember
{
    public Image icone;
    public bool existing = true;
    public GameObject[] memberObjects;

    public void Enable() { foreach (GameObject memberObject in memberObjects) { memberObject?.SetActive(true); } }
    public void Disable() { foreach (GameObject memberObject in memberObjects) { memberObject?.SetActive(false); } }

    public void UpdateIcone(Sprite whenExisting, Sprite whenNotExisting) { icone.sprite = existing ? whenExisting : whenNotExisting; }
    public void UpdateIcone(Color whenExisting, Color whenNotExisting) { icone.color = existing ? whenExisting : whenNotExisting; }

    public int SetExisting(bool value)
    {
        if (existing == value) { return 0; }
        existing = value;
        return 1;
    }

    public void ChangeExistingWithUpdate(bool value, Sprite whenExisting, Sprite whenNotExisting)
    {
        if (SetExisting(value) == 0) { return; }
        UpdateIcone(whenExisting, whenNotExisting);
    }

    public void ChangeExistingWithUpdate(bool value, Color whenExisting, Color whenNotExisting)
    {
        if (SetExisting(value) == 0) { return; }
        UpdateIcone(whenExisting, whenNotExisting);
    }
}

public enum BodyMemberType { RightHand, LeftHand, RightArm, LeftArm, RightLeg, LeftLeg, RightFoot, LeftFoot, Chest, Voice, Eyes };