using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    [SerializeField] private Weapon currentWeapon;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private const float REFERENCE_BULLET_SPEED = 20;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        currentWeapon.bulletsInMagazine = currentWeapon.magazineCapacity;
    }

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
            { 
                player.weaponVisuals.PlayReloadAnimation();
            }
        };
    }

    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
    }

    public Weapon CurrentWeapon() => currentWeapon;

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            return;
        }

        weaponSlots.Add(newWeapon);
    }

    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1)
        {
            return;
        }

        weaponSlots.Remove(currentWeapon);
    }

    private void Shoot()
    {
        Debug.Log(currentWeapon.CanShoot());
        Debug.Log("fuck");

        if (currentWeapon.CanShoot() == false)
        {
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 2f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player?.aim?.CanAimPrecisely() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }

        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        return direction;
    }

    public Transform GunPoint() => gunPoint;
}
