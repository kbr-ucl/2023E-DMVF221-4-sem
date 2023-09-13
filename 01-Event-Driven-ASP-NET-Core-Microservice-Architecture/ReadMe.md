
# How to Build an Event-Driven ASP.NET Core Microservice Architecture
https://itnext.io/how-to-build-an-event-driven-asp-net-core-microservice-architecture-e0ef2976f33f
## RabitMq	

### Use RabbitMQ and Configure Exchanges and Pipelines

In the second part of this guide, you will **get RabbitMQ running**. Then you will **use the RabbitMQ admin web UI to configure the exchanges and pipelines** for the application. Optionally you can use the admin UI to send messages to RabbitMQ.

*This graphic shows how the UserService publishes messages to RabbitMQ and the PostService and a potential other service consume those messages:*

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\1K8oytI-Z7gw1Sp26CxGEng.png)

The easiest way to get RabbitMQ running is to **install** [**Docker Desktop**](https://hub.docker.com/editions/community/docker-ce-desktop-windows). Then **issue the following command** (in one line in a console window) to start a RabbitMQ container with admin UI :

```
C:\dev>docker run -d  -p 15672:15672 -p 5672:5672 --hostname my-rabbit --name some-rabbit rabbitmq:3-management
```



**Open your browser** on port 15672 and log in with the username “guest” and the password “guest”. Use the web UI to **create an Exchange** with the name “user” of type “Fanout” and **two queues** “user.postservice” and “user.otherservice”.

> It is important to use the type “Fanout” so that the exchange copies the message to all connected queues.

You can also use the web UI to publish messages to the exchange and see how they get queued:



## Test the Workflow

In the final part of this guide you will test the whole workflow:

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\1nBI724d-APCSGAYo5m3zcg.png)

**Summary of the steps in the last part of this guide** (you can access the services with the Swagger UI):

- Call the UserService REST API and add a user to the user DB
- The UserService will create an event that the PostService consumes and adds the user to the post DB
- Access the PostService REST API and add a post for the user.
- Call the PostService REST API and load the post and user from the post DB
- Call the UserService REST API and rename the user
- The UserService will create an event that the PostService consumes and updates the user’s name in the post DB
- Call the PostService REST API and load the post and renamed user from the post DB

> The user DB must be empty. You can delete the user.db (in the Visual Studio explorer) if you created users in previous steps of this guide. The calls to Database.EnsureCreated() will recreate the DBs on startup.

**Configure both projects to run as service**:

Use the Swagger UI to create a user in the UserService:

```
{
 "name": "Chris",
 "mail": "chris@chris.com",
 "otherData": "Some other data"
}
```

The generated userId might be different in your environment:

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\1irm6bDO86SNAURShpWWT6w.png)

The integration event replicates the user to the PostService:

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\1K8qzwMngXwwR6PzhnFYaTQ.png)

Now you can create a post in the PostServive Swagger UI (use your userId):

```
{
  "title": "MyFirst Post",
  "content": "Some interesting text",
  "userId": 1
}
```

Read all posts. The username is included in the result:

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\15Jmh4yzAnARKeQQp96wMCA.png)

Change the username in the UserService Swagger UI:

```
{
  "id": 1,
  "name": "My new name"
}
```

Then read the posts again and see the changed username:

![img](C:\Dropbox\SourceCode\2023E-DMVF221-4-sem\01-Event-Driven-ASP-NET-Core-Microservice-Architecture\ReadMe.assets\1xNOsX0lltHO-nG9LaEbHQA.png)