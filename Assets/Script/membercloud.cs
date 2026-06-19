using UnityEngine;

public class membercloud : MonoBehaviour
{
    public string inthecloud = "y";
    private Rigidbody2D rb;
    private bool hasCollided = false;
    public GameObject[] memberPrefabs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("membercloud requires a Rigidbody2D component!");
            return;
        }

        if (transform.position.y < 1.0f)
        {
            inthecloud = "n";
            rb.gravityScale = 1;
        }
        else
        {
            inthecloud = "y";
            rb.gravityScale = 0;
        }
    }

    void Update()
    {
        // Follow the cloud only while still in the cloud
        if (inthecloud == "y")
        {
            Vector3 pos = Cloud.CloudxPos;
            pos.y = transform.position.y;
            pos.z = transform.position.z;
            transform.position = pos;
        }

        bool drop = false;

#if UNITY_ANDROID || UNITY_IOS
        // Android → Tap on the object
        drop = IsTouched();
#else
        // Windows / Laptop → Click + IsTouched()
        drop = (IsTouched() && Input.GetMouseButtonDown(0));
#endif

        if (inthecloud == "y" && drop)
        {
            SpawnObject();
        }
    }

    bool IsTouched()
    {
        Vector2 touchPos = Vector2.zero;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        else
            return false;
#else
        if (Input.GetMouseButtonDown(0))
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else
            return false;
#endif

        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null)
        {
            return myCollider == Physics2D.OverlapPoint(touchPos);
        }

        return false;
    }

    void SpawnObject()
    {
        if (rb != null)
        {
            rb.gravityScale = 1;
        }
        inthecloud = "n";
        Cloud.spawnedYet = "n";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided || inthecloud == "y")
        {
            return;
        }
        if (collision.gameObject.tag == gameObject.tag)
        {
            membercloud otherMember = collision.gameObject.GetComponent<membercloud>();
            if (otherMember != null && otherMember.hasCollided)
                return;

            if (int.TryParse(gameObject.tag, out int tagNum))
            {
                hasCollided = true;
                if (otherMember != null)
                    otherMember.hasCollided = true;

                int nextLevelIndex = tagNum + 1;

                // === SCORE BASED ON MERGE LEVEL ===
                int mergePoints = (int)Mathf.Pow(2, nextLevelIndex);
                GameManager.instance.AddScore(mergePoints);

                // PLAY MERGE EFFECT
                if (MergeEffectManager.instance != null)
                    MergeEffectManager.instance.PlayMergeEffect(transform.position);

                // MAX LEVEL CHECK
                if (nextLevelIndex >= memberPrefabs.Length)
                {
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    return;
                }

                // SPAWN NEXT MERGED MEMBER
                Cloud.SpawnPos = transform.position;
                Cloud.newMember = "y";
                Cloud.whichMember = nextLevelIndex;

                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
