# Docker Deployment

After you've installed docker, you can now setup a environment for Apache Guacamole.

# Step One: Deploy guacamole-server

Hosts the guacd container

## Download the image from dockerhub

```
$ sudo docker pull guacamole/guacd
```

Output:

```
[sudo] password for admin: 
Using default tag: latest
latest: Pulling from guacamole/guacd
af4b0a2388c6: Pull complete 
dcdd9c2ece80: Pull complete 
674a18e16f54: Pull complete 
0e6dd708fc82: Pull complete 
300b33db8291: Pull complete 
Digest: sha256:3cb2ad5cfecda2bc3b5383a272d22c106b7e860cebe0600345c57f4b61e4484d
Status: Downloaded newer image for guacamole/guacd:latest
```

# Running the guacd Docker image

E.g., 

```
$ sudo docker run --name some-guacd -d guacamole/guacd
```

Output:

```
64321f7931472acfcf4a69465f335ab234c0eaa146ab4b5ff5350c8c7b0d0d8d
```


```
$ sudo docker ps
```

```
CONTAINER ID        IMAGE               COMMAND                  CREATED             STATUS              PORTS               NAMES
64321f793147        guacamole/guacd     "/usr/local/sbin/gua…"   25 seconds ago      Up 24 seconds       4822/tcp            some-guacd
$
```

# Step Two: Deploy MySQL container

To use Guacamole with the MySQL authentication backend, you will need either a Docker container running the mysql image, or network access to a working installation of MySQL. The connection to MySQL can be specified using either environment variables or a Docker link.

## Pull the mysql docker image from DockerHub

```
docker pull mysql
```

## Run the mysql server

```
docker run --name some-mysql -e MYSQL_ROOT_PASSWORD=secret -p 3306:3306 mysql:latest
```

## Initializing the MySQL database

### Option 2a:

You can use the SQL scripts included with the database authentication.

Once this script is generated, you must:

1. Create a database for Guacamole within MySQL, such as guacamole_db.
2. Create a user for Guacamole within MySQL with access to this database, such as guacamole_user.
3. Run the script on the newly-created database.

The process for doing this via the mysql utility included with MySQL is documented in Chapter 6, Database authentication.

I used the following steps to initialize the MySQL Server:

Upon a shell inside the mysql container

```
sudo docker exec -it some-mysql /bin/bash
```

Manually log into the mysql server and created a database and granted access to a new user.

```
mysql -h localhost -p
CREATE DATABASE guacamole_db;
CREATE USER 'guacamole_user'@'localhost' IDENTIFIED BY 'some_password';
GRANT SELECT,INSERT,UPDATE,DELETE ON guacamole_db.* TO 'guacamole_user'@'localhost';
FLUSH PRIVILEGES;
quit
```

This following initializes the database, there might've been some conflicts with the previous steps.

```
apt-get update
apt-get install -y wget
wget https://raw.githubusercontent.com/glyptodon/guacamole-client/master/extensions/guacamole-auth-jdbc/modules/guacamole-auth-jdbc-mysql/schema/001-create-schema.sql
wget https://raw.githubusercontent.com/glyptodon/guacamole-client/master/extensions/guacamole-auth-jdbc/modules/guacamole-auth-jdbc-mysql/schema/002-create-admin-user.sql
cat *.sql | mysql -u root -p guacamole_db
```

For some reason, I had to use user root rather than guacamole_user when accessing the mysql database.  This might've been some permission issue with the mysql container.  I didn't spend time to figure it out, since I planned to use a production mysql server later.

### Option 2b: (untested)

Alternative approach provided by the documentation:

You can alternatively try the following command when initially running the mysql server.

Your database is not already initialized with the Guacamole schema, you'll need to do so prior to using Guacamole. A convenience script for generating the necessary SQL to do this is included in the Guacamole image.

To generate a SQL script which can be used to initialize a fresh MySQL database as documented in Chapter 6, Database authentication:

```
$ docker run --rm guacamole/guacamole /opt/guacamole/bin/initdb.sh --mysql > initdb.sql
```

# Step Three: Deploy the Guacamole Docker image

## Compile the guacamole-client container image

## Checkout a particular branch

```
mkdir ldap
cd ldap
git clone https://github.com/michaelbarkdoll/guacamole-client.git
cd guacamole-client
git checkout master
git pull origin jira/234
```

## Compile guacamole-client

```
sudo docker build -t mbarkdoll-test/guacamole .
```

```
Successfully built 36da894e9194
Successfully tagged mbarkdoll-test/guacamole:latest
```

## Create custom GUACAMOLE_HOME dir

```
mkdir -p ~/guacamole/test
```

#### Enable debugging:

```
$ echo "\
<configuration>

    <!-- Appender for debugging -->
    <appender name="GUAC-DEBUG" class="ch.qos.logback.core.ConsoleAppender">
        <encoder>
            <pattern>%d{HH:mm:ss.SSS} [%thread] %-5level %logger{36} - %msg%n</pattern>
        </encoder>
    </appender>

    <!-- Log at DEBUG level -->
    <root level="debug">
        <appender-ref ref="GUAC-DEBUG"/>
    </root>

</configuration>" > ~/guacamole/test/logback2.xml 
```

Some of the following ip addresses might need to be adjusted based on your docker container ip addresses that were assigned.

```
mysql-hostname: 172.17.0.3
mysql-port: 3306
mysql-database: guacamole_db
mysql-username: root
mysql-password: secret
ldap-hostname: ad.siu.edu
ldap-port: 636
ldap-encryption-method: ssl

ldap-user-base-dn: dc=ad,dc=siu,dc=edu
ldap-username-attribute: sAMAccountName

# Uncomment your line and update your dawg tag

#ldap-search-bind-dn: cn=Ldap Query2,ou=Users,ou=CS,ou=COS,ou=Academic
Affairs,dc=ad,dc=siu,dc=edu
#ldap-search-bind-dn: cn=Andrew M Cowden-
SIU85XXXXXXX,ou=Student,ou=Roles,ou=IDM,dc=ad,dc=siu,dc=edu
#ldap-search-bind-dn: cn=Mark R Beussink-
SIU85XXXXXXX,ou=Student,ou=Roles,ou=IDM,dc=ad,dc=siu,dc=edu
#ldap-search-bind-dn: cn=Edward J Byrne-
SIU85XXXXXXX,ou=Student,ou=Roles,ou=IDM,dc=ad,dc=siu,dc=edu
#ldap-search-bind-dn: cn=Justin M Sieling-
SIU85XXXXXXX,ou=Student,ou=Roles,ou=IDM,dc=ad,dc=siu,dc=edu

# Put your sso password in the line below
ldap-search-bind-password: plain_text_passwd_here

ldap-follow-referrals: false
ldap-user-search-filter: (objectClass=user)
ldap-operation-timeout: 180
ldap-max-search-results: 30000
ldap-max-referral-hops: 20" > ~/guacamole/test/guacamole.properties
```

## Run the container

```
$ sudo docker run \
--name some-guacamole \
--link some-guacd:guacd \
--link some-mysql:mysql \
-e MYSQL_DATABASE=guacamole_db \
-e MYSQL_USER=root -e MYSQL_PASSWORD=secret -e \
MYSQL_HOSTNAME=some_mysql \
-e MYSQL_PORT=3306 -e "LDAP_PORT=636" \
-e "LDAP_ENCRYPTION_METHOD=ssl" \
-e LDAP_HOSTNAME=ad.siu.edu -e \
"LDAP_USER_BASE_DN=dc=ad,dc=siu,dc=edu" \
-e "EXTENSIONS=auth-ldap" \
-v /home/admin/guacamole/test:/home/admin/guacamole/test -e GUACAMOLE_HOME=/home/admin/guacamole/test -p 8080:8080 mbeussink-test/guacamole
```

# Remove old container

```
docker rm de56c472613f5daaec9af0c14555e6da36afc28867e83f22f915657a8fcc5157
```

# Test it out

http://localhost:8080/guacamole

username:
guacadmin
password:
guacadmin

## Optionally, assume bash control of a container named some-guacamole

```
sudo docker exec -it some-guacamole /bin/bash
```

