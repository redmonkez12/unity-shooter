using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;
    private int currentWeaponIndex = 0; // Pro tracking aktuální zbraně

    private Rig rig;

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("Left Hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    [SerializeField] private float leftHandIncreaseStep;
    private bool shouldIncreaseLeftHandIKWeight;

    private bool busyGrabbingWeapon;

    // PŘIDÁNO: Cooldown systém pro prevenci spam switchingu
    [Header("Switch Cooldown")]
    [SerializeField] private float switchCooldown = 0.3f;
    private float lastSwitchTime = 0f;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        
        // OPRAVENO: Inicializace na začátku
        SwitchOn(pistol);
        SwitchAnimationLayer(1); // Layer pro pistol
        currentWeaponIndex = 0;
    }

    public void ReturnRigWeightToOne() => rigShouldBeIncreased = true;
    public void ReturnWeightToLeftHandIK() => shouldIncreaseLeftHandIKWeight = true;

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && busyGrabbingWeapon == false)
        {
            PlayReloadAnimation();
            PauseRig();
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    public void PlayReloadAnimation()
    {
        if (busyGrabbingWeapon)
        {
            return;
        }

        anim.SetTrigger("Reload");
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
                rig.weight = 1f; // OPRAVENO: Ujisti se, že je přesně 1
            }
        }
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIncreaseStep * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                shouldIncreaseLeftHandIKWeight = false;
                leftHandIK.weight = 1f; // OPRAVENO: Ujisti se, že je přesně 1
            }
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
        rigShouldBeIncreased = false; // PŘIDÁNO: Reset flag
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        // OPRAVENO: Kontrola, jestli už není busy
        if (busyGrabbingWeapon)
        { 
            return;
        }

        leftHandIK.weight = 0;
        shouldIncreaseLeftHandIKWeight = false; // PŘIDÁNO: Reset flag
        PauseRig();
        
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busyOrNot)
    {
        busyGrabbingWeapon = busyOrNot;
        anim.SetBool("BusyGrabbingWeapon", busyOrNot);
    }

    private void CheckWeaponSwitch()
    {
        // PŘIDÁNO: Kontrola cooldownu a busy stavu
        if (busyGrabbingWeapon || Time.time - lastSwitchTime < switchCooldown)
        { 
            return;
        }

        int targetWeaponIndex = -1;
        Transform targetWeapon = null;
        int targetLayer = 1;
        GrabType grabType = GrabType.SideGrab;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            targetWeaponIndex = 0;
            targetWeapon = pistol;
            targetLayer = 1;
            grabType = GrabType.SideGrab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            targetWeaponIndex = 1;
            targetWeapon = revolver;
            targetLayer = 1;
            grabType = GrabType.SideGrab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            targetWeaponIndex = 2;
            targetWeapon = autoRifle;
            targetLayer = 1;
            grabType = GrabType.BackGrab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            targetWeaponIndex = 3;
            targetWeapon = shotgun;
            targetLayer = 2;
            grabType = GrabType.BackGrab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            targetWeaponIndex = 4;
            targetWeapon = rifle;
            targetLayer = 3;
            grabType = GrabType.BackGrab;
        }

        // OPRAVENO: Kontrola, jestli není stejná zbraň
        if (targetWeaponIndex != -1 && targetWeaponIndex != currentWeaponIndex)
        {
            SwitchWeapon(targetWeapon, targetLayer, grabType, targetWeaponIndex);
        }
    }

    // PŘIDÁNO: Centralizovaná metoda pro switch
    private void SwitchWeapon(Transform weapon, int layerIndex, GrabType grabType, int weaponIndex)
    {
        lastSwitchTime = Time.time;
        currentWeaponIndex = weaponIndex;

        SwitchOn(weapon);
        SwitchAnimationLayer(layerIndex);
        PlayWeaponGrabAnimation(grabType);
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        // OPRAVENO: Null check pro bezpečnost
        if (currentGun == null)
        {
            return;
        }

        LeftHandTargetTransform leftHandTarget = currentGun.GetComponentInChildren<LeftHandTargetTransform>();
        if (leftHandTarget == null)
        {
            Debug.LogWarning($"LeftHandTargetTransform not found on {currentGun.name}!");
            return;
        }

        Transform targetTransform = leftHandTarget.transform;
        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        // OPRAVENO: Bezpečnostní kontrola indexu
        if (layerIndex >= anim.layerCount)
        {
            Debug.LogWarning($"Layer index {layerIndex} is out of range! Max layers: {anim.layerCount}");
            return;
        }

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

}

public enum GrabType
{
    SideGrab,
    BackGrab,
}