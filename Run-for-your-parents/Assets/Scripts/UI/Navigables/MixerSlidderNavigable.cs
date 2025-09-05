using System.Collections;
using UnityEngine;

public class MixerSlidderNavigable : SliderNavigableSO
{
#region Variables

    [Tooltip("The name of the volume to be modified")]
    [SerializeField] protected SoundMixerManager.VolumeType volumeName;


#endregion

#region Accessors


#endregion


#region Built-in
    protected override void Start()
    {
        base.Start();
        StartCoroutine(DelayRecoveringValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#endregion

#region Methods
    public override void SliderValueChanged()
    {
        SoundMixerManager.Instance.SetVolume(volumeName, slider.value);
    }


    #endregion

    #region Coroutines

    IEnumerator DelayRecoveringValue()
    {
        while(!Game.Instance.DataRecovered)
        {
            yield return null;
        }

        oldValue = SoundMixerManager.Instance.GetVolumePercentage(volumeName);
        slider.value = oldValue;
    }

    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}