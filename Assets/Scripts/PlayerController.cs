using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

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

    // Controller Info

    public bool aButton;
    public bool menuButton;
    public float leftAnalogStickHorizontal;
    public float leftAnalogStickVertical;
    public float leftTrigger;



    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _rotateSpeed = 150f;
        _speed = 4f;
        _currentHP = _maxHp;
    }

    void Update()
    {
        MovementController();
        WeaponControls();
        SwapWeapon();

        _healthTxt.text = "Health: " + _currentHP;
        if (isUsingHandgun == true)
        {
            _ammoTxt.text = "Ammo: " + _handgunAmmo;
        }
        else
        {
            _ammoTxt.text = "Ammo" + _shotgunAmmo;
        }
        if (isMoving == true)
        {
            _anim.SetBool("isMoving", true);
        }
        else
        {
            _anim.SetBool("isMoving", false);

        }
        if (isAiming == true)
        {
            _anim.SetBool("isAiming", true);
        }
        else
        {
            _anim.SetBool("isAiming", false);

        }
        aButton = Input.GetButton("A Button");
        menuButton = Input.GetButton("Menu Button");
        leftAnalogStickHorizontal = Input.GetAxis("Left Analog Stick (Horizontal)");
        leftAnalogStickVertical = Input.GetAxis("Left Analog Stick (Vertical)");
        leftTrigger = Input.GetAxis("Left Trigger");

    }
    private void MovementController()
    {
        if (Input.GetButton("Left Analog Stick (Horizontal)") || Input.GetButton("Left Analog Stick (Vertical)"))
        {
            Debug.Log("Reading Axis Input");
            isMoving = true;
            _horizMove = Input.GetAxis("Horizontal") * Time.deltaTime * _rotateSpeed;
            _vertMove = Input.GetAxis("Vertical") * Time.deltaTime * _speed;
            this.gameObject.transform.Rotate(0, _horizMove, 0);
            this.gameObject.transform.Translate(0, 0, _vertMove);
        }
        else
        {
            isMoving = false;

        }
    }
    private void WeaponControls()
    {
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Button Working");
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
    }

    private void SwapWeapon()
    {
        // Pressing the left stick in swaps weapon(s)
    }
}
