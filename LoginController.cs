using System.Collections;
using System.Data;
using System.Data.SqlClient;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public InputField userNameField;
    public InputField passwordField;
    public Button loginButton;
    private string connectionString;
    public GameObject usernotfound;
    private static SqlConnection connection;
    void Start()
    {
        usernotfound.SetActive(false);
        connectionString = "data source=DESKTOP-HEHO5PK;Database=unityMR;Integrated Security=true;";
        connection = new SqlConnection(connectionString);
    }
    //connectionString = "data source=DESKTOP-HEHO5PK;Database=unityMR;Trusted_Connection=True;";
    //connectionString = "Server=DESKTOP-8FCRLH5\\SQLEXPRESS;Database=unityMR;User ID=sa;Password=Ggs###2023;Trusted_Connection=True;";
    public void Login()
    {
        string username = userNameField.text;
        string password = passwordField.text;

        using (connection)
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
                Debug.Log("Connected to database");

            string query = $"SELECT * FROM userDetails WHERE cUserName = @username AND cPassword = @password";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
              command.Parameters.AddWithValue("@username", username);
               command.Parameters.AddWithValue("@password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        SceneManager.LoadScene("Success");
                    else
                    {
                        usernotfound.SetActive(true);
                        StartCoroutine(DeactivateUserNotFound(3f));
                    }

                }
            }
            Debug.Log("Database connection ccc" + connection.State);
            // connection.Close();
        }
    }
    public void Logout()
    {
        Debug.Log("Database connection ss" + connection.State);
        if (connection != null && connection.State != ConnectionState.Closed)
        {
            connection.Close(); // Close the database connection if it's not already closed
            Debug.Log("Database connection closed.");
        }
        // Perform any additional logout actions here (e.g., navigating to another scene)
    }
    IEnumerator DeactivateUserNotFound(float delay)
    {
        yield return new WaitForSeconds(delay);
        usernotfound.SetActive(false);
    }
    
}