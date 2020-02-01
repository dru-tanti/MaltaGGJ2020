using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerController : MonoBehaviour
{
    private const float OFFSET_Y = 20f;
    private const float OFFSET_Z = -5.5f;
    public Transform PlayerTorso;
    public Transform PlayerLegs;

    public Transform[] Arms = new Transform[2];
    public Transform[] Shoulders = new Transform[2];
    private float m_currentMoveSpeed = 3f;

    private Camera MainCamera;
    private Vector3 _lookPoint  = Vector3.zero;
    private Rigidbody _rb;

    private bool _isRightClicked = false;

    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        _instance = this;
        _rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(1)){
            _isRightClicked = true;
        }

        if(Input.GetMouseButtonUp(1)){
            _isRightClicked = false;
        }
    }
    void FixedUpdate()
    {
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(transform.position.x, transform.position.y + OFFSET_Y, transform.position.z + OFFSET_Z), Time.deltaTime * 5);
        Movement();
        Aim();
    }

    void Movement(){

        float xInput = Input.GetAxis("Horizontal");

        float yInput = Input.GetAxis("Vertical");

        Vector3 _horMove = transform.right * xInput;
        Vector3 _vertMove = transform.forward * yInput;
        Vector3 velocity = (_horMove + _vertMove).normalized * m_currentMoveSpeed;

        if(velocity != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);

            PlayerLegs.rotation = Quaternion.LookRotation(velocity);
        }
    }

    void Aim(){
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            _lookPoint = hit.point;
        }
        if(!_isRightClicked){
            PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
            Arms[0].localRotation = Quaternion.Euler(0,0,0);
            Arms[1].localRotation = Quaternion.Euler(0,0,0);
            Shoulders[0].localRotation = Quaternion.Euler(0,0,0);
            Shoulders[1].localRotation = Quaternion.Euler(0,0,0);
        }
        else{
            float DotResult = Vector3.Dot(PlayerTorso.right, _lookPoint);
            float angle = Vector3.Angle(_lookPoint, PlayerTorso.forward);
            if (DotResult > 0)
            {
                Arms[0].localRotation = Quaternion.Euler(0, -angle, 0);
                Arms[1].localRotation = Quaternion.Euler(0, angle, 0);
                Shoulders[0].localRotation = Quaternion.Euler(0, -angle, 0);
                Shoulders[1].localRotation = Quaternion.Euler(0, angle, 0);
                if(angle > 90){
                    PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                    PlayerTorso.rotation *= Quaternion.Euler(0,-90,0);
                }
            }
            else
            {
                Arms[0].localRotation = Quaternion.Euler(0, -angle, 0);
                Arms[1].localRotation = Quaternion.Euler(0, angle, 0);
                Shoulders[0].localRotation = Quaternion.Euler(0, -angle, 0);
                Shoulders[1].localRotation = Quaternion.Euler(0, angle, 0);
                if(angle > 90){
                    PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                    PlayerTorso.rotation *= Quaternion.Euler(0,90,0);
                }
                    
            }
        }

        
    }

    public Vector3 LookPoint { get { return _lookPoint;}}
}
