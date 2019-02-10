# Analysis of Twitter Data
## How it works

I have built a tool that can work with data from a MongoDB database in C#.

When you launch the program you get a command interface where you can execute a series of commands:

1. help.
2. total twitter users.
3. users that link to most other users.
4. most mentioned users.
5. most active users.

## How to use it

### Prerequisites

You need to have the .NET Core SDK installed on your machine in order to run this project that can be done from [here](https://dotnet.microsoft.com/download).

You need to have Docker installed on your machine in order to collect the data needed for the project to run that can be done from [here](https://www.docker.com/products/docker-desktop).

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
   
10. Which Twitter users link the most to other Twitter users? (Provide the top ten.)

   ```bash
   $ users that link to most other users
   ```
   
11. Who is are the most mentioned Twitter users? (Provide the top five.)

   ```bash
   $ most mentioned users
   ```

### My systems responses to the following questions:
1. How many Twitter users are in the database?

   ```
   1598315
   ```
   
2. Which Twitter users link the most to other Twitter users? (Provide the top ten.)
  
   ```
   Username: lost_dog Links: 549
   Username: tweetpet Links: 310
   Username: VioletsCRUK Links: 251
   Username: what_bugs_u Links: 246
   Username: tsarnick Links: 245
   Username: SallytheShizzle Links: 229
   Username: mcraddictal Links: 217
   Username: Karen230683 Links: 216
   Username: keza34 Links: 211
   Username: DarkPiano Links: 202
   ```
    
3. Who is are the most mentioned Twitter users? (Provide the top five.)
  
   ```
   Username: mileycyrus Mentions: 4147
   Username: tommcfly Mentions: 3732
   Username: ddlovato Mentions: 3163
   Username: Jonasbrothers Mentions: 1218
   Username: DavidArchie Mentions: 1184
   ```
   
4. Who are the most active Twitter users (top ten)?
  
   ```
   Username: lost_dog Tweets: 549
   Username: webwoke Tweets: 345
   Username: tweetpet Tweets: 310
   Username: SallytheShizzle Tweets: 281
   Username: VioletsCRUK Tweets: 279
   Username: mcraddictal Tweets: 276
   Username: tsarnick Tweets: 248
   Username: what_bugs_u Tweets: 246
   Username: Karen230683 Tweets: 238
   Username: DarkPiano Tweets: 236
   ```
   
5. Who are the five most grumpy (most negative tweets) and the most happy (most positive tweets)?
  
   ```
   TBD
   ```
   
