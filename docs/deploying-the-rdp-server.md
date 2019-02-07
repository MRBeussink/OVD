# Deploying the RDP Server

This documentation outlines the steps involved for creating the encompassing Remote Desktop Protocol (RDP) Server for use within the Open Virtual Desktop Project (OVD). The RDP server is used to store and access the virtual machines that can be access remotley through Apache Guacamole. The following commands are to be run from your base machines outside of any Docker containers.

# Step One: Installing VirtualBox

The following installation instructions for installing VirtualBox on an Ubuntu Linux System are derived from the post included below.
https://websiteforstudents.com/installing-virtualbox-5-2-ubuntu-17-04-17-10/

In order to ensure that VirtualBox works properly and is at its newest version, update the repository and dependency information for your operating system.
```
sudo apt-get update
```

Ensure that your linux gcc header definitions and package are included.
```
sudo apt-get -y install gcc make linux-headers-$(uname -r) dkms
```

Install the VirtualBox repository keys.
```
wget -q https://www.virtualbox.org/download/oracle_vbox_2016.asc -O- | sudo apt-key add -
wget -q https://www.virtualbox.org/download/oracle_vbox.asc -O- | sudo apt-key add -
```

Add the VirtualBox repository to your machine.
```
sudo sh -c 'echo "deb http://download.virtualbox.org/virtualbox/debian $(lsb_release -sc) contrib" >> /etc/apt/sources.list'
```

Update your repository listing and install VirtualBox. Note that the version number of VirtualBox may be different based upon which release you choose to install. 
```
sudo apt-get update
sudo apt-get install virtualbox-5.2
```

We now have the basic installation of VirtualBox completed. Since we are using VirtualBox as an RDP server, we need additional features to allow remote connections which can be found in the VirtualBox Extension Pack.
```
curl -O http://download.virtualbox.org/virtualbox/5.2.0/Oracle_VM_VirtualBox_Extension_Pack-5.2.0-118431.vbox-extpack
sudo VBoxManage extpack install Oracle_VM_VirtualBox_Extension_Pack-5.2.0-118431.vbox-extpack
```

VirtualBox is now installed on your system and ready for the next steps of configuraiton. If prompted, please update both the base VirtualBox application as well as the Extension Pack to their newest releases if desired.

# Step 2: Creating a Virtual Machine
This step involves the basic procedure of creating a Virtual Machine instance with VirtualBox. This virtual machine is being used for test purposes but the major steps involved will be the same for actual implementation. The name and additional space parameters do not effect the outcome of the RDP server so you may choose default values for RAM, Hard Disk, and CPU limits. In order to use your virtual machine, you must download your desired Operating System in the form of an .iso file which will be added later.

Once you have a test virtual machine created, we must add an Operating System image to it. To do this, right click on your virtual machine and select **Settings**. From here you will navigate to the **Storage** tab. From here, click on the **Adds Optical Drive** button and choose your .iso image. 

![](https://i.imgur.com/Gqx5g7I.png)

Now you can boot into your created virtual machine. Here you can install your operating system onto your virtual machine and create a test user.


# Step 3: Configuring the Virtual Machine as an RDP Connection
Now that we have created the test virtual machine, we need to give that virtual machine remote access. Start at the default screen within VirtualBox. Right click on your virtual machine and select **Settings**. Then, navigate to the **Display**. From here, your will select the **Remote Display** tab and select the **Enable Server** check box.

For this setup, I have chosen the default RDP sever port (3389) for connection. Currently, I have left the other options at their default values.

# Step 4: Retrieving Your Docker0 Ip Address
Since we are using Docker containers to encapsulate portions of our Guacamole setup, we need to ensure that we are using the correct **localhost** address when testing testing our RDP server.

On your Linux based machine, execute the following command to get your docker0 address.

```
ip addr show docker0
```

The output of the above command should be similar to the following. Your docker0 address will be the corresponding 172.17.0.1 portion of the output below.
```
5: docker0: <BROADCAST,MULTICAST,UP,LOWER_UP> mtu 1500 qdisc noqueue state UP group default 
    link/ether 02:42:f6:b3:66:2a brd ff:ff:ff:ff:ff:ff
    inet 172.17.0.1/16 brd 172.17.255.255 scope global docker0
       valid_lft forever preferred_lft forever
    inet6 fe80::42:f6ff:feb3:662a/64 scope link 
       valid_lft forever preferred_lft forever
```

# Step 5: Connecting Your Virtual Machine to Guacamole
Navigate to your Guacamole test environment and login as guacadmin.
http://localhost:8080/guacamole/#/

Under the **guacadmin** dropdown menu, go to the **Settings** option. From there, navigate to the **Connections** tab where you will begin configuring a new connection with the **New Connection** button. The only fields that need to be changed are the following:

* Name: *Some Arbitrary Name*
* Protocol: RDP
* Network---Hostname: *your docker0 address*
* Network---Port: 3389

![](https://i.imgur.com/lNlCkNW.png)

Once this is completed, hit **Save** and navigate back to the home screen. From here you should have access to a fully functional virtual machine from your browser. If you wish to try out an SSH session on your local machine, complete step 5 again but change the Protocol to SSH and the Network Port to 22.
