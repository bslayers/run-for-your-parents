using UnityEngine;

public class WeightDetector : DetectorSO, IGenerator
{
    #region Variables

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string pressedParameterName = "Pressed";


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator?.SetBool(pressedParameterName, !isActive);
    }

    public void Generate()
    {
        IsActive = Random.Range(0, 4) == 0;
    }

    #endregion

    #region Methods

    protected override void DigitalizeBehaviour(Collider other)
    {
        animator.SetBool(pressedParameterName, true);
        base.DigitalizeBehaviour(other);
    }

    protected override void OnCollisionExitBehaviour(Collider collider)
    {
        animator.SetBool(pressedParameterName, false);
        base.OnCollisionExitBehaviour(collider);
    }

    protected override void IsActiveUpdated()
    {
        animator?.SetBool(pressedParameterName, !isActive);
    }

    #endregion


    #region Events

    #endregion

    #region Editor


    #endregion
}
