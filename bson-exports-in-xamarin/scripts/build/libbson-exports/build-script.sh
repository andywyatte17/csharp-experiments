#!/bin/sh

mkdir -p ~/bson
cd ~
wget https://github.com/mongodb/mongo-c-driver/releases/download/1.19.0/mongo-c-driver-1.19.0.tar.gz
tar -xzf mongo-c-driver-1.19.0.tar.gz
cd ~/mongo-c-driver-1.19.0
cmake -G"Unix Makefiles" .

cd /bson-exports
ln -s ~/mongo-c-driver-1.19.0 mongo-c-driver

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
