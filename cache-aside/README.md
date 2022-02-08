# Cache-Aside
`Cache-Aside` design pattern improve performance and usibility for reading data from a store by placing the fetched data into in-memory-store (cache) for re-use.
It is related to [Data Flow Management](../README.md#cqrs).

Upon request for stored data `cache-aside` perffroms the following steps:
1. Is the fetched data exists in cache?
2. If yes - the cached value is returned as request's result
3. If no:
4. Fetches the data from the store
5. Place the fetches value into cache
6. Return the fetched value as request's result

## When (partial)
* Same data-value is required
* Fetching data is "expensive" operation
* Data-value size can be accommodated within cache 

## Managing data-entry lifetime with cache
* Creation - clear cache
* Read data - set expiration
* Update - evict from cache
* Delete - evict from cache

## How to Manage Cache for Multiple Application Instances
Central cache that updates local cache

## Implemented Flows
![Cache-Aside Command Flow](../images/cache-aside/command_flow.jpg)
![Cache-Aside Query Flow](../images/cache-aside/query_flow.jpg)
![Cache-Aside Services Relation](../images/cache-aside/services_relation.jpg)
