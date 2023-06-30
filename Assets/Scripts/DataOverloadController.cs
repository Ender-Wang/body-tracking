using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataOverloadController : MonoBehaviour
{
    public GameObject dataOverloadObj;
    public GameObject errorTextObj;

    void OnEnable()
    {
        StartCoroutine(PopErrorMsgStreamToScreen(errorTextObj, 42, 250f, 1019f));
    }

    IEnumerator PopErrorMsgStreamToScreen(GameObject errorTextObj, int iteration, float xOffset, float yOffset)
    {
        for (int i = 0; i < iteration; i++)
        {
            GameObject errorText = Instantiate(errorTextObj, dataOverloadObj.transform);
            errorText.SetActive(true);
            float localXOffset = i < 20 ? 0f : xOffset;
            float localYOffset = i < 20 ? 0f : yOffset;
            errorText.transform.localPosition = new Vector3(errorTextObj.transform.localPosition.x + localXOffset, errorTextObj.transform.localPosition.y + localYOffset - 50f * i, errorTextObj.transform.localPosition.z);
            TextMeshProUGUI textMesh = errorText.GetComponent<TextMeshProUGUI>();
            string text = textMesh.text;    // Get the text from the text mesh
            textMesh.text = "";            // Clear the text mesh
            for (int j = 0; j < text.Length; j++)
            {
                textMesh.text += text.Substring(j, 1);
                yield return new WaitForSeconds(0.238f / text.Length);
            }
        }
    }
}
