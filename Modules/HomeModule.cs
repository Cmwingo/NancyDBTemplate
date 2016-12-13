using Nancy;
using System.Collections.Generic;

namespace Template
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
      return View["index.cshtml"];
      };
      Get["/object2s"] = _ => {
        List<Object2> AllObject2s = Object2.GetAll();
        return View["object2s.cshtml", AllObject2s];
      };
      Get["/object1s"] = _ => {
        List<Object1> AllObject1s = Object1.GetAll();
        return View["object1s.cshtml", AllObject1s];
      };
      Get["/object2s/new"] = _ => {
      return View["object2s_form.cshtml"];
      };
      Post["/object2s/new"] = _ => {
        Object2 newObject2 = new Object2(Request.Form["object2-description"]);
        newObject2.Save();
        return View["success.cshtml"];
      };
      Get["/object1s/new"] = _ => {
        return View["object1s_form.cshtml"];
      };
      Post["/object1s/new"] = _ => {
        Object1 newObject1 = new Object1(Request.Form["object1-name"]);
        newObject1.Save();
        return View["success.cshtml"];
      };
      Get["object2s/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Object2 SelectedObject2 = Object2.Find(parameters.id);
        List<Object1> Object2Object1s = SelectedObject2.GetObject1s();
        List<Object1> AllObject1s = Object1.GetAll();
        model.Add("object2", SelectedObject2);
        model.Add("object2Object1s", Object2Object1s);
        model.Add("allObject1s", AllObject1s);
        return View["object2.cshtml", model];
      };

      Get["object1s/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Object1 SelectedObject1 = Object1.Find(parameters.id);
        List<Object2> Object1Object2s = SelectedObject1.GetObject2s();
        List<Object2> AllObject2s = Object2.GetAll();
        model.Add("object1", SelectedObject1);
        model.Add("object1Object2s", Object1Object2s);
        model.Add("allObject2s", AllObject2s);
        return View["object1.cshtml", model];
      };
      Post["object2/add_object1"] = _ => {
        Object1 object1 = Object1.Find(Request.Form["object1-id"]);
        Object2 object2 = Object2.Find(Request.Form["object2-id"]);
        object2.AddObject1(object1);
        return View["success.cshtml"];
      };
      Post["object1/add_object2"] = _ => {
        Object1 object1 = Object1.Find(Request.Form["object1-id"]);
        Object2 object2 = Object2.Find(Request.Form["object2-id"]);
        object1.AddObject2(object2);
        return View["success.cshtml"];
      };
      Get["object2s/update/{id}"] = parameters =>
      {
        Object2 foundObject2 = Object2.Find(parameters.id);
        return View["object2_update.cshtml", foundObject2];
      };
      Patch["object2s/update/{id}"] = parameters =>
      {
        Object2 foundObject2 = Object2.Find(parameters.id);
        foundObject2.Update(Request.Form["new-description"]);
        return View["success.cshtml"];
      };
      Get["object1s/update/{id}"] = parameters =>
      {
        Object1 foundObject1 = Object1.Find(parameters.id);
        return View["object1_update.cshtml", foundObject1];
      };
      Patch["object1s/update/{id}"] = parameters =>
      {
        Object1 foundObject1 = Object1.Find(parameters.id);
        foundObject1.Update(Request.Form["new-description"]);
        return View["success.cshtml"];
      };
      Delete["object1/delete/{id}"] = parameters =>
      {
        Object1 foundObject1 = Object1.Find(parameters.id);
        foundObject1.Delete();
        return View["success.cshtml"];
      };
      Delete["object2/delete/{id}"] = parameters =>
      {
        Object2 foundObject2 = Object2.Find(parameters.id);
        foundObject2.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
