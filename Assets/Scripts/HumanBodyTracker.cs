using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Collections;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The cube prefab to be placed above the tracked body.")]
        GameObject m_CubePrefab;

        [SerializeField]
        [Tooltip("The Skeleton prefab to be controlled.")]
        GameObject m_SkeletonPrefab;

        [SerializeField]
        [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
        ARHumanBodyManager m_HumanBodyManager;

        [Header("Frame arms and lock circle")]
        public GameObject[] initializationArmObjs;
        public GameObject initializationLockCircleObj;
        public TextMeshProUGUI initializationTextAsset;

        // Data leak text list to be shown when a body is detected
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

        /// <summary>
        /// Get/Set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager
        {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }

        /// <summary>
        /// Get/Set the cube prefab.
        /// </summary>
        /// <value></value>
        public GameObject cubePrefab
        {
            get { return m_CubePrefab; }
            set { m_CubePrefab = value; }
        }

        /// <summary>
        /// Get/Set the skeleton prefab.
        /// </summary>
        public GameObject skeletonPrefab
        {
            get { return m_SkeletonPrefab; }
            set { m_SkeletonPrefab = value; }
        }

        Dictionary<TrackableId, BoneController> m_SkeletonTracker = new Dictionary<TrackableId, BoneController>();

        void OnEnable()
        {
            Debug.Assert(m_HumanBodyManager != null, "Human body manager is required.");
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
        }

        void OnDisable()
        {
            if (m_HumanBodyManager != null)
                m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
        }

        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {
            BoneController boneController;

            foreach (var humanBody in eventArgs.added)
            {
                if (!m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                    var newSkeletonGO = Instantiate(m_SkeletonPrefab, humanBody.transform);
                    boneController = newSkeletonGO.GetComponent<BoneController>();
                    m_SkeletonTracker.Add(humanBody.trackableId, boneController);

                    // Deactivate the arms and lock circle, and change the initializingText to BodyDetectedText when body is detected
                    foreach (GameObject armObj in initializationArmObjs)
                    {
                        armObj.SetActive(false);
                    }
                    initializationLockCircleObj.SetActive(false);
                    initializationTextAsset.text = "Body Detected and Tracking!";
                }
                boneController.InitializeSkeletonJoints();
                boneController.ApplyBodyPose(humanBody);

                // Instantiate the cube prefab
                GameObject cubeGO = Instantiate(m_CubePrefab);
                cubeGO.transform.parent = humanBody.transform;
                cubeGO.transform.localPosition = Vector3.forward * -1.01f; // Position the cube in front of the body

                // //TODO: Generate text around the body
                // // GenerateTextAroundBody(humanBody.transform.gameObject);
                // // Generate an empty textMeshProUGUI object
                GameObject dataObj = new GameObject();
                dataObj.transform.parent = cubeGO.transform;
                dataObj.transform.localPosition = Vector3.forward * -1.01f;

                // //Add a textMeshProUGUI component to the empty text object
                TextMeshProUGUI dataTexts = dataObj.AddComponent<TextMeshProUGUI>();
                dataTexts.text = "Loading Data...";
                float fontSize = Screen.dpi / 5f;
                dataTexts.fontSize = fontSize;
                dataTexts.alignment = TextAlignmentOptions.Left;
                dataTexts.color = Color.red;
                // // dataTexts.fontSizeMin = 5f;
                // // dataTexts.fontSizeMax = 10f;

                // for (int i = 0; i < dataLeakTextList.Count; i++)
                // {
                //     dataTexts.text += dataLeakTextList[i] + "\n";
                // }

                // // Adjust the dataObj to face the text towards the camera
                // dataObj.transform.localRotation = Quaternion.Euler(0, -180f, 0);
            }

            foreach (var humanBody in eventArgs.updated)
            {
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    boneController.ApplyBodyPose(humanBody);
                }
            }

            foreach (var humanBody in eventArgs.removed)
            {
                Debug.Log($"Removing a skeleton [{humanBody.trackableId}].");
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Destroy(boneController.gameObject);
                    m_SkeletonTracker.Remove(humanBody.trackableId);
                }
            }
        }

        // Generate text around the body
        private void GenerateTextAroundBody(GameObject humanBody)
        {
            Vector3 initialPosition = humanBody.transform.position + Vector3.up * 50f;
            Vector3 positionOffset = Vector3.down * 0.5f; // Adjust the vertical spacing between rows
        }
    }
}