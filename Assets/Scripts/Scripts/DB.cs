using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using System.Threading;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using UnityEditor.MemoryProfiler;
using UnityEditor.Search;

public class DB : MonoBehaviour
{
    private string dbName = "URI=file:" + Application.dataPath + "/scoreboard.s3db";

    // Start is called before the first frame update
    void Start()
    {
        CreateDb();
        CreatePlayerTable();
        //DisplayDB();
    }

    public void CreateDb(){
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS scoreboard (name VARCHAR(60), points SMALLINT)";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void CreatePlayerTable()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS player (id INTEGER PRIMARY KEY, name VARCHAR(60))";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void DisplayDB(TextMeshProUGUI textDisplayName, TextMeshProUGUI textDisplayPoints)
    {
        textDisplayName.text = "";
        textDisplayPoints.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM scoreboard ORDER by points DESC";   

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textDisplayName.text += reader["name"] + "\n";
                        textDisplayPoints.text += reader["points"] + "\n";
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    public List<string> GetPlayers()
    {
        var list = new List<string>();

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM player ORDER BY name";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Convert.ToString(reader["name"]));
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return list;
    }

    public List<Scoreboard> InsertDBAndSearch(string name, int points)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO scoreboard ('name', 'points') VALUES ('{name}', '{points}');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        var list = new List<Scoreboard>();

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM scoreboard ORDER BY points LIMIT 3";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Scoreboard(Convert.ToInt32(reader["points"]), Convert.ToString(reader["name"])));
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return list;
    }

    public void InsertPlayer(string name)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO player ('name') VALUES ('{name}');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public bool CheckIfPlayerExists(string name)
    {
        try
        {
            var validated = false;

            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT id, name FROM player WHERE name = '{name}';";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            validated = true;

                        reader.Close();
                    }
                }

                connection.Close();
            }

            return validated;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    //public List<Scoreboard> GetThreeBestScores()
    //{
    //    var list = new List<Scoreboard>();

    //    using (var connection = new SqliteConnection(dbName))
    //    {
    //        connection.Open();

    //        using (var command = connection.CreateCommand())
    //        {
    //            command.CommandText = "SELECT * FROM scoreboard ORDER BY points LIMIT 3";

    //            using (IDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    list.Add(new Scoreboard(Convert.ToInt32(reader["points"]), Convert.ToString(reader["name"])));
    //                }

    //                reader.Close();
    //            }
    //        }

    //        connection.Close();
    //    }

    //    return list;
    //}
}
