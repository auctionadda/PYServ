using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

[Serializable]
public class SubMeshess
{
    public GameObject meshRenderer;
    public Vector3 originalPosition;
    public Vector3 explodedPosition;
    
}
public class ExplosionData : MonoBehaviour
{
    [SerializeField]
    private VolvoEventManager volvoEventManager;
    [SerializeField]
    private ClickDetector clickdetector;
    public List<SubMeshess> childMeshRenderers;
    bool isInExplodedView = false;
    public float explosionSpeed = 1f; // Adjust speed as needed
    bool isMoving = false;
    public float Explodedistance = 1.2f;
    private static int hasClicked = 0;


    private void OnEnable()
    {
        volvoEventManager = GameObject.FindObjectOfType<VolvoEventManager>();
        if (volvoEventManager == null)
        {
            Debug.LogError("VolvoEventManager not found.");
        }

        clickdetector = GameObject.FindObjectOfType<ClickDetector>();
        if (clickdetector == null)
        {
            Debug.LogError("ClickDetector not found.");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasClicked == 0)
        {
                Debug.LogWarning("Mouse Clicked");
                RaycastFromInput(Input.mousePosition);
                hasClicked++;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            hasClicked = 0;
            Debug.LogWarning("Mouse up Clicked");

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && hasClicked == 0)
        {
            Debug.LogWarning("Touched ");
            RaycastFromInput(Input.GetTouch(0).position);
            hasClicked++;
        }
        else if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
        {
            hasClicked = 0;
            Debug.LogWarning("Touch Ended or Canceled");
        }
    }

    private void RaycastFromInput(Vector2 inputPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPosition), out hit))
        {
            GameObject touchedObject = hit.collider.gameObject;
            if (touchedObject != null)
            {
                Debug.Log("Touched Object name is " + touchedObject.name);
                Debug.Log("Childrens of this = " + touchedObject.transform.childCount);
                if (touchedObject.transform.childCount > 0)
                {
                    ExplosionData modelFunctions = touchedObject.GetComponent<ExplosionData>();
                    if (modelFunctions == null)
                        modelFunctions = touchedObject.AddComponent<ExplosionData>();
                    modelFunctions.ToggleExplosion();
                    modelFunctions.InitializeChildMeshRenderers();
                    if (volvoEventManager != null)
                        volvoEventManager.OnSelectedrotationEvent.Invoke(touchedObject);
                    DisableOtherModels(touchedObject);
                }
                else if (touchedObject.GetComponent<CamView>() == null)
                {
                    Debug.Log("Already Clicked 2");
                    touchedObject.AddComponent<CamView>();
                    if (volvoEventManager != null)
                        volvoEventManager.OnSelectedPartEvent.Invoke(touchedObject);
                    else
                        Debug.Log("Error in eventmanager");
                }
                else
                {
                    Debug.Log("Already Clicked 1");
                }
            }
        }
    }
    public void InitializeChildMeshRenderers()
    {
        childMeshRenderers = new List<SubMeshess>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            Vector3 originalPosition = childTransform.position; // Get the original world position of the child
            Vector3 explodedPosition = originalPosition + (childTransform.position - transform.position) * Explodedistance; // Calculate exploded position relative to child's original world position
            SubMeshess mesh = new SubMeshess
            {
                meshRenderer = childTransform.gameObject,
                originalPosition = originalPosition,
                explodedPosition = explodedPosition
            };
            childMeshRenderers.Add(mesh);
        }
    }
    public void ToggleExplosion()
    {
        if (!isMoving)
        {
            isInExplodedView = !isInExplodedView;
            StartCoroutine(MoveToTargetPosition(isInExplodedView));
        }
    }
    private IEnumerator MoveToTargetPosition(bool explode)
    {
        isMoving = true;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * explosionSpeed;
            foreach (var item in childMeshRenderers)
            {
                if (explode)
                    item.meshRenderer.transform.position = Vector3.Lerp(item.originalPosition, item.explodedPosition, t);
                //else - Rework if toggle implode is needed
                //     item.meshRenderer.transform.position = Vector3.Lerp(item.explodedPosition, item.originalPosition, t);
                item.meshRenderer.GetComponent<Collider>().enabled = true;
                if (item.meshRenderer.transform.childCount == 0)
                {
                    Debug.Log("No Children found");
                    if (item.meshRenderer.GetComponent<ExplosionData>() == null)
                        item.meshRenderer.AddComponent<ExplosionData>();
                }
            }
            yield return null;
        }
        gameObject.GetComponent<Collider>().enabled = false; // Disable maingameobject collider
        gameObject.GetComponent<ExplosionData>().enabled = false;
        isMoving = false;
    }
private void DisableOtherModels(GameObject touchedObject)
{
    // Get the parent of the touched object
    Transform parentObject = touchedObject.transform.parent;
    print("DisableOtherModels parent object = " + parentObject.name);

    // Iterate through all the children of the parent object
    for (int i = 0; i < parentObject.childCount; i++)
    {
        // Get the child transform
        Transform model = parentObject.GetChild(i);

        // If the child is not the touched object
        if (model.gameObject != touchedObject)
                model.gameObject.SetActive(false); // Added to hide the other gameobjects
    }
}
}