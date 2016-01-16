Configuration notes for .config files in client applications
------------------------------------------------------------
In the "appSettings" element of the "configuration" element in a client .config
file e.g web.config, app.config, etc

For example: 

<configuration>
	<appSettings>
		<add key="myKey" value="usefulStuff" />
	</appSettings>
<configuration>

Add the following keys:

1. An email address for sending exception emails from. I'm using no-reply@ccis.com,
	which isn't a real address, but it doesn't matter.

	<add key="SendExceptionEmailsFrom" value="no-reply@ccis.com"/>

2. An email address to receive exception emails at. These emails will help with
	debugging issues that come up, and the address used should be an actual email
	address if you intend to actually receive this emails.
	
	<add key="SendExceptionEmailsTo" value="ccis.developer@gmail.com"/>

3. The ISP's mail server address. If you know your ISP, you can usually google
	for this. ISPs like TWC, Comcast, Consolidated Comms, etc.

	<add key="MailServer" value="mail.consolidated.net"/>

4. A location on the file system where exceptions should be logged to. The value 
	be a fully qualified directory name like in the example.

	<add key="LogFileDirectory" value="C:\Users\Philip\Documents\
		Visual Studio 2010\Projects\CCSolution\LogFiles"/>

5. Whether or not exceptions should be logged to the LogFileDirectory. The value
	can be "Y" for yes or anything else for no. 

	<add key="ShouldLogExceptions" value="Y"/>

6. Whether or not exceptions should be emailed. The value can be "Y" for yes 
	or anything else for no. 

	<add key="ShouldSendExceptionEmails" value="N"/>
