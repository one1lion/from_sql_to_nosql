# from_sql_to_nosql
A sample project setup that is using Entity Framework Core and SQL Server that I would like to change to use Cosmos DB with Core SQL API

# Introduction
I am attempting to understand NoSQL by starting from what I do know: RDBMS.  The application I am partially assisting with is one that will allow users to scan a QR Code that is the encoded Url endpoint for Tracking Tickets, with the Tracking Ticket's Guid as a route parameter (e.g. https://{domain}/track/{ticket_guid}).  Users will stick QR Code stickers on equipment/instruments, chemicals, and samples.  Then, most likely using an iPad or other tablet device, scan the barcode and be directed to the Url to enter information about the thing they scanned.  Sounds pretty simple, and it was easy to model using a relational database.  However, the items that are being logged can be any number of real-world entities, which should be allowed to take whatever form is needed for a particular items type.  The person I am attempting to help also has the requirement to use Cosmos DB for the database, which, I believe, means that I am not able to apply much of my knowledge of using EF Core for a SQL Server provider.  

Another thing to note is that the way the system can retrieve the various types of data are noted below, but in short, we should be able to query for Tracking Tickets (with the associated Item and all of the data for that particular item's type), Mill Run Sheets (and the associated Samples -- which are a particular Item type), or individual Items (which will be displayed on their own page, and should be able to be navigated to directly if, for instance, a user bookmarks the page and navigates directly to it).

Lastly, the product that this is going to be for is just something that someone asked me to help them figure out. I do not have all of the information regarding the usage, terminology, nor structure.  This is my best guess based on the conversations we've had.  I still think I can use this to figure out how to setup and use a NoSQL database for other scenarios.

# Definitions
**Mill Run Sheet**: A Mill Run Sheet is a physical document used for tracking samples taken from Storage Tanks -- Although it looks like the definitions I've found through Google point to this being a sampling of solid material from a mill, whereas the samples used for this project are liquid, I believe. 
>*Note that a Mill Run Sheet is created for taking multiple samples.  Once the samples are added to the sheet and handed over, the Mill Run Sheet will not be modified further.  So the entry of the Mill Run Sheet happens onces, and the samples are associated with it by adding the Mill Run Sheet Id to the Sample item's document.*

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
The following should be Documents that can be queried directly.  The Mill Run Sheets can have multiple Samples (Samples are an Item with the `"type"` property set to `"Sample"`).  Note that the same Sample item will be referenced by a `Tracking Ticket` document.  I believe each section would be its own collection:
 - Plants collection: Each document is created once, seldom edited, and seldom read
 - Mill Run Sheets collection: Each document is created once, seldom edited, and seldom read
 - Tracking Items collection: Each document is created once, moderately edited, and often read
 - Tracking Tickets collection: Each document is created once, moderately edited, and often read
 
I am still not sure about partitions.  I get the concept is to use a property that is unique, unchanging, and is for referencing documents from other documents, but the tutorials and blogs I've looked at deal with using 1 partition.  With my relational mindset, I'm thinking FKs, where a Sample item type can have an FK of the Tracking Ticket Id as well as the Mill Run Sheet Id, since it can reach out to get the data of both of those objects (entities) from the Sample record.  Likewise, the Tracking Ticket would have the Sample Item's Id.  Does this mean a partition on Plant Id, Mill Run Sheet Id, Tracking Item Id (as in the Chemical, Instrument, or Sample document's Id), and Tracking Ticket Id?  I'll keep trying to figure that out.

## Plant collection
These are the collection of documents related to the physical Plants, which are "Sites" or "Organizations" that have personnel that will use this system.
```
{
  "id": "1", // An auto-increment Id
  "guid": "{some-guid}", // For using in a Url when displaying the Plant directly
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
 
## Mill Run Sheet collection
These are the digital representations of the physical sheets used to document samples and from where they are taken.
```
{
  "id": "1", // An auto-increment Id
  "guid": "{some-guid}",  // For using in a Url to pull up the Mill Run Sheet
  "millRunSheetId": "2020SEP-13392", // Internal Id
  "tankId": "PTR-2492", // Internal Tank Id/Name
  "batchId": "BB-4",
  "lot": "AHIK"
}
```

## Tracking Items collection
These are the items that will be referenced on a `Tracking Ticket` by `itemId`...although I think `itemId` is a reserved object in a Cosmos DB record, so I might need to change the actual property name, or be okay with using the value in `itemId`.  In any case, I think this is how I would model it, but I am certainly looking for input on this.  For now, items can be of type: Chemical, Instrument, or Sample; but this can be extended in the future.

### Sample Item type document
```
{
  "id": "1", // An auto-increment Id
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

### Instrument Item type document
```
{
  "id": "2", // An auto-increment Id
  "guid": "{some-guid}",  // For using in a Url when displaying the Item directly
  "type": "Instrument",
  "trackingTicketId": 2 // From a partition? - to reference the Tracking Ticket from the Instrument Item document
  "name": "Coupled Argon Atomic Emission Spectrometer (ICP-AES)",
  "instructions": "some instructions", // Optional
  "comments": "some comments" // Optional
}
```

### Chemical Item type document
```
{
  "id": "3", // An auto-increment Id
  "guid": "{some-guid}",  // For using in a Url when displaying the Item directly
  "type": "Chemical",
  "trackingTicketId": 3 // From a partition? - to reference the Tracking Ticket from the Chemical Item document
  "name": "Methylene Chloride, Trichloroethylene",
  "commonName": "1,1,1 Trichloroethane"
}
```

## Tracking Ticket Collection
The main entry point for the tracking and QC system.  This is used for documenting a specific Item, whether the item be a Chemical, Instrument, Sample, or other Item Type added in the future.  The Tracking Ticket will be associated with a Guid that will be encoded in a QR Code with the corresponding /track/{guid} Url.  Each ticket will be associated with one, and only one, Item.
```
{
  "id": "1", // An auto-increment Id, to be partitioned on
  "guid": "{some-guid}",  // For using in a Url when displaying the Tracking Ticket (e.g. https://{domain}/track/{ticket_guid})
  "plantId": 1, // From a partition? - to reference a Plant document
  "techName": "Bee Jursef",
  "itemId": 1 // From a partition? - to reference the Tracking Item document  
}
```
