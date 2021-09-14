//
// bson-export.cpp
//

#include "bson-export.h"
#include "mongo-c-driver/src/libbson/src/bson/bson.h"
#include <algorithm>
#include <functional>
#include <mutex>
#include <sstream>
#include <vector>


std::mutex g_mutex;
std::vector<std::vector<char>> g_bson;

namespace {
std::shared_ptr<void> MakeScopeGuard(std::function<void(void)> f) {
  static int dummy;
  auto res =
      std::shared_ptr<void>{static_cast<void *>(&dummy), [f](void *) { f(); }};
  return res;
}

BsonResult Store(const char *p, uint32_t size) {
  std::unique_lock<std::mutex> lock(g_mutex);
  std::vector<char> v(p, p + size);
  g_bson.push_back(std::move(v));
  return BsonResult{g_bson.back().data(),
                    static_cast<uint32_t>(g_bson.back().size())};
}

int ParseSize(const char *ptr, unsigned len) {
  bson_t *b = nullptr;
  b = bson_new_from_data(reinterpret_cast<const uint8_t *>(ptr), len);
  if (!b) {
    fprintf(stderr, "The specified length embedded in <my_data> did not match "
                    "<my_data_len>\n");
    return -1;
  }

  auto sg = MakeScopeGuard([=]() { bson_destroy(b); });

  // Use dot-notation to extract

  bson_iter_t iter;
  bson_iter_t size;

  if (bson_iter_init(&iter, b) &&
      bson_iter_find_descendant(&iter, "data.size", &size) &&
      BSON_ITER_HOLDS_INT32(&size)) {
    return bson_iter_int32(&size);
  }

  return -1;
}
} // namespace

// ...

extern "C" BsonResult BsonExport(const char *ptr, uint32_t len) {
  auto sizeBuf = ParseSize(ptr, len);
  if (sizeBuf < 0) {
    bson_t parent;
    bson_init(&parent);
    auto sg = MakeScopeGuard([&]() { bson_destroy(&parent); });
    bson_append_int32(&parent, "error", 5, 1);
    const char *data = reinterpret_cast<const char *>(bson_get_data(&parent));
    auto size = static_cast<uint32_t>(parent.len);
    return Store(data, size);
  }

  bson_t parent;
  bson_t child;

  std::vector<uint8_t> mega;
  mega.resize(sizeBuf);

  bson_init(&parent);

  auto ObjectScope = [](bson_t *parent, std::string key,
                        std::function<void(bson_t * /* child */)> f) {
    bson_t child;
    bson_append_document_begin(parent, key.c_str(), key.size(), &child);
    f(&child);
    bson_append_document_end(parent, &child);
  };

  auto ArrayScope = [](bson_t *parent, std::string key,
                       std::function<void(bson_t * /* child */)> f) {
    bson_t child;
    bson_append_array_begin(parent, key.c_str(), key.size(), &child);
    f(&child);
    bson_append_array_end(parent, &child);
  };

  // Add elements to an array with key 'some_array'

  ObjectScope(&parent, "result", [&ArrayScope, &ObjectScope, &mega](bson_t *child) {

    // writing result.loads-a-bytes binary

    bson_append_binary(child,              // ...
                       "loads-a-bytes", 13, // ...
                       BSON_SUBTYPE_BINARY, mega.data(),
                       static_cast<unsigned>(mega.size()));

    // writing result.some_array array-of-ints

    ArrayScope(child, "some_array", [](bson_t *array) {
      for (int16_t i = 1, m = 1; i <= 4; i++, m *= 2) {
        bson_append_int32(array, "", 0, m);
      }
    });
  });

  auto sg = MakeScopeGuard([&]() { bson_destroy(&parent); });

  const char *data = reinterpret_cast<const char *>(bson_get_data(&parent));
  auto size = static_cast<uint32_t>(parent.len);
  return Store(data, size);
}

extern "C" void BsonResultFree(const char *ptr) {
  std::unique_lock<std::mutex> lock(g_mutex);

  g_bson.erase(
      std::remove_if(g_bson.begin(), g_bson.end(),
                     [&](std::vector<char> &v) { return v.data() == ptr; }),
      g_bson.end());
}
