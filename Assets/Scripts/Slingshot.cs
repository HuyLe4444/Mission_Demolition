using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] private LineRenderer rubber;
    [SerializeField] private Transform slinghand1;
    [SerializeField] private Transform slinghand2;

    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    [Header("Audio")]
    [SerializeField] private AudioClip rubberSnapSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        rubber.SetPosition(0, slinghand1.position + offset);
        rubber.SetPosition(2, slinghand2.position + offset);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButton(0)){
        //     rubber.SetPosition(1, GetMousePositionInWorld());
        // }

        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
            
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        rubber.SetPosition(1, projPos);

        if(Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);

            if (rubberSnapSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(rubberSnapSound);
            }

            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }

    // Vector3 GetMousePositionInWorld(){
    //     Vector3 mousePosition = Input.mousePosition;
    //     mousePosition.z += -Camera.main.transform.position.z;
    //     Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
    //     return mousePositionInWorld - transform.position;
    // }

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnMouseEnter() {
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
