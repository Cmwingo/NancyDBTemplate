using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Template
{
  public class Object2Test : IDisposable
  {
    public Object2Test()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=template_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_EmptyAtFirst()
    {
      //Arrange, Act
      int result = Object2.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameName()
    {
      //Arrange, Act
      Object2 firstObject2 = new Object2("Name");
      Object2 secondObject2 = new Object2("Name");

      //Assert
      Assert.Equal(firstObject2, secondObject2);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();

      //Act
      List<Object2> result = Object2.GetAll();
      List<Object2> testList = new List<Object2>{testObject2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();

      //Act
      Object2 savedObject2 = Object2.GetAll()[0];

      Console.WriteLine(testObject2.GetName());
      int result = savedObject2.GetId();
      int testId = testObject2.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsObject2InDatabase()
    {
      //Arrange
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();

      //Act
      Object2 result = Object2.Find(testObject2.GetId());

      //Assert
      Assert.Equal(testObject2, result);
    }
    [Fact]
    public void Test_AddObject1_AddsObject1ToObject2()
    {
      //Arrange
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();

      Object1 testObject1 = new Object1("Other object name");
      testObject1.Save();

      //Act
      testObject2.AddObject1(testObject1);

      List<Object1> result = testObject2.GetObject1s();
      List<Object1> testList = new List<Object1>{testObject1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetObject1s_ReturnsAllObject2Object1s()
    {
      //Arrange
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();

      Object1 testObject11 = new Object1("Other object name");
      testObject11.Save();

      Object1 testObject12 = new Object1("Another object name");
      testObject12.Save();

      //Act
      testObject2.AddObject1(testObject11);
      List<Object1> result = testObject2.GetObject1s();
      List<Object1> testList = new List<Object1> {testObject11};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesObject2AssociationsFromDatabase()
    {
      //Arrange
      Object1 testObject1 = new Object1("Other object name");
      testObject1.Save();

      string testName = "Name";
      Object2 testObject2 = new Object2(testName);
      testObject2.Save();

      //Act
      testObject2.AddObject1(testObject1);
      testObject2.Delete();

      List<Object2> resultObject1Object2s = testObject1.GetObject2s();
      List<Object2> testObject1Object2s = new List<Object2> {};

      //Assert
      Assert.Equal(testObject1Object2s, resultObject1Object2s);
    }


    [Fact]
    public void Test_Update_UpdatesInDb()
    {
      Object2 testObject2 = new Object2("Name");
      testObject2.Save();
      testObject2.Update("Other name");

      Object2 newObject2 = new Object2("Other name", testObject2.GetId());

      Assert.Equal(testObject2, newObject2);
    }

    public void Dispose()
    {
      Object2.DeleteAll();
      Object1.DeleteAll();
    }
  }
}
