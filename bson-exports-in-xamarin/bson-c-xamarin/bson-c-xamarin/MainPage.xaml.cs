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
      string text = "";

      foreach (var loop in new[] { 1, 2, 3 })
      {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        text += "\n" + $"--- {loop} ---" + "\n";

        JObject bsonRes;
        if (loop == 1 || loop == 2)
        {
          var v = new { data = new { size = 1024 * 512 * loop } };
          text += $"Input to BsonExport is {v}\n";
          bsonRes = BsonExports.BsonExport(v);
        }
        else
        {
          var v = new { eh = 1 };
          bsonRes = BsonExports.BsonExport(v);
        }

        watch.Stop();
        text += $"watch-1 = {watch.ElapsedMilliseconds}ms\n";

        try
        {
          var j1 = (JObject)bsonRes["result"];
          var s = j1["loads-a-bytes"].ToObject<string>();
          j1["loads-a-bytes"] = $"string of length {s.Length}";
        }
        catch (Exception)
        {
        }

        text += $"bsonRes = {bsonRes}\n";
      }

      // ...

      editor.Text = text;
    }
  }
}
