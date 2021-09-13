
cmake_minimum_required (VERSION 3.1)

project(bson_exports)

set(SOURCES
  "bson-export.h"
  "bson-export.cpp"
  "bson-export-c-check.c"
)

add_compile_options("-pthread")
add_link_options("-static-libstdc++")
add_link_options("/libs/libbson-1.0.so.0")

add_library (bson_exports SHARED ${SOURCES} ${HEADERS})
