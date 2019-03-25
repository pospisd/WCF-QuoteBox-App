--==================================================================
--
--  README.txt
--
--==================================================================


--==================================================================
--  Problems with using LocalDB and IIS Application Pool.
--==================================================================
Unlike SQL Server Express instances, which are running as Windows 
services, LocalDB instances are running as user processes. When 
different Windows users are connecting to LocalDB, they will end up 
with different LocalDB processes started for each of them. When we 
connect to (localdb)\v11.0 from Visual Studio, a LocalDB instance 
is started for us and runs as our Windows account. But when Web 
Application, running in IIS as ApplicationPoolIdentity, is connecting 
to LocalDB, another LocalDB instance is started for it and is running 
as ApplicationPoolIdentity! In effect, even though both Visual Studio 
and Web Application are using the same LocalDB connection string, 
they are connecting to different LocalDB instances. Obviously the 
database created from Visual Studio on our LocalDB instance will not 
be available in Web Application's LocalDB instance.

To get around these problems with IIS ApplicationPoolIdentity and 
LocalDB, SQL Server Developer Edition is installed on the machine
(this SQL Server version is a free version - download from Microsoft).

Once the stand alone instance of SQL Server is up and running, connect 
with SSMS and create security accounts for the ApplicationPoolIdentity:

On the Security => Logins folder on the Server instance (not database 
instance), Rt-click and select 'New Login...'
General:
    Login name = IIS APPPOOL\QuoteBoxService
	Window authenication = checked
	Default database = QuoteBox
Server Roles:
    public = checked
User Mapping:
	QuoteBox
	    db_datareader = checked
	    db_datawriter = checked
	    public = checked
Ok



And then the Security Users in the Database should be automatically
created.  Rt-click properties and update these...
Owned Schemas:
	db_datareader = checked
	db_datawriter = checked
	db_owner = checked
Membership:
	db_datareader = checked
	db_datawriter = checked
Securables:
	Search => Specfic objects... => Ok
		Object Types => Schemas = checked => Ok
	Enter the object names to select...
		dbo => Check Names... => Ok
	Now select in the bottom pane the 'Permissions for dbo'
		...[Whatever is required]
Ok


--==================================================================
--  Connection Strings
--==================================================================
For the connection string just make sure Integrated Security=True
for this identity.  It will look something like this...
"Data Source=PC180346\SQLDEV2014; Initial Catalog=QuoteBox; 
Integrated Security=True"


--==================================================================
--  Debugging: Attaching to process for Visual Studio debugging with
--  IIS hosted web service.
--==================================================================
To debug from Visual Studio => Debug => Attach to Process...
Then check the 'Show process from all users' box.
Select w3p.exe or w3wp.exe from the process list.


--==================================================================
--  IIS Stop / Start
--==================================================================
Command Prompt => 'iisreset'.

--==================================================================
--  WCF Collection Types: Arrays of Generic Lists?
--==================================================================
WCF serializes generic lists as arrays to send across the wire.  Just
Rt-click the ServiceRefernece => Configure Service Reference... => 
'Data Type' and choose 'System.Collections.Generic.List' for svcutil
to create proxy that converts them back into generic lists again.


