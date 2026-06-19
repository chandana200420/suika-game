using UnityEngine;
using UnityEngine.UI;

public class NextMemberDisplay : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Image backgroundCircle;
    public Image memberPreviewImage;
    public GameObject[] MemberPrefabs;

    [Header("Preview Size Settings")]
    [Tooltip("Percentage of the circle size to use for the member preview (0-1)")]
    public float previewSizePercent = 0.7f;

    private int lastShownIndex = -1;
    private GameObject previewImageObject;

    private void Start()
    {
        SetupPreviewImage();
    }

    private void Update()
    {
        if (Cloud.whichMember != lastShownIndex)
        {
            UpdateNextMemberSprite();
            lastShownIndex = Cloud.whichMember;
        }
    }

    private void SetupPreviewImage()
    {
        if (memberPreviewImage == null && backgroundCircle != null)
        {
            GameObject childObj = new GameObject("MemberPreview");
            childObj.transform.SetParent(backgroundCircle.transform, false);

            memberPreviewImage = childObj.AddComponent<Image>();
            memberPreviewImage.raycastTarget = false;
            memberPreviewImage.preserveAspect = true;

            RectTransform rt = childObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;

            RectTransform bgRect = backgroundCircle.GetComponent<RectTransform>();
            float circleSize = Mathf.Min(bgRect.sizeDelta.x, bgRect.sizeDelta.y);
            float previewSize = circleSize * previewSizePercent;
            rt.sizeDelta = new Vector2(previewSize, previewSize);

            previewImageObject = childObj;
     
        }
    }

    private void UpdateNextMemberSprite()
    {
        if (memberPreviewImage == null || MemberPrefabs == null || MemberPrefabs.Length == 0)
        {
            Debug.LogWarning("Cannot update preview: memberPreviewImage or MemberPrefabs is null/empty");
            return;
        }

        int index = Mathf.Clamp(Cloud.whichMember, 0, MemberPrefabs.Length - 1);

        if (MemberPrefabs[index] == null)
        {
            Debug.LogWarning($"Member prefab at index {index} is null!");
            memberPreviewImage.enabled = false;
            return;
        }

        GameObject prefab = MemberPrefabs[index];
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();

        if (sr != null && sr.sprite != null)
        {
            memberPreviewImage.sprite = sr.sprite;
            memberPreviewImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("Prefab has no SpriteRenderer or sprite: " + prefab.name);
            memberPreviewImage.enabled = false;
        }
    }
}
