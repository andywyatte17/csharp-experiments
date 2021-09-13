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
      var result = BsonExports.BsonExport(IntPtr.Zero, 0);

      var data = new byte[result.len];
      System.Runtime.InteropServices.Marshal.Copy(result.ptr, data, 0, (int)result.len);

      BsonExports.BsonResultFree(result.ptr);

      var ms = new MemoryStream(data);
      JObject o;
      using (BsonReader reader = new BsonReader(ms))
      {
        o = (JObject)JToken.ReadFrom(reader);
        var foo = (JObject)o["foo"];
        var baz = foo["baz"];
        System.Diagnostics.Debug.WriteLine($"obj = {o}");
      }

    }
  }
}
