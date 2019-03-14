#!/usr/bin/env bash

# Description: This script is for the purpose of CLONING AN EXISTING virtualbox virtual machine from the
# command line. The primary functionality of this script is to be used for connection with the
# Open Virtual Desktop project.

# Usage: Required are the virtual machine image to clone and the desired clone name.

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

# Instantiate Custom Variables
inputname=""
outputname=""

# Check Empty Argument Case
if [ $# -eq 0 ]; then
    clone_virtual_box_usage
    exit 1
fi

# Argument Parsing
while [[ $# -gt 0 ]]; do
    opt="$1"
    case "$opt" in
	-h|--help)
	    clone_virtual_box_help
	    exit 0
	    ;;
	-d|--debug)
	    start_debugging
	    ;;
	-i|--inputname)
	    shift
	    inputname="$1"
	    ;;
	-i=*|--inputname=*)
	    inputname="${opt#*=}"
	    ;;
	-o|--outputname)
	    shift
	    outputname="$1"
	    ;;
	-o=*|--outputname=*)
	    outputname="${opt#*=}"
	    ;;
	*)
	    clone_virtual_box_usage
	    exit 1
	    ;;
    esac
    shift
done

# Clone the Virtual Machine Specified, Register it with VirtualBox, and Rename it
VBoxManage clonevm --register "$inputname" --name "$outputname"

exit $?

