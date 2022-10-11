using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeFinish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<PlatformMoveInMaze>().isBrake = true;
        Debug.Log("Finish!");
    }
}
