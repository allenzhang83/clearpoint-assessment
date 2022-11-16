# Todo List APIs

The backend is a simple todo list API application. The APIs support standard LIST, GET, CREATE and UPDATE operations to manage todo items.

<br/><br/>

## Run the Application

Install .net 6 if not already installed.<br/>
Start debugging normally in Visual Studio. The landing page is the swagger UI.
<br/><br/>

## Assumptions

1. It is the correct behaviour that only the LIST API takes "IsCompleted" into account. All other APIs work with both completed and active todo items.
2. It is OK to modify the API signature (without versioning). The PUT method had the item id as a parameter, this is unnecessary as the id is available in the update todo item payload.
3. It is expected that the frontend will pass the id to the API when creating a todo item. This is a bit strange as normally the backend creates the id (normally also the db table primary key).
