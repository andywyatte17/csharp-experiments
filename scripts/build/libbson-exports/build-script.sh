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
cmake -G"Unix Makefiles" .. -DCMAKE_BUILD_TYPE=Release
cmake --build .
# make
cp *.so /result/