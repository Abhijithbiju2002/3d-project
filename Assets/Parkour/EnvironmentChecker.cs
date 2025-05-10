using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float ForwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] LayerMask obstacleLayer;


    public ObstacleHitData CheckObstacle()
    {
        var hitData = new ObstacleHitData();

        var forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(transform.position + forwardRayOffset, transform.forward,
          out hitData.forwadHit, ForwardRayLength, obstacleLayer);//this line gives the foeward raycast

        Debug.DrawRay(forwardOrigin, transform.forward * ForwardRayLength, (hitData.forwardHitFound) ? Color.red : Color.green);
        //to be visible in game


        if (hitData.forwardHitFound)
        {
            var heightOrigin = hitData.forwadHit.point + Vector3.up * heightRayLength;
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down,
                out hitData.heightHit, heightRayLength, obstacleLayer);//height raycast after the hit data hits

            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitData.forwardHitFound) ? Color.red : Color.green);
            //to be visible in game

        }
        return hitData;

    }


}
public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwadHit;
    public RaycastHit heightHit;
}




