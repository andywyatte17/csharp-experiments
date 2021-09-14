using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
#if !IOS
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
    private static extern BsonResult BsonExportF(IntPtr ptr, UInt32 size);

    /// <summary>
    /// Call the BsonExport method with parameters specified in 'obj'.
    /// </summary>
    internal static JObject BsonExport(object obj)
    {
      // Write 'obj' to bson data

      MemoryStream mso = new MemoryStream();
      using (BsonWriter writer = new BsonWriter(mso))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(writer, obj);
      }

      // Call BsonExportF 'C' export

      IntPtr bsonBuf = IntPtr.Zero;
      BsonResult bsonRes;
      try
      {
        byte[] bson = mso.ToArray();
        bsonBuf = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(bson.Length);
        System.Runtime.InteropServices.Marshal.Copy(bson, 0, bsonBuf, bson.Length);
        bsonRes = BsonExportF(bsonBuf, (uint)bson.Length);
      }
      finally
      {
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(bsonBuf);
      }

      // Extract result as bytes

      var data = new byte[bsonRes.len];
      System.Runtime.InteropServices.Marshal.Copy(bsonRes.ptr, data, 0, (int)bsonRes.len);
      BsonResultFree(bsonRes.ptr);

      // Read result to JObject

      using (var ms = new MemoryStream(data))
      {
        using (BsonReader reader = new BsonReader(ms))
        {
          return (JObject)JToken.ReadFrom(reader);
        }
      }
    }

    [DllImport(DllName, EntryPoint = "BsonResultFree")]
    private static extern void BsonResultFree(IntPtr ptr);
  }
}
