#!/usr/bin/env bash

# Description: This script is for the purpose of CREATING A NEW virtualbox virtual machine
# from the command line. The primary functionality of this script is to be used for connection with the
# Open Virtual Desktop project.

# Usage: The follwing are required: inputname, isolocation, ostype

# Author: Andrew Cowden
# Contact: am.cowden.97@siu.edu

# Link External Function File NOTE: This will need to be changed if the script moves
. /home/amcowden97/Workspace/scripts/open_virtual_desktop_bash_functions.sh

# Initiate Proper Error Termination and Debugging Options
set -o errexit
set -o nounset
set -o pipefail

# Instantiate Facilitating Variables
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
__file="${__dir}/$(basename "${BASH_SOURCE[0]}")"
__base="$(basename ${__file} .sh)"
__remoteport=3390

# Instantiate Custom Variables
inputname=""
isolocation=""
ostype=""
hdsize=10240 # Default HD is is 10GB in MB
ramsize=4096 # Default RAM is 4GB in MB

# Check Empty Argument Case
if [ $# -eq 0 ]; then
    create_virtual_box_usage
    exit 1
fi

# Argument Parsing
while [[ $# -gt 0 ]]; do
    opt="$1"
    case "$opt" in
	-h|--help)
	    create_virtual_box_help
	    exit 0
	    ;;
	-d|--debug)
	    start_debugging
	    ;;
	-i|--inputname)
	    shift
	    inputname="$1"
	    ;;
	--isolocation)
            shift
            isolocation="$1"
            ;;
	--ostype)
	    shift
	    ostype="$1"
	    ;;
	--hd)
	    shift
	    hdsize="$1"
	    ;;
	--ram)
	    shift
	    ramsize="$1"
	    ;;
	-i=*|--inputname=*)
	    inputname="${opt#*=}"
	    ;;
	--isolocation=*)
	    isolocation="${opt#*=}"
	    ;;
	--ostype=*)
            ostype="${opt#*=}"
            ;;
        --hd=*)
            hdsize="${opt#*=}"
            ;;
        --ram=*)
            ramsize="${opt#*=}"
            ;;
	*)
	    create_virtual_box_usage
	    exit 1
	    ;;
    esac
    shift
done

# Create the Specified Virtual Machine
VBoxManage createhd --filename "$inputname" --ostype "$ostype" --register

# Set Virtual Machine RAM
VBoxManage modifyvm "$inputname" --memory "$ramsize"

# Change Network Mode to Bridged to Circumvent the Host OS Network Stack
VBoxManage modifyvm "$inputname" --bridgeadapter1 msk1
VBoxManage modifyvm "$inputname" --nic1 bridged

# Set and Attach Virtual Machine HD
VBoxManage createhd --filename "$inputname".vdi --size "$hdsize" --format VDI
VBoxManage storagectl "$inputname" --name "SATA Controller" --add sata --controller IntelAHCI
VBoxManage storageattach "$inputname" --storagectl "SATA Controller" --port 0 --device 0 --type hdd \
	   --medium "$inputname".vdi

# Connection Virtual Machine to the ISO
VBoxManage storagectl "$inputname" --name "IDE Controller" --add ide
VBoxManage storageattach "$inputname" --storagectl "IDE Controller" --port 0 --device 0 \
	   --type dvddrive --medium "$isolocation"

# Setup Remote Connection Ability
VBoxManage modifyvm "$inputname" --vrde on
VBoxManage modifyvm "$inputname" --vrdemulticon on --vrdeport "$__remoteport"

exit $?

