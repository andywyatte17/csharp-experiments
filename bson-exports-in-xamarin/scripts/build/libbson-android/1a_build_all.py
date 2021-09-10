#!/usr/bin/env python3

import subprocess
import sys

for script in \
  ("1a_build_android_lib_armv7.py",
   "1a_build_android_lib_armv8.py",
   "1a_build_android_lib_x86.py",
   "1a_build_android_lib_x86_64.py"):
    # ...
    subprocess.check_call([sys.executable, script])
