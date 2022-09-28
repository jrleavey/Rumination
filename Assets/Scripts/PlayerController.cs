using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool isMoving;
    [SerializeField]
    private bool isAiming;
    [SerializeField]
    private bool isUsingHandgun = true;
    private bool isInvincible = false;
    public bool aButton;
    public bool menuButton;
    public bool leftAnalogStickVertical;
    public bool leftTrigger;
    [SerializeField]
    private bool haveIFoundShotgun = false;
    [SerializeField]
    private bool canShowMedkitPrompt = true;
    [SerializeField]
    private bool istTimedStopped = false;
    [SerializeField]
    private bool isJournalActive = false;
    [SerializeField]
    private bool canIUnpause = false;
    [SerializeField]
    private bool doIHaveTheKey = false;
    [SerializeField]
    private bool isTheGamePaused = false;
    [SerializeField]
    private bool introductionGameStartOn = true;
    [SerializeField]
    private bool amIInteracting = false;
    [SerializeField]
    private bool haveIPickedUpAmmo = false;
    [SerializeField]
    private bool isDying = false;


    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _speed;
    private float _horizMove;
    private float _vertMove;
    private float gunDamage = 1;
    [SerializeField]
    private float fireRate = 5f;
    [SerializeField]
    private float nextFire = -1f;
    public float leftAnalogStickHorizontal;


    [SerializeField]
    private int _handgunAmmo;
    [SerializeField]
    private int _shotgunAmmo;
    [SerializeField]
    private int _currentHP;
    [SerializeField]
    private int _maxHp = 5;


    private Animator _anim;
    [SerializeField]
    private GameObject[] RaycastHolders;
    [SerializeField]
    private GameObject[] _weapons;
    [SerializeField]
    private GameObject _footstepHolder;


    [SerializeField]
    private AudioClip _handgunShot;
    [SerializeField]
    private AudioClip _shotgunShot;
    [SerializeField]
    private AudioClip _outOfAmmo;
    [SerializeField]
    private AudioClip _heal;
    [SerializeField]
    private AudioClip _pickuphAmmo;
    [SerializeField]
    private AudioClip _pickupsAmmo;
    [SerializeField]
    private AudioClip _pickupShotgun;


    private PlayerControls _playerControls;


    [SerializeField]
    private UIManager _uimanager;
    [SerializeField]
    private GameObject gameStartMenu;
    [SerializeField]
    private Text _healthTxt;
    [SerializeField]
    private Text _ammoTxt;
    [SerializeField]
    private GameObject handgunAmmoText;
    [SerializeField]
    private GameObject shotgunAmmoText;
    [SerializeField]
    private GameObject medkitText;
    [SerializeField]
    private GameObject fullHealthText;
    [SerializeField]
    private GameObject journalText;
    [SerializeField]
    private GameObject journal1Text;
    [SerializeField]
    private GameObject journal2Text;
    [SerializeField]
    private GameObject journal3Text;
    [SerializeField]
    private GameObject journal4Text;
    [SerializeField]
    private GameObject shotgunText;
    [SerializeField]
    private GameObject keyText;
    [SerializeField]
    private GameObject lockedDoorNoKeyText;
    [SerializeField]
    private GameObject lockedDoorWithKeyText;





    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        istTimedStopped = true;
        _rotateSpeed = 200f;
        _speed = 1.7f;
        _currentHP = _maxHp;

        _playerControls = new PlayerControls();
        _playerControls.Controller.Enable();
        InputSetup();

        _anim.SetBool("isUsingHandgun", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        MovementController();
        WeaponControls();
        VisibleWeapon();

        _healthTxt.text = "Health: " + _currentHP;



        if (istTimedStopped == false)
        {
            Time.timeScale = 1;
        }
        else if (istTimedStopped == true)
        {
            Time.timeScale = 0;
        }


        if (amIInteracting == true)
        {
            _speed = 0;
        }
        else if (amIInteracting == false && isAiming == false && isDying == false)
        {
            _speed = 1.7f;
        }


        if (isAiming == false)
        {
            if (isUsingHandgun == true)
            {
                RaycastHolders[0].SetActive(false);
            }
            else
            {
                RaycastHolders[1].SetActive(true);

            }
        }

        if (aButton == true && canIUnpause == true && istTimedStopped == true)
        {
            KillAllJournals();
            istTimedStopped = false;
        }
    }

    private void InputSetup()
    {
        _playerControls.Controller.LeftTrigger.performed += LeftTrigger_performed;
        _playerControls.Controller.LeftTrigger.canceled += LeftTrigger_canceled;
        _playerControls.Controller.LeftStickClick.performed += LeftStickClick_performed;
        _playerControls.Controller.Menu.performed += Menu_performed;
        _playerControls.Controller.Interact.performed += Interact_performed;
    }
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        aButton = true;
        StartCoroutine(ButtonFailsafeEnd());

        if (introductionGameStartOn == true)
        {
            gameStartMenu.SetActive(false);
            istTimedStopped = false;
            introductionGameStartOn = false;
        }
    }

    private void Menu_performed(InputAction.CallbackContext obj)
    {
        if (gameStartMenu == true)
        {
            istTimedStopped = true;
            _uimanager.GetComponent<UIManager>().Pause();
            isTheGamePaused = true;
        }    
    }

    private void LeftStickClick_performed(InputAction.CallbackContext obj)
    {
        if (haveIFoundShotgun == true)
        {
            isUsingHandgun = !isUsingHandgun;
            SwapWeapon();
        }
    }

    private void LeftTrigger_canceled(InputAction.CallbackContext obj)
    {
        leftTrigger = false;
    }

    private void LeftTrigger_performed(InputAction.CallbackContext obj)
    {
        leftTrigger = true;
    }

    private void MovementController()
    {
        Vector2 leftStick = _playerControls.Controller.LeftStickMovement.ReadValue<Vector2>();

        if (Mathf.Abs(leftStick.y) > 0.2f)
        {
            isMoving = true;
            _vertMove = leftStick.y * Time.deltaTime * _speed;
            this.gameObject.transform.Translate(0, 0, _vertMove * _speed);

            if (istTimedStopped == false && isAiming == false)
            {
                _footstepHolder.SetActive(true);

            }

        }
        else
        {
            isMoving = false;
            _footstepHolder.SetActive(false);
        }
        if (Mathf.Abs(leftStick.x) > 0.2f)
        {
            _horizMove = leftStick.x * Time.deltaTime * _rotateSpeed;
            this.gameObject.transform.Rotate(0, _horizMove, 0);
        }


        if (isMoving == true)
        {
            _anim.SetBool("isMoving", true);
        }
        else
        {
            _anim.SetBool("isMoving", false);

        }
    }
    private void WeaponControls()
    {
        if (leftTrigger == true)
        {
            if (isUsingHandgun == true)
            {
                RaycastHolders[0].SetActive(true);
            }
            else
            {
                RaycastHolders[1].SetActive(true);

            }
            isAiming = true;
            _speed = 0;
            _anim.SetBool("isAiming", true);
        }
        else
        {
            _anim.SetBool("isAiming", false);
            isAiming = false;
            _speed = 1.7f;

        }

        if (isAiming == true && aButton == true && _handgunAmmo >= 1 && isUsingHandgun == true && Time.time > nextFire)
        {
            aButton = false;
            RaycastHit hit;
            nextFire = Time.time + fireRate;

            if (Physics.Raycast(RaycastHolders[0].transform.position, RaycastHolders[0].transform.forward, out hit, Mathf.Infinity))
            {
                hit.collider.SendMessage("Damage", gunDamage);
                Debug.DrawLine(RaycastHolders[0].transform.position, hit.point, Color.red, 1f);
            }
            _handgunAmmo--;
            AudioSource.PlayClipAtPoint(_handgunShot, transform.position);
        }
        else if (isAiming == true && aButton == true && _shotgunAmmo >= 1 && isUsingHandgun == false && Time.time > nextFire)
        {
            {
                aButton = false;
                nextFire = Time.time + fireRate;
                int amountOfProjectiles = 8;
                for (int i = 0; i < amountOfProjectiles; i++)
                {
                    ShotgunRay();
                }

                _shotgunAmmo--;
                AudioSource.PlayClipAtPoint(_shotgunShot, transform.position);
            }
        }
        else if (isAiming == true && aButton == true && _handgunAmmo <= 0)
        {
            aButton = false;
            AudioSource.PlayClipAtPoint(_outOfAmmo, transform.position);
        }
        else if (isAiming == true && aButton == true && _shotgunAmmo <= 0 && isUsingHandgun == false)
        {
            aButton = false;
            AudioSource.PlayClipAtPoint(_outOfAmmo, transform.position);
        }

        if (isAiming == true)
        {
            isMoving = false;
        }
        if (isUsingHandgun == true)
        {
            fireRate = .5f;
        }
        else
        {
            fireRate = 1.7f;
        }

    }
    private void ShotgunRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(RaycastHolders[1].transform.position, RaycastHolders[1].transform.forward, out hit, Mathf.Infinity))
        {
            Vector3 direction = RaycastHolders[1].transform.forward;
            Vector3 spread = Vector3.zero;
            spread += RaycastHolders[1].transform.up * Random.Range(-.05f, .05f);
            spread += RaycastHolders[1].transform.right * Random.Range(-.05f, .05f);
            direction += spread.normalized * Random.Range(0f, 0.2f);
            hit.collider.SendMessage("Damage", gunDamage);

            if (Physics.Raycast(RaycastHolders[1].transform.position, direction, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(RaycastHolders[1].transform.position, hit.point, Color.red, 1f);
            }
        }
    }
    public void Heal(int healingAmount)
    {
        Debug.Log("Player has healed");
        _currentHP = _currentHP + healingAmount;


    }
    public void HandgunAmmoPickup()
    {
        // Run logic for gaining bullets
        Debug.Log("Player has picked up rounds");
    }
    public void ShotgunAmmoPickup()
    {
        // Run logic for gaining Shells
        Debug.Log("Player has picked up shells");
    }
    public void KeyPickup()
    {
        //Array needed for keys
        Debug.Log("Player has picked up Key");

    }
    public void TookDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(BecomeInvincible());

    }
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(1f);

        isInvincible = false;
    }
    public void Die()
    {
        isDying = true;
        _speed = 0;
        _anim.SetTrigger("isDying");
        StartCoroutine(DeathAnimTimer());
    }
    private void SwapWeapon()
    {
        if (isUsingHandgun == true)
        {
            Debug.Log("Handgun");
        }
        else
        {
            Debug.Log("Shotgun");
        }
        if (isUsingHandgun == true)
        {
            _ammoTxt.text = "Ammo: " + _handgunAmmo;
        }
        else
        {
            _ammoTxt.text = "Ammo" + _shotgunAmmo;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hunter_Damage_hit")
        {
            TookDamage(2);
        }
        if (other.tag == "Zombie_Damage_hit")
        {
            TookDamage(1);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Handgun Ammo")
        {
            handgunAmmoText.SetActive(true);
            if (aButton == true)
            {

                if (amIInteracting == false)
                {
                    _handgunAmmo += 10;
                    haveIPickedUpAmmo = true;
                    _anim.SetTrigger("Interact");
                    amIInteracting = true;
                    StartCoroutine(InteractTimer());
                }
                Destroy(other.gameObject);
                handgunAmmoText.SetActive(false);
                AudioSource.PlayClipAtPoint(_pickuphAmmo, transform.position);

            }

        }
        if (other.tag == "Shotgun Ammo")
        {
            shotgunAmmoText.SetActive(true);
            if (aButton == true)
            {
                if (amIInteracting == false)
                {
                    _anim.SetTrigger("Interact");
                    amIInteracting = true;
                    StartCoroutine(InteractTimer());
                }
                _shotgunAmmo = _shotgunAmmo + 5;
                shotgunAmmoText.SetActive(false);
                Destroy(other.gameObject);
                AudioSource.PlayClipAtPoint(_pickupsAmmo, transform.position);

            }

        }
        if (other.tag == "Medkit" && canShowMedkitPrompt == true)
        {
            medkitText.SetActive(true);
            if (aButton == true && _currentHP == 5)
            {

                fullHealthText.SetActive(true);
                canShowMedkitPrompt = false;
                StartCoroutine(FullHealthTimer());

            }
            if (aButton == true && _currentHP < 5)
            {
                _anim.SetTrigger("Interact");
                medkitText.SetActive(false);
                _currentHP = _currentHP + 2;
                if (_currentHP > _maxHp)
                {
                    _currentHP = 5;
                }
                medkitText.SetActive(false);
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Journal1")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal1Text.SetActive(true);
                istTimedStopped = true;
                canIUnpause = false;
                Destroy(other.gameObject);
                StartCoroutine(PauseTimer());             
            }
        }
        if (other.tag == "Journal2")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal2Text.SetActive(true);
                istTimedStopped = true;
                canIUnpause = false;
                Destroy(other.gameObject);
                StartCoroutine(PauseTimer());
            }
        }
        if (other.tag == "Journal3")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal3Text.SetActive(true);
                istTimedStopped = true;
                canIUnpause = false;
                Destroy(other.gameObject);
                StartCoroutine(PauseTimer());
            }
        }
        if (other.tag == "Journal4")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal4Text.SetActive(true);
                istTimedStopped = true;
                canIUnpause = false;
                Destroy(other.gameObject);
                StartCoroutine(PauseTimer());
            }
        }
        if (other.tag == "Door")
        {
            if (aButton == true)
            {
                if (amIInteracting == false)
                {
                    _anim.SetTrigger("Interact");
                    amIInteracting = true;
                    StartCoroutine(InteractTimer());
                }

                other.gameObject.GetComponent<DoorLogic>().DoorStuff();
            }
        }
        if (other.tag == "LockedDoor")
        {
            if (doIHaveTheKey == false && aButton == true)
            {
                lockedDoorNoKeyText.SetActive(true);
            }
            else if (doIHaveTheKey == true && aButton == true)
            {
                lockedDoorWithKeyText.SetActive(true);
                other.gameObject.GetComponent<DoorLogic>().DoorStuff();

            }
        }
        if (other.tag == "EndDoor")
        {
            // Ending Screen
        }
        if (other.tag == "Shotgun")
        {
            shotgunText.SetActive(true);
            if (aButton == true)
            {
                if (amIInteracting == false)
                {
                    _anim.SetTrigger("Interact");
                    amIInteracting = true;
                    StartCoroutine(InteractTimer());
                }
                haveIFoundShotgun = true;
                _shotgunAmmo = 5;
                shotgunText.SetActive(false);
                AudioSource.PlayClipAtPoint(_pickupShotgun, transform.position, 100f);
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Key")
        {
            keyText.SetActive(true);

            if (aButton == true)
            {
                if (amIInteracting == false)
                {
                    _anim.SetTrigger("Interact");
                    amIInteracting = true;
                    StartCoroutine(InteractTimer());
                }
                doIHaveTheKey = true;
                keyText.SetActive(false);
                Destroy(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Handgun Ammo")
        {
            handgunAmmoText.SetActive(false);
            _anim.ResetTrigger("Interact");
        }
        if (other.tag == "Shotgun Ammo")
        {
            shotgunAmmoText.SetActive(false);
            _anim.ResetTrigger("Interact");

        }
        if (other.tag == "Medkit")
        {
            medkitText.SetActive(false);
            _anim.ResetTrigger("Interact");

        }
        if (other.tag == "Journal1")
        {
            journal1Text.SetActive(false);
            journalText.SetActive(false);
        }
        if (other.tag == "Journal2")
        {
            journal2Text.SetActive(false);
            journalText.SetActive(false);
        }
        if (other.tag == "Journal3")
        {
            journal3Text.SetActive(false);
            journalText.SetActive(false);
        }
        if (other.tag == "Journal4")
        {
            journal4Text.SetActive(false);
            journalText.SetActive(false);
        }
        if (other.tag == " Shotgun")
        {
            shotgunText.SetActive(false);
            _anim.ResetTrigger("Interact");
        }
        if (other.tag == "Key")
        {
            keyText.SetActive(false);
            _anim.ResetTrigger("Interact");
        }
        if (other.tag == "LockedDoor")
        {
            lockedDoorWithKeyText.SetActive(false);
            lockedDoorNoKeyText.SetActive(false);
        }
    }
    private void VisibleWeapon()
    {
        if (isUsingHandgun == true)
        {
            _weapons[0].SetActive(true);
            _weapons[1].SetActive(false);
            _weapons[2].SetActive(false);

        }
        if (isUsingHandgun == false)
        {
            _weapons[0].SetActive(false);
            _weapons[1].SetActive(true);
            if (isAiming == true)
            {
                _weapons[1].SetActive(false);
                _weapons[2].SetActive(true);
            }
            else
            {
                _weapons[2].SetActive(false);

            }

        }

        if (isUsingHandgun == true)
        {
            _ammoTxt.text = "Ammo:" + _handgunAmmo;
            _anim.SetBool("isUsingHandgun", true);
        }
        else
        {
            _ammoTxt.text = "Ammo:" + _shotgunAmmo;
            _anim.SetBool("isUsingHandgun", false);

        }
    }

    private IEnumerator ButtonFailsafeEnd()
    {
        yield return new WaitForSecondsRealtime(.4f);
        aButton = false;

    }

    private IEnumerator FullHealthTimer()
    {
        medkitText.SetActive(false);
        yield return new WaitForSeconds(2f);
        fullHealthText.SetActive(false);
        canShowMedkitPrompt = true;
    }
    private IEnumerator PauseTimer()
    {
        yield return new WaitForSecondsRealtime(.8f);
        canIUnpause = true;
    }
    private IEnumerator InteractTimer()
    {
        haveIPickedUpAmmo = true;
        yield return new WaitForSeconds(1f);
        amIInteracting = false;
        haveIPickedUpAmmo = false;
    }
    private IEnumerator DeathAnimTimer()
    {
        yield return new WaitForSeconds(3f);
        _uimanager.GetComponent<UIManager>().DeathMenu();
        istTimedStopped = true;

    }

    public void KillAllJournals()
    {
        journal1Text.SetActive(false);
        journal2Text.SetActive(false);
        journal3Text.SetActive(false);
        journal4Text.SetActive(false);

    }

    public void UnfreezeTime()
    {
        istTimedStopped = false;
    }

}
