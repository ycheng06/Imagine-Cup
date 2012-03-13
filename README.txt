TEAM: EOS
MEMBERS: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
PROJECT: Medivise
================================
DESIGN
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
HTML forms. 

================================
TRYING OUT THE PROTOTYPE
==========================
1. Login with the following account/password
   account: xihan
   password: xih@n123
2. create a patient under your name and phone number
3. confirmation SMS message will be sent to you
4. First medicine reminder is set to 10 AM EST
5. Medicication Warning is set to 5PM EST
6. A Miss medication alert will be created if you don't reply before the next medicine reminder.

ENJOY !! 