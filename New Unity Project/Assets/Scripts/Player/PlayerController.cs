using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerController : MonoBehaviour
{
    private const float OFFSET_Y = 15f;
    private const float OFFSET_Z = -4f;
    public Transform PlayerTorso;
    public Transform PlayerLegs;

    public LayerMask Mask;

    public List<Transform> Limbs = new List<Transform>();
    public Transform[] Arms = new Transform[2];
    private float m_currentMoveSpeed = 5f;

    private Camera MainCamera;
    private Vector3 _lookPoint  = Vector3.zero;
    private Rigidbody _rb;

    private List<AbilityScript> Abilities = new List<AbilityScript>();
    private bool _isRightClicked = false;

    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }


    void Awake(){
        _instance = this;
    } 
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(1)){
            _isRightClicked = true;
        }

        if(Input.GetMouseButtonUp(1)){
            _isRightClicked = false;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            foreach(AbilityScript ablity in Abilities){
                ablity.ActivateAbility(KeyCode.Space);
            }
        }

        if(Input.GetKeyDown(KeyCode.R)){
            foreach(AbilityScript ablity in Abilities){
                ablity.ActivateAbility(KeyCode.R);
            }
        }
    }

    void OnTriggerStay(Collider col){
        if(col.gameObject.tag == "Pickup"){
            Vector3 dir = transform.position - col.gameObject.transform.position;
            dir = dir.normalized;
            col.transform.root.transform.GetComponent<Rigidbody>().AddForce(dir * 8);
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Pickup"){
            Debug.Log("Add Attachment");
            Destroy(col.gameObject);

            int randLimb = Random.Range(0, Limbs.Count);
            int randAttachment = Random.Range(0, Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects.Count);
            foreach(Transform attachment in Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects){
                attachment.gameObject.SetActive(false);
            }
            Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.SetActive(true);
            /*if(!Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.activeInHierarchy){
                Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.SetActive(true);
            }*/

        }
    }
    void FixedUpdate()
    {
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(transform.position.x, transform.position.y + OFFSET_Y, transform.position.z + OFFSET_Z), Time.deltaTime * 3);
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

        float DotResult = Vector3.Dot(PlayerTorso.right, _lookPoint);
        float angle = Vector3.Angle(new Vector3(_lookPoint.x, 0, _lookPoint.z), PlayerTorso.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask))
        {
            _lookPoint = hit.point;
        }
        if(!_isRightClicked){
            var lookPos = _lookPoint - PlayerTorso.transform.position;
            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            
            PlayerTorso.transform.rotation = Quaternion.Slerp(PlayerTorso.transform.rotation, rotation, Time.deltaTime * 20);

            Arms[0].localRotation = Quaternion.Euler(0,90,0);
            Arms[1].localRotation = Quaternion.Euler(0,90,0);
        }
        else{
            if (DotResult > 0)
            {   
                Arms[1].LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                Arms[1].localRotation *= Quaternion.Euler(-180,-90,0);
                //Arms[0].localRotation = Quaternion.Inverse(Arms[1].localRotation);
                Arms[0].localRotation = Arms[1].localRotation;
                if(angle >= 90){
                    PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                    PlayerTorso.rotation *= Quaternion.Euler(0,-89.5f,0);
                }
            }
            else
            {
                Arms[0].LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                Arms[0].localRotation *= Quaternion.Euler(0,90,0);
                //Arms[1].localRotation = Quaternion.Inverse(Arms[0].localRotation);
                Arms[1].localRotation = Arms[0].localRotation;
                if(angle >= 90){
                    PlayerTorso.LookAt(new Vector3(_lookPoint.x, PlayerTorso.position.y, _lookPoint.z));
                    PlayerTorso.rotation *= Quaternion.Euler(0,89.5f,0);
                }
                    
            }
        }   
    }

    public void AddAbility(AbilityScript ability){
        Abilities.Add(ability);
    }

    public void RemoveAbility(AbilityScript ability){
        Abilities.Remove(ability);
    }

    public Vector3 LookPoint { get { return _lookPoint;}}
}
