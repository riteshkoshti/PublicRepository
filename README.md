# PublicRepository

# Contact Management Services
	
## Project Description:
   Design and implement a production ready application for maintaining contact information. Please choose the frameworks, packages and/or technologies that best suit the requirements.

## Supported Functionalities
   - List contacts
   - Add a contact
   - Edit contact
   - Delete/Inactivate a contact

## Contact Model
   - First Name
   - Last Name
   - Email
   - Phone Number
   - Status (Possible values: Active/Inactive)

## Website URL
	http://localhost:portnumber/swagger/index.html


## Technologies used for development
   * Web API (.Net Core)
   * Entity Framework Core
   * SQL Server 2019
   * Microsoft Unit Test framework
   
   	
## Projects in Solution
1. Web API :- ContactManagementServices
2. DAL:- DataAccessLayer
3. Unit Test:- UnitTests


## Solution Structure

![Alt text]( https://github.com/riteshkoshti/PublicRepository/blob/master/SolutionStructure.JPG)
 

## Database Design

![Alt text]( https://github.com/riteshkoshti/PublicRepository/blob/master/DatabaseDesign.JPG)


## Contact Services Swagger UI 

![Alt text]( https://github.com/riteshkoshti/PublicRepository/blob/master/SwaggerUI.JPG)
 

## Deployment Instructions
   1. Open Solution in Visual Studio
   2. Create the Database as per design mentioned 
   3. Add the connection string in appsettings.json file
   4. Publish the ContactManagementServices project code to any hosting environment
   5. Browse the application as per below example Swagger URL for API details
      http://localhost:portnumber/swagger/index.html
