//
// bson-export.h
//

#pragma once

#include <stdint.h>

struct BsonResult
{
  char* ptr;
  uint32_t size;
};

#ifdef __cplusplus

extern "C" BsonResult BsonExport(char* ptr, uint32_t size);
extern "C" void BsonResultFree(char* ptr);

#else

struct BsonResult BsonExport(char* ptr, uint32_t size);
void BsonResultFree(char* ptr);

#endif
