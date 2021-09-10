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

git clone https://github.com/mongodb/libbson.git
cd libbson
mkdir bin || true
cd bin
cmake -G"Unix Makefiles" .. \
  -DCMAKE_BUILD_TYPE=Release
  #-DCMAKE_TOOLCHAIN_FILE:STRING=/opt/android-ndk-r13b/build/cmake/android.toolchain.cmake
make
cp *.so /result/