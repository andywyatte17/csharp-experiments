#!/usr/bin/env python3

from build_android_cmn import *
import os
from os.path import join

slash = join(".", ".").replace('.', '')

if not os.path.exists("libbson-armv7"):
  os.mkdir("libbson-armv7")

Build(so_result_location=join(os.getcwd(), "libbson-x86") + slash,
  log_file_name="build-libbson-x86.txt",
  conan_docker_name="conanio/android-clang8-x86")
