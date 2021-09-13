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
      var watch = System.Diagnostics.Stopwatch.StartNew();
      var result = BsonExports.BsonExport(IntPtr.Zero, 0);
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
