using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerController : MonoBehaviour
{
    private const float OFFSET_Y = 16f;
    private const float OFFSET_Z = -2.5f;
    public Transform PlayerTorso;
    public Transform PlayerLegs;

    private float m_currentMoveSpeed = 3f;

    private Camera MainCamera;
    private Vector3 _lookPoint  = Vector3.zero;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
        }
    }

    void Aim(){
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            _lookPoint = hit.point;
        }

        PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
        
    }
}
