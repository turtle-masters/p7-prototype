using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab = null;
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
    private GameObject projectile;
    private Rigidbody projectileRb;
    private bool isLoaded = true;
    private bool isReturning = true;
    private Vector3 projectileGoalPos;
    //Variable for StuckCheck function
    private Vector3 previousPos = Vector3.zero;

    private bool returnTimerCoroutineRunning = false;
    private float returnTimeStart;

    // Start is called before the first frame update
    void Start()
    {
        projectile = Instantiate(projectilePrefab,bulletSource.transform.position,bulletSource.transform.rotation);
        //projectile.transform.SetParent(bulletSource.transform);
        projectileRb = projectile.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         
        if(Input.GetKeyDown(KeyCode.Space) && isLoaded) {
            //projectile.transform.SetParent(null);
            projectileRb.velocity=Vector3.zero;
            //projectileRb.AddForce(transform.up*projectileSpeed);
            isLoaded = false;
            isReturning = false;
            projectileGoalPos = transform.position+transform.up*goalDistance;
            if(!returnTimerCoroutineRunning) {
                StartCoroutine("ProjectileReturnTimer",isReturning);
            }
        } else if(isLoaded) {
            projectile.transform.SetPositionAndRotation(bulletSource.transform.position,bulletSource.transform.rotation);
        } else if(!isLoaded) {
            //Shot projectile is leaving
            if(!isReturning) {
                //Move towards target
                projectileRb.velocity = (projectileGoalPos-projectile.transform.position).normalized*projectileSpeed;
                //Snap to target and start returning
                if(Vector3.Distance(projectile.transform.position,projectileGoalPos)<returnSnapbackRange) {
                    isReturning=true;
                    projectile.transform.position = projectileGoalPos;
                    projectileRb.velocity = Vector3.zero;
                    previousPos = Vector3.zero;
                }
            } else { //Shot projectile is returning
                //Move towards gun
                projectileRb.velocity = (bulletSource.transform.position-projectile.transform.position).normalized*returnSpeed;
                //Snap to gun bullet source and act as a loaded gun
                if(Vector3.Distance(projectile.transform.position,bulletSource.transform.position)<returnSnapbackRange) {
                    isLoaded=true;
                    projectile.transform.position = bulletSource.transform.position;
                    projectileRb.velocity = Vector3.zero;
                    previousPos = Vector3.zero;
                    StopCoroutine("ProjectileReturnTimer");
                    returnTimerCoroutineRunning = false;
                    //projectile.transform.SetParent(bulletSource.transform);
                }
            }
        }

        //If the projectile is not in the gun and is stuck
        /*if(!isLoaded && StuckCheck(projectile.transform.position,(isReturning)?bulletSource.transform.position:projectileGoalPos)) {
            Vector3 tempGoalPos = (isReturning)?bulletSource.transform.position:projectileGoalPos;
            Debug.Log("Projectile stuck, currentpos to goal: "+Vector3.Distance(projectile.transform.position,tempGoalPos)+" previousPos to goal: "+Vector3.Distance(previousPos,tempGoalPos)+" goal: "+ ((isReturning)?"bulletSource":"projectileGoal"));
            if(!isReturning) {
                isReturning = true;
            } else {
                isReturning = false;
                isLoaded=true;
                projectile.transform.position = bulletSource.transform.position;
                projectileRb.velocity = Vector3.zero;
            }
        }*/
    }

    private bool StuckCheck(Vector3 currentPos, Vector3 goalPos) {
        //If currentposition is not closer to the goal than previous position, return true
        bool isStuck;
        if(previousPos==Vector3.zero) { //Initialize previous position if unset
            previousPos = currentPos;
            return false;
        } else { //it is stuck if it is further away from its goal than it was before
            isStuck = Vector3.Distance(currentPos,goalPos)>=Vector3.Distance(previousPos,goalPos);
        }
        return isStuck;
    }

    IEnumerator ProjectileReturnTimer() {
        if(returnTimerCoroutineRunning) {
            yield return null;
        }
        returnTimerCoroutineRunning = true;
        returnTimeStart = Time.time;
        while(true) {
            Debug.Log(Time.time-returnTimeStart);
            if(Time.time - returnTimeStart>=returnTime) {
                isLoaded=true;
                projectile.transform.position = bulletSource.transform.position;
                projectileRb.velocity = Vector3.zero;
                returnTimerCoroutineRunning = false;
                returnTimeStart = Time.time;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
