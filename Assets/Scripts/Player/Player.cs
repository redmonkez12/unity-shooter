using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; }

    public PlayerMovement movement { get; private set; }
    public PlayerWeaponController weaponController { get; private set; }
    public WeaponVisualController weaponVisuals { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weaponController = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<WeaponVisualController>();
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.Enable();
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }
}
