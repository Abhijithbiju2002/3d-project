using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float ForwardRayLength = 0.8f;
    [SerializeField] LayerMask obstacleLayer;

    public ObstacleHitData CheckObstacle()
    {
        var hitData = new ObstacleHitData();

        var forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(transform.position + forwardRayOffset, transform.forward,
          out hitData.forwadHit, ForwardRayLength, obstacleLayer);//this line gives the foeward raycast


        Debug.DrawRay(forwardOrigin, transform.forward * ForwardRayLength, (hitData.forwardHitFound) ? Color.red : Color.green);

        return hitData;
    }


}
public struct ObstacleHitData
{
    public bool forwardHitFound;
    public RaycastHit forwadHit;
}




