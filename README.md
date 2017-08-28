# crud-exercise
A bare bones CRUD (create, read, update, delete) web application built using C#/ASP.NET Core with Visual Studio 2017.


## Goal
This web application is a cruise line passenger management tool which performs the following functions:

1. Stores passengers' number, first name, last name, and phone number
	 
2. Generates unique passenger numbers
	 
3. Allows users to create new passengers including first name, last name, and phone number; the passenger number is auto-generated when saving the record
	 
4. Allow users to update passengers, including the three user-chosen values
	 
5. Allow users to delete a passenger
	 
6. Allow users to view a list of passengers 


## Details
The bulk of the project is based on the following tutorial:
https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro

This project uses web interface to allow users to view and control a list of passengers.  Users can create new passengers, update passenger information, and delete passengers from the list.  Passenger data will persist between sessions on a local database.

This repository does not include that database.  When run for the first time, the project will create a local database and populate it with test data.  From that point forward, the database will continue to exist locally.

