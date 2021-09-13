#!/bin/sh

#mkdir /build
#cd /build
#tar -xzf /host/libbson-1.9.5.tar.gz
#cd libbson-1.9.5
#./configure --host=arm-linux-gnueabi \
#     CC=/android-ndk-r19c/toolchains/llvm/prebuilt/linux-x86_64/bin/armv7a-linux-androideabi16-clang \
#     --prefix=/usr --libdir=/usr/lib64
#make
#sudo make install
#cp /usr/lib64/cmake/libbson-1.0/* /host/

# git clone https://github.com/mongodb/mongo-c-driver/tree/master/src/libbson
cd ~
git clone https://github.com/mongodb/mongo-c-driver
cd ~/mongo-c-driver/
mkdir bin
cd bin

sed -i -e "s/libbson-1.0.so.0/libbson-1.0.so/g" ../bin/src/libbson/bson/bson-targets.cmake

rm CMakeCache.txt || true
cmake -G"Unix Makefiles" ~/mongo-c-driver -DCMAKE_BUILD_TYPE=Release
cmake --build . --target bson_shared

cp src/libbson/libbson-1.0.so.0 /result/

nm -g /result/libbson-1.0.so.0 | grep " U "
nm -g /result/libbson-1.0.so.0 | grep " T "
readelf -h /result/libbson-1.0.so.0 | grep 'Class\|File\|Machine'
readelf --dynamic /result/libbson-1.0.so.0
