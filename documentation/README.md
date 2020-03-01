![Hackathon Logo](documentation/images/hackathon.png?raw=true "Hackathon Logo")
# Documentation 
## Summary

**Category:** Replacement of Sitecore Marketplace Website

Our team decided to take the challenge of building a replacement of the Sitecore Marketplace Website. The main goal was to reproduce some of the most important features of the current website and add some new ideas using as few custom code as possible, taking advantage of SXA components potential.

## Pre-requisites

Please make sure you have the following requirements:
Sitecore 9.3.0 rev. 003498 
Sitecore Experience Accelerator 9.3.0

## Installation

Install Sitecore 9.3 rev 003498 https://dev.sitecore.net/Downloads/Sitecore_Experience_Platform/93/Sitecore_Experience_Platform_93_Initial_Release.aspx

Install Sitecore Experience Accelerator 9.3.0 (included on Sitecore 9.3 installation)

## Configuration

Everything should be included in the provided package (ADICIONAR LINK) and steps above.
1- First install the two packages provided. This packages are to be used in a clean instance. Install the Package 1 https://github.com/Sitecore-Hackathon/2020-Noesis-Team/blob/master/documentation/2020-Noesis-Team-Hackathon-1-1.zip and then the Package 2: https://github.com/Sitecore-Hackathon/2020-Noesis-Team/blob/master/documentation/2020-Noesis-Team-Hackathon-2.zip

2- Run the SQL script https://github.com/Sitecore-Hackathon/2020-Noesis-Team/blob/master/documentation/clean93_MarketModule.sql to create the additional database a table that will be used to create the module items.
3- Add the new connectionstring to the connectionstring.config.
4- Deploy the code of the repository.

## Technical breakdown

Having in mind that the goal of this website is to allow community members to share their customizations and developments on Sitecore, it should focus at:
•	Allow visitors to know and download the Marketplace modules
•	Allow contributors to upload their own development

In order to easily explain the website structure, let’s break it into the main functionalities:
1.	Search
2.	Authentication (Login Page)
3.	Main Page (Most Popular, Top Rated)
4.	Detail Pages (Individual Module)
5.	Upload/Download Module

### Search
The overall search functionality was implemented using the SXA components: Search Box and Search Results. The Search Box exists across all pages (header), when the user searches for any keyword, there’s a specific page serving the Search Results.

### Authentication
Regarding the authentication process, it was implemented using SXA Login and Logout components and Security Editor. The authentication must be made with a Sitecore account.
Both buttons are included on the header section, being shown/hidden using a Personalization Rule based on the status of the user (logged in). If a user logged out, he gets redirected to the login page.
We configured the extranet\Anonymous Sitecore account on the Security Editor, assigning the necessary pages with Denied read permissions for this user.

### Home Page
Since this is the main page of the website, we understand that is important to every visitor to be able to have information on the “top” modules. The central section of the page has lists of the Most Popular and Top Rated modules. 
Both of the lists were implemented using the SXA Page List component. Under the Item Queries item, we create two custom queries to achieve that end.

### Detail Pages
These pages show the information about the selected module. Information, images, requirements and ratings. 

### Upload/Download Module
In order to implement de upload feature, the technical approach was based around the creation of a new database in order to allow information to flow from CD environment to CM in a secure way. This database gets the data from filling the necessary fields in the form and push it to the master database through a scheduled job in order to render the necessary fields again on the web database through the Publish feature using a Workflow.

## Usage

### Search
1.	Navigate to any page
2.	Write any keyword on the search box
3.	Check if you have predictive results
4.	Click to search
5.	Check if the presented page with the results is correctly displayed

### Authentication
1.	Navigate to the login page (/login)
2.	Click on the Login button
3.	Fill the credentials 
4.	Click on the logout button
5.	Check that the user no longer have access to the pages

### Home Page
1.	Navigate to the home page
2.	Click on the “Contributor” button
3.	Fill the form and upload your module

### Detail Pages
1.	Navigate to any module detail page
2.	Click on the download button
3.	Check if the download was successful

## Video

Direct link to the video: https://youtu.be/4Q9uHsMAZt4
