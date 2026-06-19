using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFixer : MonoBehaviour
{
    void LateUpdate()
    {
        //if new member is waiting to be spawned , do it immediately
        if (Cloud.newMember == "y")
        {
            Cloud.newMember = "n";
            Cloud.spawnedYet = "n";
            Vector2 pos = Cloud.SpawnPos;
            if (Cloud.whichMember >= 0)
            {
                Instantiate(FindObjectOfType<Cloud>().memberObject[Cloud.whichMember], pos, Quaternion.identity);
            }
        }
    }
}
