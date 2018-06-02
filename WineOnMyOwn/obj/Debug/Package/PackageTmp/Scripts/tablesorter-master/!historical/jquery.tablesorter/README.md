# Introduction
Louisville Paving needs a system to keep inventory of their subcontractors and their associated compliance/risk profile.  

With every subctontractor available, the system can become a sort of Compliance Audit and Job Approval platform where routine checks are performed to ensure everyone is in compliance and new work is reviewed by a compliance team member to be approved before beginning the work.  

There are several admin/management screens necessary to pull this off. This includes tracking the subs, their various insurance info, etc... The other part of the system is a queue of job that will need to be reviewd/approved. The Project manager will fill out the form and then the compliance team will need to sign-off on the job.

>##[http://lpxcompliance.us-east-2.elasticbeanstalk.com/](http://lpxcompliance.us-east-2.elasticbeanstalk.com/)

# Getting Started
Standard tech-stack in play (C# - Asp.Net MVC - Sql Server). We're using Dapper and inline SQL or Stored Procs instead of Entity Framework this time. 

Hosting in AWS for both Sql and the Web Server. Nick will handle code deployment from source control. Credentials and connection info for Sql provided in the Slack channel
