using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private GameManager _gameManager;
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
    private float damage;
    private float range;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _handgunShot;
    [SerializeField]
    private AudioClip _shotgunShot;
    [SerializeField]
    private AudioClip _outOfAmmo;

    private PlayerControls _playerControls;

    public bool aButton;
    public bool menuButton;
    public float leftAnalogStickHorizontal;
    public bool leftAnalogStickVertical;
    public bool leftTrigger;
    private bool isMenuOpen = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _rotateSpeed = 150f;
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
        }
        else
        {
            _ammoTxt.text = "Ammo:" + _shotgunAmmo;
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
    }

    private void Menu_performed(InputAction.CallbackContext obj)
    {
        isMenuOpen = !isMenuOpen;
    }

    private void LeftStickClick_performed(InputAction.CallbackContext obj)
    {
        isUsingHandgun = !isUsingHandgun;
        SwapWeapon();
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

        //if (Mathf.Abs(leftStick.x) > 1f || Mathf.Abs(leftStick.y) > 0)
        //{
        //    isMoving = true;

        //    _horizMove = leftStick.x * Time.deltaTime * _rotateSpeed;
        //    _vertMove = leftStick.y * Time.deltaTime * _speed;

        //    this.gameObject.transform.Rotate(0, _horizMove, 0);
        //    this.gameObject.transform.Translate(0, 0, _vertMove * _speed);
        //}
        //if (Mathf.Abs(leftStick.x) <= 0)
        //{
        //    isMoving = false;

        //}

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

        if (isAiming == true && aButton == true && _handgunAmmo >= 1)
        {
            aButton = false;
            RaycastHit hit;

            if (Physics.Raycast(RaycastHolder.transform.position, RaycastHolder.transform.forward, out hit, Mathf.Infinity))
            {
                Debug.Log("Fired Raycast");
                Debug.Log(hit.transform);
                hit.collider.SendMessage("Damage", damage);
                Debug.DrawRay(RaycastHolder.transform.position, RaycastHolder.transform.forward * Mathf.Infinity, Color.red);
            }
            _handgunAmmo--;
            AudioSource.PlayClipAtPoint(_handgunShot, transform.position);
        }
        else if (isAiming == true && aButton == true && _handgunAmmo <= 0)
        {
            aButton = false;
            AudioSource.PlayClipAtPoint(_outOfAmmo, transform.position);
        }

        if (isAiming == true)
        {
            isMoving = false;
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
        // Enemy collider can call this function to apply damage, based on the int "Damage" value
        _currentHP = _currentHP - damage;
        if (_currentHP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }

    private void SwapWeapon()
    {
        if (isUsingHandgun == true)
        {
            range = 1000;
            damage = 1;
            Debug.Log("Handgun");
        }
        else
        {
            range = 500;
            damage = Random.Range(3, 8);
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
    }
}
