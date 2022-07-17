# ResolveCSV
ASP.NET Core Web API that parses a CSV file and facilitates querying data from it.

 ## Query Categories
  

- ### Company Name
  
  Get people whose company name contains the given input.

 **GET** ``http://localhost:5251/api/people/company``
 
#### Parameters
  
| Name  | Type | Example  | Description |
| ------------- | ------------- | ------------- | ------------- |
| companyName  | string  | Esq  | Whole or part of a company name  |

  
  
- ### County
  
  Get people who reside in the given county.

 **GET** ``http://localhost:5251/api/people/county``
 
#### Parameters
  
| Name  | Type | Example  | Description |
| ------------- | ------------- | ------------- | ------------- |
| countyName  | string  | Kent  | Exact county name  |
  

- ### House Number
  
  Get people whose house number has the given number of digits.

 **GET** ``http://localhost:5251/api/people/houseNumber``
 
#### Parameters
  
| Name  | Type | Example  | Description |
| ------------- | ------------- | ------------- | ------------- |
| digits  | number  | 2  | The number of digits in the house number  |

 
- ### Website
  
  Get people whose URL is longer than given number of characters.

 **GET** ``http://localhost:5251/api/people/website``
 
#### Parameters
  
| Name  | Type | Example  | Description |
| ------------- | ------------- | ------------- | ------------- |
| length  | number  | 40  | The minimum length of the website URL  |


- ### PostCode
  
  Get people who live in a postcode area with the given number of digits.

 **GET** ``http://localhost:5251/api/people/postcode``
 
#### Parameters
  
| Name  | Type | Example  | Description |
| ------------- | ------------- | ------------- | ------------- |
| areaDigits  | number  | 1  | The number of digits in the area code of the postcode  |

  
- ### Phone Number
  
  Get people whose first phone number is numerically larger than their second phone number.
  
 **GET** ``http://localhost:5251/api/people/phoneNumber``
 

