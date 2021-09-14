using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using Xamarin.Forms;

#if Android
using bson_c_xamarin.Droid;
#endif

namespace bson_c_xamarin
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
      foreach (var loop in new[] { 1, 2, 3 })
      {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        Debug.WriteLine("\n" + $"--- {loop} ---" + "\n");

        JObject bsonRes;
        if (loop == 1 || loop == 2)
        {
          var v = new { data = new { size = 1024 * 512 * loop } };
          Debug.WriteLine($"Input to BsonExport is {v}");
          bsonRes = BsonExports.BsonExport(v);
        }
        else
        {
          var v = new { eh = 1 };
          bsonRes = BsonExports.BsonExport(v);
        }

        watch.Stop();
        Debug.WriteLine($"watch-1 = {watch.ElapsedMilliseconds}ms");

        try
        {
          var j1 = (JObject)bsonRes["foo"];
          var s = j1["loads-a-bytes"].ToObject<string>();
          j1["loads-a-bytes"] = $"string of length {s.Length}";
        }
        catch (Exception)
        {
        }

        Debug.WriteLine($"bsonRes = {bsonRes}");

        // ...
      }
    }
  }
}
