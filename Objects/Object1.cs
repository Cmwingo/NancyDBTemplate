using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Template
{
  public class Object1
  {
    private int _id;
    private string _name;

    public Object1(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherObject1)
    {
        if (!(otherObject1 is Object1))
        {
          return false;
        }
        else
        {
          Object1 newObject1 = (Object1) otherObject1;
          bool idEquality = this.GetId() == newObject1.GetId();
          bool nameEquality = this.GetName() == newObject1.GetName();
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
    public static List<Object1> GetAll()
    {
      List<Object1> allObject1s = new List<Object1>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM object1s;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int object1Id = rdr.GetInt32(0);
        string object1Name = rdr.GetString(1);
        Object1 newObject1 = new Object1(object1Name, object1Id);
        allObject1s.Add(newObject1);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allObject1s;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO object1s (name) OUTPUT INSERTED.id VALUES (@Object1Name);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Object1Name";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM object1s;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM object1s WHERE id = @Object1Id; DELETE FROM object1s_object2s WHERE object1_id = @Object1Id;", conn);

      SqlParameter catId = new SqlParameter();
      catId.ParameterName = "@Object1Id";
      catId.Value = this.GetId();

      cmd.Parameters.Add(catId);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Object1 Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM object1s WHERE id = @Object1Id;", conn);
      SqlParameter object1IdParameter = new SqlParameter();
      object1IdParameter.ParameterName = "@Object1Id";
      object1IdParameter.Value = id.ToString();
      cmd.Parameters.Add(object1IdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundObject1Id = 0;
      string foundObject1Description = null;

      while(rdr.Read())
      {
        foundObject1Id = rdr.GetInt32(0);
        foundObject1Description = rdr.GetString(1);
      }
      Object1 foundObject1 = new Object1(foundObject1Description, foundObject1Id);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundObject1;
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE object1s SET name = @NewName OUTPUT INSERTED.name where id = @Object1Id;", conn);

      SqlParameter descParam = new SqlParameter();
      descParam.ParameterName = "@NewName";
      descParam.Value = newName;


      SqlParameter idParam = new SqlParameter();
      idParam.ParameterName = "@Object1Id";
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

    public void AddObject2(Object2 newObject2)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO object1s_object2s (object1_id, object2_id) VALUES (@Object1Id, @Object2Id);", conn);

      SqlParameter object1IdParameter = new SqlParameter();
      object1IdParameter.ParameterName = "@Object1Id";
      object1IdParameter.Value = this.GetId();
      cmd.Parameters.Add(object1IdParameter);

      SqlParameter object2IdParameter = new SqlParameter();
      object2IdParameter.ParameterName = "@Object2Id";
      object2IdParameter.Value = newObject2.GetId();
      cmd.Parameters.Add(object2IdParameter);

      cmd.ExecuteNonQuery();

      if(conn!=null)
      {
        conn.Close();
      }
    }

    public List<Object2> GetObject2s()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT object2s.* FROM object1s JOIN object1s_object2s ON (object1s.id = object1s_object2s.object1_id) JOIN object2s ON (object1s_object2s.object2_id = object2s.id) WHERE object1s.id = @Object1Id;", conn);

      SqlParameter object1IdParameter = new SqlParameter();
      object1IdParameter.ParameterName = "@Object1Id";
      object1IdParameter.Value = this.GetId();
      cmd.Parameters.Add(object1IdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> object2Ids = new List<int> {};
      while(rdr.Read())
      {
        int object2Id = rdr.GetInt32(0);
        object2Ids.Add(object2Id);
      }
      if (rdr!=null)
      {
        rdr.Close();
      }

      List<Object2> object2s = new List<Object2> {};
      foreach (int object2Id in object2Ids)
      {
        SqlCommand object2Query = new SqlCommand("SELECT * FROM object2s WHERE id = @Object2Id;", conn);

        SqlParameter object2IdParameter = new SqlParameter();
        object2IdParameter.ParameterName = "@Object2Id";
        object2IdParameter.Value = object2Id;
        object2Query.Parameters.Add(object2IdParameter);

        SqlDataReader queryReader = object2Query.ExecuteReader();
        while(queryReader.Read())
        {
          int thisObject2Id = queryReader.GetInt32(0);
          string object2Description = queryReader.GetString(1);
          Object2 foundObject2 = new Object2(object2Description, thisObject2Id);
          object2s.Add(foundObject2);
        }
        if(queryReader!=null)
        {
          queryReader.Close();
        }
      }
      if(conn!=null)
      {
        conn.Close();
      }
      return object2s;
    }
  }
}
