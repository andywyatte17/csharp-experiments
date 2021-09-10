//
// bson-export.cpp
//

#include "bson-export.h"
#include "libbson-1.9.5/src/bson/bson.h"
#include <mutex>
#include <vector>
#include <algorithm>

std::mutex g_mutex;
std::vector<std::vector<char>> g_bson;

namespace
{
  BsonResult Store(const char* p, uint32_t size)
  {
    std::unique_lock<std::mutex> lock(g_mutex);
    std::vector<char> v(p, p + size);
    g_bson.push_back(std::move(v));
    return BsonResult{g_bson.back().data(), static_cast<uint32_t>(g_bson.back().size())};
  }
}

extern "C" BsonResult BsonExport(char *ptr, uint32_t len)
{
  (void)ptr;
  (void)len;

  bson_t parent;
  bson_t child;

  bson_init(&parent);
  bson_append_document_begin(&parent, "foo", 3, &child);
  bson_append_int32(&child, "baz", 3, 1);
  bson_append_document_end(&parent, &child);

  struct AtExit {
    ~AtExit() { bson_destroy(p); }
    bson_t* p;
  } atExit{&parent};

  const char *data = reinterpret_cast<const char *>(bson_get_data(&parent));
  auto size = static_cast<uint32_t>(parent.len);
  return Store(data, size);
}

extern "C" void BsonResultFree(char* ptr)
{
  std::unique_lock<std::mutex> lock(g_mutex);

  g_bson.erase(
    std::remove_if(
        g_bson.begin(), g_bson.end(),
        [&](std::vector<char>& v) { return v.data()==ptr; }
    ),
    g_bson.end()
  );
}
