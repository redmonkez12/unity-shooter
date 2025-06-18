using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController weaponVisualController;
    private PlayerWeaponController playerWeaponController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
        playerWeaponController = GetComponentInParent<PlayerWeaponController>();
    }

    public void ReloadIsOver()
    {
        weaponVisualController.ReturnRigWeightToOne();
        playerWeaponController.CurrentWeapon().ReloadBullets();
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
