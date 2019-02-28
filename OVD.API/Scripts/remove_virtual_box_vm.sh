#!/usr/bin/env bash

# Description: This script is for the purpose of REMOVING AN EXISTING virtualbox virtual machine
# from the command line. The primary functionality of this script is to be used for connection with the
# Open Virtual Desktop project.

# Usage: Required is the virtual machine name to remove

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

# Check Empty Argument Case
if [ $# -eq 0 ]; then
    remove_virtual_box_usage
    exit 1
fi

# Argument Parsing
while [[ $# -gt 0 ]]; do
    opt="$1"
    case "$opt" in
	-h|--help)
	    remove_virtual_box_help
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
	*)
	    remove_virtual_box_usage
	    exit 1
	    ;;
    esac
    shift
done


# Stop the Specified Virtual Machine
VBoxManage controlvm "$inputname" --poweroff

# Delete the Specified Virtual Machine
VBoxManage unregister "$inputname" --delete

exit $?

