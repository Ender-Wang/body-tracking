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
            "Loading Citizen Profile...",
            "",

            "Identity: Jane Smith\nGender: Female\nDate of Birth: 09/12/1985\nNationality: Citizen\nResidence: Sector 12B\nSocial Credit Score: 950\nEmployment: Employed\nEducation: Bachelor's Degree\nFinancial Status: Stable\nCriminal Record: None",
            "",

            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class B, Expires 05/15/2026\nPassport: Valid, Expires 03/25/2030\n",
            "",

            "Loading Contact Information...",
            "",
            "Phone Number: +49 4643577\nEmail Address: janesmith@email.com",
            "",

            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456\n- Account 2: IBAN: 9876_5432_1098_7654\n- Account 3: IBAN: 2468_1357_8024_6802",

            "Residence: Apartment 123\nSocial Credit Score: 820\nEmployment: Self-employed\nEducation: High School Diploma\nFinancial Status: Moderate\nCriminal Record: None\n",
            "",

            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class C, Expires 10/15/2024\nPassport: Valid, Expires 08/31/2029",
            "",

            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890\nEmail Address: johndoe@email.com",
            "",

            "Loading Financial Data...",
            "",
            "Bank Accounts:\n- Account 1: IBAN: 9876_5432_1098_7654\n- Account 2: IBAN: 1234_5678_9012_3456\n- Account 3: IBAN: 2468_1357_8024_6802\n",
            "",

            "Nationality: Canadian\nResidence: Unit 45A\nSocial Credit Score: 700\nEmployment: Unemployed\nEducation: Master's Degree\nFinancial Status: Limited\nCriminal Record: None\n",
            "",

            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class G, Expires 07/22/2025\nPassport: Valid, Expires 06/15/2032",
            "",

            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210\nEmail Address: emilyjohnson@email.com",
            "",

            "Loading Financial Data...",
            "",
            "Bank Accounts:\n- Account 1: IBAN: 1234_5678_9012_3456\n- Account 2: IBAN: 9876_5432_1098_7654\n- Account 3: IBAN: 1357_2468_8024_6802\n",
            "",

            "Residence: House 789\nSocial Credit Score: 55\nEmployment: Employed\nEducation: Doctorate Degre\nFinancial Status: Stabl\nCriminal Record: Non\n",
            "",

            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class D, Expires 11/30/2027\nPassport: Valid, Expires 09/20/2035",
            "",

            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890\nEmail Address: michaelbrown@email.com\n",
            "",

            "Loading Financial Data...",
            "",
            "Bank Accounts:\n- Account 1: IBAN: 9876_5432_1098_7654\n- Account 2: IBAN: 1234_5678_9012_3456\n- Account 3: IBAN: 8024_6802_1357_2468\n",
            "",

            "Loading Citizen Profile...",
            "",
            "Identity: Jane Smith\nGender: Female\nDate of Birth: 09/12/1985\nNationality: Citizen\nResidence: Sector 12B\nSocial Credit Score: 950\nEmployment: Employed\nEducation: Bachelor's Degree\nFinancial Status: Stable\nCriminal Record: None\n",
            "",

            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class B, Expires 05/15/2026\nPassport: Valid, Expires 03/25/2030",
            "",

            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210\nEmail Address: janesmith@email.com",
            "",

            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456",
            "- Account 2: IBAN: 9876_5432_1098_7654",
            "- Account 3: IBAN: 2468_1357_8024_6802",
            "",
            "Residence: Apartment 123",
            "Social Credit Score: 820",
            "Employment: Self-employed",
            "Education: High School Diploma",
            "Financial Status: Moderate",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class C, Expires 10/15/2024",
            "Passport: Valid, Expires 08/31/2029",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: johndoe@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 2468_1357_8024_6802",
            "",
            "Nationality: Canadian",
            "Residence: Unit 45A",
            "Social Credit Score: 700",
            "Employment: Unemployed",
            "Education: Master's Degree",
            "Financial Status: Limited",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class G, Expires 07/22/2025",
            "Passport: Valid, Expires 06/15/2032",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210",
            "Email Address: emilyjohnson@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456",
            "- Account 2: IBAN: 9876_5432_1098_7654",
            "- Account 3: IBAN: 1357_2468_8024_6802",
            "",
            "Residence: House 789",
            "Social Credit Score: 550",
            "Employment: Employed",
            "Education: Doctorate Degree",
            "Financial Status: Stable",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class D, Expires 11/30/2027",
            "Passport: Valid, Expires 09/20/2035",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: michaelbrown@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 8024_6802_1357_2468",
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
            "Residence: Apartment 123",
            "Social Credit Score: 820",
            "Employment: Self-employed",
            "Education: High School Diploma",
            "Financial Status: Moderate",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class C, Expires 10/15/2024",
            "Passport: Valid, Expires 08/31/2029",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: johndoe@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 2468_1357_8024_6802",
            "",
            "Nationality: Canadian",
            "Residence: Unit 45A",
            "Social Credit Score: 700",
            "Employment: Unemployed",
            "Education: Master's Degree",
            "Financial Status: Limited",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class G, Expires 07/22/2025",
            "Passport: Valid, Expires 06/15/2032",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210",
            "Email Address: emilyjohnson@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456",
            "- Account 2: IBAN: 9876_5432_1098_7654",
            "- Account 3: IBAN: 1357_2468_8024_6802",
            "",
            "Residence: House 789",
            "Social Credit Score: 550",
            "Employment: Employed",
            "Education: Doctorate Degree",
            "Financial Status: Stable",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class D, Expires 11/30/2027",
            "Passport: Valid, Expires 09/20/2035",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: michaelbrown@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 8024_6802_1357_2468",
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
            "Residence: Apartment 123",
            "Social Credit Score: 820",
            "Employment: Self-employed",
            "Education: High School Diploma",
            "Financial Status: Moderate",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class C, Expires 10/15/2024",
            "Passport: Valid, Expires 08/31/2029",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: johndoe@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 2468_1357_8024_6802",
            "",
            "Nationality: Canadian",
            "Residence: Unit 45A",
            "Social Credit Score: 700",
            "Employment: Unemployed",
            "Education: Master's Degree",
            "Financial Status: Limited",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class G, Expires 07/22/2025",
            "Passport: Valid, Expires 06/15/2032",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +9876543210",
            "Email Address: emilyjohnson@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 1234_5678_9012_3456",
            "- Account 2: IBAN: 9876_5432_1098_7654",
            "- Account 3: IBAN: 1357_2468_8024_6802",
            "",
            "Residence: House 789",
            "Social Credit Score: 550",
            "Employment: Employed",
            "Education: Doctorate Degree",
            "Financial Status: Stable",
            "Criminal Record: None",
            "",
            "Loading Identification Documents...",
            "",
            "Driver's License: Valid, Class D, Expires 11/30/2027",
            "Passport: Valid, Expires 09/20/2035",
            "",
            "Loading Contact Information...",
            "",
            "Phone Number: +1234567890",
            "Email Address: michaelbrown@email.com",
            "",
            "Loading Financial Data...",
            "",
            "Bank Accounts:",
            "- Account 1: IBAN: 9876_5432_1098_7654",
            "- Account 2: IBAN: 1234_5678_9012_3456",
            "- Account 3: IBAN: 8024_6802_1357_2468",
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

                //Generate hundreds of text objects around the body
                StartCoroutine(GenerateRandomText(cubeGO));
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
        private IEnumerator GenerateRandomText(GameObject cubeGO)
        {
            int textCount = dataLeakTextList.Count;
            // loop through dataLeakTextList and instantiate a text object for each string
            for (int i = 0; i < dataLeakTextList.Count; i++)
            {
                GameObject textObj = new GameObject("Text");
                textObj.transform.parent = cubeGO.transform;
                textObj.transform.localPosition = GetRandomPosition();

                // add a TextMeshPro component to the text object, set proper text size and scale
                TextMeshPro textMesh = textObj.AddComponent<TextMeshPro>();
                textMesh.text = dataLeakTextList[i];
                textMesh.enableWordWrapping = false;
                textMesh.fontSize = 24f;
                textMesh.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                // rotate text object to face the camera
                Transform cameraTransform = Camera.main.transform;
                textObj.transform.rotation = Quaternion.LookRotation(textObj.transform.position - cameraTransform.position);
                StartCoroutine(UpdateTextRotation(textObj.transform, cameraTransform));

                // lines of dataLeakTextList divided by 30 seconds
                yield return new WaitForSeconds(30f / textCount);
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

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(-3f, 3f);
            float y = Random.Range(-2f, 2f);

            return new Vector3(x, y, 0);
        }
    }
}