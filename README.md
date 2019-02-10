# Analysis of Twitter Data
## How it works

I have built a tool that can work with data from a MongoDB database in C#.

When you launch the program you get a command interface where you can execute a series of commands:

1. help.
2. total twitter users.

## How to use it

### Prerequisites

You need to have the .NET Core SDK installed on your machine in order to run this project that can be done from [here](https://dotnet.microsoft.com/download).

If you are using Windows it is recommended to run the program with the [Git Bash](https://git-scm.com/downloads) terminal or any other Linux-based terminal.

### Windows/Linux/Mac

1. Clone this repository in a location of your choice:

   ```````bash
   $ git clone https://github.com/kagejohn/db_assignment_02.git
   ```````
2. CD into the project folder:

   ```bash
   $ cd db_assignment_02/Assignment_2_DB/
   ```

3. Build the docker compose file:

   ```bash
   $ docker-compose up --build
   ```
   
4. Open a new terminal and run this command:

  ```bash
  $ docker exec -d assignment_2_db_mongodb_1 mongoimport --drop --db social_net --collection tweets --type csv --headerline --file training.1600000.processed.noemoticon.csv
  ```

5. CD into the project folder:

   ```bash
   $ cd db_assignment_02/Assignment_2_DB/
   ```

6. Build the project:

   ```bash
   $ dotnet build
   ```

7. CD into the program folder:

   ```bash
   $ cd Assignment_2_DB/bin/Debug/netcoreapp2.1
   ```
   
8. Run the program:

   ```bash
   $ dotnet Assignment_2_DB.dll
   ```

9. How many Twitter users are in the database?:

   ```bash
   $ total twitter users
   ```
