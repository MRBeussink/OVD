#!/usr/bin/env bash

# Description: This script is for the purpose of LISTING virtualbox virtual machines
# from the command line. The primary functionality of this script is to be used for connection with the
# Open Virtual Desktop project.

# Usage: No Arguments are Required

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
viewrunning=0

# Argument Parsing
while [[ $# -gt 0 ]]; do
    opt="$1"
    case "$opt" in
	-h|--help)
	    start_virtual_box_help
	    exit 0
	    ;;
	-d|--debug)
	    start_debugging
	    ;;
	-r|--running)
	    viewrunning=1
	    ;;
	*)
	    list_virtual_box_usage
	    exit 1
	    ;;
    esac
    shift
done


# List Running Virtual Machines
if [ "$viewrunning" -eq 1 ]; then
    VBoxManage list runningvms --sorted

# List All Virtual Machines
else
    VBoxManage list vms --sorted
fi

exit $?

