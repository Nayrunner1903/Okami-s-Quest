using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nay{


public class CameraHandler : MonoBehaviour
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers;
    public LayerMask environmentLayer;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler singleton;

    public float lookspeed = 0.1f;
    public float followspeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    public float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;

    public float camSphereRad = 0.2f;
    public float camCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public float lockPivotPosition = 2.25f;
    public float unlockPivotPosition = 1.65f;

    public Transform currentLockOnTarget;
    public Transform leftLockTarget;
    public Transform rightLockTarget;

    public float maximumLockOnDistance = 30f;
    public Transform nearestLockOnTarget;
    List<CharactorManager> avilableTargets = new List<CharactorManager>();




    private void Awake() {
        singleton = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 <<9 | 1 << 10);

        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputHandler = FindObjectOfType<InputHandler>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start() 
    {
        environmentLayer = LayerMask.NameToLayer("Environment");
    }


    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position,targetTransform.position , ref cameraFollowVelocity , delta / followspeed);
        myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }







    public void  HandleCameraRotation(float delta,float mouseXInput , float mouseYInput )
    {
        if(inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookspeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot,maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            float velocity = 0;

            Vector3 dir = currentLockOnTarget.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.position - cameraPivotTransform.position;
            dir.Normalize ();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;

        }

        
    }



    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if(   Physics.SphereCast
            ( cameraPivotTransform.position , camSphereRad , direction , out hit , Mathf.Abs(targetPosition) , ignoreLayers ))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position,hit.point);
            targetPosition = -(dis - camCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition = -minCollisionOffset;
        }

        cameraTransformPosition.z  = Mathf.Lerp(cameraTransform.localPosition.z,targetPosition,delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;

    }


    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(targetTransform.position , 26);

        for(int i =0; i < colliders.Length; i++)
        {
            CharactorManager charactor = colliders[i].GetComponent<CharactorManager>();

            if(charactor != null)
            {
                Vector3 lockTargetDiraction =charactor.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position , charactor.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDiraction, cameraTransform.forward);
                RaycastHit hit;

                if(charactor.transform.root != targetTransform.transform.root 
                    && viewableAngle > -50 && viewableAngle <50 && distanceFromTarget <=  maximumLockOnDistance)
                    {
                        if( Physics.Linecast(playerManager.lockOnTransform.position , charactor.lockOnTransform.position , out hit) )
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position , charactor.lockOnTransform.position);
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                //cannot lock
                            }
                            else
                            {
                                avilableTargets.Add(charactor);
                            }
                        }
                        
                    }
            }
        }//forloop

        for(int k =0; k < avilableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position , avilableTargets[k].transform.position);

            if(distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = avilableTargets[k].lockOnTransform;
            }

            if (inputHandler.lockOnFlag)
            {
                Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(avilableTargets[k].transform.position);
                var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - avilableTargets[k].transform.position.x;
                var distanceFromRightTarget = currentLockOnTarget.transform.position.x + avilableTargets[k].transform.position.x;

                if(relativeEnemyPosition.x> 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = avilableTargets[k].lockOnTransform;
                }

                if(relativeEnemyPosition.x< 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = avilableTargets[k].lockOnTransform;
                }

            }

        }//forloop

    }//HandleLockOn


    public void ClearLockOnTarget()
    {
        avilableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }


    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0,lockPivotPosition);
        Vector3 newUnlockPositon = new Vector3(0 , unlockPivotPosition);

        if(currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp( cameraPivotTransform.transform.localPosition 
                                                                                , newLockedPosition , ref velocity , Time.deltaTime );
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp( cameraPivotTransform.transform.localPosition 
                                                                                , newUnlockPositon , ref velocity , Time.deltaTime );
        }
    }



}//class
}//Nay