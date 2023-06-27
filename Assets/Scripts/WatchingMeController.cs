using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WatchingMeController : MonoBehaviour
{
    public GameObject watchingMeObj;
    public GameObject watchingMeTextObj;

    void OnEnable()
    {
        StartCoroutine(PopWatchingMeMsgStreamToScreen(watchingMeTextObj, 42, 250f, 1019f));
    }

    IEnumerator PopWatchingMeMsgStreamToScreen(GameObject watchingMeTextObj, int iteration, float xOffset, float yOffset)
    {
        // 3s of white screen
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < iteration; i++)
        {
            GameObject watchingText = Instantiate(watchingMeTextObj, watchingMeObj.transform);
            watchingText.SetActive(true);
            float localXOffset = i < 20 ? 0f : xOffset;
            float localYOffset = i < 20 ? 0f : yOffset;
            watchingText.transform.localPosition = new Vector3(watchingMeTextObj.transform.localPosition.x + localXOffset, watchingMeTextObj.transform.localPosition.y + localYOffset - 50f * i, watchingMeTextObj.transform.localPosition.z);
            TextMeshProUGUI textMesh = watchingText.GetComponent<TextMeshProUGUI>();
            string text = textMesh.text;    // Get the text from the text mesh
            textMesh.text = "";            // Clear the text mesh
            for (int j = 0; j < text.Length; j++)
            {
                textMesh.text += text.Substring(j, 1);
                yield return new WaitForSeconds(0.3333f / text.Length);
            }
        }
    }
}
