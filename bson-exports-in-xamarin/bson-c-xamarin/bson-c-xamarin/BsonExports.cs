using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace bson_c_xamarin
{
  /*
  extern "C" BsonResult BsonExport(char* ptr, uint32_t size);
  extern "C" void BsonResultFree(char* ptr);
  */
  internal static class BsonExports
  {
// #if Android
#if ! IOS
    const string DllName = "libbson_exports.so";
    const string BsonDllName = "libbson-1.0.so";
#else
    const string DllName = "__Internal";
    const string BsonDllName = "__Internal";
#endif

    public struct BsonResult
    {
      public IntPtr ptr;
      public UInt32 len;
    }

    [DllImport(DllName, EntryPoint = "BsonExport")]
    internal static extern BsonResult BsonExport(IntPtr ptr, UInt32 size);

    [DllImport(DllName, EntryPoint = "BsonResultFree")]
    internal static extern void BsonResultFree(IntPtr ptr);

    [DllImport(BsonDllName, EntryPoint = "bson_new")]
    internal static extern IntPtr bson_new();    
  }
}
