using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    // [Header("Canvas Prefab")]
    // public GameObject canvasPrefab; // The prefab of the Canvas objects

    [Header("UI Objects")]
    public GameObject autoScrollTextObj;
    public GameObject verifyingConnectionObj;
    public GameObject initializationObj;
    public GameObject bodyTrackingObj;
    public GameObject dataOverloadObj;
    public GameObject watchingMeObj;
    public GameObject endingObj;

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

    private bool monitoringSwipe;
    void Start()
    {
        StartCoroutine(DelayedSwipeMonitoring());
        // CanvasStateManager.Instance.InstantiateCanvas();
    }

    IEnumerator DelayedSwipeMonitoring()
    {
        yield return new WaitForSeconds(5f);
        monitoringSwipe = true;
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        if (monitoringSwipe)
        {
            swipeUp();
        }
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

        StartCoroutine(DelayErrorAndWatchingEnding());
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

    IEnumerator DelayErrorAndWatchingEnding()
    {
        // Destroy bodyTrackingObj after 30 seconds
        yield return new WaitForSeconds(30f);
        Destroy(bodyTrackingObj);
        Destroy(initializingText);
        Destroy(initializationObj);

        // Activate dataOverloadObj and show the dataLeakTextList
        dataOverloadObj.SetActive(true);

        // Show the dataLeakTextList for 22 seconds
        yield return new WaitForSeconds(22f);
        Destroy(dataOverloadObj);

        // Activate watchingMeObj and show the watchingMeText
        watchingMeObj.SetActive(true);

        // Show the watchingMeText for 22 seconds
        yield return new WaitForSeconds(22f);
        Destroy(watchingMeObj);

        // Activate endingObj and show the endingText
        endingObj.SetActive(true);
        yield return null;
    }
}
