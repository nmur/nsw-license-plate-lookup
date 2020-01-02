[![Build Status](https://dev.azure.com/nickmurray91/nickmurray91/_apis/build/status/nsw-license-plate-lookup-app%20-%20CI?branchName=master)](https://dev.azure.com/nickmurray91/nickmurray91/_build/latest?definitionId=2&branchName=master) ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/nickmurray91/nickmurray91/2?label=Coverage)

# nsw-license-plate-lookup
This service provides basic information on individual vehicles that are registered in NSW.
  
## Usage  
Either navigate to or send a GET request to the to the following address:
```http
GET nickmurray.dev/api/plate/<plateNumber>
```

For example: [nickmurray.dev/api/plate/ABC123](https://nickmurray.dev/api/plate/ABC123).

The response data structure is as follows:  
```json
{
  "vehicle": {
    "bodyShape": "SEDAN",
    "manufacturer": "MER",
    "manufactureYear": 2012,
    "model": "C250 CDI BE",
    "NSWPlateNumber": "ABC123",
    "plateType": "O",
    "tareWeight": 1587,
    "variant": "100 2.1L DIE 7SPA STD SEDAN",
    "vehicleColour": "GREY",
    "vehicleType": "PASSENGER VEHICLES",
    "vinNumber": "xxxxxxxxxxxxx9818"
  }
}
```

Some values, such as the `manufacturer` field, are returned as their raw abbreviated values as recieved from NSW Service.
