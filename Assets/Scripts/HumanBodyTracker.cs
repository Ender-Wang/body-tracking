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

                //TODO: Generate thousands of text text around the body

                // Create a text object and parent it to the cube
                GameObject textObj = new GameObject("Text2");
                textObj.transform.parent = cubeGO.transform;
                textObj.transform.localPosition = new Vector3(0, 0, 0);

                // add a TextMeshPro component to the text object, set proper text size and scale
                TextMeshPro textMesh2 = textObj.AddComponent<TextMeshPro>();
                textMesh2.text = "Loading Data 2...";
                textMesh2.fontSize = 24f;
                textMesh2.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                // rotate text object to face the camera
                Transform cameraTransform = Camera.main.transform;
                textObj.transform.rotation = Quaternion.LookRotation(textObj.transform.position - cameraTransform.position);
                StartCoroutine(UpdateTextRotation(textObj.transform, cameraTransform));
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
        IEnumerator UpdateTextRotation(Transform textTransform, Transform cameraTransform)
        {
            while (true)
            {
                // Calculate the desired rotation to face the camera
                Quaternion targetRotation = Quaternion.LookRotation(textTransform.position - cameraTransform.position);

                // Smoothly interpolate the rotation to avoid sudden changes
                textTransform.rotation = Quaternion.Slerp(textTransform.rotation, targetRotation, Time.deltaTime * 5f);

                yield return null;
            }
        }

        // Generate text around the body
        private void GenerateRandomTextAroundBody(GameObject cubeGO)
        {
            int randomIndex = Random.Range(0, dataLeakTextList.Count);
            return dataLeakTextList[randomIndex];
        }
    }
}