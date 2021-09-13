#!/usr/bin/env python3

from build_android_cmn import *
import os
from os.path import join

HERE = os.path.dirname(os.path.realpath(__file__))
os.chdir(HERE)

slash = join(".", ".").replace('.', '')

SO_DIR = join("..", "..", "..", "bson-c-xamarin", "bson-c-xamarin.Android", "libs", "x86_64")
SO_DIR = os.path.realpath(SO_DIR)
if not os.path.exists(SO_DIR):
  os.mkdir(SO_DIR)
  if not os.path.exists(SO_DIR):
    raise RuntimeError("Wrong path - " + SO_DIR)
SO_DIR += slash

Build(so_result_location = SO_DIR + slash,
  log_file_name="build-libbson-x86-64.txt",
  conan_docker_name="conanio/android-clang8")
