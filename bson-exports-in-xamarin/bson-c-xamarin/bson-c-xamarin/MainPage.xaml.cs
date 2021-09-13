using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

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
        byte[] data_o = null;
        {
          MemoryStream mso = new MemoryStream();
          using (BsonWriter writer = new BsonWriter(mso))
          {
            if (loop == 1 || loop == 2)
            {
              var v = new { data = new { size = 1024 * 512 * loop } };
              JsonSerializer serializer = new JsonSerializer();
              serializer.Serialize(writer, v);
            }
            else
            {
              var v = new { eh = 1 };
              JsonSerializer serializer = new JsonSerializer();
              serializer.Serialize(writer, v);
            }
          }
          data_o = mso.ToArray();
        }

        var watch = System.Diagnostics.Stopwatch.StartNew();
        var msoPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(data_o.Length);
        System.Runtime.InteropServices.Marshal.Copy(data_o, 0, msoPtr, data_o.Length);
        var result = BsonExports.BsonExport(msoPtr, (uint)data_o.Length);
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(msoPtr);
        watch.Stop();
        System.Diagnostics.Debug.WriteLine($"watch-1 = {watch.ElapsedMilliseconds}ms");

        watch.Reset();
        watch.Start();
        var data = new byte[result.len];
        System.Runtime.InteropServices.Marshal.Copy(result.ptr, data, 0, (int)result.len);
        BsonExports.BsonResultFree(result.ptr);
        watch.Stop();
        System.Diagnostics.Debug.WriteLine($"watch-2 = {watch.ElapsedMilliseconds}ms");

        watch.Reset();
        watch.Start();
        var ms = new MemoryStream(data);
        JObject o;
        using (BsonReader reader = new BsonReader(ms))
        {
          o = (JObject)JToken.ReadFrom(reader);
        }
        watch.Stop();
        System.Diagnostics.Debug.WriteLine($"watch-3 = {watch.ElapsedMilliseconds}ms");

        System.Diagnostics.Debug.WriteLine($"obj = {o}");
      }
    }
  }
}
