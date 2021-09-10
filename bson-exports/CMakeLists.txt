
cmake_minimum_required (VERSION 3.1)

project(bson_exports)

set(SOURCES
  "bson-export.h"
  "bson-export.cpp"
  "bson-export-c-check.c"
)

add_library (bson_exports SHARED ${SOURCES} ${HEADERS})
