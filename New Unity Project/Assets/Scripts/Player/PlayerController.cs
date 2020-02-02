using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerController : MonoBehaviour
{
    private float OFFSET_Y = 14f;
    private float OFFSET_Z = -3.8f;
    public Transform PlayerTorso;
    public Transform PlayerLegs;
    public Animator Anim;

    public LayerMask Mask;

    public List<Transform> Limbs = new List<Transform>();
    public Transform[] Arms = new Transform[2];
    private float m_currentMoveSpeed = 4f;
    private int _shields = 0;

    private Camera MainCamera;
    private Vector3 _lookPoint  = Vector3.zero;
    private Rigidbody _rb;

    private List<AbilityScript> Abilities = new List<AbilityScript>();
    private bool _isMiddleClicked = false;
    private bool _isCharging = false;

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
        if(Input.GetMouseButtonDown(2)){
            _isMiddleClicked = true;
        }

        if(Input.GetMouseButtonUp(2)){
            _isMiddleClicked = false;
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

            int randLimb = Random.Range(0, Limbs.Count - 1);
            int randAttachment = Random.Range(0, Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects.Count);

            if(Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.activeInHierarchy){
                return;
            }
            
            if(randLimb == (Limbs.Count - 2)){
                foreach(Transform attachment in Limbs[Limbs.Count - 1].GetComponent<Attachments>().AttachmentObjects){
                    attachment.gameObject.SetActive(false);
                }

                foreach(Transform attachment in Limbs[Limbs.Count - 2].GetComponent<Attachments>().AttachmentObjects){
                    attachment.gameObject.SetActive(false);
                }

                Limbs[Limbs.Count - 1].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.SetActive(true);

                Limbs[Limbs.Count - 2].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.SetActive(true);
            }
            else{
                foreach(Transform attachment in Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects){
                    attachment.gameObject.SetActive(false);
                }

                Limbs[randLimb].GetComponent<Attachments>().AttachmentObjects[randAttachment].gameObject.SetActive(true);
            }
        }

        if(col.gameObject.tag == "Enemy"){
            if(_isCharging){
                 EnemyBase enemy = col.gameObject.GetComponent<EnemyBase>();
                enemy.health -= 50f;
            }
            else{
                Debug.Log("Took Damage");
            }
        }
    }
    void FixedUpdate()
    {
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(transform.position.x, transform.position.y + OFFSET_Y, transform.position.z + OFFSET_Z), Time.deltaTime * 4);
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

            Anim.SetBool("IsMoving", true);
        }
        else{
            Anim.SetBool("IsMoving", false);
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
        _lookPoint.y = PlayerTorso.position.y;
        var lookPos = _lookPoint - PlayerTorso.transform.position;
        lookPos.y = 0;

        if(!_isMiddleClicked){


            Quaternion rotation = Quaternion.LookRotation(lookPos);
            
            //PlayerTorso.transform.rotation = Quaternion.Slerp(PlayerTorso.transform.rotation, rotation, Time.deltaTime * 20);
            PlayerTorso.transform.LookAt(_lookPoint);

            Arms[0].localRotation = Quaternion.Euler(0,90,0);
            Arms[1].localRotation = Quaternion.Euler(0,90,0);
        }
        else{
            if (DotResult > 0)
            {   
                Arms[1].LookAt(_lookPoint);
                Arms[1].localRotation *= Quaternion.Euler(-180,-90,0);
                //Arms[0].localRotation = Quaternion.Inverse(Arms[1].localRotation);
                Arms[0].localRotation = Arms[1].localRotation;
                if(angle >= 90){
                    PlayerTorso.LookAt(_lookPoint);
                    PlayerTorso.rotation *= Quaternion.Euler(0,-89.5f,0);
                }
            }
            else
            {
                Arms[0].LookAt(_lookPoint);
                Arms[0].localRotation *= Quaternion.Euler(0,90,0);
                //Arms[1].localRotation = Quaternion.Inverse(Arms[0].localRotation);
                Arms[1].localRotation = Arms[0].localRotation;
                if(angle >= 90){
                    PlayerTorso.LookAt(_lookPoint);
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

    public void IncreaseRange(){
        OFFSET_Y = 20f;
        OFFSET_Z = -5.5f;
    }

    public void ResetRange(){
        OFFSET_Y = 14f;
        OFFSET_Z = -3.8f;
    }

    public void IncreaseSpeed(){
        m_currentMoveSpeed = 7f;
    }

    public void ResetSpeed(){
        m_currentMoveSpeed = 4f;
    }

    public void GainShield(){
        _shields++;
    }

    public void RemoveShield(){
        _shields--;
    }

    public void Charge(){
        Timing.RunCoroutine(StartCharge().CancelWith(gameObject));
    }

    private IEnumerator<float> StartCharge(){
        Anim.SetTrigger("Dash");
        _isCharging = true;
        yield return Timing.WaitForSeconds(1);
        _isCharging = false;
    }

    public void TakeDamage(){
        
    }
    public Vector3 LookPoint { get { return _lookPoint;}}
}
