using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ShootingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectilePrefab = new GameObject[0];
    [SerializeField]
    private GameObject bulletSource = null;
    [SerializeField]
    private float projectileSpeed = 5.0f;
    [SerializeField]
    private float returnSpeed = 5f;
    [SerializeField]
    private float returnSnapbackRange = 0.2f;
    [SerializeField]
    private float goalDistance = 10f;
    [SerializeField]
    private float returnTime = 2f;
    private GameObject projectileEnzyme;
    private Rigidbody projectileRb;
    private bool isLoaded = true;
    private bool isReturning = true;
    private Vector3 projectileGoalPos;
    //Variable for StuckCheck function
    private Vector3 previousPos = Vector3.zero;
    private bool returnTimerCoroutineRunning = false;
    private float returnTimeStart;
    public int gunMode = 1;
    public float spreadAngle = 0f;
    public float automaticProjectileDecayTime = 2f;
    public float automaticFireRate = 1f;
    private float fireRateCounter = 0;
    public float firingTightness = 1f;

    public SteamVR_Action_Boolean input;
    SteamVR_Input_Sources isource;
    private bool grabbed = false;
    private bool previouslyNADplus = true;

    // Start is called before the first frame update
    void Awake()
    {
        
        if(gunMode==4)
        { 
            
            projectileEnzyme = Instantiate(projectilePrefab[gunMode - 1], bulletSource.transform.position, bulletSource.transform.rotation);
            //projectile.transform.SetParent(bulletSource.transform);
            projectileRb = projectileEnzyme.GetComponent<Rigidbody>();
            projectileEnzyme.SetActive(false);
            previouslyNADplus = projectileEnzyme.GetComponent<ChemData>().name=="NAD+";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.GetComponent<Prompt>().IsActive()) return;
        isource = gameObject.GetComponent<Interactable>().hoveringHand.handType;
        if (grabbed)
        {
            if (gunMode == 1)
            { //Semi-auto enzyme

                if (Input.GetKeyDown(KeyCode.Space) || input.GetStateDown(isource) && isLoaded)
                {
                    
                    GameObject tempProjectile;
                    tempProjectile = Instantiate(projectilePrefab[0], bulletSource.transform.position, bulletSource.transform.rotation);
                    tempProjectile.GetComponent<Rigidbody>().velocity = bulletSource.transform.up * projectileSpeed;
                    tempProjectile.AddComponent<DecayScript>().SetDecayTime(automaticProjectileDecayTime);
                    gameObject.GetComponent<AudioSource>().Play();
                    projectileGoalPos = transform.position + transform.up * goalDistance;
                    /*if(!returnTimerCoroutineRunning) {
                        StartCoroutine("ProjectileDestroyTimer");
                    }
                } else if(isLoaded) {
                    projectileEnzyme.transform.SetPositionAndRotation(bulletSource.transform.position,bulletSource.transform.rotation);
                } else if(!isLoaded) {
                    //Move towards target
                    projectileRb.velocity = (projectileGoalPos-projectileEnzyme.transform.position).normalized*projectileSpeed;
                    //Snap to target and start returning
                    if(Vector3.Distance(projectileEnzyme.transform.position,projectileGoalPos)<returnSnapbackRange) {
                        isReturning=true;
                        projectileEnzyme.transform.position = projectileGoalPos;
                        projectileRb.velocity = Vector3.zero;
                        previousPos = Vector3.zero;
                    }*/
                }
            }
            else if (gunMode == 2)
            { //Automatic alpha amylase
              //Shoot bullets when space is held, according to fire rate
                if (Input.GetKey(KeyCode.Space) || input.GetState(isource) && fireRateCounter >= 1 / automaticFireRate)
                {
                    fireRateCounter -= 1 / automaticFireRate;
                    //Shoot bullet
                    //float spreadRadius = Mathf.Tan(Mathf.Deg2Rad * spreadAngle/2f);
                    Vector3 firingDirection = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle) * firingTightness, Random.Range(-spreadAngle, spreadAngle) * firingTightness, 0f) * transform.up;
                    //firingDirection = (Random.insideUnitSphere * spreadRadius + bulletSource.transform.forward).normalized;
                    Quaternion tempProjectileRotation = Quaternion.LookRotation(firingDirection);
                    gameObject.GetComponent<AudioSource>().Play();
                    GameObject tempProjectile = Instantiate(projectilePrefab[gunMode - 1], bulletSource.transform.position, tempProjectileRotation);
                    tempProjectile.AddComponent<DecayScript>().SetDecayTime(automaticProjectileDecayTime);
                    tempProjectile.GetComponent<Rigidbody>().velocity = tempProjectile.transform.forward * projectileSpeed;
                }
                else if (fireRateCounter < 1 / automaticFireRate)
                {
                    fireRateCounter += Time.deltaTime;
                }
            }
            else if (gunMode == 3)
            { //Auto aim beta amylase
                /*GameObject targetJoint = null;
                foreach(GameObject joint in SnakeSpawner.GetJoints()) {
                    GameObject tempJoint = joint;
                    if(targetJoint==null) {
                        targetJoint = tempJoint;
                        continue;
                    }

                    //target snake if it is in front of you, it is the closest
                }*/

                //You have a target if you are x angle away from looking at target snake

                //When space is pressed and you have a target
                //shoot it towards your target
                //
                //determine where it's going
                //
            }
            else if (gunMode == 4)
            { //Returning enzyme projectile
                if (Input.GetKeyDown(KeyCode.Space) || input.GetStateDown(isource) && isLoaded)
                {
                    gameObject.GetComponent<AudioSource>().Play();
                    projectileEnzyme.SetActive(true);
                    //projectile.transform.SetParent(null);
                    projectileRb.velocity = Vector3.zero;
                    //projectileRb.AddForce(transform.up*projectileSpeed);
                    isLoaded = false;
                    isReturning = false;
                    projectileGoalPos = transform.position + transform.up * goalDistance;
                    if (!returnTimerCoroutineRunning)
                    {
                        StartCoroutine("ProjectileReturnTimer");
                    }
                }
                else if (isLoaded)
                {
                    projectileEnzyme.transform.SetPositionAndRotation(bulletSource.transform.position, bulletSource.transform.rotation);
                }
                else if (!isLoaded)
                {
                    //Shot projectile is leaving
                    if (!isReturning)
                    {
                        //Move towards target
                        projectileRb.velocity = (projectileGoalPos - projectileEnzyme.transform.position).normalized * projectileSpeed;
                        //Snap to target and start returning
                        if (Vector3.Distance(projectileEnzyme.transform.position, projectileGoalPos) < returnSnapbackRange)
                        {
                            isReturning = true;
                            projectileEnzyme.transform.position = projectileGoalPos;
                            projectileRb.velocity = Vector3.zero;
                            previousPos = Vector3.zero;
                        }
                    }
                    else
                    { //Shot projectile is returning
                      //Move towards gun
                        projectileRb.velocity = (bulletSource.transform.position - projectileEnzyme.transform.position).normalized * returnSpeed;
                        //Snap to gun bullet source and act as a loaded gun
                        if (Vector3.Distance(projectileEnzyme.transform.position, bulletSource.transform.position) < returnSnapbackRange)
                        {
                            isLoaded = true;
                            projectileEnzyme.transform.position = bulletSource.transform.position;
                            projectileRb.velocity = Vector3.zero;
                            previousPos = Vector3.zero;
                            StopCoroutine("ProjectileReturnTimer");
                            returnTimerCoroutineRunning = false;
                            projectileEnzyme.SetActive(false);
                            //projectile.transform.SetParent(bulletSource.transform);
                        }
                    }
                }

                //Highlight Glucose if enzyme is NAD+, highlight Acetaldehyde if enzyme is NADH
                if(projectileEnzyme.GetComponent<ChemData>().Name =="NADH" && previouslyNADplus) { //Switched to nadh, acetaldehyde target
                    //Highlight acetaldehyde
                    Debug.LogError("Highlight acetaldehyde");
                    GameObject[] glucoseArray = GameObject.FindGameObjectsWithTag("Glucose");
                    GameObject[] acetArray = GameObject.FindGameObjectsWithTag("Acetaldehyde");
                    previouslyNADplus=false;
                    for(int i=0;i<glucoseArray.Length;i++)
                    {
                        glucoseArray[i].GetComponent<MoleculeHighlightScript>().ToggleHighlight(false);
                    }
                    for(int i=0;i<acetArray.Length;i++)
                    {
                        acetArray[i].GetComponent<MoleculeHighlightScript>().ToggleHighlight(true);
                    }
                } else if(projectileEnzyme.GetComponent<ChemData>().Name == "NAD+" && !previouslyNADplus) { //Switched to nad+
                    //Highlight glucose
                    Debug.LogError("Highlight glucose");
                    GameObject[] glucoseArray = GameObject.FindGameObjectsWithTag("Glucose");
                    GameObject[] acetArray = GameObject.FindGameObjectsWithTag("Acetaldehyde");
                    previouslyNADplus=true;
                    for(int i=0;i<glucoseArray.Length;i++)
                    {
                        glucoseArray[i].GetComponent<MoleculeHighlightScript>().ToggleHighlight(true);
                    }
                    for(int i=0;i<acetArray.Length;i++)
                    {
                        acetArray[i].GetComponent<MoleculeHighlightScript>().ToggleHighlight(false);
                    }
                    
                }

            }

        }
        if (!grabbed && input.GetStateDown(isource))
        {
            gameObject.transform.parent = GetComponent<Interactable>().hoveringHand.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.15f, 0.15f);
            gameObject.transform.localRotation = Quaternion.Euler(135f,0f,0f);
            
            grabbed = true;
        }
    }
    /*private bool StuckCheck(Vector3 currentPos, Vector3 goalPos) {
        //If currentposition is not closer to the goal than previous position, return true
        bool isStuck;
        if(previousPos==Vector3.zero) { //Initialize previous position if unset
            previousPos = currentPos;
            return false;
        } else { //it is stuck if it is further away from its goal than it was before
            isStuck = Vector3.Distance(currentPos,goalPos)>=Vector3.Distance(previousPos,goalPos);
        }
        return isStuck;
    }*/

    IEnumerator ProjectileReturnTimer() {
        if(returnTimerCoroutineRunning) {
            yield return null;
        }
        returnTimerCoroutineRunning = true;
        returnTimeStart = Time.time;
        while(true) {
            if(Time.time - returnTimeStart>=returnTime) {
                isLoaded=true;
                projectileEnzyme.transform.position = bulletSource.transform.position;
                projectileRb.velocity = Vector3.zero;
                returnTimerCoroutineRunning = false;
                projectileEnzyme.SetActive(false);
                returnTimeStart = Time.time;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ProjectileDestroyTimer() {
        if(returnTimerCoroutineRunning) {
            yield return null;
        }
        returnTimerCoroutineRunning = true;
        returnTimeStart = Time.time;
        while(true) {
            if(Time.time - returnTimeStart>=returnTime) {
                isLoaded=true;
                projectileEnzyme.transform.position = bulletSource.transform.position;
                projectileRb.velocity = Vector3.zero;
                returnTimerCoroutineRunning = false;
                projectileEnzyme.SetActive(false);
                returnTimeStart = Time.time;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void detach()
    {
        transform.parent = null;
        transform.position = new Vector3(0, 1, 0);
        grabbed = false;
        Destroy(gameObject);
    }
}