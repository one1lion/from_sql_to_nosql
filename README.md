# from_sql_to_nosql
A sample project setup that is using Entity Framework Core and SQL Server that I would like to change to use Cosmos DB with Core SQL API

# Introduction
I am attempting to understand NoSQL by starting from what I do know: RDBMS.  The application I am partially assisting with is one that will allow users to scan a QR Code that is the encoded Url endpoint for Tracking Tickets, with the Tracking Ticket's Guid as a route parameter (e.g. https://{domain}/track/{ticket_guid}).  Users will stick QR Code stickers on equipment/instruments, chemicals, and samples.  Then, most likely using an iPad or other tablet device, scan the barcode and be directed to the Url to enter information about the thing they scanned.  Sounds pretty simple, and it was easy to model using a relational database.  However, the items that are being logged can be any number of real-world entities, which should be allowed to take whatever form is needed for a particular items type.  The person I am attempting to help also has the requirement to use Cosmos DB for the database, which, I believe, means that I am not able to apply much of my knowledge of using EF Core for a SQL Server provider.  

One more thing to note is that the way the system can retrieve the various types of data are noted below, but in short, we should be able to query for Tracking Tickets (with the associated Item and all of the data for that particular item's type), Mill Run Sheets (and the associated Samples -- which are a particular Item type), or individual Items (which will be displayed on their own page, and should be able to be navigated to directly if, for instance, a user bookmarks the page and navigates directly to it).

# Definitions
**Mill Run Sheet**: A Mill Run Sheet is a physical document used for tracking samples taken from Storage Tanks -- Although it looks like the definitions I've found through Google point to this being a sampling of solid material from a mill, whereas the samples used for this project are liquid, I believe

**Tracking Ticket**: A record that is used for tracking a particular Item

**Tracking Item**: Anything that can be tracked by this system, to include (but can be expanded upon):
- **Chemical**: A Chemical is a compound or substance available within a Lab
- **Instrument**: An Instrument is a device used for analyical measurements and observations (or some similar, broader, or more inclusive definition)
- **Sample**: A Sample is a container of material drawn from a Tank or taken from another source and logged on a Mill Run Sheet

**Plant**: The buildings or other physical equipment of a specific organization using this system

# How the documents should be accessed
Things that can be displayed by routing to them directly:
 - Tracking Ticket: By scanning a QR Code that is stuck onto a Sample, Instrument, Chemical, or other Tracking Item.  The QR Code is the encoded Url endpoint for Tracking Tickets, with the Tracking Ticket's Guid as a route parameter (e.g. https://{domain}/track/{ticket_guid}).  When routing to the page, if the Guid is associated with a saved Tracking ticket document, the existing record is displayed, otherwise the user will be prompted to pick the type of Tracking Item, and enter the details for it depending on the Type.

Things that can be searched for and displayed by clicking on a result, or by clicking on a link from a parent/related document (such as a Tracking Ticket, or Mill Run Sheet)
 - Mill Run Sheet
   - Search for by
     - Mill Run Sheet Id
     - Tank Id
     - Batch Id
     - Lot
     - Date
     - Plant Name/Acronym
   - Linked to from
     - Tracking Ticket
     - Plant (overview/dashboard)
     - Sample display page
 - Items
   - Chemical
     - Searched for by 
       - Name
       - CommonName
     - Linked to from
       - Tracking Ticket
       - List of Chemicals page
   - Instrument
     - Searched for by Name
     - Link to from 
       - Tracking Ticket
       - List of Instruments page
   - Sample
     - Searched for by
       - Mill Run Sheet Id
       - Container Type
       - Retrieved By
       - Sample Date
     - Linked to from
       - Tracking Ticket
       - Mill Run Sheet (list of Samples on a Mill Run Sheet)

# Documents

## Mill Run Sheet
```
{
  "id": "1", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url to pull up the Mill Run Sheet
  "millRunSheetId": "2020SEP-13392", // Internal Id
  "tankId": "PTR-2492", // Internal Tank Id/Name
  "batchId": "
}
```

## Tracking Items
### Sample Item
```
{
  "id": "1", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url without the parent Mill Run Sheet, nor Tracking Ticket data
  "type": "Sample",
  "trackingTicketId": "1" // From a partition? - to reference the Tracking Ticket from the Sample Item document
  "millRunSheetId": "1",  // From a partition? - to reference the Mill Run Sheet from the Sample document
  "containerType": "1L Beaker",
  "sampleDate": "2020-09-15T03:10:38.4139427Z",
  "sampledBy": "Humberto",
  // ...other stuff about samples, like 
  "tests": 
  [
    {
      "sequence": "1",
      "testtype": "one-of-the-test-types"
      "methods": 
      [
        "one-of-the-test-methods"        
      ],
      "completedDate": "2020-09-15T12:52:18.1984236Z",
      "resultsSummary": "some summary",
      "tester": "This Guy"
    },
    {
      "sequence": "2",
      "testtype": "one-of-the-test-types"
      "methods": 
      [
        "one-of-the-test-methods",
        "another-test-method"
      ],
      "completedDate": "2020-09-15T08:34:52.5329571Z",
      "resultsSummary": "some summary",
      "tester": "This Guy"
    }
  ]
}
```

### Instrument Item
```
{
  "id": "2", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url when displaying the Item directly without any other "related" data
  "type": "Instrument",
  "trackingTicketId": 2 // From a partition? - to reference the Tracking Ticket from the Instrument Item document
  "name": "Coupled Argon Atomic Emission Spectrometer (ICP-AES)",
  "instructions": "some instructions", // Optional
  "comments": "some comments" // Optional
}
```

### Chemical Item
```
{
  "id": "3", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url when displaying the Item directly without any other "related" data
  "type": "Chemical",
  "trackingTicketId": 3 // From a partition? - to reference the Tracking Ticket from the Chemical Item document
  "name": "Methylene Chloride, Trichloroethylene",
  "commonName": "1,1,1 Trichloroethane"
}
```

## Plant
```
{
  "id": "1",
  "guid": "{some-guid}",
  "name": "Organization 1",
  "acronym": "ORG-1",
  "contactInfo": // Optional
  [
    {
      "type": "address",
      "addressLine1": "Line 1",
      "addressLine1": "Line 2",
      "addressLine1": "Line 3",
      "city": "Here City",
      "state_province": "ST",
      "zip": "12345"
    },
    {
      "type": "person",
      "role": "Personnel Management",
      "name": "John Dunbar",
      "title": "HR Specialist",
      "contact": 
      [
        {
          "type": "email",
          "email": "john.dunbar@org1.org"
        },
        {
          "type": "phone",
          "phoneNumber": "+1-123-456-7890"
        }
      ]
    }
  ]
}
```

## Tracking Ticket
```
{
  "id": "1", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url when displaying the Tracking Ticket (e.g. https://{domain}/track/{ticket_guid})
  "plantId": 1, // From a partition? - to reference a Plant document
  "techName": "Bee Jursef",
  "itemId": 1 // From a partition? - to reference the Tracking Item document  
}
```
