using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject autoScrollTextObj;
    public GameObject verifyingConnectionObj;
    public GameObject initializationObj;
    public GameObject bodyTrackingObj;

    [Header("UI Texts")]
    public GameObject readyText;
    public GameObject verifyingConnectionText;
    public GameObject initializingText;

    // Adjust this value to control the minimum swipe distance required
    private float swipeDistanceThreshold = 100f;
    private Vector2 swipeStartPos;
    private bool isSwipeActive;

    // Fade out duration in seconds
    private bool isFadingOut;
    private float fadeTimer;
    private float fadeOutDuration = 1f;

    private List<string> dataLeakTextList = new List<string>(){

            "Loading Avaliable Data:",
            "",
            "Loading Citizen Profile...",
            "",
            "Identity: Jane Smith",
            "Gender: Female",
            "Date of Birth: 09/12/1985",
            "Nationality: Citizen",
            "Residence: Sector 12B",
            "Social Credit Score: 950",
            "Employment: Employed",
            "Education: Bachelor's Degree",
            "Financial Status: Stable",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class B, Expires 05/15/2026",
            "Passport: Valid, Expires 03/25/2030",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210",
            "Email Address: janesmith@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456",
            "- Account 2: IBAN: 9876_5432_1098_7654",
            "- Account 3: IBAN: 2468_1357_8024_6802",
            "",
            "Data loading complete. Please note that all the information provided here is fictional and randomly generated for illustrative purposes only."
        };

    // Update is called once per frame
    void Update()
    {
        swipeUp();
    }

    //monitor the swipe up gesture
    private void swipeUp()
    {
        // Check for touch input or mouse input
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPos = touch.position;
                isSwipeActive = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwipeActive)
            {
                float swipeDistance = touch.position.y - swipeStartPos.y;

                // Check if the swipe distance exceeds the threshold and it is an upward swipe
                if (swipeDistance > swipeDistanceThreshold && touch.position.y > swipeStartPos.y)
                {
                    if (verifyingConnectionObj != null)
                    {
                        activateVerifyingConnectionAndInitializationAndBodyTracking();
                        FadeOutAutoScrollTextObj();
                    }
                }

                isSwipeActive = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            isSwipeActive = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwipeActive)
        {
            float swipeDistance = Input.mousePosition.y - swipeStartPos.y;

            // Check if the swipe distance exceeds the threshold and it is an upward swipe
            if (swipeDistance > swipeDistanceThreshold && Input.mousePosition.y > swipeStartPos.y)
            {
                activateVerifyingConnectionAndInitializationAndBodyTracking();
                FadeOutAutoScrollTextObj();
            }
            isSwipeActive = false;
        }
    }

    // activate the verifying connection animation
    private void activateVerifyingConnectionAndInitializationAndBodyTracking()
    {
        verifyingConnectionObj.SetActive(true);
        StartCoroutine(DelayActivationOfInitializationObj());
        bodyTrackingObj.SetActive(true);
    }

    // Delay activation of initializationObj
    IEnumerator DelayActivationOfInitializationObj()
    {
        //VerifyingConnectionObj needs 1 seconds to finish its swipe animation
        yield return new WaitForSeconds(1f);
        Destroy(autoScrollTextObj); //Destroy autoScrollTextObj since it is no longer needed

        //Activate initializationObj and readyText
        initializationObj.SetActive(true);
        readyText.SetActive(true);

        //Show the readyText for 0.5 seconds
        yield return new WaitForSeconds(1f);

        //Hide the readyText and show the verifyingConnectionText for 0.875 seconds
        readyText.SetActive(false);
        verifyingConnectionText.SetActive(true);
        yield return new WaitForSeconds(1f);

        //Hide the verifyingConnectionText and show the initializingText
        verifyingConnectionText.SetActive(false);
        initializingText.SetActive(true);
        Destroy(verifyingConnectionObj);// Destroy the verifyingConnectionObj since it is no longer needed
    }

    private void FadeOutAutoScrollTextObj()
    {
        if (autoScrollTextObj != null)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = autoScrollTextObj.transform.localScale;

        while (elapsedTime < fadeOutDuration)
        {
            float t = elapsedTime / fadeOutDuration;
            float scale = Mathf.Lerp(1f, 0f, t);

            autoScrollTextObj.transform.localScale = originalScale * scale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        autoScrollTextObj.SetActive(false);
        autoScrollTextObj.transform.localScale = originalScale;
    }
}
