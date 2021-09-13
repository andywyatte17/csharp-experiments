#!/usr/bin/env python3

import sys, os
from os.path import join
import subprocess
from datetime import datetime
import glob
from pprint import pprint
import shutil

HERE = os.path.dirname(os.path.realpath(__file__))

def GetLibBson():
  if not os.path.exists("libbson-1.9.5.tar.gz"):
    import wget
    wget.download(url = \
      "https://github.com/mongodb/libbson/releases/download/1.9.5/libbson-1.9.5.tar.gz"
    )

def Build(so_result_location, log_file_name, conan_docker_name):
  os.chdir(HERE)
  slash = join(".", ".").replace('.', '')

  with open(log_file_name, "w") as f:
    f.write( datetime.now().strftime("%d/%m/%Y %H:%M:%S") )

    bson_exports = join(os.getcwd(), "..", "..", "..", "bson-exports")
    docker_script = \
        ["docker", "run",
            "-v", bson_exports + slash + ":/bson-exports/",
            "-v", os.getcwd() + slash + ":/host/",
            "-v", so_result_location + ":/result/",
            "--rm", conan_docker_name,
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
