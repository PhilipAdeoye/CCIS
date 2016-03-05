# CCIS: Cross Country Information System

A little tool to keep track of cross country runner's races, times, etc. 

Techs used: ASP.NET MVC 4, SQL Server 2012, Twitter Bootstrap, etc

## Project Layout

### CCData

A class library project that serves as the data access layer for CCIS. Entity Framework (with Database First) is used as the Object Relational Mapper.

### CCMvc

An ASP.NET MVC project that makes CCIS a web application. 
External Front End Libraries Used:

  - [jQuery 2.1.4](https://jquery.com/) and [jQuery UI 1.11.4](http://jqueryui.com/)
  - [Bootswatch Paper Theme with Bootstrap 3](https://bootswatch.com/paper/)  
  - [MVC Foolproof Validation](https://foolproof.codeplex.com/) to augment `ComponentModel.DataAnnotations`'s declarative validations
  - [DataTables](https://www.datatables.net/) for sweet client side table functionality
  - [Bootstrap 3 Date/Time Picker](https://github.com/Eonasdan/bootstrap-datetimepicker)
  - [Moment.js](http://momentjs.com/) is used as part of Bootstrap 3 Date/Time Picker
  - [Twitter Typeahead](https://twitter.github.io/typeahead.js/)
  - [Toastr](http://codeseven.github.io/toastr/) for showing success/info/error feedback to the user after actions
  - [Robin Herbot's jquery.inputmask](https://github.com/RobinHerbots/jquery.inputmask)
  
### Forloop.HtmlHelpers.Clone

A class library project which is a clone of [Forloop.HtmlHelpers](https://bitbucket.org/forloop/forloop-htmlhelpers/wiki/Home), but ported to work with .NET 4.0. The original version is built for .NET 4.5 and above. Used in CCMvc to enable putting JavaScript code block and files at the bottom of the page.

### Helpers

A class library project that provides common, cross-cutting functionality like logging, exception handling, email notifications, etc across projects.

### DBScripts

T-SQL scripts to be run on SQL Server 2012 (that's what I've tried them on) to create the CrossCountry database and intialize some data.

A note on creating the DB: When running the initial script in DBScripts: `001_DBinit.sql`, replace the `FILENAME`s on lines 7 and 9 with your desired location
