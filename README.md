# ToDo App

This is a web application for creating and managing to do lists.

## API - ToDoApi

REST API done in .NET Core framework which includes the following:

* Preview of to do list
* Update of to do list & list items (including list/item reordering)
* Creation of to-do list containing list items
* Removing to do list/item
* Searching to do lists by title
* To do list sharing via link with expiration period
* Reminder functionality implying email sending for all of to do lists which "remind me" date has expired. (Reminder runs as backgroud service and for sending email is used 
SendGrid email server provider)

## Web Client - to-do-web-app

The client application is done in Angular and provides user interface which supports the following:

* Dashboard page contains all available to do lists which you can search, delete or share. "Reminded" lists are on the top and visually different from others
* Page for creating/editing to do list/item and adding reminder
* To do lists/items position change via drag-and-drop

## Security

Both, the API and web client support authentication and authorization done with Auth0 identity provider
