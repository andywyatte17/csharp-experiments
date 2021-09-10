#!/bin/sh

mkdir -p ~/bson
cd ~/bson
tar -xzf /host/libbson-1.9.5.tar.gz
cd ~/bson/libbson-1.9.5
cmake -G"Unix Makefiles" .

cd /bson-exports
ln -s ~/bson/libbson-1.9.5 libbson-1.9.5

mkdir -p bin
cd bin
rm libbson_exports.so || true
cmake -G"Unix Makefiles" .. -DCMAKE_BUILD_TYPE=Release \
  -DCMAKE_ANDROID_STL_TYPE=c++_static \
  -DCMAKE_TOOLCHAIN_FILE=/android-ndk-r19c/build/cmake/android.toolchain.cmake
cmake --build . --

ls -l libbson_exports.so
cp libbson_exports.so /result/
nm -g /result/libbson_exports.so
readelf --dynamic /result/libbson_exports.so