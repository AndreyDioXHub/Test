using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _weaponLeftGameObject;
    [SerializeField]
    private GameObject _weaponRightGameObject;
    [SerializeField]
    private Weapon _weaponLeft;
    [SerializeField]
    private Weapon _weaponRight;

    [SerializeField]
    private CharacterController _character;

    [SerializeField]
    private Vector2 _mouse;
    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private float _mouseSensitivity = 100f;
    [SerializeField]
    private Vector2 _minMaxAngle;
    [SerializeField]
    private float _farLook = 1f;
    public float _xRotation = 0f;
    private float _yRotation = 0f;

    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private AnimationCurve _jumpCurve;
    private float _experationCurTime = 0;
    private float _experationLastTime = 0;
    [SerializeField]
    private float _duration = 1;
    [SerializeField]
    private float _jumpHeight = 1;
    [SerializeField]
    private float _gravity= 1;
    private bool _isJump;

    private Vector2 _inputMovement;
    private Vector3 _moveVectorForward;
    private Vector3 _moveVectorRight;
    private Vector3 _velocityVector;
    private Vector3 _resultVector;

    private int _chekersCount = 4;
    private int _ringCount = 1;
    private float _distance = 0.1f;
    private float _degToRad = 0;
    private bool _isGrounded;

    [SerializeField]
    private GameObject _lookedGameObject;
    private bool _mayGetWeapon;
    private bool _mayShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        _weaponLeft = _weaponLeftGameObject.GetComponent<Weapon>();
        _weaponRight = _weaponRightGameObject.GetComponent<Weapon>();

        Cursor.lockState = CursorLockMode.Locked;
        _velocityVector = new Vector3(0, -0.002f, 0);
        _degToRad = Mathf.PI / 180.0f;
        _distance = _character.skinWidth;
    }

    // Update is called once per frame
    void Update()
    {
        _xRotation -= _mouse.y * _mouseSensitivity * Time.deltaTime;
        _yRotation = _mouse.x * _mouseSensitivity * Time.deltaTime;

        _xRotation = Mathf.Clamp(_xRotation, _minMaxAngle.x, _minMaxAngle.y);
        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * _yRotation);

        GroundCheck();
        
        Move();

        if (_isGrounded)
        {
            _velocityVector = new Vector3(0, -0.002f, 0);//Захардкодил потому, что иначе гравитация сильно сильная
        }
        else
        {
            _velocityVector += new Vector3(0, _gravity * Time.deltaTime * Time.deltaTime, 0);
        }

        if (_isJump == true)
        {
            Jump();
        }

        _resultVector = _moveVectorForward + _moveVectorRight + _velocityVector;
        _character.Move(_resultVector);

        if (Physics.Raycast(_playerCamera.position, _playerCamera.forward, out RaycastHit hit, _farLook))
        {
            Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _farLook, Color.red);
            if (hit.collider.tag == "WeaponSpawn")
            {
                _mayGetWeapon = true;
                if(_lookedGameObject != hit.collider.gameObject) 
                {
                    _lookedGameObject = hit.collider.gameObject;
                }
                
            }
            else
            {
                _mayGetWeapon = false;
            }
        }
        else
        {
            Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _farLook, Color.green);
            _mayGetWeapon = false;
        }

        /*if(_weaponRight == null)
        {
            _weaponRight = _weaponRightGameObject.GetComponent<Weapon>();
        }*/
    }

    private void GroundCheck()
    {
        int count = 0;
        float angle = 0;
        float sin = 0;
        float cos = 0;

        for (int j = 1; j < _ringCount + 1; j++)
        {
            for (int i = 0; i < _chekersCount; i++)
            {
                angle = 360 * i / _chekersCount;

                if (angle >= 360 || angle <= -360)
                {
                    angle = 0;
                }

                sin = Mathf.Sin(_degToRad * angle);
                cos = Mathf.Cos(_degToRad * angle);

                Vector3 origin = new Vector3(transform.position.x + _character.radius * cos / j, transform.position.y - (_character.height * 0.5f), transform.position.z + _character.radius * sin / j);
                Vector3 direction = transform.TransformDirection(Vector3.down);

                if (Physics.Raycast(origin, direction, out RaycastHit hit, _distance))
                {
                    Debug.DrawRay(origin, direction * _distance, Color.red);
                    count++;
                }
                else
                {
                    Debug.DrawRay(origin, direction * _distance, Color.green);
                }
            }
        }


        if (count > 0)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (_character.isGrounded == true)
        {
            _isGrounded = true;
        }
    }
    private void Move()
    {
        _moveVectorRight = transform.right * _inputMovement.x * _speed * Time.deltaTime;
        _moveVectorForward = transform.forward * _inputMovement.y * _speed * Time.deltaTime;
    }

    private void Jump()
    {
        _experationCurTime += Time.deltaTime;
        if (_experationCurTime > _duration)
        {
            _experationCurTime = 0;
            _experationLastTime = 0;
            _isJump = false;
        }
        else
        {
            float progress = _experationCurTime / _duration;
            float lastprogress = _experationLastTime / _duration;
            _velocityVector = new Vector3(0, (_jumpCurve.Evaluate(progress) - _jumpCurve.Evaluate(lastprogress)) * _jumpHeight, 0);
            _experationLastTime = _experationCurTime;
        }
    }

    public void GetJump(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {
            if(_isGrounded == true)
            {
                _isJump = true;
            }
        }
    }

    /* private void SwapWeapon(Weapon weapon, GameObject weaponslot)//не работает надо дебажить, но будь время я бы сделал свап оружия через одну функции, чтобы не дублировать код
     {


         if (_mayGetWeapon == true)
         {
             weapon.Remove();
             Type weaponType = _lookedGameObject.GetComponent<Weapon>().GetType();

             if (weaponType == weapon.GetType())
             {

             }
             else
             {
                 weapon.Remove();
                 weaponslot.AddComponent(weaponType);

                 ComponentUtility.CopyComponent(_lookedGameObject.GetComponent(weaponType));
                 ComponentUtility.PasteComponentValues(weaponslot.GetComponent(weaponType));

                 weapon = (Weapon)weaponslot.GetComponent(weaponType);
                 weapon.Init();
             }

             _mayGetWeapon = false;
         }
     }*/

    public void GetActionLeft(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {
            if (_mayGetWeapon == true)
            {
                _weaponLeftGameObject.SetActive(true);
                _mayShoot = true;
                Type weaponType = _lookedGameObject.GetComponent<Weapon>().GetType();
                Weapon weapon = _lookedGameObject.GetComponent<Weapon>();

                if (weaponType == _weaponLeft.GetType())
                {

                }
                else
                {
                    _weaponLeft.Remove();
                    _weaponLeftGameObject.AddComponent(weaponType);

                    /*ComponentUtility.CopyComponent(_lookedGameObject.GetComponent(weaponType));
                    ComponentUtility.PasteComponentValues(_weaponLeftGameObject.GetComponent(weaponType));*/

                    _weaponLeft = (Weapon)_weaponLeftGameObject.GetComponent(weaponType);
                    _weaponLeft.Init(weapon.CupyGun());
                }
                
                _mayGetWeapon = false;
            }
            else
            {
                _weaponLeftGameObject.SetActive(!_weaponLeftGameObject.activeSelf);
                _mayShoot = _weaponLeftGameObject.activeSelf;
            }

        }
    }

    public void GetActionRight(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {

            if (_mayGetWeapon == true)
            {
                _weaponRightGameObject.SetActive(true);
                _mayShoot = true;
                Type weaponType = _lookedGameObject.GetComponent<Weapon>().GetType();
                Weapon weapon = _lookedGameObject.GetComponent<Weapon>();

                if (weaponType == _weaponRight.GetType())
                {

                }
                else
                {
                    _weaponRight.Remove();
                    _weaponRightGameObject.AddComponent(weaponType);

                    /*ComponentUtility.CopyComponent(_lookedGameObject.GetComponent(weaponType));
                    ComponentUtility.PasteComponentValues(_weaponRightGameObject.GetComponent(weaponType));*/

                    _weaponRight = (Weapon)_weaponRightGameObject.GetComponent(weaponType);
                    _weaponRight.Init(weapon.CupyGun());
                }
                
                _mayGetWeapon = false;
            }
            else
            {
                _weaponRightGameObject.SetActive(!_weaponRightGameObject.activeSelf);
                _mayShoot = _weaponRightGameObject.activeSelf;
            }
        }
    }

    public void GetFireLeft(InputAction.CallbackContext value)
    {
        //Debug.Log(value.phase.ToString());
        if (value.phase.ToString().Equals("Started"))
        {
            if(_weaponLeft != null)
            {
                if (_mayShoot == true)
                {
                    _weaponLeft.Shoot();
                }                
            }            
        }
        else if (value.phase.ToString().Equals("Canceled"))
        {
            _weaponLeft.ShootRelease();
        }
    }

    public void GetFireRight(InputAction.CallbackContext value)
    {
        if (value.phase.ToString().Equals("Started"))
        {
            if (_weaponRight != null)
            {
                if(_mayShoot == true)
                {
                    _weaponRight.Shoot();
                }                
            }
        }
        else if (value.phase.ToString().Equals("Canceled"))
        {
            _weaponRight.ShootRelease();
        }
    }

    public void GetMovement(InputAction.CallbackContext value)
    {
        _inputMovement = value.ReadValue<Vector2>();
    }

    public void MouseLookX(InputAction.CallbackContext value)
    {
        _mouse.x = value.ReadValue<float>();
    }

    public void MouseLookY(InputAction.CallbackContext value)
    {
        _mouse.y = value.ReadValue<float>();
    }
}


