#!/usr/bin/env bash

# Description: This file contains the bash function definitions for each of the Open Virtual Desktop
# project's bash scripts.

# Usage: This script is not meant to be executed.

# Author: Andrew Cowden
# Contact: am.cowden.97@siu.edu

# Initiate Proper Error Termination and Debugging Options
set -o errexit
set -o nounset
set -o pipefail
# set -o xtrace

# Instantiate Facilitating  Variables
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
__file="${__dir}/$(basename "${BASH_SOURCE[0]}")"
__base="$(basename ${__file})"


# *************** Function Definitions ***************

# Default Debugging Function
function start_debugging () {
    set -o xtrace
}

# clone_virtual_box_vm Usage Function
function clone_virtual_box_usage () {
    echo "usage: $__base -i inputname [-o outputname] [-s] [-h] [-d]"
    echo "    -i|--inputname inputname         specify the name of the vm to clone"
    echo "    -o|--outputname outputname       specify the name of the new cloned vm"
    echo "    -s|--start                       start the newly cloned vm"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    echo "    -o=outputname|--outputname=outputname"
    exit 1
}

# start_virtual_box_vm Usage Function
function start_virtual_box_usage() {
    echo "usage: $__base -i inputname [-h] [-d]"
    echo "    -i|--inputname inputname         specify the name of the vm to start"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    exit 1
}

# stop_virtual_box_vm Usage Function                                                                   
function stop_virtual_box_usage() {
    echo "usage: $__base -i inputname [-h] [-d]"
    echo "    -i|--inputname inputname         specify the name of the vm to stop"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    exit 1
}


# create_virtual_box_vm Usage Function                                                                  
function create_virtual_box_usage () {
    echo "usage: $__base -i inputname --ostype ostype --isolocation pathtoiso"
    echo "        [hd hdsize] [ram ramsize] [-h] [-d]"
    echo
    echo "    -i|--inputname inputname         specify the name of the vm to clone"
    echo "    --ostype ostype                  input the type of Operating System"
    echo "    --isolocation pathtoiso          input the absolute path to the desired iso file"
    echo "    --hd hdsize                      choose the desired hard disk size in MB"
    echo "    --ram ramsize                    choose the desired ram size in MB"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    echo "    --ostype=ostype"
    echo "    --isolocation=pathtoiso"
    echo "    --hd=hdsize"
    echo "    --ram=ramsize"
    exit 1
}


# remove_virtual_box_vm Usage Function                                                                  
function remove_virtual_box_usage() {
    echo "usage: $__base -i inputname [-h] [-d]"
    echo "    -i|--inputname inputname         specify the name of the vm to remove"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    exit 1
}


# list_virtual_box_vm Usage Function                                                                    
function list_virtual_box_usage() {
    echo "usage: $__base [-r] [-n] [-h] [-d]"
    echo "    -r|--running                     display only running vms"
    echo "    -d|--down                        display only vms that are not running"
    echo "    -h|--help                        display help info"
    echo "    -d|--debug                       start debugging mode"
    echo
    echo "---------------------------Optional Argument Method---------------------------"
    echo "    -i=inputname|--inputname=inputname"
    exit 1
}


# clone_virtual_box_vm Help Function
function clone_virtual_box_help () {
    echo "The clone_virtual_box script is meant to allow an administrative user to"
    echo "input the name of an existing virtual machine within VirtualBox and"
    echo "recieve an identical clone of the original. The inputname is required"
    echo "to be given as an argument to this script. All other parameters are optional"
    echo "for the use of this script."
    echo
    clone_virtual_box_usage
    exit 1
}

# start_virtual_box_vm Help Function                                                                    
function start_virtual_box_help () {
    echo "The start_virtual_box script is meant to allow an administrative user to"
    echo "input the name of an existing virtual machine within VirtualBox and"
    echo "start it using the headless mode option. The inputname is required to be given"
    echo "as an argument to this script. All other parameters are optional for the use of this script."
    echo
    start_virtual_box_usage
    exit 1
}

# stop_virtual_box_vm Help Function                                                                    
function stop_virtual_box_help () {
    echo "The stop_virtual_box script is meant to allow an administrative user to"
    echo "input the name of an existing virtual machine within VirtualBox and"
    echo "cleanly shut it down. The inputname is required to be given as an argument to this script."
    echo "All other parameters are optional for the use of this script."
    echo
    stop_virtual_box_usage
    exit 1
}


# create_virtual_box_vm Help Function
function create_virtual_box_help () {
    echo "The create_virtual_box script is meant to allow an administrative user to"
    echo "initialize a new virtual machine. The inputname, isolocation, and ostype are required to"
    echo "as arguments to this script. All other parameters are optional for the use of this script"
    echo " with hd and ram having default values of 4GB and 10GB respectivley."
    echo
    create_virtual_box_usage
    exit 1
}


# remove_virtual_box_vm Help Function
function remove_virtual_box_help () {
    echo "The remove_virtual_box script is meant to allow an administrative user to"
    echo "input the name of an existing virtual machine within VirtualBox and delete it and its files."
    echo "The inputname is required to be given as an argument to this script."
    echo "All other parameters are optional for the use of this script."
    echo
    stop_virtual_box_usage
    exit 1
}


# list_virtual_box_vm Help Function 
function list_virtual_box_help () {
    echo "The list_virtual_box script is meant to allow an administrative user to"
    echo "list all of the virtual machines within VirtualBox. Filters for seeing all currently"
    echo "running virtual machines as well as all inactive virtual machines are available as arguments."
    echo
    list_virtual_box_usage
    exit 1
}
