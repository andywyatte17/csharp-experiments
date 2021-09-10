#!/usr/bin/env python3

from build_android_cmn import *
import os
from os.path import join

slash = join(".", ".").replace('.', '')

if not os.path.exists("libbson-armv8"):
  os.mkdir("libbson-armv8")

Build(so_result_location=join(os.getcwd(), "libbson-armv8") + slash,
  log_file_name="build-libbson-armv8.txt",
  conan_docker_name="conanio/android-clang8-armv8")
