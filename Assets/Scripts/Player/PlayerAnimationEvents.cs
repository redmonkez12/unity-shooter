using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        weaponVisualController.ReturnRigWeightToOne();
    }

    public void ReturnRig()
    {
        weaponVisualController.ReturnRigWeightToOne();
        weaponVisualController.ReturnWeightToLeftHandIK();
    }

    public void WeaponGrabIsOver()
    {

        weaponVisualController.SetBusyGrabbingWeaponTo(false);
    }
}
