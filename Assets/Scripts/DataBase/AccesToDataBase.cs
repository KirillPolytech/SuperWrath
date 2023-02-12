using UnityEngine;
using System.Data.Odbc;
using System.Collections.Generic;
using System;

public class AccesToDataBase : MonoBehaviour
{
    private static GameObject __dataBaseObject;
    readonly private static string connectionstring = "Dsn=RecordsDataBase;uid=root"; 
    private void Awake()
    {
        if (!__dataBaseObject)
        {
            __dataBaseObject = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static bool LoginUser(string Login, string Password)
    {
        // находим нашего юзера в бд.
        string queryStringLogin = $"SELECT * FROM user WHERE Login = '{Login}' AND Password = '{Password}'";

        using (OdbcConnection connection = new OdbcConnection(connectionstring))
        {
            OdbcCommand SQLcommand = new OdbcCommand(queryStringLogin, connection);

            connection.Open();

            // Execute the DataReader and access the data.
            OdbcDataReader reader = SQLcommand.ExecuteReader();
            if (reader.Read())
            {
                CurrentUserData.CurrentUserId = Convert.ToInt32(reader[0]);
                CurrentUserData.CurrentEmail = Convert.ToString(reader[1]);

                Debug.Log("Successesful Find: user_id = " + CurrentUserData.CurrentUserId);
                reader.Close();
                connection.Close();
                return true;
            }

            Debug.Log("Find Error");
            reader.Close();
            connection.Close();
            return false;
        }
    }
    public static void RegisterUser(string userName, string email, string password)
    {
        OdbcConnection con = new OdbcConnection(connectionstring);
        try
        {
            con.Open();
            Debug.Log("Connection success");
        }
        catch (Exception e)
        {
            Debug.Log("Connection error " + e.Message);
        }

        string sql = $"insert into user(Login,Password, Email) values('{userName}','{password}','{email}')";
        OdbcCommand cm = new OdbcCommand(sql, con);
        cm.ExecuteNonQuery();

        Debug.Log("Register Succssed");

        con.Close();
    }
    public static void SetRecord(int time, string date, int userid)
    {
        string setrecordstring = $"INSERT INTO records(Time, Date, FK_user_id) values('{time}','{date}', {userid})";
        OdbcConnection con = new OdbcConnection(connectionstring);
        try
        {
            con.Open();
            Debug.Log("Connection success");
        }
        catch (Exception e)
        {
            Debug.Log("Connection error " + e.Message);
        }

        OdbcCommand cm = new OdbcCommand(setrecordstring, con);
        cm.ExecuteNonQuery();

        con.Close();
    }
    public static void ShowRecords()
    {
        string queryString = "SELECT * FROM records";

        using (OdbcConnection connection = new OdbcConnection(connectionstring))
        {
            OdbcCommand command = new OdbcCommand(queryString, connection);

            connection.Open();

            // Execute the DataReader and access the data.
            OdbcDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Debug.Log(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3]);
            }

            reader.Close();
            connection.Close();
        }
    }
    public static LinkedList<RecordData> GetDataFromRecords()
    {
        string queryString = "SELECT * FROM records ORDER BY Time";
        LinkedList<RecordData> _linkedListUserData = new LinkedList<RecordData>();

        using (OdbcConnection connection = new OdbcConnection(connectionstring))
        {
            OdbcCommand command = new OdbcCommand(queryString, connection);

            connection.Open();

            // Execute the DataReader and access the data.
            OdbcDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {    
                RecordData RecordData = new RecordData(Convert.ToInt32(reader[1]), Convert.ToString(reader[2]));
                _linkedListUserData.AddLast(RecordData);
                //Debug.Log(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3]);
            }

            reader.Close();
            connection.Close();
        }
        return _linkedListUserData;
    }
    public static void ShowDataBase()
    {
        //string queryString = "SHOW TABLES"; // Выводи только сущности
        string queryString = "SELECT * FROM user";

        using (OdbcConnection connection = new OdbcConnection(connectionstring))
        {
            OdbcCommand command = new OdbcCommand(queryString, connection);

            connection.Open();

            // Execute the DataReader and access the data.
            OdbcDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Debug.Log(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3]);
            }

            reader.Close();
            connection.Close();
        }
    }   
    public static void DeleteDataBase()
    {
        //string queryStringLogin = $"DELETE FROM user";
        string queryStringLogin = $"DELETE FROM records";

        using (OdbcConnection connection = new OdbcConnection(connectionstring))
        {
            OdbcCommand SQLcommand = new OdbcCommand(queryStringLogin, connection);

            connection.Open();

            OdbcDataReader reader = SQLcommand.ExecuteReader();

            reader.Close();
            connection.Close();
        }
    }
}
public struct RecordData
{
    public int RecordTime;
    public string RecordDate;
    public RecordData(int time, string date)
    {
        RecordTime = time;
        RecordDate = date;
    }
}
// SELECT idUser FROM user WHERE Login == "{Login}";
// Замнить логин пользователя, найти его user id и сделать запрос в records

//OdbcDataAdapter d = new OdbcDataAdapter("select * from user", con);
/*
private void FindLastUserId()
{
    using (OdbcConnection connection = new OdbcConnection(connectionstring))
    {
        string queryString = "SELECT * FROM user";
        OdbcCommand command = new OdbcCommand(queryString, connection);

        connection.Open();

        // Execute the DataReader and access the data.
        OdbcDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            user_id = (int)reader[0];
            //Debug.Log(" reader " + reader[0]);
        }
        user_id++;
        reader.Close();
        connection.Close();
    }
}
*/