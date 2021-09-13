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
rm /result/libbson_exports.so || true

rm CMakeCache.txt || true
cmake -G"Unix Makefiles" .. -DCMAKE_BUILD_TYPE=Debug \
  -DCMAKE_ANDROID_STL_TYPE=c++_static
cmake --build .

cp libbson_exports.so /result/
nm -g /result/libbson_exports.so | grep " U "
nm -g /result/libbson_exports.so | grep " T "
readelf -h /result/libbson_exports.so | grep 'Class\|File\|Machine'
readelf --dynamic /result/libbson_exports.so
