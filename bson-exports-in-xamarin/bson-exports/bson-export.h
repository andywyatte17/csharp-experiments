//
// bson-export.h
//

#pragma once

#include <stdint.h>

struct BsonResult
{
  const char* ptr;
  uint32_t size;
};

#ifdef __cplusplus

extern "C" BsonResult BsonExport(const char* ptr, uint32_t size);
extern "C" void BsonResultFree(const char* ptr);

#else

struct BsonResult BsonExport(const char* ptr, uint32_t size);
void BsonResultFree(const char* ptr);

#endif
