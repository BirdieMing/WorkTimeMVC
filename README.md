# WorkTimeMVC

This is a basic ASP.NET MVC application that allows user to track their work time by clocking in/out. Register as a user in top right. All users can clock in / out. Managers can modify work times and add / delete employees.

TODO: 
- Figure out why time is 4 hours ahead when deployed to AWS
- Encrypt passwords in database

# Deployment

Local
- Create connection string in web.config

`<add name="Todo"
    providerName="System.Data.SqlClient"
    connectionString="Data Source=.\SQLEXPRESS;AttachDbFileName=|DataDirectory|\TodoDatabase.mdf;Integrated Security=True;User Instance=True;MultipleActiveResultSets=True" />`
- Update-Database -ConnectionStringName "MyConnectionString"

AWS 
- Create RDS Database in AWS. 
- Reference the database in connection string file.
- Update-Database -ConnectionStringName "MyConnectionString"
- Install plugin for Elastic Beantalk using Nuget. Deploy to environment.

Azure
- https://www.youtube.com/watch?v=31kxmM-WZLU
