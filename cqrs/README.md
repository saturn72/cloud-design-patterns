# CQRS
`CQRS` design pattern separates domain's entity update and read operations.
By providing separate logic for each operation cross-domain-optimization is achieved.
It is related to [Data Flow Management](../README.md#data-flow-management) and sets different behaviors for querying data records and creating and updating these records.

Each executable can leverage 3 channels to acomplish its logic:
1. command - changes the state of memory object
2. query - provide info about memory object
3. notify - declare an event that occured

`CQRS` defines a general usage of these 3 channels

## When (partial)
* High performant services for query and/or command
* [Bounded Context](https://martinfowler.com/bliki/BoundedContext.html) exists between the update and read models
* Different security models for command and query services

## Implemented Domain and story
The example is for an online marketplace product management and implementats 3 services:
* `IdentityServer` - Utilizes [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) for identity management
* `CommandAPI` - Command endpoint(s)
* `QueryAPI` - Query endpoint(s)

The services are:
### 1. `CommandAPI` - Product Administration Service
This service providing product owner to adminitrate products over multiple aspects such inventory management, ordering, SEO and analytics, etc.
And also "exposes" the products to marketplace search engine
Whenevent product is created/updated all attribute and data related to these aspects are managed by product administrator

### 2. `QueryAPI` - Marketplace's Product Search Service
This service recieves product search query from a marketplace app and returns the matching results

## Implemented Flows
![CQRS Command Flow](../images/cqrs/command_flow.jpg)
![CQRS Query Flow](../images/cqrs/query_flow.jpg)
![CQRS Services Relation](../images/cqrs/services_relation.jpg)
