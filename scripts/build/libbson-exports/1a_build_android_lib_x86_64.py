#!/usr/bin/env python3

from build_android_cmn import *
import os
from os.path import join

slash = join(".", ".").replace('.', '')

GetLibBson()

Build(so_result_location=join(os.getcwd(), "libbson-x86-64") + slash,
  log_file_name="build-libbson-x86-64.txt",
  conan_docker_name="conanio/android-clang8")