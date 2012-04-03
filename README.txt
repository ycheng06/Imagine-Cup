TEAM: EOS
MEMBERS: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
SCHOOL: Tufts University
PROJECT: Medivise
================================
DESIGN Spec
=======
The design for this prototype has two main parts, SQL Azure and ASP.NET MVC3 webrole.
SQL Azure contains all the patient/user information. ASP.NET MVC3 handles all the
front end requests and views of the application. A task scheduler is also implemented
inside the webrole. This task scheduler performs sql queries and send out SMS messages
to patients subscribed for the service. In beta version, the task scheduler will probably
be moved into a separate webrole, so scaling will be easier to handle. Twilio API provides
all the functionality that Medivise needed to communicate with patients. Task Scheduler is
implemented using Quartz.NET

===============================
PROTOTYPE
=========
This prototype only contains the core features of Medivise. 
It supports only Eastern Time Zone and has a very basic UI.
For beta release, the UI will consist more drag and drop functionlities and less of
HTML forms.In the beta version the register function will also be removed. Hospitals
subscribed to our service will be given a set of account/password. This will be a better
way to handle security and maintain our service. 

================================
TRYING OUT THE PROTOTYPE
==========================
*Note: If you want to run the project locally, please go to the web.config file in the root 
 directory and comment out the SQL Azure connection string and uncomment the 
 SQLEXPRESS connection string. SQL Azure will cannot run locally without my approval of
	your computer's IP address, so it will not work
1. go to http:\\eosimaginecupp.cloudapp.net 
2. Login with the following account/password
   account: xihan
   password: xih@n123
3. create a patient under your name and phone number
4. confirmation SMS message will be sent to you
5. First medicine reminder is set to 10 AM EST
6. Medicication Warning is set to 5PM EST
7. A Miss medication alert will be created if you don't reply before the next medicine reminder.

ENJOY !! 
