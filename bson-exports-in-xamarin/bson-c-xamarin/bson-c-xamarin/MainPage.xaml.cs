using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      var result = BsonExports.BsonExport(IntPtr.Zero, 0);
    }
  }
}
