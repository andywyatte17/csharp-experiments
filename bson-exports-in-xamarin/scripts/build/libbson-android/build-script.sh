#!/bin/sh

export MONGO_V=mongo-c-driver-1.19.0
mongo
cd ~
# git clone https://github.com/mongodb/mongo-c-driver
wget https://github.com/mongodb/mongo-c-driver/releases/download/1.19.0/mongo-c-driver-1.19.0.tar.gz

tar -xzf mongo-c-driver-1.19.0.tar.gz

cd ~/${MONGO_V}/ || exit 1
mkdir bin
cd bin

rm CMakeCache.txt || true
cmake -G"Unix Makefiles" ~/${MONGO_V} -DCMAKE_BUILD_TYPE=Release
cmake --build . --target bson_shared

cp src/libbson/libbson-1.0.so.0 /result/

nm -g /result/libbson-1.0.so.0 | grep " U "
nm -g /result/libbson-1.0.so.0 | grep " T "
readelf -h /result/libbson-1.0.so.0 | grep 'Class\|File\|Machine'
readelf --dynamic /result/libbson-1.0.so.0
