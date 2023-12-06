using UnityEngine;
using UnityEngine.UI;
using Vuforia;
 
public class DatabaseSwitcher : MonoBehaviour
{
    public string[] databaseNames; // Set the database names in the Inspector
    public float switchDelay = 1f; // Set a delay between switching databases
    public Button switchButton; // Reference to your UI button
 
    private ObjectTracker objectTracker;
 
    void Start()
    {
        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        switchButton.onClick.AddListener(SwitchDatabases);
    }
 
    void SwitchDatabases()
    {
        StartCoroutine(SwitchDatabasesSequentially());
    }
 
    IEnumerator SwitchDatabasesSequentially()
    {
        foreach (string databaseName in databaseNames)
        {
            yield return StartCoroutine(SwitchDatabase(databaseName));
            yield return new WaitForSeconds(switchDelay);
        }
    }
 
    IEnumerator SwitchDatabase(string newDatabaseName)
    {
        // Stop the Vuforia tracker before switching databases
        objectTracker.Stop();
 
        // Unload the current dataset
        var activeDataSets = objectTracker.GetActiveDataSets();
        if (activeDataSets.Length > 0)
        {
            objectTracker.DeactivateDataSet(activeDataSets[0]);
            objectTrackerBehaviour.DestroyAllDataSets(false);
        }
 
        // Load the new database
        ObjectTrackerBaseBehaviour objectTrackerBehaviour = TrackerManager.Instance.GetTrackerBehaviour<ObjectTrackerBaseBehaviour>();
        DataSet dataSet = objectTrackerBehaviour.DataSetExists(newDatabaseName) ?
                         objectTrackerBehaviour.ActivateDataSet(newDatabaseName) :
                         objectTrackerBehaviour.AddDataSet(newDatabaseName);
 
        // Reactivate the tracker
        objectTrackerBehaviour.ActivateDataSet(dataSet);
        objectTracker.Start();
    }
}