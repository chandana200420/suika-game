using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform[] memberObject;

    public static string spawnedYet = "n";
    public static Vector2 CloudxPos;

    public static Vector2 SpawnPos;
    public static string newMember = "n";
    public static int whichMember = 0;

    private Rigidbody2D rb;

    [Header("Spawn Settings")]
    [Tooltip("only these early levels can appear directly from the cloud")]
    public int maxSpawnLevel = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Cloud requires a Rigidbody2D component!");
        }
    }

    void Update()
    {
        Vector2 velocity = Vector2.zero;

        bool inputDetected = false;
        Vector2 inputPos = Vector2.zero;

        if (Input.touchCount > 0)
        {
            inputDetected = true;
            inputPos = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            inputDetected = true;
            inputPos = Input.mousePosition;
        }

        if (inputDetected)
        {
            if (inputPos.x > Screen.width / 2)
                velocity = new Vector2(moveSpeed, 0);
            else
                velocity = new Vector2(-moveSpeed, 0);

            if (rb != null)
            {
                rb.velocity = velocity;
            }
        }
        else
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }

        SpawnMember();
        ReplaceMember();
        CloudxPos = transform.position;
    }

    void SpawnMember()
    {
        if (spawnedYet == "n")
        {
            StartCoroutine(SpawnTimer());
            spawnedYet = "y";
        }
    }

    void ReplaceMember()
    {
        if (newMember == "y")
        {
            newMember = "n";

            if (whichMember >= 0 && whichMember < memberObject.Length)
            {
                Instantiate(memberObject[whichMember], SpawnPos, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"Invalid whichMember index: {whichMember}. Array length: {memberObject.Length}");
            }
        }
    }

    System.Collections.IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(0.75f);

        if (memberObject == null || memberObject.Length == 0)
        {
            Debug.LogError("memberObject array is null or empty!");
            yield break;
        }

        int maxIndex = Mathf.Min(maxSpawnLevel + 1, memberObject.Length);
        int randIndex = Random.Range(0, maxIndex);
        whichMember = randIndex;

        if (memberObject[randIndex] != null)
        {
            Instantiate(memberObject[randIndex], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Member prefab at index {randIndex} is null!");
        }
    }
}