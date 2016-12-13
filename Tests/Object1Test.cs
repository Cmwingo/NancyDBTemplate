using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Template
{
  public class Object1Test : IDisposable
  {
    public Object1Test()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=template_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_Object1sEmptyAtFirst()
    {
      //Arrange, Act
      int result = Object1.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Object1 firstObject1 = new Object1("Name");
      Object1 secondObject1 = new Object1("Name");

      //Assert
      Assert.Equal(firstObject1, secondObject1);
    }

    [Fact]
    public void Test_Save_SavesObject1ToDatabase()
    {
      //Arrange
      Object1 testObject1 = new Object1("Name");
      testObject1.Save();

      //Act
      List<Object1> result = Object1.GetAll();
      List<Object1> testList = new List<Object1>{testObject1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject1Object()
    {
      //Arrange
      Object1 testObject1 = new Object1("Name");
      testObject1.Save();

      //Act
      Object1 savedObject1 = Object1.GetAll()[0];

      int result = savedObject1.GetId();
      int testId = testObject1.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsObject1InDatabase()
    {
      //Arrange
      Object1 testObject1 = new Object1("Name");
      testObject1.Save();

      //Act
      Object1 foundObject1 = Object1.Find(testObject1.GetId());

      //Assert
      Assert.Equal(testObject1, foundObject1);
    }

    public void Dispose()
    {
      Object2.DeleteAll();
      Object1.DeleteAll();
    }
    [Fact]
    public void Test_Delete_DeletesObject1FromDatabase()
    {
      //Arrange
      string name1 = "Name";
      Object1 testObject11 = new Object1(name1);
      testObject11.Save();

      string name2 = "Other name";
      Object1 testObject12 = new Object1(name2);
      testObject12.Save();

      //Act
      testObject11.Delete();
      List<Object1> resultObject1s = Object1.GetAll();
      List<Object1> testObject1List = new List<Object1> {testObject12};

      //Assert
      Assert.Equal(testObject1List, resultObject1s);
    }
    [Fact]
    public void Test_AddObject2_AddsObject2ToObject1()
    {
     //Arrange
      Object1 testObject1 = new Object1("Name");
      testObject1.Save();

      Object2 testObject2 = new Object2("Object name");
      testObject2.Save();

      Object2 testObject22 = new Object2("Other object name");
      testObject22.Save();

     //Act
      testObject1.AddObject2(testObject2);
      testObject1.AddObject2(testObject22);

      List<Object2> result = testObject1.GetObject2s();
      List<Object2> testList = new List<Object2>{testObject2, testObject22};

     //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_GetObject2s_ReturnsAllObject1Object2s()
    {
      //Arrange
      Object1 testObject1 = new Object1("Name");
      testObject1.Save();

      Object2 testObject21 = new Object2("Object name");
      testObject21.Save();

      Object2 testObject22 = new Object2("Other object name");
      testObject22.Save();

      //Act
      testObject1.AddObject2(testObject21);
      List<Object2> savedObject2s = testObject1.GetObject2s();
      List<Object2> testList = new List<Object2> {testObject21};

      //Assert
      Assert.Equal(testList, savedObject2s);
    }
    [Fact]
    public void Test_Delete_DeletesObject1AssociationsFromDatabase()
    {
      //Arrange
      Object2 testObject2 = new Object2("Object name");
      testObject2.Save();

      string testName = "Name";
      Object1 testObject1 = new Object1(testName);
      testObject1.Save();

      //Act
      testObject1.AddObject2(testObject2);
      testObject1.Delete();

      List<Object1> resultObject2Object1s = testObject2.GetObject1s();
      List<Object1> testObject2Object1s = new List<Object1> {};

      //Assert
      Assert.Equal(testObject2Object1s, resultObject2Object1s);
    }
  }
}
