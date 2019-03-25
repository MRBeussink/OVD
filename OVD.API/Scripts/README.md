# Ansible Quickstart

The purpose of this guide is quickly to illistrate how to quickly create a virtual machine with ansible.

This repository has already created the basic directory structure required for an **ansible role.**

# Dev workstation:

## Create ansible role directory structure:

In this example we'll be creating a ubuntu18.04-server deployment.  Note: This repository holds several os deployment examples.
```shell
$ mkdir -p ~/Desktop/Documentation/Ansible/QuickStart/ubuntu18.04-server
$ cd ~/Desktop/Documentation/Ansible/QuickStart/ubuntu18.04-server
```

```shell
$ mkdir -p group_vars host_vars library module_utils filter_plugins
# Creates a role called common
$ mkdir -p roles/common/{tasks,handlers,templates,files,vars,defaults,meta,library,module_utils,lookup_plugins}
$ touch production staging site.yml roles/common/{tasks,handlers,templates,files,vars,defaults,meta}/main.yml
```

````
production                # inventory file for production servers
staging                   # inventory file for staging environment

group_vars/
   group1                 # here we assign variables to particular groups
   group2                 # ""
host_vars/
   hostname1              # if systems need specific variables, put them here
   hostname2              # ""

library/                  # if any custom modules, put them here (optional)
module_utils/             # if any custom module_utils to support modules, put them here (optional)
filter_plugins/           # if any custom filter plugins, put them here (optional)

site.yml                  # master playbook
webservers.yml            # playbook for webserver tier
dbservers.yml             # playbook for dbserver tier

roles/
    common/               # this hierarchy represents a "role"
        tasks/            #
            main.yml      #  <-- tasks file can include smaller files if warranted
        handlers/         #
            main.yml      #  <-- handlers file
        templates/        #  <-- files for use with the template resource
            ntp.conf.j2   #  <------- templates end in .j2
        files/            #
            bar.txt       #  <-- files for use with the copy resource
            foo.sh        #  <-- script files for use with the script resource
        vars/             #
            main.yml      #  <-- variables associated with this role
        defaults/         #
            main.yml      #  <-- default lower priority variables for this role
        meta/             #
            main.yml      #  <-- role dependencies
        library/          # roles can also include custom modules
        module_utils/     # roles can also include custom module_utils
        lookup_plugins/   # or other types of plugins, like lookup in this case

    webtier/              # same kind of structure as "common" was above, done for the webtier role
    monitoring/           # ""
    fooapp/               # ""
````

https://docs.ansible.com/ansible/latest/user_guide/playbooks_best_practices.html

# On Dev Workstation 

## Let ssh know who the host is:

```shell
$ vi ~/.ssh/config
```

```bash
Host test
  Hostname 127.0.0.1
  # Optional following line (VirtualBox PortForwarding)
  # Port 1919 
  IdentityFile ~/.ssh/id_rsa
Host ubuntu1804
  Hostname 10.100.194.22
  IdentityFile ~/.ssh/id_rsa
```

## Make sure you already have a ssh private and public keys available

You should see id_rsa and id_rsa.pub
```shell
$ ls -la ~/.ssh
# Optionally, generate with:
$ ssh-keygen
```

# Use Vagrant to manage host
```shell
$ vi ~/Desktop/Documentation/Ansible/QuickStart/ubuntu18.04-server/Vagrantfile
```

# CentOS Host
```bash
# -*- mode: ruby -*-
# vi: set ft=ruby :

VAGRANTFILE_API_VERSION = "2"

#javapackage = ENV['java_package'] || "openjdk-8-jre"
#javahome = ENV['java_home'] || "/usr/lib/jvm/java-8-openjdk-amd64/jre"

Vagrant.configure(VAGRANTFILE_API_VERSION) do |config|
  # Ubuntu 16.04 LTS (Xenial Xerus)
  # config.vm.box = "ubuntu/xenial64"
  # Ubuntu 18.04 LTS (Bionic Beaver) Server
  config.vm.box = "ubuntu/bionic64"
  # Ubuntu 18.04 LTS (Bionic Beaver) Desktop
  #config.vm.box = "peru/ubuntu-18.04-desktop-amd64"
  #config.vm.box_version = "20181210.01"
  # Centos 7
  # config.vm.box = "centos/7"

  #config.vm.hostname = "centos7sp19"
  config.vm.hostname = "ubuntusp19"

  # Configure vm name (later used for ansible group)
  #config.vm.define "centos7sp19"
  config.vm.define = "ubuntusp19"

  config.vm.provider "virtualbox" do |vb|
    vb.memory = 2048
    vb.cpus = 2
  end

  # manual ip
  #config.vm.network "public_network", :bridge => "enxa0cec8039e98", ip: "10.100.194.21", :netmask => "255.255.248.0", auto_config: false
  # CentOS 7 manual ip address (interface enxa0cec8039e98)
  config.vm.network "public_network", ip: "10.100.194.22", bridge: "enxa0cec8039e98", bootproto: "static", gateway: "10.100.199.254"

  #config.vm.provision "shell",
    #inline: "sudo apt-get update && sudo apt install -y python"
    #inline: "sudo yum update && sudo yum install -y python"
    #inline: "sudo yum update"
  
  config.vm.provision "shell" do |s|
    ssh_pub_key = File.readlines("#{Dir.home}/.ssh/id_rsa.pub").first.strip
    s.inline = <<-SHELL
      echo #{ssh_pub_key} >> /home/vagrant/.ssh/authorized_keys
      # echo #{ssh_pub_key} >> /root/.ssh/authorized_keys
    SHELL
  end

  config.vm.provision "ansible" do |ansible|
    ansible.compatibility_mode = "2.0"
    ansible.playbook = "playbook.yml"

    #ansible.groups = {
    #  "test" => ["centos7sp19"]
    #}
    ansible.groups = {
      "test" => ["ubuntusp19"]
    }

#    ansible.extra_vars = {
#      java_package: javapackage,
#      java_home: javahome
#    }
  end

end
```

# On Dev Workstation

## Create the playbook.yml

```shell
$ vi ~/Desktop/Documentation/Ansible/QuickStart/ubuntu18.04-server/playbook.yml
```

```bash
---
- hosts: test
#- hosts: all
  become: true
  roles:
    - common
```

```shell
$ vi ~/Desktop/Documentation/Ansible/QuickStart/ubuntu18.04-server/staging
```

```bash
[testgroup]
test ansible_user=vagrant
```

## Optionally, recreate VM as required
```shell
$ cd ~/Desktop/Documentation/Ansible/QuickStart/centos-7-server
$ vagrant destroy -f; vagrant up
```

# Inside the VM/host being managed

## Setup passwordless sudo
```shell
$ vi /etc/sudoers.d/90-cloud-init-users
```

```bash
cisadmin ALL=(ALL) NOPASSWD:ALL
```

Note: Vagrant by default creates:
```shell
$ cat /etc/sudoers.d/vagrant
%vagrant ALL=(ALL) NOPASSWD: ALL
```

## Install required packages on the host (Ubuntu)
````
sudo apt-get -y install python-pip
````



## Manually run changes from the playbook after initial VM installed
```shell
$ ssh-keygen -f "/home/cisadmin/.ssh/known_hosts" -R "10.100.194.21"
$ ansible-playbook -i staging playbook.yml
```

