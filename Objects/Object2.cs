using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Template
{
  public class Object2
  {
    private int _id;
    private string _name;

    public Object2(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherObject2)
    {
        if (!(otherObject2 is Object2))
        {
          return false;
        }
        else {
          Object2 newObject2 = (Object2) otherObject2;
          bool idEquality = this.GetId() == newObject2.GetId();
          bool nameEquality = this.GetName() == newObject2.GetName();
          return (idEquality && nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public static List<Object2> GetAll()
    {
      List<Object2> AllObject2s = new List<Object2>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM object2s;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int object2Id = rdr.GetInt32(0);
        string object2Name = rdr.GetString(1);
        Object2 newObject2 = new Object2(object2Name, object2Id);
        AllObject2s.Add(newObject2);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllObject2s;
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE object2s SET name = @NewName OUTPUT INSERTED.name where id = @Object2Id;", conn);

      SqlParameter descParam = new SqlParameter();
      descParam.ParameterName = "@NewName";
      descParam.Value = newName;


      SqlParameter idParam = new SqlParameter();
      idParam.ParameterName = "@Object2Id";
      idParam.Value = this._id;

      cmd.Parameters.Add(descParam);
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._name = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO object2s (name) OUTPUT INSERTED.id VALUES (@Object2Name);", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@Object2Name";
      nameParam.Value = this.GetName();


      cmd.Parameters.Add(nameParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM object2s;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Object2 Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM object2s WHERE id = @Object2Id", conn);
      SqlParameter object2IdParameter = new SqlParameter();
      object2IdParameter.ParameterName = "@Object2Id";
      object2IdParameter.Value = id.ToString();
      cmd.Parameters.Add(object2IdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundObject2Id = 0;
      string foundObject2Name = null;

      while(rdr.Read())
      {
        foundObject2Id = rdr.GetInt32(0);
        foundObject2Name = rdr.GetString(1);
      }
      Object2 foundObject2 = new Object2(foundObject2Name, foundObject2Id);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundObject2;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM object2s WHERE id = @Object2Id;", conn);

      SqlParameter object2IdParameter = new SqlParameter();
      object2IdParameter.ParameterName = "@Object2Id";
      object2IdParameter.Value = this.GetId();

      cmd.Parameters.Add(object2IdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddObject1(Object1 newObject1)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO object1s_object2s (object1_id, object2_id) VALUES (@Object1Id, @Object2Id);", conn);

      SqlParameter object1IdParameter = new SqlParameter();
      object1IdParameter.ParameterName = "@Object1Id";
      object1IdParameter.Value = newObject1.GetId();
      cmd.Parameters.Add(object1IdParameter);

      SqlParameter object2IdParameter = new SqlParameter();
      object2IdParameter.ParameterName = "@Object2Id";
      object2IdParameter.Value = this.GetId();
      cmd.Parameters.Add(object2IdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Object1> GetObject1s()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT object1_id FROM object1s_object2s WHERE object2_id = @Object2Id;", conn);

      SqlParameter object2IdParameter = new SqlParameter();
      object2IdParameter.ParameterName = "@Object2Id";
      object2IdParameter.Value = this.GetId();
      cmd.Parameters.Add(object2IdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> object1Ids = new List<int> {};

      while (rdr.Read())
      {
        int object1Id = rdr.GetInt32(0);
        object1Ids.Add(object1Id);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Object1> object1s = new List<Object1> {};

      foreach (int object1Id in object1Ids)
      {
        SqlCommand object1Query = new SqlCommand("SELECT * FROM object1s WHERE id = @Object1Id;", conn);

        SqlParameter object1IdParameter = new SqlParameter();
        object1IdParameter.ParameterName = "@Object1Id";
        object1IdParameter.Value = object1Id;
        object1Query.Parameters.Add(object1IdParameter);

        SqlDataReader queryReader = object1Query.ExecuteReader();
        while (queryReader.Read())
        {
          int thisObject1Id = queryReader.GetInt32(0);
          string object1Name = queryReader.GetString(1);
          Object1 foundObject1 = new Object1(object1Name, thisObject1Id);
          object1s.Add(foundObject1);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      if (conn != null)
      {
        conn.Close();
      }
      return object1s;
    }
  }
}
