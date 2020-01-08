# Project Name: MovieTicketBooking

I had provided the entire MovieTicketBooking project to download with Sql server Script which contains tables, which are used in the preoject. 
Displayed all the seats in the theater and allowed the users to book it by clicking it. Only one user wil be allowed to reserved a specific seat.
If another user clicks a seat that was booked, he will get an error.

## Database Part:

1. First things to do is to Create Database, an scripts file is placed inside the "DbScripts" folder in the "MovieTicketBooking" project solution with 
  name "DBScripts_Part1.sql" and "DBScripts_Part2.sql".

2. Open these above Scripts file in the SQL server management studio and execute the DBScripts_Part1.sql script, then DBScripts_Part2.sql script. 

3. This execution of script will automatically create the database with name "CinemaTicketBookingDB" and associated tables along with data inside this database.

4. After Creating Database now make changes for the ConnectionStrings in Web.Config file which is residing within "MovieTicketBooking" project solution.

5. Change this connectionStrings to your Own Data Source with Sql UserName and Password.

6. MovieTicketBooking.Tests
  a.In the "MovieTicketBooking.Tests" solution change connection string from App.config file.
  b.Change the data source,user id and password according to your sql credentials.


## MovieTicketBooking.Tests Project

1. If any error occur while executing the unit test cases, then check the properties of all the .xml files.

2. If "Copy to Ouput Directory" is not set to "Copy if newer" then make it "Copy if newer".

## Email

To enable the send email feature so that user get the confirmation mail after booking and un-booking the seats, follow the below steps :-

1. Go to "Web.config" page in the "MovieTicketBooking" project solution and add the below parameters in it :-

    a. Add the value of "senderEmailId" to assign the sender email address.
  
    b. Add the value of "senderEmailPassword" to assign the sender email address valid password.
  
    c. Add the value of "senderPort" to assign the smtp port number from which email service is getting used to send email.

2. If after adding valid credentials any error occur then enable your security login from your respective email id.

## Error

If after downloading the code, the code does not execute in the visual studio then delete the ".vs"  folder and build the solution once again.




