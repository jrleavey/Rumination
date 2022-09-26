using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
    private float _rotateSpeed;
    [SerializeField]
    private float _speed;
    private float _horizMove;
    private float _vertMove;
    [SerializeField]
    private UIManager _uimanager;
    [SerializeField]
    private int _handgunAmmo;
    [SerializeField]
    private int _shotgunAmmo;
    [SerializeField]
    private int _currentHP;
    [SerializeField]
    private int _maxHp = 5;
    [SerializeField]
    private Text _healthTxt;
    [SerializeField]
    private Text _ammoTxt;
    [SerializeField]
    private bool isUsingHandgun = true;
    private Animator _anim;
    [SerializeField]
    private GameObject RaycastHolder;
    private float gunDamage = 1;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _handgunShot;
    [SerializeField]
    private AudioClip _shotgunShot;
    [SerializeField]
    private AudioClip _outOfAmmo;
    [SerializeField]
    private GameObject[] _weapons;


    [SerializeField]
    private float fireRate = 5f;
    [SerializeField]
    private float nextFire = -1f;

    private bool isInvincible = false;



    private PlayerControls _playerControls;

    public bool aButton;
    public bool menuButton;
    public float leftAnalogStickHorizontal;
    public bool leftAnalogStickVertical;
    public bool leftTrigger;
    private bool isMenuOpen = false;
    [SerializeField]
    private bool haveIFoundShotgun = false;
    [SerializeField]
    private bool canShowMedkitPrompt = true;



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
    private bool amIReading = false;
    [SerializeField]
    private bool istTimedStopped = false;
    [SerializeField]
    private bool isJournalActive = false;
    [SerializeField]
    private bool canIUnpause = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _rotateSpeed = 200f;
        _speed = 4f;
        _currentHP = _maxHp;

        _playerControls = new PlayerControls();
        _playerControls.Controller.Enable();
        InputSetup();

        //if (abutton is true && conditional)
        {
            //do game logic
        }

        //Switch statement to check pickups
        //check tag as string for switch value
        // do logic

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
        _healthTxt.text = "Health: " + _currentHP;

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
        VisibleWeapon();
        if (isUsingHandgun == true)
        {
            fireRate = .5f;
        }
        else
        {
            fireRate = 1.7f;
        }

        if (istTimedStopped == false)
        {
            Time.timeScale = 1;
        }
        else if (istTimedStopped == true)
        {
            Time.timeScale = 0;
        }

        if (isJournalActive == true && aButton == true && canIUnpause == true)
        {
            istTimedStopped = !istTimedStopped;
            canIUnpause = false;
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
    }

    private void Menu_performed(InputAction.CallbackContext obj)
    {
        isMenuOpen = !isMenuOpen;
        _uimanager.GetComponent<UIManager>().Pause();
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

        }
        else
        {
            isMoving = false;
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
            isAiming = true;
            _speed = 0;
            _anim.SetBool("isAiming", true);
        }
        else
        {
            _anim.SetBool("isAiming", false);
            isAiming = false;
            _speed = 2;

        }

        if (isAiming == true && aButton == true && _handgunAmmo >= 1 && isUsingHandgun == true && Time.time > nextFire)
        {
            aButton = false;
            RaycastHit hit;
            nextFire = Time.time + fireRate;

            if (Physics.Raycast(RaycastHolder.transform.position, RaycastHolder.transform.forward, out hit, Mathf.Infinity))
            {
                hit.collider.SendMessage("Damage", gunDamage);
                Debug.DrawLine(RaycastHolder.transform.position, hit.point, Color.red, 1f);
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

    }
    private void ShotgunRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(RaycastHolder.transform.position, RaycastHolder.transform.forward, out hit, Mathf.Infinity))
        {
            Vector3 direction = RaycastHolder.transform.forward;
            Vector3 spread = Vector3.zero;
            spread += RaycastHolder.transform.up * Random.Range(-.05f, .05f);
            spread += RaycastHolder.transform.right * Random.Range(-.05f, .05f);
            direction += spread.normalized * Random.Range(0f, 0.2f);
            hit.collider.SendMessage("Damage", gunDamage);

            if (Physics.Raycast(RaycastHolder.transform.position, direction, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(RaycastHolder.transform.position, hit.point, Color.red, 1f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


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

    private void AccessMenu()
    {

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
                _handgunAmmo = _handgunAmmo + 10;
                handgunAmmoText.SetActive(false);
                Destroy(other.gameObject);
            }

        }
        if (other.tag == "Shotgun Ammo")
        {
            shotgunAmmoText.SetActive(true);
            if (aButton == true)
            {
                _shotgunAmmo = _shotgunAmmo + 5;
                shotgunAmmoText.SetActive(false);
                Destroy(other.gameObject);
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
                amIReading = true;
                journalText.SetActive(false);
                journal1Text.SetActive(true);
                istTimedStopped = !istTimedStopped;
                isJournalActive = true;
                StartCoroutine(ReadingJournalTimer());
                StartCoroutine(PauseTimer());
                if (aButton == true && amIReading == false)
                {
                    journal1Text.SetActive(false);
                    Destroy(other.gameObject);
                    istTimedStopped = !istTimedStopped;

                }
            }
        }
        if (other.tag == "Journal2")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal2Text.SetActive(true);
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Journal3")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal3Text.SetActive(true);
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Journal4")
        {
            journalText.SetActive(true);
            if (aButton == true)
            {
                journalText.SetActive(false);
                journal4Text.SetActive(true);
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Door")
        {
            if (aButton == true)
            {
                other.gameObject.GetComponent<DoorLogic>().DoorStuff();
            }
        }
        if (other.tag == "LockedDoor")
        {

        }
        if (other.tag == "EndDoor")
        {

        }
        if (other.tag == "Shotgun")
        {

        }
        if (other.tag == "Key")
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Handgun Ammo")
        {
            handgunAmmoText.SetActive(false);
        }
        if (other.tag == "Shotgun Ammo")
        {
            shotgunAmmoText.SetActive(false);
        }
        if (other.tag == "Medkit")
        {
            medkitText.SetActive(false);
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
        }
        if (other.tag == "Key")
        {
            keyText.SetActive(false);
        }
    }
    //public void ReadingJournal()
    //{
    //    amIReading = true;
    //    StartCoroutine(ReadingJournalTimer());
    //    if (aButton == true && amIReading == false)
    //    {
    //        journal1Text.SetActive(false);
    //    }
    //}

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
    private IEnumerator ReadingJournalTimer()
    {
        yield return new WaitForSecondsRealtime(.8f);
        amIReading = false;

    }
    private IEnumerator PauseTimer()
    {
        yield return new WaitForSecondsRealtime(.8f);
        canIUnpause = true;
    }
}
