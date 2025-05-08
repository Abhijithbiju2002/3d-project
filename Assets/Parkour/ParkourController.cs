using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;

    private void Update()
    {
        var hitData = environmentChecker.CheckObstacle();
        //  environmentChecker.CheckObstacle();
        if (hitData.hitFound)
        {
            Debug.Log("Object found " + hitData.hitInfo.transform.name);
        }

    }
}
