#!/usr/bin/env python3

import sys, os
from os.path import join
import subprocess
from datetime import datetime
import glob
from pprint import pprint
#try: import wget
#except:
#  print("Needs py -m pip install --user wget or equivalent.", file=sys.stderr)
#  sys.exit(1)

HERE = os.path.dirname(os.path.realpath(__file__))
os.chdir(HERE)

# wget
#LIBBSON_AND_VERSION = "libbson-1.9.5"
#url = "https://github.com/mongodb/libbson/releases/download/1.9.5/{}.tar.gz".format(LIBBSON_AND_VERSION)
#if os.path.exists(LIBBSON_AND_VERSION + ".tar.gz"): os.unlink(LIBBSON_AND_VERSION + ".tar.gz")
#wget.download(url)

if not os.path.exists(join(HERE, "libbson-armv7")):
  os.mkdir(join(HERE, "libbson-armv7"))

f = open("build-log-armv7.txt", "w")
f.write( datetime.now().strftime("%d/%m/%Y %H:%M:%S") )

slash = join(".", ".").replace('.', '')

docker_script = \
  ["docker", "run",
    "-v", os.getcwd() + slash + ":/host/",
    "--rm", "conanio/android-clang8-armv7",
    "/bin/sh", "-c", "sh /host/build-script.sh"
  ]
pprint(docker_script)
docker_run = subprocess.run(docker_script)

for stream in (docker_run.stdout, docker_run.stderr):
  if stream==None: continue
  s = stream.decode('utf-8')
  print(s)
  f.write(s)

f.write( datetime.now().strftime("%d/%m/%Y %H:%M:%S") )
